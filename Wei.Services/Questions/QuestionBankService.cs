using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Wei.Core;
using Wei.Core.Data;
using Wei.Core.Domain.Questions;
using Wei.Core.Domain.Users;
using Wei.Data;

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

        public IList<QuestionBank> Query()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 分页获取题卷
        /// </summary>
        /// <param name="filterlist"></param>
        /// <param name="sortlist"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="showDel"></param>
        /// <returns></returns>
        public IPagedList<QuestionBank> QueryByPaged(string title = null, int type = -1, int[] tags = null
            , int pageindex = 0, int pagesize = int.MaxValue, bool showDel = false)
        {
            var query = _questionBankRepository.Table;
            if (!showDel)
                query = query.Where(x => x.Status > -1);

            if (title != null)
                query = query.Where(x => x.Title.ToLower().IndexOf(title.ToLower()) != -1);

            if (type != -1)
                query = query.Where(x => x.Type == type);

            query = query.OrderByDescending(x => x.Id);
            //if(tags != null)
            //    query.Where(x=> x.)
            //if(filterlist != null && filterlist.Count > 0)
            //{
            //    foreach(var filter in filterlist)
            //    {
            //        query = filter.GetFiltered<QuestionBank>(query);
            //    }
            //}
            //if(sortlist != null && sortlist.Count > 0)
            //{
            //    IOrderedQueryable<QuestionBank> sortedlist = null;
            //    foreach (var sort in sortlist)
            //    {
            //        var tmp = sort.GetSorted<QuestionBank>(query);
            //        if (tmp != null)
            //            sortedlist = tmp;
            //    }
            //    if (sortlist != null)
            //        query = sortedlist;
            //}
            //else
            //{
            //    query = query.OrderBy(x => x.Id);
            //}

            var result = new PagedList<QuestionBank>(query, pageindex, pagesize);
            return result;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="entity"></param>
        public void CreateQuestionBank(QuestionBank entity)
        {
            if (entity == null)
                throw new System.ArgumentNullException("QuestionBank.CreateQuestionBank");
            this._questionBankRepository.Insert(entity);
        }

        public void UpdateQuestionBank(QuestionBank entity)
        {
            if (entity == null)
                throw new System.ArgumentNullException("QuestionBank.UpdateQuestionBank");
            this._questionBankRepository.Update(entity);
        }

        public void DeleteQuestionBank(int id)
        {
            var entity = this.GetQuestionBankById(id);
            if (entity == null)
                throw new System.ArgumentNullException("QuestionBank.DeleteQuestionBank");
            entity.Status = -1;
            this._questionBankRepository.Update(entity);
        }

        /// <summary>
        /// 获取题目
        /// 优先更新用户信息
        /// </summary>
        /// <param name="questionbankid"></param>
        /// <param name="questionno"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Question GetQuestion(int questionbankid, decimal questionno = 0, User user = null)
        {
            //var questionbank = this._questionBankRepository.GetById(questionbankid);
            //if (questionno == 0)
            //{
            //    //// 从用户属性获取
            //    //UserAttribute uattr = questionbank.UserAttributeList.FirstOrDefault(x =>
            //    //{
            //    //    // 用户填入该项信息
            //    //    if (!user.UserAttributeList.Any(y => y.UserAttribute.Name == x.Name))
            //    //        return true;
            //    //    if (user.UserAttributeList.Any(y => y.UserAttribute.Name == x.Name && string.IsNullOrEmpty(y.Value)))
            //    //        return true;
            //    //    return false;
            //    //});
            //    //if(uattr != null)
            //    //{
            //    //    return new Question()
            //    //    {
            //    //        Text = uattr.DisplayName,
            //    //        QuestionType = QuestionType.Text,
            //    //        AnswerType = AnswerType.Text,
            //    //        Remark = uattr.Name
            //    //    };
            //    //}
            //}
            // 从题库第一题开始答
            if(questionno == 0)
                questionno = 1;
            return this._questionRepository.Table.First(x => x.QuestionBank_Id == questionbankid && x.Sort == questionno);
        }

        public QuestionBank GetQuestionBankById(int id)
        {
            return this._questionBankRepository.GetById(id);
        }

        /// <summary>
        /// 获取有效的题库关键词
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, QuestionBank> KeyWordQuestionBank()
        {
            IDictionary<string, QuestionBank> result = new Dictionary<string, QuestionBank>();
            var list = _questionBankRepository.Table.Where(x => x.Status == 0 && x.AutoResponse != null && x.AutoResponse.Value
                && (x.ExpireDateBegin == null || x.ExpireDateBegin.Value.Date <= DateTime.Now.Date) 
                && (x.ExpireDateEnd == null || x.ExpireDateEnd.Value.Date >= DateTime.Now.Date));
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

        /// <summary>
        /// 导入问卷题目
        /// </summary>
        /// <param name="qlist"></param>
        /// <returns></returns>
        public void ImportQuestion(IList<Question> qlist)
        {
            _questionRepository.Insert(qlist);
        }
    }
}
