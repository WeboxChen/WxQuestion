using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wei.Web.Models.Users
{
    public class UserViewModel
    {
        private ICollection<UserAttributeValueViewModel> _userAttributeList;

        public string LoginName { get; set; }
        /// <summary>
        /// 姓氏
        /// </summary>
        [DisplayName("姓氏：")]
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入姓氏！")]
        public string FirstName { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [DisplayName("名字：")]
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入名字！")]
        public string LastName { get; set; }
        public string UserName { get { return FirstName + LastName; } }
        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayName("邮箱：")]
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入邮箱信息！")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "请输入正确的电子邮箱地址！")]
        public string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [DisplayName("电话：")]
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入电话！")]
        [StringLength(15, MinimumLength = 8)]
        public string Phone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        [DisplayName("QQ：")]
        [AllowHtml]
        [StringLength(20, MinimumLength = 5)]
        public string QQ { get; set; }
        public int IsAdmin { get; set; }
        public string OpenId { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DisplayName("性别：")]
        [Required]
        public int Sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        [DisplayName("语言：")]
        [AllowHtml]
        public string Language { get; set; }
        /// <summary>
        /// 出身日期
        /// </summary>
        [DisplayName("出身日期：")]
        [AllowHtml]
        [Required(ErrorMessage = "请输入出身日期！")]
        public DateTime Birthdate { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        [DisplayName("家庭地址：")]
        [AllowHtml]
        public string Address { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        [DisplayName("学历：")]
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入学历！")]
        public string Education { get; set; }
        /// <summary>
        /// 婚姻
        /// </summary>
        [DisplayName("婚姻状况：")]
        [Required]
        public bool Married { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        [DisplayName("身份证：")]
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入身份证！")]
        [StringLength(18)]
        public string IdentityCard { get; set; }
        

        /// <summary>
        /// 用户扩展数据
        /// </summary>
        public virtual ICollection<UserAttributeValueViewModel> UserAttributeList
        {
            get { return _userAttributeList ?? (_userAttributeList = new List<UserAttributeValueViewModel>()); }
            protected set { _userAttributeList = value; }
        }

        public bool Success { get; set; }
    }

    //public enum MarriedState
    //{
    //    mvc
    //}
}