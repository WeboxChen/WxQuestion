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
        End,
        /// <summary>
        /// 提示
        /// </summary>
        [Description("提示")]
        Tips,
        /// <summary>
        /// 终止
        /// </summary>
        [Description("终止")]
        Break
    }
}
