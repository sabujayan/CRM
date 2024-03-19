using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indo.TransferOrderDetails;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.TransferOrders
{
    public class TransferOrderManager : DomainService
    {
        private readonly ITransferOrderRepository _transferOrderRepository;
        private readonly ITransferOrderDetailRepository _transferOrderDetailRepository;
        private readonly TransferOrderDetailManager _transferOrderDetailManager;

        public TransferOrderManager(
            ITransferOrderRepository transferOrderRepository,
            ITransferOrderDetailRepository transferOrderDetailRepository,
            TransferOrderDetailManager transferOrderDetailManager
            )
        {
            _transferOrderRepository = transferOrderRepository;
            _transferOrderDetailRepository = transferOrderDetailRepository;
            _transferOrderDetailManager = transferOrderDetailManager;
        }
        public async Task<TransferOrder> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid fromWarehouseId,
            [NotNull] Guid toWarehouseId,
            [NotNull] DateTime orderDate
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(fromWarehouseId, nameof(fromWarehouseId));
            Check.NotNull<Guid>(toWarehouseId, nameof(toWarehouseId));
            Check.NotNull<DateTime>(orderDate, nameof(orderDate));

            var existing = await _transferOrderRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new TransferOrderAlreadyExistsException(number);
            }

            return new TransferOrder(
                GuidGenerator.Create(),
                number,
                fromWarehouseId,
                toWarehouseId,
                orderDate
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] TransferOrder transferOrder,
           [NotNull] string newName)
        {
            Check.NotNull(transferOrder, nameof(transferOrder));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _transferOrderRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != transferOrder.Id)
            {
                throw new TransferOrderAlreadyExistsException(newName);
            }

            transferOrder.ChangeName(newName);
        }
        public async Task<TransferOrder> ConfirmAsync(Guid transferOrderId)
        {
            var obj = await _transferOrderRepository.GetAsync(transferOrderId);
            if (obj.Status == TransferOrderStatus.Draft)
            {
                obj.Status = TransferOrderStatus.Confirm;
                return await _transferOrderRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Draft can be confirm!");
            }
        }


        public async Task<TransferOrder> GenerateReturn(Guid transferOrderId)
        {
            var origin = await _transferOrderRepository.GetAsync(transferOrderId);
            if (origin.Status == TransferOrderStatus.Confirm)
            {
                var result = await CreateAsync(
                        origin.Number + "-RN",
                        origin.ToWarehouseId,
                        origin.FromWarehouseId,
                        DateTime.Now
                    );
                result.OriginalId = transferOrderId;
                await _transferOrderRepository.InsertAsync(result, autoSave: true);
                var query = await _transferOrderDetailRepository.GetQueryableAsync();
                var lines = await AsyncExecuter.ToListAsync(query.Where(x => x.TransferOrderId.Equals(origin.Id)));
                var details = new List<TransferOrderDetail>();
                foreach (var item in lines)
                {
                    var detail = await _transferOrderDetailManager.CreateAsync(
                            result.Id,
                            item.ProductId,
                            item.Quantity
                        );
                    details.Add(detail);
                }
                await _transferOrderDetailRepository.InsertManyAsync(details, autoSave: true);
                return result;
            }
            return null;
        }

    }
}
