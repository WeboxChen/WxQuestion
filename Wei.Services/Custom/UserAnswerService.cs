using System;
using System.Collections.Generic;
using System.Linq;
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

        #region common methods
        /// <summary>
        /// 单选题解析
        /// 解析用户回答，获取下一题
        /// </summary>
        /// <param name="uanswer">用户答题对象</param>
        /// <param name="question">问题对象</param>
        /// <param name="answer">用户答案文本</param>
        /// <returns>下一题</returns>
        private Question SingleCheckAnalysis(UserAnswer uanswer, Question question, string answer)
        {
            decimal next = decimal.Ceiling(question.Sort + 1);
            var qamodel = question.QuestionAnswerList.OrderBy(x=>x.Sort)
                .FirstOrDefault(x => string.Equals(answer, x.AnswerKeys, StringComparison.CurrentCultureIgnoreCase));
            if (qamodel != null && qamodel.Next != null)
                next = qamodel.Next.Value;
            Question nextq = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, next);
            return nextq;
        }

        /// <summary>
        /// 多选题解析
        /// 解析用户回答，获取下一题
        /// </summary>
        /// <param name="uanswer">用户答题对象</param>
        /// <param name="question">问题对象</param>
        /// <param name="answer">用户答案文本</param>
        /// <returns>下一题</returns>
        private Question MultiCheckAnalysis(UserAnswer uanswer, Question question, string answer)
        {
            decimal next = decimal.Ceiling(question.Sort + 1);
            // 用户的答案数组
            var tmpU = answer.ToCharArray().Distinct().Except(CommonHelper.SPECIALCHARACTERS);
            var qamodel = question.QuestionAnswerList.OrderBy(x=>x.Sort)
                .FirstOrDefault(x => {
                    // 问题答案数组
                    var tmpQ = x.AnswerKeys.ToCharArray().Distinct().Except(CommonHelper.SPECIALCHARACTERS);
                    // 取相差数为0
                    return tmpQ.Except(tmpU).Count() == 0;
                });
            if (qamodel != null && qamodel.Next != null)
                next = qamodel.Next.Value;
            Question nextq = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, next);
            return nextq;
        }

        /// <summary>
        /// 简答题解析
        /// 解析用户回答，获取下一题
        /// </summary>
        /// <param name="uanswer">用户答题对象</param>
        /// <param name="question">问题对象</param>
        /// <param name="answer">用户答案文本</param>
        /// <returns>下一题</returns>
        private Question TextAnalysis(UserAnswer uanswer, Question question, string answer)
        {
            decimal next = decimal.Ceiling(question.Sort + 1);
            var qamodel = question.QuestionAnswerList.OrderBy(x=>x.Sort)
                .FirstOrDefault(x => {
                    // 问题答案数组
                    var tmpQ = x.AnswerKeys.Split('|');
                    // 用户回答到了这个点，记录并引导进入下一题
                    return tmpQ.Any(y => answer.IndexOf(y) >= 0);
                });
            if (qamodel != null && qamodel.Next != null)
                next = qamodel.Next.Value;
            Question nextq = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, next);
            return nextq;
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
            var uanswerdetail = uanswer.UserAnswerDetailList.FirstOrDefault(x => x.End == null);
            if(uanswerdetail == null)
            {
                uanswer.CompletedTime = DateTime.Now;
                this._userAnswerRepository.Update(uanswer);
                return null;
            }
            uanswerdetail.Answer = answer;
            uanswerdetail.End = DateTime.Now;

            // 获取当前问题
            var question = this._questionBankService.GetQuestion(uanswerdetail.UserAnswer.QuestionBank_Id, uanswerdetail.QuestionNo);
            Question nextQ = null;
            // 判断当前答案
            switch (question.AnswerType)
            {
                case AnswerType.SingleCheck:
                    nextQ = this.SingleCheckAnalysis(uanswer, question, answer);
                    break;
                case AnswerType.MultiCheck:
                    nextQ = this.MultiCheckAnalysis(uanswer, question, answer);
                    break;
                case AnswerType.Text:
                    nextQ = this.TextAnalysis(uanswer, question, answer);
                    break;
            }
            if(nextQ != null)
            {
                // 判断是否是最后一题
                switch (nextQ.AnswerType)
                {
                    case AnswerType.End:
                        uanswerdetail.Next = 0;
                        uanswer.CompletedTime = DateTime.Now;
                        break;
                    default:
                        uanswerdetail.Next = nextQ.Sort;
                        uanswer.UserAnswerDetailList.Add(new UserAnswerDetail()
                        {
                            QuestionNo = nextQ.Sort,
                            Start = DateTime.Now
                        });
                        break;
                }
                this._userAnswerRepository.Update(uanswer);
                return nextQ;
            }
            uanswerdetail.Next = 0;
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
