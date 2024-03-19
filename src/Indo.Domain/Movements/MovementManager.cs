using System;
using System.Threading.Tasks;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Movements
{
    public class MovementManager : DomainService
    {
        private readonly IMovementRepository _movementRepository;

        public MovementManager(IMovementRepository movementRepository)
        {
            _movementRepository = movementRepository;
        }
        public async Task<Movement> CreateAsync(
            [NotNull] string number,
            [NotNull] Guid fromWarehouseId,
            [NotNull] Guid toWarehouseId,
            [NotNull] DateTime movementDate,
            [NotNull] string sourceDocument,
            [NotNull] NumberSequenceModules module,
            [NotNull] Guid productId,
            [NotNull] float qty
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull<Guid>(fromWarehouseId, nameof(fromWarehouseId));
            Check.NotNull<Guid>(toWarehouseId, nameof(toWarehouseId));
            Check.NotNull<DateTime>(movementDate, nameof(movementDate));
            Check.NotNull<String>(sourceDocument, nameof(sourceDocument));
            Check.NotNull<NumberSequenceModules>(module, nameof(module));
            Check.NotNull<Guid>(productId, nameof(productId));
            Check.NotNull<float>(qty, nameof(qty));

            var existing = await _movementRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new MovementAlreadyExistsException(number);
            }

            return new Movement(
                GuidGenerator.Create(),
                number,
                fromWarehouseId,
                toWarehouseId,
                movementDate,
                sourceDocument,
                module,
                productId,
                qty
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Movement movement,
           [NotNull] string newName)
        {
            Check.NotNull(movement, nameof(movement));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _movementRepository.FindAsync(x => x.Number.Equals(newName));
            if (existing != null && existing.Id != movement.Id)
            {
                throw new MovementAlreadyExistsException(newName);
            }

            movement.ChangeName(newName);
        }
    }
}
