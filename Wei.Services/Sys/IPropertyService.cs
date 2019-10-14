namespace Wei.Services.Sys
{
    public interface IPropertyService
    {
        /// <summary>
        /// 根据code获取值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        string GetValue(string code, int sortType = 0);

        /// <summary>
        /// 验证code和值是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CheckCodeValue(string code, string value);
    }
}
