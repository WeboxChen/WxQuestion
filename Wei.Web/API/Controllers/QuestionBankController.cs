using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Wei.Core;
using Wei.Core.Domain.Questions;
using Wei.Services.Logging;
using Wei.Services.Questions;
using Wei.Services.Users;
using Wei.Web.API.Models.QuestionBank;
using Wei.Web.Framework.ApiControllers;
using Wei.Web.Framework.ExtJs;
using Wei.Web.Framework.NPOI;
using Wei.Web.Models;

namespace Wei.Web.API.Controllers
{
    public class QuestionBankController : BaseApiController
    {
        private readonly IQuestionBankService _questionBankService;
        private readonly IUserAttributeService _userAttributeService;
        private readonly HttpRequestBase _request;
        private readonly ILogger _logger;

        public QuestionBankController(IQuestionBankService questionBankService
            , IUserAttributeService userAttributeService
            , HttpRequestBase request
            , ILogger logger)
        {
            this._questionBankService = questionBankService;
            this._userAttributeService = userAttributeService;
            this._request = request;
            this._logger = logger;
        }

        /// <summary>
        /// 获取题卷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetQuestionBankList(RequestPaging request)
        {
            IList<FilterModel> filterlist = null;
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
            if(filterlist != null)
            {
                var titleFilter = filterlist.FirstOrDefault(x => string.Equals(x.Property.ToLower(), "title"));
                if (titleFilter != null)
                    title = titleFilter.Value.ToStringN();

                var typeFilter = filterlist.FirstOrDefault(x => string.Equals(x.Property.ToLower(), "type"));
                if (typeFilter != null)
                    type = typeFilter.Value.ToInt();
            }

            var table = _questionBankService.QueryByPaged(title, type, null, request.Page - 1, request.Limit);

            var result = table.Select(x =>
            {
                var entity = CommonHelper.InstanceBy<QuestionBankViewModel, QuestionBank>(x);
                entity.userattributes = string.Join(",", x.UserAttributeList.Select(ua => ua.Id.ToString()).ToArray());
                return entity;
            }).ToList();
            return new
            {
                total = table.TotalCount,
                data = result
            };
        }

        /// <summary>
        /// 新增题卷
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMessageExt CreateQuestionBank(object obj)
        {
            if(obj is JObject)
            {
                JObject jobj = obj as JObject;
                var qbviewmodel = jobj.ToObject<QuestionBankViewModel>();
                QuestionBank qbmodel = CommonHelper.InstanceBy<QuestionBank, QuestionBankViewModel>(qbviewmodel);
                if(qbviewmodel.userattributes != null && qbviewmodel.userattributes.Length > 0)
                {
                    string[] tmpArray = qbviewmodel.userattributes.Split(',');
                    foreach (var item in tmpArray)
                    {
                        var ua = this._userAttributeService.Get(item.ToInt());
                        qbmodel.UserAttributeList.Add(ua);
                    }
                }
                this._questionBankService.CreateQuestionBank(qbmodel);
            }
            else if(obj is JArray)
            {
                JArray jarray = obj as JArray;
                foreach(JObject jobj in jarray)
                {
                    var qbviewmodel = jobj.ToObject<QuestionBankViewModel>();
                    QuestionBank qbmodel = CommonHelper.InstanceBy<QuestionBank, QuestionBankViewModel>(qbviewmodel);
                    if (qbviewmodel.userattributes != null && qbviewmodel.userattributes.Length > 0)
                    {
                        string[] tmpArray = qbviewmodel.userattributes.Split(',');
                        foreach (var item in tmpArray)
                        {
                            var ua = this._userAttributeService.Get(item.ToInt());
                            qbmodel.UserAttributeList.Add(ua);
                        }
                    }
                    this._questionBankService.CreateQuestionBank(qbmodel);
                }
            }
            return ResponseMessageExt.Success();
        }

