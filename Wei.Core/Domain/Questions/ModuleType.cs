namespace Wei.Core.Domain.Questions
{
    public class ModuleType: BaseEntity
    {
        /// <summary>
        /// 模块类型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 模块备注
        /// </summary>
        public string Remark { get; set; }
    }
}
