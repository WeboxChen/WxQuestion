using System;
using Wei.Web.Models;

namespace Wei.Web.API.Models.QuestionBank
{
    public class QuestionBankViewModel : BaseViewModel
    {
        public int id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 自动响应
        /// </summary>
        public bool? autoresponse { get; set; }
        /// <summary>
        /// 自动响应语句，多条用“|”隔开
        /// </summary>
        public string responsekeywords { get; set; }
        /// <summary>
        /// 状态 -1：删除 0：禁用 1：启用
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 有效起始日期
        /// </summary>
        public DateTime? expiredatebegin { get; set; }
        /// <summary>
        /// 有效结束日期
        /// </summary>
        public DateTime? expiredateend { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int creatorid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string creatorname { get; set; }

        /// <summary>
        /// 题卷必须的用户属性
        /// </summary>
        public string userattributes { get; set; }
        
        
    }
}