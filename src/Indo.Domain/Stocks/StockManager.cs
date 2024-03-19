using System;
using System.Threading.Tasks;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Stocks
{
    public class StockManager : DomainService
    {
        private readonly IStockRepository _stockRepository;

        public StockManager(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<Stock> CreateAsync(
            [NotNull] Guid movementId,
            [NotNull] Guid warehouseId,
            [NotNull] DateTime transactionDate,
            [NotNull] string sourceDocument,
            [NotNull] StockFlow flow,
            [NotNull] Guid productId,
            [NotNull] float qty
            )
        {
            await Task.Yield();

            Check.NotNull<Guid>(movementId, nameof(movementId));
            Check.NotNull<Guid>(warehouseId, nameof(warehouseId));
            Check.NotNull<DateTime>(transactionDate, nameof(transactionDate));
            Check.NotNull<String>(sourceDocument, nameof(sourceDocument));
            Check.NotNull<StockFlow>(flow, nameof(flow));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(qty, nameof(qty));


            return new Stock(
                GuidGenerator.Create(),
                movementId,
                warehouseId,
                transactionDate,
                sourceDocument,
                flow,
                productId,
                qty
            );
        }
    }
}
