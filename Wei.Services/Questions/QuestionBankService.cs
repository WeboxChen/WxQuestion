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

        public Question GetQuestion(int questionbankid, int questionno)
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
    }
}
