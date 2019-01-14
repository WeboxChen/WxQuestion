﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core.Domain.Questions;

namespace Wei.Services.Questions
{
    public interface IQuestionBankService
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        IList<QuestionBank> Query();

        /// <summary>
        /// 获取题卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QuestionBank GetQuestionBankById(int id);

        /// <summary>
        /// 根据题卷id和题目序号获取题目
        /// </summary>
        /// <param name="questionbankid"></param>
        /// <param name="questionno"></param>
        /// <returns></returns>
        Question GetQuestion(int questionbankid, decimal questionno);

        /// <summary>
        /// 获取默认的
        /// </summary>
        /// <returns></returns>
        IDictionary<string, QuestionBank> KeyWordQuestionBank();
    }
}
