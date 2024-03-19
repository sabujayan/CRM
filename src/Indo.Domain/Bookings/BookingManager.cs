using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Bookings
{
    public class BookingManager : DomainService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingManager(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public async Task<Booking> CreateAsync(
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime,
            [NotNull] Guid resourceId
            )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(startTime, nameof(startTime));
            Check.NotNull(endTime, nameof(endTime));
            Check.NotNull(resourceId, nameof(resourceId));

            var existing = await _bookingRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new BookingAlreadyExistsException(name);
            }

            return new Booking(
                GuidGenerator.Create(),
                name,
                startTime,
                endTime,
                resourceId
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Booking booking,
           [NotNull] string newName)
        {
            Check.NotNull(booking, nameof(booking));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _bookingRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != booking.Id)
            {
                throw new BookingAlreadyExistsException(newName);
            }

            booking.ChangeName(newName);
        }
    }
}
