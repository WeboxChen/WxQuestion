using System.ComponentModel;

namespace Wei.Core.Domain.Questions
{
    /// <summary>
    /// 问答类型
    /// </summary>
    public enum AnswerType
    {
        /// <summary>
        /// 单选
        /// </summary>
        [Description("单选")]
        SingleCheck,
        /// <summary>
        /// 多选
        /// </summary>
        [Description("多选")]
        MultiCheck,
        /// <summary>
        /// 简答
        /// </summary>
        [Description("简答")]
        Text,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        End
    }
}
