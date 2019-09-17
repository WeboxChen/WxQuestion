using System.ComponentModel;

namespace Wei.Core.Domain.Custom
{
    /// <summary>
    /// 多媒体文件类型
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// 空
        /// </summary>
        [Description("无")]
        None,
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        Image,
        /// <summary>
        /// 语音
        /// </summary>
        [Description("语音")]
        Voice,
        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        Video
    }
}
