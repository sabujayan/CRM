using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Notes
{
    public class NoteManager : DomainService
    {
        private readonly INoteRepository _expenseRepository;

        public NoteManager(INoteRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        public async Task<Note> CreateAsync(
            [NotNull] string description,
            [NotNull] Guid customerId
            )
        {
            await Task.Yield();

            Check.NotNullOrWhiteSpace(description, nameof(description));
            Check.NotNull(customerId, nameof(customerId));


            return new Note(
                GuidGenerator.Create(),
                description,
                customerId
            );
        }
    }
}
