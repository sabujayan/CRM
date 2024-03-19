using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Bookings
{
    public interface IBookingRepository : IRepository<Booking, Guid>
    {
    }
}
