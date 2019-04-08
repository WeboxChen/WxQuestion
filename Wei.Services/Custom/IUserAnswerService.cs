﻿using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using Wei.Core;
using Wei.Core.Domain.Custom;
using Wei.Core.Domain.Questions;
using Wei.Core.Domain.Users;

namespace Wei.Services.Custom
{
    public interface IUserAnswerService
    {
        /// <summary>
        /// 获取所有答题记录
        /// </summary>
        /// <returns></returns>
        IList<UserAnswer> QueryAll();

        /// <summary>
        /// 根据条件获取答题信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="questionbankid"></param>
        /// <param name="begintime"></param>
        /// <param name="completedtime"></param>
        /// <returns></returns>
        IList<UserAnswer> QueryByUser(int userid, int questionbankid = 0
            , DateTime begintime = default(DateTime), DateTime completedtime = default(DateTime));
        
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filterlist"></param>
        /// <param name="sortlist"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        IPagedList<UserAnswer> QueryByPaged(string nickname = null, string title = null, int type = -1
            , int pageindex = 0, int pagesize = int.MaxValue);

        /// <summary>
        /// 根据id获取答题记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserAnswer GetUserAnswerById(int id);

        /// <summary>
        /// 获取已经开始的答题
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        UserAnswer GetDoingQuestionAnswer(int userid);

        /// <summary>
        /// 获取用户最后的答题记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        UserAnswer GetLastQuestionAnswer(int userid);

        /// <summary>
        /// 开始答题
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="questionbankid"></param>
        /// <returns></returns>
        Question BeginQuestion(int userid, int questionbankid);

        /// <summary>
        /// 保存回答，获取下一个问题
        /// </summary>
        /// <param name="openid">用户</param>
        /// <param name="uanswer">答案</param>
        /// <returns></returns>
        Question SaveAnswer(UserAnswer uanswer, string answer, User user, string voicepath = null);

        /// <summary>
        /// 用户是否做过改题卷
        /// </summary>
        /// <param name="bankid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsAnswered(int bankid, User user);

    }
}