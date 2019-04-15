using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Wei.Core;
using Wei.Services.Custom;
using Wei.Web.Framework.ApiControllers;
using Wei.Web.Framework.ExtJs;
using Wei.Web.API.Models.UserAnswer;

namespace Wei.Web.API.Controllers
{
    public class UserAnswerController : BaseApiController
    {
        private readonly IUserAnswerService _userAnswerService;
        private readonly HttpRequestBase _request;

        public UserAnswerController(IUserAnswerService userAnswerService
            , HttpRequestBase request)
        {
            this._userAnswerService = userAnswerService;
            this._request = request;
        }

        /// <summary>
        /// 获取用户答题信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetUserAnswerList(RequestPaging request)
        {
            IList<FilterModel> filterlist = null;
            string nickname = null;
            string title = null;
            int type = -1;
            //IList<SortModel> sortlist = null;
            if (!string.IsNullOrEmpty(request.Filter))
            {
                filterlist = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<FilterModel>>(request.Filter);
            }
            //if(!string.IsNullOrEmpty(request.Sort))
            //{
            //    sortlist = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SortModel>>(request.Sort);
            //}
            if (filterlist != null)
            {
                var useridFilter = filterlist.FirstOrDefault(x => string.Equals(x.Property.ToLower(), "nickname"));
                if (useridFilter != null)
                    nickname = useridFilter.Value.ToStringN();

                var titleFilter = filterlist.FirstOrDefault(x => string.Equals(x.Property.ToLower(), "title"));
                if (titleFilter != null)
                    title = titleFilter.Value.ToStringN();

                var typeFilter = filterlist.FirstOrDefault(x => string.Equals(x.Property.ToLower(), "type"));
                if (typeFilter != null)
                    type = typeFilter.Value.ToInt();
            }

            var table = _userAnswerService.QueryByPaged(nickname, title, type, request.Page - 1, request.Limit);

            var result = table.Select(x =>
            {
                return new UserAnswerViewModel()
                {
                    id = x.Id,
                    begintime = x.BeginTime,
                    completedtime = x.CompletedTime,
                    nickname = x.User.NickName,
                    user_id = x.User_Id,
                    questionbank_id = x.QuestionBank_Id, 
                    questionbanktype = x.QuestionBank.Type,
                    questionbank = x.QuestionBank.Title,
                    status = x.UserAnswerStatus.ToString()
                };
            }).ToList();
            return new
            {
                total = table.TotalCount,
                data = result
            };
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMessageExt Discard(UserAnswerDiscardRequestModel model)
        {
            if(model == null || model.ids.Length == 0)
            {
                return ResponseMessageExt.Error("参数错误");
            }
            foreach(var id in model.ids)
            {
                var useranswer = this._userAnswerService.GetUserAnswerById(id);
                if (useranswer == null)
                    continue;
                useranswer.UserAnswerStatus = Core.Domain.Custom.UserAnswerStatus.已作废;
                this._userAnswerService.Save(useranswer);
            }
            return ResponseMessageExt.Success();
        }
    }
}
