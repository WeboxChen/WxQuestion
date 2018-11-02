using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wei.Web.Framework.ExtJs
{
    public class TreeRequest : RequestBase
    {

        /// <summary>
        /// 筛选条件
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        public string ExtraCols { get; set; }
        /// <summary>
        /// 视图名称
        /// </summary>
        public string ExtraView { get; set; }
        /// <summary>
        /// 标识parentid字段名
        /// </summary>
        public string F_ParentId { get; set; }
        /// <summary>
        /// 标识leaf字段名
        /// </summary>
        public string F_Leaf { get; set; }
        /// <summary>
        /// 标识expanded字段名
        /// </summary>
        public string F_Expanded { get; set; }
    }
}
