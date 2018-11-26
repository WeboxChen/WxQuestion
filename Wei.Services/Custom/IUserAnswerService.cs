using System;
using System.Collections.Generic;
using Wei.Core.Domain.Custom;
using Wei.Core.Domain.Questions;

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
        Question SaveAnswer(UserAnswer uanswer, string answer);

    }
}
