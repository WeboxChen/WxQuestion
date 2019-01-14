using System;
using System.Collections.Generic;
using System.Linq;
using Wei.Core.Data;
using Wei.Core.Domain.Questions;

namespace Wei.Services.Questions
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IRepository<QuestionBank> _questionBankRepository;
        private readonly IRepository<Question> _questionRepository;

        public QuestionBankService(IRepository<QuestionBank> questionBankRepository
            , IRepository<Question> questionRepository)
        {
            _questionBankRepository = questionBankRepository;
            _questionRepository = questionRepository;
        }

        public Question GetQuestion(int questionbankid, decimal questionno)
        {
            return this._questionRepository.Table.First(x => x.QuestionBank_Id == questionbankid && x.Sort == questionno);
        }

        public QuestionBank GetQuestionBankById(int id)
        {
            return this._questionBankRepository.GetById(id);
        }

        public IList<QuestionBank> Query()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, QuestionBank> KeyWordQuestionBank()
        {
            IDictionary<string, QuestionBank> result = new Dictionary<string, QuestionBank>();
            var list = _questionBankRepository.Table.Where(x => x.AutoResponse != null && x.AutoResponse.Value);
            foreach(var item in list)
            {
                if (string.IsNullOrEmpty(item.ResponseKeyWords))
                    continue;
                string[] arr = item.ResponseKeyWords.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string key in arr)
                {
                    if (result.ContainsKey(key))
                        continue;
                    result.Add(key, item);
                }
            }
            return result;
        }
    }
}