        /// <summary>
        /// 修改题卷
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMessageExt UpdateQuestionBank(object obj)
        {
            if (obj is JObject)
            {
                JObject jobj = obj as JObject;
                var qbviewmodel = jobj.ToObject<QuestionBankViewModel>(); 
                QuestionBank qbmodel = this._questionBankService.GetQuestionBankById(qbviewmodel.id);
                if (qbmodel == null)
                    return ResponseMessageExt.Error("参数错误！");
                CommonHelper.UpdateT<QuestionBank>(qbmodel, jobj);
                if (qbviewmodel.userattributes != null && qbviewmodel.userattributes.Length > 0)
                {
                    string[] tmpArray = qbviewmodel.userattributes.Split(',');
                    foreach (var item in tmpArray)
                    {
                        int tmpid = item.ToInt();
                        if (qbmodel.UserAttributeList.Any(x => x.Id == tmpid))
                            continue;
                        var ua = this._userAttributeService.Get(tmpid);
                        qbmodel.UserAttributeList.Add(ua);
                    }
                }
                this._questionBankService.UpdateQuestionBank(qbmodel);
            }
            else if (obj is JArray)
            {
                JArray jarray = obj as JArray;
                foreach (JObject jobj in jarray)
                {
                    var qbviewmodel = jobj.ToObject<QuestionBankViewModel>();
                    QuestionBank qbmodel = this._questionBankService.GetQuestionBankById(qbviewmodel.id);
                    if (qbmodel == null)
                        return ResponseMessageExt.Error("参数错误！");
                    CommonHelper.UpdateT<QuestionBank>(qbmodel, jobj);
                    if (qbviewmodel.userattributes != null && qbviewmodel.userattributes.Length > 0)
                    {
                        string[] tmpArray = qbviewmodel.userattributes.Split(',');
                        foreach (var item in tmpArray)
                        {
                            int tmpid = item.ToInt();
                            if (qbmodel.UserAttributeList.Any(x => x.Id == tmpid))
                                continue;
                            var ua = this._userAttributeService.Get(tmpid);
                            qbmodel.UserAttributeList.Add(ua);
                        }
                    }
                    this._questionBankService.UpdateQuestionBank(qbmodel);
                }
            }
            return ResponseMessageExt.Success();
        }


        [HttpPost]
        public ResponseMessageExt DestroyQuestionBank(object obj)
        {
            if (obj is JObject)
            {
                JObject jobj = obj as JObject;
                var id = jobj.GetValue("id").ToInt();
                if (id <= 0)
                    return ResponseMessageExt.Error("参数错误！");
                this._questionBankService.DeleteQuestionBank(id);

            }
            else if (obj is JArray)
            {
                JArray jarray = obj as JArray;
                foreach (JObject jobj in jarray)
                {
                    var id = jobj.GetValue("id").ToInt();
                    if (id <= 0)
                        return ResponseMessageExt.Error("参数错误！");
                    this._questionBankService.DeleteQuestionBank(id);
                }
            }
            return ResponseMessageExt.Success();
        }

        /// <summary>
        /// 导入题卷数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseMessageExt QuestionBankImport()
        {
            int qbid;
            if(!int.TryParse(_request.Form["QuestionBank_Id"], out qbid))
            {
                return Framework.ExtJs.ResponseMessageExt.Error("参数错误！");
            }
            QuestionBank qbank = _questionBankService.GetQuestionBankById(qbid);
            if(qbank == null || _request.Files.Count == 0)
            {
                return Framework.ExtJs.ResponseMessageExt.Error("数据错误！");
            }
            var file = _request.Files[0];
            ExcelReader ereader = new ExcelReader(file.InputStream);

            string msg;
            var questionlist = ereader.Read<Question>("Questions", "Q_Question", out msg);
            var answerlist = ereader.Read<QuestionAnswer>("Answers", "Q_QuestionAnswer", out msg);
            foreach(var q in questionlist)
            {
                q.QuestionBank_Id = qbid;
                q.QType = default(QuestionType).ToString();
                q.AType = CommonHelper.GetEnumValueByDesc<AnswerType>(q.AType).ToString();
                q.QuestionAnswerList = answerlist.Where(x => x.Sort == q.Sort).ToList();
            }
            _questionBankService.ImportQuestion(questionlist);
            return Wei.Web.Framework.ExtJs.ResponseMessageExt.Success();
        }

    }
}
