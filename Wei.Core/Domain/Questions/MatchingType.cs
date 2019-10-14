using System.ComponentModel;

namespace Wei.Core.Domain.Questions
{
    public enum MatchingType
    {
        /// <summary>
        /// 单结果匹配
        /// </summary>
        [Description("单结果匹配")]
        Single,

        /// <summary>
        /// 多结果匹配
        /// </summary>
        [Description("多结果匹配")]
        Multiple
    }
}
