using System;

namespace Wei.Core.Domain.Custom
{
    /// <summary>
    /// 用户答题明细，多媒体答案记录
    /// </summary>
    public class UserAnswerDetail_Media: BaseEntity
    {
        /// <summary>
        /// 用户答题明细
        /// </summary>
        public int UserAnswerDetail_Id { get; set; }
        public virtual UserAnswerDetail UserAnswerDetail { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public int MediaTypeId { get; set; }
        public MediaType MediaType
        {
            get { return (MediaType)MediaTypeId; }
            set { MediaTypeId = (int)value; }
        }

        /// <summary>
        /// 多媒体文件
        /// </summary>
        public string MediaContent { get; set; }

        /// <summary>
        /// 文本描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
