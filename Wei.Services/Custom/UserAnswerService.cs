using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core;
using Wei.Core.Caching;
using Wei.Core.Data;
using Wei.Core.Domain.Custom;
using Wei.Core.Domain.Questions;
using Wei.Services.Events;
using Wei.Services.Questions;

namespace Wei.Services.Custom
{
    public partial class UserAnswerService : IUserAnswerService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string ROLES_ALL_KEY = "WEI.useranswer.all-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ROLES_PATTERN_KEY = "WEI.useranswer.";
        #endregion

        #region Fields
        private readonly IRepository<UserAnswer> _userAnswerRepository;
        private readonly IRepository<UserAnswerDetail> _userAnswerDetailRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IQuestionBankService _questionBankService;
        #endregion

        #region Ctor
        public UserAnswerService(IRepository<UserAnswer> userAnswerRepository
            , IRepository<UserAnswerDetail> userAnswerDetailRepository
            , ICacheManager cacheManager
            , IEventPublisher eventPublisher
            , IQuestionBankService questionBankService)
        {
            _userAnswerRepository = userAnswerRepository;
            _userAnswerDetailRepository = userAnswerDetailRepository;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _questionBankService = questionBankService;
        }
        #endregion

        #region methods
        /// <summary>
        /// 根据id获取答题记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserAnswer GetUserAnswerById(int id)
        {
            return this._userAnswerRepository.GetById(id);
        }

        /// <summary>
        /// 开始答题
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="questionbankid"></param>
        /// <returns></returns>
        public Question BeginQuestion(int userid, int questionbankid)
        {
            if (GetDoingQuestionAnswer(userid) != null)
                throw new WeiException("当前有未完成的题卷！");
            UserAnswer ua = new UserAnswer()
            {
                User_Id = userid,
                QuestionBank_Id = questionbankid,
                BeginTime = DateTime.Now
            };
            UserAnswerDetail uanswerdetail = new UserAnswerDetail()
            {
                QuestionNo = 1,
                Start = DateTime.Now
            };
            ua.UserAnswerDetailList.Add(uanswerdetail);
            this._userAnswerRepository.Insert(ua);
            return this._questionBankService.GetQuestion(questionbankid, uanswerdetail.QuestionNo);
        }

        /// <summary>
        /// 获取正在做的题卷
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public UserAnswer GetDoingQuestionAnswer(int userid)
        {
            return this._userAnswerRepository.Table.FirstOrDefault(x => x.User_Id == userid && x.CompletedTime == null);
        }

        /// <summary>
        /// 获取最近一次的答题记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public UserAnswer GetLastQuestionAnswer(int userid)
        {
            return (from t in this._userAnswerRepository.Table where t.User_Id == userid orderby t.Id descending select t).FirstOrDefault();
        }

        /// <summary>
        /// 保存答案并获取下一题
        /// </summary>
        /// <param name="uanswer"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public Question SaveAnswer(UserAnswer uanswer, string answer)
        {
            var uanswerdetail = uanswer.UserAnswerDetailList.First(x => x.End == null);
            uanswerdetail.Answer = answer;
            uanswerdetail.End = DateTime.Now;

            // 获取当前问题
            var question = this._questionBankService.GetQuestion(uanswerdetail.UserAnswer.QuestionBank_Id, uanswerdetail.QuestionNo);
            bool result = false;
            // 判断当前答案
            switch (question.AnswerType)
            {
                case AnswerType.SingleCheck:
                    result = string.Equals(question.Answer.Trim(), uanswerdetail.Answer.Trim(), StringComparison.CurrentCultureIgnoreCase);
                    break;
                case AnswerType.MultiCheck:
                    var arr1 = question.Answer.Split('|');
                    var answer1 = uanswerdetail.Answer.ToLower();
                    result = !arr1.Any(x => answer1.IndexOf(x.Trim().ToLower()) == -1);
                    break;
                case AnswerType.Text:
                    var arr2 = question.Answer.Split('|');
                    var answer2 = uanswerdetail.Answer.ToLower();
                    result = arr2.Any(x => answer2.IndexOf(x.Trim().ToLower()) >= 0);
                    break;
            }
            int nextno = 0;
            if (result && question.Next1 != null && question.Next1.Value > 0)
            {
                nextno = question.Next1.Value;
            }
            else if (!result && question.Next2 != null && question.Next2.Value > 0)
            {
                nextno = question.Next2.Value;
            }
            uanswerdetail.Next = nextno;
            // 保存当前答案
            this._userAnswerDetailRepository.Update(uanswerdetail);

            if (uanswerdetail.Next > 0)
            {
                // 写入新题目
                UserAnswerDetail detail = new UserAnswerDetail()
                {
                    UserAnswer_Id = uanswer.Id,
                    QuestionNo = uanswerdetail.Next.Value,
                    Start = DateTime.Now
                };
                this._userAnswerDetailRepository.Insert(detail);
                return this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, uanswerdetail.Next.Value);
            }
            // 更新标识题卷已完成
            uanswer.CompletedTime = DateTime.Now;
            this._userAnswerRepository.Update(uanswer);
            return null;
        }

        public IList<UserAnswer> QueryAll()
        {
            throw new NotImplementedException();
        }

        public IList<UserAnswer> QueryByUser(int userid, int questionbankid = 0
            , DateTime begintime = default(DateTime), DateTime completedtime = default(DateTime))
        {
            if (completedtime == default(DateTime))
                completedtime = DateTime.Now;
            var query = this._userAnswerRepository.Table;
            if (userid > 0)
                query = query.Where(x => x.User_Id == userid);
            if (questionbankid > 0)
                query = query.Where(x => x.QuestionBank_Id == questionbankid);
            if (begintime != default(DateTime))
                query = query.Where(x => x.BeginTime >= begintime);
            if (completedtime != default(DateTime))
                query = query.Where(x => x.CompletedTime <= completedtime);
            return null;
        }
        #endregion


    }
}
