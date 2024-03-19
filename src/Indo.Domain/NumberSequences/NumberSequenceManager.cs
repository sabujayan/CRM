using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.NumberSequences
{
    public class NumberSequenceManager : DomainService
    {
        private readonly INumberSequenceRepository _numberSequenceRepository;

        public NumberSequenceManager(INumberSequenceRepository numberSequenceRepository)
        {
            _numberSequenceRepository = numberSequenceRepository;
        }
        public async Task<NumberSequence> CreateAsync(
            [NotNull] string prefix)
        {
            Check.NotNullOrWhiteSpace(prefix, nameof(prefix));

            var existing = await _numberSequenceRepository.FindAsync(x => x.Suffix.Equals(prefix));
            if (existing != null)
            {
                throw new NumberSequenceAlreadyExistsException(prefix);
            }

            return new NumberSequence(
                GuidGenerator.Create(),
                prefix
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] NumberSequence numberSequence,
           [NotNull] string newPrefix)
        {
            Check.NotNull(numberSequence, nameof(numberSequence));
            Check.NotNullOrWhiteSpace(newPrefix, nameof(newPrefix));

            var existing = await _numberSequenceRepository.FindAsync(x => x.Suffix.Equals(newPrefix));
            if (existing != null && existing.Id != numberSequence.Id)
            {
                throw new NumberSequenceAlreadyExistsException(newPrefix);
            }

            numberSequence.ChangeSuffix(newPrefix);
        }

        private static readonly object LOCK_NUMBER_SEQUENCE = new object();
        public async Task<string> GetNextNumberAsync(NumberSequenceModules module)
        {
            string result = "";
            var maxAttempt = 10;

            await Task.Yield();

            for (int i = 0; i < maxAttempt; i++)
            {
                try
                {
                    lock (LOCK_NUMBER_SEQUENCE)
                    {
                        NumberSequence seq = _numberSequenceRepository
                            .Where(x => x.Module.Equals(module))
                            .FirstOrDefault();

                        if (seq == null)
                        {
                            throw new UserFriendlyException($"Number Sequence for module {module.ToString()} does not exists!");
                        }

                        if (module == NumberSequenceModules.CustomerRootFolder)
                        {
                            result = $"{seq.Suffix}{seq.NextNumber.ToString().PadLeft(6, '0')}";
                        }
                        else
                        {
                            result = $"{seq.NextNumber.ToString().PadLeft(6, '0')}/{DateTime.Now.ToString("yyyyMMdd")}/{seq.Suffix}";
                        }

                        seq.NextNumber++;
                        _numberSequenceRepository.UpdateAsync(seq);
                        return result;
                    }

                }
                catch (Exception)
                {
                    if (i >= maxAttempt)
                        throw new UserFriendlyException($"Number Sequence generation for module {module.ToString()} is fail!");
                }
            }

            return result;
        }
    }
}
