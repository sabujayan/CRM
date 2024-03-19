using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.Movements;
using Indo.NumberSequences;
using Indo.StockAdjustmentDetails;
using Indo.Stocks;
using Indo.Warehouses;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.StockAdjustments
{
    public class StockAdjustmentManager : DomainService
    {
        private readonly WarehouseManager _warehouseManager;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IStockAdjustmentDetailRepository _stockAdjustmentDetailRepository;
        private readonly StockAdjustmentDetailManager _stockAdjustmentDetailManager;
        private readonly NumberSequenceManager _numberSequenceManager;
        private readonly IMovementRepository _movementRepository;
        private readonly MovementManager _movementManager;
        private readonly IStockRepository _stockRepository;
        private readonly StockManager _stockManager;

        public StockAdjustmentManager(
            WarehouseManager warehouseManager,
            IStockAdjustmentRepository stockAdjustmentRepository,
            IStockAdjustmentDetailRepository stockAdjustmentDetailRepository,
            StockAdjustmentDetailManager stockAdjustmentDetailManager,
            NumberSequenceManager numberSequenceManager,
            IMovementRepository movementRepository,
            MovementManager movementManager,
            IStockRepository stockRepository,
            StockManager stockManager
            )
        {
            _warehouseManager = warehouseManager;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _stockAdjustmentDetailRepository = stockAdjustmentDetailRepository;
            _stockAdjustmentDetailManager = stockAdjustmentDetailManager;
            _numberSequenceManager = numberSequenceManager;
            _movementRepository = movementRepository;
            _movementManager = movementManager;
            _stockRepository = stockRepository;
            _stockManager = stockManager;
        }
        public async Task<StockAdjustment> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid warehouseId,
            [NotNull] StockAdjustmentType adjustmentType,
            [NotNull] DateTime adjustmentDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(warehouseId, nameof(warehouseId));
            Check.NotNull<StockAdjustmentType>(adjustmentType, nameof(adjustmentType));
            Check.NotNull<DateTime>(adjustmentDate, nameof(adjustmentDate));

            var existing = await _stockAdjustmentRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new StockAdjustmentAlreadyExistsException(number);
            }

            var stockAdjustment = new StockAdjustment(
                GuidGenerator.Create(),
                number,
                warehouseId,
                adjustmentType,
                adjustmentDate
            );

            stockAdjustment = await ChangeType(stockAdjustment);

            return stockAdjustment;
        }
        public async Task ChangeNameAsync(
           [NotNull] StockAdjustment stockAdjustment,
           [NotNull] string newName)
        {
            Check.NotNull(stockAdjustment, nameof(stockAdjustment));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _stockAdjustmentRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != stockAdjustment.Id)
            {
                throw new StockAdjustmentAlreadyExistsException(newName);
            }

            stockAdjustment.ChangeName(newName);
        }

        public async Task<StockAdjustment> ChangeType(StockAdjustment stockAdjustment)
        {

            var adjustmentWH = await _warehouseManager.GetDefaultWarehouseAdjustmentAsync();
            if (stockAdjustment.AdjustmentType == StockAdjustmentType.Addition)
            {
                stockAdjustment.FromWarehouseId = adjustmentWH.Id;
                stockAdjustment.ToWarehouseId = stockAdjustment.WarehouseId;
            }
            else
            {
                stockAdjustment.FromWarehouseId = stockAdjustment.WarehouseId;
                stockAdjustment.ToWarehouseId = adjustmentWH.Id;
            }

            return stockAdjustment;
        }

        public async Task<StockAdjustment> ConfirmStockAdjustmentAsync(Guid stockAdjustmentId)
        {
            var stockAdjustment = await _stockAdjustmentRepository.GetAsync(stockAdjustmentId);
            if (stockAdjustment.Status == StockAdjustmentStatus.Draft)
            {
                stockAdjustment.Status = StockAdjustmentStatus.Confirm;
                var query = await _stockAdjustmentDetailRepository.GetQueryableAsync();
                var lines = await AsyncExecuter.ToListAsync(query.Where(x => x.StockAdjustmentId.Equals(stockAdjustment.Id)));
                foreach (var item in lines)
                {
                    var movement = await _movementManager.CreateAsync(
                        await _numberSequenceManager.GetNextNumberAsync(NumberSequenceModules.Movement),
                        stockAdjustment.FromWarehouseId,
                        stockAdjustment.ToWarehouseId,
                        stockAdjustment.AdjustmentDate,
                        stockAdjustment.Number,
                        NumberSequenceModules.StockAdjustment,
                        item.ProductId,
                        item.Quantity
                        );
                    var result = await _movementRepository.InsertAsync(movement, autoSave: true);

                    //stock outlet
                    var outlet = await _stockManager.CreateAsync(
                        result.Id,
                        movement.FromWarehouseId,
                        movement.MovementDate,
                        movement.Number,
                        StockFlow.Outlet,
                        movement.ProductId,
                        movement.Qty * -1
                        );
                    await _stockRepository.InsertAsync(outlet, autoSave: true);

                    //stock inlet
                    var inlet = await _stockManager.CreateAsync(
                        result.Id,
                        movement.ToWarehouseId,
                        movement.MovementDate,
                        movement.Number,
                        StockFlow.Inlet,
                        movement.ProductId,
                        movement.Qty * 1
                        );
                    await _stockRepository.InsertAsync(inlet, autoSave: true);
                }
                await _stockAdjustmentRepository.UpdateAsync(stockAdjustment, autoSave: true);
            }
            return stockAdjustment;
        }

        public async Task<StockAdjustment> GenerateReturn(Guid stockAdjustmentId)
        {
            var origin = await _stockAdjustmentRepository.GetAsync(stockAdjustmentId);
            if (origin.Status == StockAdjustmentStatus.Confirm)
            {
                var result = await CreateAsync(
                        origin.Number + "-RN",
                        origin.WarehouseId,
                        origin.AdjustmentType == StockAdjustmentType.Addition ? StockAdjustmentType.Deduction : StockAdjustmentType.Addition,
                        DateTime.Now
                    );
                result.OriginalId = stockAdjustmentId;
                await _stockAdjustmentRepository.InsertAsync(result, autoSave: true);
                var query = await _stockAdjustmentDetailRepository.GetQueryableAsync();
                var lines = await AsyncExecuter.ToListAsync(query.Where(x => x.StockAdjustmentId.Equals(origin.Id)));
                var details = new List<StockAdjustmentDetail>();
                foreach (var item in lines)
                {
                    var detail = await _stockAdjustmentDetailManager.CreateAsync(
                            result.Id,
                            item.ProductId,
                            item.Quantity
                        );
                    details.Add(detail);
                }
                await _stockAdjustmentDetailRepository.InsertManyAsync(details, autoSave: true);
                return result;
            }
            return null;
        }
    }
}
