using System;
using System.Collections.Generic;
using System.Linq;

namespace Wei.Core.Domain.Users
{
    public class User:BaseEntity
    {
        private ICollection<UserAttributeValue> _userAttributeList;
        private ICollection<Department> _departments;
        private ICollection<Role> _roles;

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 出身日期
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 注册渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        
        /// <summary>
        /// 最后登录Ip
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后操作时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// 状态 
        /// -1: 禁用
        /// 0: 未激活
        /// 1: 在使用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否是管理员  1:管理员 0:普通用户， 做快速区别
        /// </summary>
        public int IsAdmin { get; set; }

        /// <summary>
        /// 登录票据
        /// </summary>
        public string LoginTicket { get; set; }

        public virtual ICollection<UserAttributeValue> UserAttributeList
        {
            get { return _userAttributeList ?? (_userAttributeList = new List<UserAttributeValue>()); }
            protected set { _userAttributeList = value; }
        }

        public virtual ICollection<Department> Departments
        {
            get { return _departments ?? (_departments = new List<Department>()); }
            protected set { _departments = value; }
        }

        public virtual ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            protected set { _roles = value; }
        }

        public string UserName
        {
            get
            {
                string username = "";
                if (!string.IsNullOrEmpty(FirstName))
                    username += FirstName;
                if (!string.IsNullOrEmpty(LastName))
                    username += LastName;
                return username;
            }
        }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginsCount { get; set; }

        #region 微信扩展参数
        /// <summary>
        /// 是否已关注
        /// </summary>
        public int Subscribe { get; set; }
        /// <summary>
        /// 用户标识
        /// openid
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// sex  1:男  2:女  0:未知
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 头像图片
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public long Subscribe_Time { get; set; }
        /// <summary>
        /// 标识id
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 分组id
        /// </summary>
        public int? GroupId { get; set; }
        /// <summary>
        /// 用户标签id
        /// </summary>
        public string TagIds { get; set; }
        public int[] TagId_List
        {
            get { return TagIds.Split(',').Select(x => x.ToInt()).ToArray(); }
        }
        /// <summary>
        /// 用户关注渠道来源
        /// ADD_SCENE_SEARCH 公众号搜索
        /// ADD_SCENE_ACCOUNT_MIGRATION 公众号迁移，
        /// ADD_SCENE_PROFILE_CARD 名片分享，
        /// ADD_SCENE_QR_CODE 扫描二维码，
        /// ADD_SCENEPROFILE LINK 图文页内名称点击，
        /// ADD_SCENE_PROFILE_ITEM 图文页右上角菜单，
        /// ADD_SCENE_PAID 支付后关注，
        /// ADD_SCENE_OTHERS 其他
        /// </summary>
        public string Subscribe_Scene { get; set; }
        /// <summary>
        /// 二维码扫描场景
        /// </summary>
        public int? QR_Scene { get; set; }
        /// <summary>
        /// 二维码扫描场景描述
        /// </summary>
        public string QR_Scene_Str { get; set; }
        #endregion

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public bool? Married { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentityCard { get; set; }

        /// <summary>
        /// 详细住址
        /// </summary>
        public string Address { get; set; }
    }
}
