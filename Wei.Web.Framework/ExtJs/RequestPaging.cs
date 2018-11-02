namespace Wei.Web.Framework.ExtJs
{
    public class RequestPaging
    {

        /// <summary>
        /// 起始位置
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public int Dc { get; set; }
        /// <summary>
        /// 筛选条件
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        public string ExtraCols { get; set; }
        /// <summary>
        /// 视图名称
        /// </summary>
        public string ExtraView { get; set; }
        /// <summary>
        /// 筛选id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Direction { get; set; }
    }
}
