using System.Collections.Generic;
using Wei.Core.Domain.Sys;

namespace Wei.Services.Sys
{
    public interface IWordsSubstitutionService
    {
        /// <summary>
        /// 获取修正词
        /// </summary>
        /// <param name="qbid">文件id</param>
        /// <param name="hasCommon">是否包含公有</param>
        /// <returns></returns>
        IDictionary<string, string> GetSubstitutions(int qbid = 0, bool hasCommon = true);
    }
}
