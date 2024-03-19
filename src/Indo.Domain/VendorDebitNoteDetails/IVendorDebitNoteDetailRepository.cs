using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.VendorDebitNoteDetails
{
    public interface IVendorDebitNoteDetailRepository : IRepository<VendorDebitNoteDetail, Guid>
    {
    }
}
