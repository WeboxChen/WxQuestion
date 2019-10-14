using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wei.Core;
using Wei.Core.Caching;
using Wei.Core.Configuration;
using Wei.Core.Data;
using Wei.Core.Domain.Custom;
using Wei.Core.Domain.Questions;
using Wei.Core.Domain.Users;
using Wei.Services.Events;
using Wei.Services.Logging;
using Wei.Services.Questions;
using Wei.Services.Sys;
using Wei.Services.Users;

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
        private readonly IRepository<UserAnswerDetail_Media> _userAnswerDetailMediaRepository;
        private readonly IRepository<UserAnswerQueue> _userAnswerQueueRepository;
        private readonly IPropertyService _propertyService;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IQuestionBankService _questionBankService;
        private readonly IUserAttributeService _userAttributeService;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly WeiConfig _weiConfig;
        #endregion

        #region Ctor
        public UserAnswerService(IRepository<UserAnswer> userAnswerRepository
            , IRepository<UserAnswerDetail> userAnswerDetailRepository
            , IRepository<UserAnswerDetail_Media> userAnswerDetailMediaRepository
            , IRepository<UserAnswerQueue> userAnswerQueueRepository
            , IPropertyService propertyService
            , ICacheManager cacheManager
            , IEventPublisher eventPublisher
            , IQuestionBankService questionBankService
            , IUserAttributeService userAttributeService
            , IWorkContext workContext
            , IUserService userService
            , ILogger logger
            , WeiConfig weiConfig)
        {
            this._userAnswerRepository = userAnswerRepository;
            this._userAnswerDetailRepository = userAnswerDetailRepository;
            this._userAnswerDetailMediaRepository = userAnswerDetailMediaRepository;
            this._userAnswerQueueRepository = userAnswerQueueRepository;
            this._propertyService = propertyService;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._questionBankService = questionBankService;
            this._userAttributeService = userAttributeService;
            this._userService = userService;
            this._workContext = workContext;
            this._logger = logger;
            this._weiConfig = weiConfig;
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
        private WeiExecuteResult SingleCheckAnalysis(UserAnswerDetail uanswer, Question question, UserAnswerDetail nuseranswerdetail)
        {
            decimal? next = null; // decimal.Floor(question.Sort + 1);
            var qalist = question.QuestionAnswerList.OrderBy(x => x.Sort);

            string answer = DataConvertHelper.ChineseNumberAnalysis(uanswer.Answer).ToLower();
            if (!question.QuestionItemList.Any(x => answer.IndexOf(x.Code.ToLower()) != -1))
                return WeiExecuteResultHelper.Failed(this._propertyService.GetValue("Answer.NotConsistent"));

            string keycode = null;
            QuestionAnswer qamodel = null;
            foreach(var qanswer in qalist)
            {
                var tmpQ = qanswer.AnswerKeys.ToLower().Split('|');
                keycode = tmpQ.FirstOrDefault(y =>
                {
                    if (string.IsNullOrEmpty(y))
                        return false;
                    if (y == "*")
                        return true;

                    var tmp2 = y.ToLower().Split('&');
                    bool result = !tmp2.Any(z => answer.IndexOf(z) == -1);
                    return result;
                });
                if (!string.IsNullOrEmpty(keycode))
                {
                    qamodel = qanswer;
                    break;
                }
            }

            //if (qamodel == null)
            //{
            //    return WeiExecuteResultHelper.Failed(this._propertyService.GetValue("Answer.NotConsistent"));
            //}

            if(qamodel != null)
            {
                nuseranswerdetail.QCode = keycode;
                //if (qamodel.Next != null)
                next = qamodel.Next;
            }
            else
            {
                next = decimal.Floor(question.Sort + 1);
            }

            return WeiExecuteResultHelper.Success(null, next);
        }

        /// <summary>
        /// 多选题解析
        /// 解析用户回答，获取下一题
        /// </summary>
        /// <param name="uanswer">用户答题对象</param>
        /// <param name="question">问题对象</param>
        /// <param name="answer">用户答案文本</param>
        /// <returns>下一题</returns>
        private WeiExecuteResult MultiCheckAnalysis(UserAnswerDetail uanswer, Question question, UserAnswerDetail nuseranswerdetail)
        {
            decimal? next = null;// decimal.Floor(question.Sort + 1);
            // 用户的答案数组
            //var tmpU = uanswer.Answer.ToCharArray().Distinct().Except(CommonHelper.SPECIALCHARACTERS);
            var qalist = question.QuestionAnswerList.OrderBy(x => x.Sort);

            string answer = DataConvertHelper.ChineseNumberAnalysis(uanswer.Answer).ToLower();
            if (!question.QuestionItemList.Any(x => answer.IndexOf(x.Code.ToLower()) != -1))
                return WeiExecuteResultHelper.Failed(this._propertyService.GetValue("Answer.NotConsistent"));

            string keycode = null;
            QuestionAnswer qamodel = null;

            switch (question.MatchingType)
            {
                case MatchingType.Multiple:
                    {
                        foreach(var qanswer in qalist)
                        {
                            var tmpQ = qanswer.AnswerKeys.ToLower().Split('|');

                            keycode = tmpQ.FirstOrDefault(y =>
                            {
                                if (string.IsNullOrEmpty(y))
                                    return false;
                                if (y == "*")
                                    return true;

                                var tmp2 = y.ToLower().Split('&');
                                bool result = !tmp2.Any(z => answer.IndexOf(z) == -1);
                                return result;
                            });
                            if (!string.IsNullOrEmpty(keycode))
                            {
                                UserAnswerQueue uaqueue = new UserAnswerQueue()
                                {
                                    QuestionAnswer_Id = qanswer.Id,
                                    UserAnswer_Id = uanswer.UserAnswer_Id,
                                    Status = 0,
                                    QCode = keycode
                                };
                                this._userAnswerQueueRepository.Insert(uaqueue);
                            }
                        }
                    }
                    break;
                default:
                    {
                        foreach (var qanswer in qalist)
                        {
                            var tmpQ = qanswer.AnswerKeys.ToLower().Split('|');

                            keycode = tmpQ.FirstOrDefault(y =>
                            {
                                if (string.IsNullOrEmpty(y))
                                    return false;
                                if (y == "*")
                                    return true;

                                var tmp2 = y.ToLower().Split('&');
                                bool result = !tmp2.Any(z => answer.IndexOf(z) == -1);
                                return result;
                            });
                            if (!string.IsNullOrEmpty(keycode))
                            {
                                qamodel = qanswer;
                                break;
                            }
                        }
                    }
                    break;
            }

            //if (qamodel == null)
            //{
            //    return WeiExecuteResultHelper.Failed(this._propertyService.GetValue("Answer.NotConsistent"));
            //}
            if (qamodel != null)
            {
                nuseranswerdetail.QCode = keycode;
                //if (qamodel.Next != null)
                next = qamodel.Next;
            }
            else
            {
                next = decimal.Floor(question.Sort + 1);
            }

            //nuseranswerdetail.QCode = keycode;
            //next = qamodel.Next;

            return WeiExecuteResultHelper.Success(null, next);
        }

        /// <summary>
        /// 简答题解析
        /// 解析用户回答，获取下一题
        /// </summary>
        /// <param name="uanswer">用户答题对象</param>
        /// <param name="question">问题对象</param>
        /// <param name="answer">用户答案文本</param>
        /// <returns>下一题 (-2：挂起; -1：重新回答; 0：不做反馈; >0：获取题目)</returns>
        private WeiExecuteResult TextAnalysis(UserAnswerDetail uanswer, Question question, UserAnswerDetail nuseranswerdetail)
        {
            decimal? next = null;
            string answer = uanswer.Answer.ToLower();
            var qalist = question.QuestionAnswerList.OrderBy(x => x.Sort);
            string keycode = null;
            QuestionAnswer qamodel = null;

            switch (question.MatchingType)
            {
                case MatchingType.Multiple:
                    {
                        foreach (var qanswer in qalist)
                        {
                            var tmpQ = qanswer.AnswerKeys.ToLower().Split('|');
                            keycode = tmpQ.FirstOrDefault(y =>
                            {
                                if (string.IsNullOrEmpty(y) || y == "*")
                                    return true;

                                var tmp2 = y.ToLower().Split('&');
                                bool result = !tmp2.Any(z => answer.IndexOf(z) == -1);
                                return result;
                            });
                            if (!string.IsNullOrEmpty(keycode))
                            {
                                UserAnswerQueue uaqueue = new UserAnswerQueue()
                                {
                                    QuestionAnswer_Id = qanswer.Id,
                                    UserAnswer_Id = uanswer.UserAnswer_Id,
                                    Status = 0,
                                    QCode = keycode
                                };
                                this._userAnswerQueueRepository.Insert(uaqueue);
                            }
                        }
                    }
                    break;
                default:
                    {
                        foreach (var qanswer in qalist)
                        {
                            var tmpQ = qanswer.AnswerKeys.ToLower().Split('|');
                            keycode = tmpQ.FirstOrDefault(y =>
                            {
                                if (string.IsNullOrEmpty(y) || y == "*")
                                    return true;

                                var tmp2 = y.ToLower().Split('&');
                                bool result = !tmp2.Any(z => answer.IndexOf(z) == -1);
                                return result;
                            });
                            if (!string.IsNullOrEmpty(keycode))
                            {
                                qamodel = qanswer;
                                break;
                            }
                        }
                    }
                    break;
            }
            
            if (qamodel != null)
            {
                nuseranswerdetail.QCode = keycode;
                next = qamodel.Next;
            }
            return WeiExecuteResultHelper.Success(null, next);
        }

        /// <summary>
        /// 提示符答题
        /// </summary>
        /// <param name="uanswer"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        private WeiExecuteResult TipsAnalysis(UserAnswerDetail uanswer, Question question, UserAnswerDetail nuseranswerdetail)
        {
            //decimal next = decimal.Floor(question.Sort + 1);
            //if (this._propertyService.CheckCodeValue("Answer.Suspend", uanswer.Answer)) // (string.Equals(uanswer.Answer, "终止") 
            //    //|| string.Equals(uanswer.Answer, "N", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    uanswer.UserAnswer.UserAnswerStatus = UserAnswerStatus.挂起;
            //    return WeiExecuteResultHelper.Failed(this._propertyService.GetValue("Answer.SuspendTips"));
            //}
            //if (this._propertyService.CheckCodeValue("Answer.Continue", uanswer.Answer)) // (string.Equals(uanswer.Answer, "继续")
            //   // || string.Equals(uanswer.Answer, "Y", StringComparison.CurrentCultureIgnoreCase))
            //{
            return WeiExecuteResultHelper.Success();
            //}
            //return WeiExecuteResultHelper.Failed(this._propertyService.GetValue("Answer.NotConsistent"));
        }

        /// <summary>
        /// 根据当前题目和用户回答后解析的题号，跳转下一题题目
        /// </summary>
        /// <param name="currentquestion"></param>
        /// <param name="next"></param>
        private Question NextQuestion(UserAnswer uanswer, Question currentquestion = null, decimal? next = null, UserAnswerDetail nuseranswerdetail = null)
        {
            decimal tmp = 1;
            if(currentquestion != null)
                tmp = decimal.Floor(currentquestion.Sort + 1);
            
            Question tmpQ = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, tmp);

            while (tmpQ != null && tmpQ.AnswerType == AnswerType.Tips)
            {

                // 发送消息
                //var msgtask = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(_weiConfig.WeixinAppId, this._workContext.CurrentUser.OpenId, tmpQ.Text);
                var result = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(WXinConfig.WeixinAppId, this._workContext.CurrentUser.OpenId, tmpQ.Text);
                if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                {
                    this._logger.Warning(result.errcode.ToString());
                }
                // 发送完成暂停0.5秒
                //msgtask.Start();
                // 调整下一题
                tmp = decimal.Floor(tmp + 1);
                tmpQ = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, tmp);
            }
            // 判断是否为组合题目
            if (currentquestion != null && !string.IsNullOrEmpty(currentquestion.GroupNo))
            {
                if(tmpQ != null)
                {
                    if(string.Equals(tmpQ.GroupNo, currentquestion.GroupNo))
                    {
                        return tmpQ;
                    }
                }
            }

            // 如果有下一题
            if(next != null && next != 0)
            {
                return this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, next.Value);
            }

            // 从队列获取下一题
            UserAnswerQueue queue = uanswer.UserAnswerQueueList.OrderBy(x=>x.Id).FirstOrDefault(x => x.Status == 0);
            if (queue != null && queue.QuestionAnswer.Next != null)
            {
                tmpQ = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, queue.QuestionAnswer.Next.Value);
                nuseranswerdetail.QCode = queue.QCode;

                queue.Status = 1;
                this._userAnswerQueueRepository.Update(queue);
            }

            // 判断组合
            while(tmpQ != null && !string.IsNullOrEmpty(tmpQ.GroupNo))
            {
                var prevQ = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, tmpQ.Sort - 1);
                if(prevQ != null && !string.IsNullOrEmpty(prevQ.GroupNo) && string.Equals(prevQ.GroupNo, tmpQ.GroupNo))
                {
                    tmpQ = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, tmpQ.Sort + 1);
                }
                else
                {
                    break;
                }
            }
            return tmpQ;
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
        /// 分页获取数据
        /// </summary>
        /// <param name="filterlist"></param>
        /// <param name="sortlist"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IPagedList<UserAnswer> QueryByPaged(string nickname = null, string title = null, int type = -1
            , int pageindex = 0, int pagesize = int.MaxValue)
        {
            var query = _userAnswerRepository.Table;

            if (nickname != null)
                query = query.Where(x => x.User.NickName.ToLower().IndexOf(nickname.ToLower()) != -1);

            if (title != null)
                query = query.Where(x => x.QuestionBank.Title.ToLower().IndexOf(title.ToLower()) != -1);

            if (type != -1)
                query = query.Where(x => x.QuestionBank.Type == type);

            query = query.OrderByDescending(x => x.Id);

            var result = new PagedList<UserAnswer>(query, pageindex, pagesize);
            return result;
        }

        /// <summary>
        /// 开始答题
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="questionbankid"></param>
        /// <returns></returns>
        public Question BeginQuestion(int userid, int questionbankid)
        {
            if (GetDoingUserAnswer(userid) != null)
                throw new WeiException("当前有未完成的题卷！");
            UserAnswer ua = new UserAnswer()
            {
                User_Id = userid,
                QuestionBank_Id = questionbankid,
                BeginTime = DateTime.Now
            };
            var question = this.NextQuestion(ua);

            UserAnswerDetail uanswerdetail = new UserAnswerDetail()
            {
                QuestionNo = question.Sort,
                Start = DateTime.Now
            };
            ua.UserAnswerDetailList.Add(uanswerdetail);
            this._userAnswerRepository.Insert(ua);
            return question;
        }

        /// <summary>
        /// 继续答题
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="qanswer"></param>
        /// <returns></returns>
        public Question BeginQuestion(User user, UserAnswer uanswer)
        {
            uanswer.UserAnswerStatus = UserAnswerStatus.答题中;
            // 获取当前的答题

            var uanswerdetail = uanswer.UserAnswerDetailList.FirstOrDefault(x => x.End == null);
            if (uanswerdetail == null)
            {
                uanswer.CompletedTime = DateTime.Now;
                uanswer.UserAnswerStatus = UserAnswerStatus.答题完成;
                this._userAnswerRepository.Update(uanswer);
                return new Question()
                {
                    Text = this._propertyService.GetValue("Answer.Complete"),
                    AnswerType = AnswerType.End
                };
            }
            // 获取当前题目
            var question = this._questionBankService.GetQuestion(uanswerdetail.UserAnswer.QuestionBank_Id, uanswerdetail.QuestionNo);

            // 如果为提示类型，自动跳过
            if(question.AnswerType == AnswerType.Tips)
            {
                question = SaveAnswer(uanswer, "Y", user);
            }

            return question;
        }

        /// <summary>
        /// 获取正在做的题卷 (进行中的答题或挂起的答题)
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public UserAnswer GetDoingUserAnswer(int userid)
        {
            return this._userAnswerRepository.Table.FirstOrDefault(x => x.User_Id == userid && x.CompletedTime == null 
                && (x.Status == (int)UserAnswerStatus.答题中));
        }

        /// <summary>
        /// 获取最近一次的答题记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public UserAnswer GetLastUserAnswer(int userid)
        {
            return (from t in this._userAnswerRepository.Table where t.User_Id == userid orderby t.Id descending select t).FirstOrDefault();
        }

        /// <summary>
        /// 根据用户，题库id获取用户答题数据
        /// 排除作废的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        public UserAnswer GetUserAnswerByQBId(int userid, int qid)
        {
            return this._userAnswerRepository.Table.FirstOrDefault(x =>
                x.User_Id == userid && x.QuestionBank_Id == qid && x.Status != (int)UserAnswerStatus.已作废
            );
        }

        /// <summary>
        /// 保存答案并获取下一题
        /// </summary>
        /// <param name="uanswer"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public Question SaveAnswer(UserAnswer uanswer, string answer, User user, MediaType mediaType = MediaType.None, string mediaContent = null)
        {
            var uanswerdetail = uanswer.UserAnswerDetailList.FirstOrDefault(x => x.End == null);
            if (uanswerdetail == null)
            {
                uanswer.CompletedTime = DateTime.Now;
                uanswer.UserAnswerStatus = UserAnswerStatus.答题完成;
                this._userAnswerRepository.Update(uanswer);
                return new Question()
                {
                    Text = this._propertyService.GetValue("Answer.Complete"),
                    AnswerType = AnswerType.End
                };
            }
            // 超过两小时，自动挂起
            if(DateTime.Now.AddHours(-2) > uanswerdetail.Start)
            {
                uanswer.UserAnswerStatus = UserAnswerStatus.挂起;
                this._userAnswerRepository.Update(uanswer);
                return new Question()
                {
                    AnswerType = AnswerType.Text,
                    Text = this._propertyService.GetValue("Answer.Timeout")
                };
            }
            // 获取当前题目
            var question = this._questionBankService.GetQuestion(uanswerdetail.UserAnswer.QuestionBank_Id, uanswerdetail.QuestionNo);
            var nuanswerdetail = new UserAnswerDetail();

            //decimal nextQ = 0;
            WeiExecuteResult analysisQResult = null;
            // 文本答题，判断是否为下一题
            if (question.AnswerType == AnswerType.Text && this._propertyService.CheckCodeValue("Answer.Next", answer)) // (answer.Length < 20 && answer.IndexOf("下一题") != -1))
            {
                // 文本题，跳转下一题
                uanswerdetail.End = DateTime.Now;
                analysisQResult = this.TextAnalysis(uanswerdetail, question, nuanswerdetail);
            }
            else
            {
                // 保存语音
                if (mediaType != MediaType.None)
                {
                    // 写入多媒体答案
                    var mediaObj = new UserAnswerDetail_Media()
                    {
                        MediaType = mediaType,
                        CreateTime = DateTime.Now,
                        MediaContent = mediaContent,
                        Text = answer,
                        UserAnswerDetail_Id = uanswerdetail.Id
                    };
                    this.SaveUserAnswerDetailMedia(mediaObj);
                }

                answer = answer.ToLower();
                // 判断当前答案
                switch (question.AnswerType)
                {
                    case AnswerType.SingleCheck:
                        uanswerdetail.Answer = answer;
                        analysisQResult = this.SingleCheckAnalysis(uanswerdetail, question, nuanswerdetail);
                        break;
                    case AnswerType.MultiCheck:
                        uanswerdetail.Answer = answer;
                        analysisQResult = this.MultiCheckAnalysis(uanswerdetail, question, nuanswerdetail);
                        break;
                    case AnswerType.Tips:
                        analysisQResult = this.TipsAnalysis(uanswerdetail, question, nuanswerdetail);
                        break;
                    default:
                        if (uanswerdetail.Answer == null)
                            uanswerdetail.Answer = answer;
                        else
                            uanswerdetail.Answer += answer;
                        analysisQResult = WeiExecuteResultHelper.Failed(this._propertyService.GetValue("Answer.TextTips"));
                        break;
                }
            }

            Question nextqObj = null;
            if (analysisQResult.Success)
            {
                decimal nextqno = analysisQResult.Obj.ToDecimal();
                nextqObj = this.NextQuestion(uanswer, question, nextqno, nuanswerdetail);
            }
            else
            {
                nextqObj = new Question()
                {
                    AnswerType = AnswerType.Text,
                    Text = analysisQResult.Message
                };
            }
            //switch (nextQ)
            //{
            //    case 0:
            //        nextqObj = new Question()
            //        {
            //            AnswerType = AnswerType.Text,
            //            Text = "您可以继续追加回答，或者告诉我“下一题”保存答案！"
            //        };
            //        break;
            //    case -1:
            //        nextqObj = new Question()
            //        {
            //            AnswerType = AnswerType.Text,
            //            Text = "您的回答似乎不符，请尝试重新作答！"
            //        };
            //        break;
            //    case -2:
            //        uanswer.UserAnswerStatus = UserAnswerStatus.挂起;
            //        nextqObj = new Question()
            //        {
            //            AnswerType = AnswerType.Text,
            //            Text = $"答题已暂停，在有效期内（{uanswer.QuestionBank.ExpireDateEnd.Value.ToString("yyyyMMdd")}）对我说题卷关键字可以继续作答。谢谢您的参与！"
            //        };
            //        break;
            //    default:
            //        nextqObj = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, nextQ);
            //        break;
            //}
            
            if (nextqObj != null)
            {
                // 判断是否是最后一题
                switch (nextqObj.AnswerType)
                {
                    case AnswerType.End:
                        uanswerdetail.Next = 0;
                        nuanswerdetail.End = DateTime.Now;
                        uanswer.CompletedTime = DateTime.Now;
                        uanswer.UserAnswerStatus = UserAnswerStatus.答题完成;
                        break;
                    case AnswerType.Break:
                        uanswer.CompletedTime = DateTime.Now;
                        nuanswerdetail.End = DateTime.Now;
                        uanswer.UserAnswerStatus = UserAnswerStatus.已作废;
                        break;
                    //case AnswerType.Tips:
                    //    Task task = new Task(() =>
                    //    {
                    //        _logger.Information("before begin task");
                    //        System.Threading.Thread.Sleep(500);
                    //        _logger.Information("begin task");
                    //        try
                    //        {
                    //            SaveAnswer(uanswer, "", user);
                    //        }
                    //        catch(Exception ex)
                    //        {
                    //            _logger.Error(ex.Message, ex);
                    //        }
                    //        _logger.Information("begin task");
                    //    });
                    //    task.Start();
                    //    break;
                    default:
                        if(nextqObj.Id > 0)
                        {
                            uanswerdetail.Next = nextqObj.Sort;
                            uanswerdetail.End = DateTime.Now;

                            nuanswerdetail.QuestionNo = nextqObj.Sort;
                            nuanswerdetail.Start = DateTime.Now;
                            uanswer.UserAnswerDetailList.Add(nuanswerdetail);
                        }
                        break;
                }
                this._userAnswerRepository.Update(uanswer);
                return nextqObj;
            }
            return null;
        }

        /// <summary>
        /// 保存图片答案
        /// </summary>
        /// <param name="uanswer"></param>
        /// <param name="user"></param>
        /// <param name="mediaContent"></param>
        /// <returns></returns>
        public Question SaveAnswerImage(UserAnswer uanswer, User user, string mediaContent = null)
        {
            var uanswerdetail = uanswer.UserAnswerDetailList.FirstOrDefault(x => x.End == null);
            if (uanswerdetail == null)
            {
                uanswer.CompletedTime = DateTime.Now;
                uanswer.UserAnswerStatus = UserAnswerStatus.答题完成;
                this._userAnswerRepository.Update(uanswer);
                return new Question()
                {
                    Text = this._propertyService.GetValue("Answer.Complete"),
                    AnswerType = AnswerType.End
                };
            }
            var question = this._questionBankService.GetQuestion(uanswerdetail.UserAnswer.QuestionBank_Id, uanswerdetail.QuestionNo);
            var media = new UserAnswerDetail_Media()
            {
                MediaType = MediaType.Image,
                CreateTime = DateTime.Now,
                MediaContent = mediaContent,
                Text = ""
            };
            // 写入多媒体答案
            uanswerdetail.UserAnswerDetailMediaList.Add(media);
            this._userAnswerRepository.Update(uanswer);
            return new Question()
            {
                AnswerType = AnswerType.Text,
                Text = ""
            };
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

        /// <summary>
        /// 是否答过题卷
        /// </summary>
        /// <param name="bankid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsAnswered(int bankid, User user)
        {
            if (this._userAnswerRepository.Table.Any(x => x.Status != (int)UserAnswerStatus.已作废 && x.User_Id == user.Id && x.QuestionBank_Id == bankid))
                return true;
            return false;
        }

        /// <summary>
        /// 保存用户答案
        /// </summary>
        /// <param name="uanswer"></param>
        public void Save(UserAnswer uanswer)
        {
            if (uanswer.Id == 0)
            {
                this._userAnswerRepository.Insert(uanswer);
            }
            else
            {
                this._userAnswerRepository.Update(uanswer);
            }
        }
        #endregion

        /// <summary>
        /// 保存多媒体文件
        /// </summary>
        /// <param name="media"></param>
        public void SaveUserAnswerDetailMedia(UserAnswerDetail_Media media)
        {
            _userAnswerDetailMediaRepository.Insert(media);
        }

    }
}
