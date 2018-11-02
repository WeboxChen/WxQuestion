namespace Wei.Core.Domain.Users
{
    public class Function : BaseEntity
    {


        /// <summary>
        /// 权限名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public string Code { get; set; }
        

        public int Sort { get; set; }

        public int Status { get; set; }
    }
}
