using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.VendorDebitNotes
{
    public interface IVendorDebitNoteRepository : IRepository<VendorDebitNote, Guid>
    {
    }
}
