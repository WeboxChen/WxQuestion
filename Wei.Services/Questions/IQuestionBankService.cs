using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core;
using Wei.Core.Domain.Questions;
using Wei.Core.Domain.Users;

namespace Wei.Services.Questions
{
    public interface IQuestionBankService
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        IList<QuestionBank> Query();

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filterlist"></param>
        /// <param name="sortlist"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        IPagedList<QuestionBank> QueryByPaged(IList<FilterModel> filterlist = null, IList<SortModel> sortlist = null
            , int pageindex = 0, int pagesize = int.MaxValue, bool showDel = false);

        /// <summary>
        /// 获取题卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QuestionBank GetQuestionBankById(int id);

        /// <summary>
        /// 新建题卷
        /// </summary>
        /// <param name="entity"></param>
        void CreateQuestionBank(QuestionBank entity);

        /// <summary>
        /// 更新题卷
        /// </summary>
        /// <param name="entity"></param>
        void UpdateQuestionBank(QuestionBank entity);

        /// <summary>
        /// 删除题卷
        /// </summary>
        /// <param name="id"></param>
        void DeleteQuestionBank(int id);

        /// <summary>
        /// 根据题卷id和题目序号获取题目
        /// </summary>
        /// <param name="questionbankid"></param>
        /// <param name="questionno"></param>
        /// <returns></returns>
        Question GetQuestion(int questionbankid, decimal questionno = 0, User user = null);

        /// <summary>
        /// 获取默认的
        /// </summary>
        /// <returns></returns>
        IDictionary<string, QuestionBank> KeyWordQuestionBank();

        /// <summary>
        /// 导入问卷题目
        /// </summary>
        /// <param name="qlist"></param>
        /// <returns></returns>
        void ImportQuestion(IList<Question> qlist);
    }
}
