using System;

namespace Wei.Core
{
    public class CustSession<T> // where T : BaseEntity
    {
        /// <summary>
        /// 对象信息
        /// </summary>
        public T Obj { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 有效时长
        /// </summary>
        public DateTime EffectiveTime { get; set; }
    }
}
