using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core.Data;
using Wei.Core.Domain.Sys;

namespace Wei.Services.Sys
{
    public class WordsSubstitutionService : IWordsSubstitutionService
    {
        private readonly IRepository<WordsSubstitution> _wordsSubstitutionRepository;

        public WordsSubstitutionService(IRepository<WordsSubstitution> wordsSubstitutionRepository)
        {
            this._wordsSubstitutionRepository = wordsSubstitutionRepository;
        }

        public IDictionary<string, string> GetSubstitutions(int qbid = 0, bool hasCommon = true)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            if (qbid > 0)
            {
                var query = this._wordsSubstitutionRepository.Table.Where(x => x.QuestionBankId == qbid);
                foreach (var item in query)
                {
                    if (!result.ContainsKey(item.Words1))
                        result.Add(item.Words1, item.Words2);
                }
            }
            if (hasCommon)
            {
                var query = this._wordsSubstitutionRepository.Table.Where(x => x.QuestionBankId == null || x.QuestionBankId == 0);
                foreach(var item in query)
                {
                    if (!result.ContainsKey(item.Words1))
                        result.Add(item.Words1, item.Words2);
                }
            }
            return result;
        }
    }
}
