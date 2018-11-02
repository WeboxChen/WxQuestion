using System.Collections.Generic;
using System.Data;

namespace Wei.Services.Common
{
    public interface ICommonService
    {
        /// <summary>
        /// 单表新增
        /// </summary>
        /// <param name="tblname"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        int Insert(string tblname, IDictionary<string, object> data);

        /// <summary>
        /// 多表新增
        /// </summary>
        /// <param name="data"></param>
        void Insert(IDictionary<string, IDictionary<string, object>> data);

        /// <summary>
        /// 更新多表数据数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="whereStr"></param>
        void Update(IDictionary<string, IDictionary<string, object>> data);

        /// <summary>
        /// 更新单表数据
        /// </summary>
        /// <param name="tblname"></param>
        /// <param name="data"></param>
        void Update(string tblname, IDictionary<string, object> data);

        /// <summary>
        /// 更新单表数据
        /// </summary>
        /// <param name="tblname"></param>
        /// <param name="data"></param>
        void Update(string tblname, IDictionary<string, object> data, string whereStr);

        /// <summary>
        /// 删除数据，直接删除
        /// </summary>
        /// <param name="data"></param>
        void Delete(IDictionary<string, IDictionary<string, object>> data);

        /// <summary>
        /// 删除mapping数据
        /// </summary>
        /// <param name="data"></param>
        void Delete(string tblname, IDictionary<string, object> data);

        /// <summary>
        /// 查询不分页的数据
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        DataTable GetByProcedure(string procedure, IDictionary<string, object> paras);

        /// <summary>
        /// 查询不分页的数据
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="msg">output</param>
        /// <returns></returns>
        DataTable GetByProcedure(string procedure, IDictionary<string, object> paras, out string msg);

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="count"></param>
        /// <param name="fcount"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        DataTable GetByProcedure(string procedure, IDictionary<string, object> paras, out int count, out int fcount);

        /// <summary>
        /// 查询不分页的数据
        /// </summary>
        /// <param name="view"></param>
        /// <param name="columns"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="filter"></param>c
        /// <param name="sort"></param>
        /// <returns></returns>
        DataTable GetByView(string view, string columns, string filter = "", string sort = "");

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="view"></param>
        /// <param name="columns"></param>
        /// <param name="count"></param>
        /// <param name="fcount"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        DataTable GetByView(string view, string columns, out int count, out int fcount, int start = 0, int length = 20, string filter = "", string sort = "Id");

        /// <summary>
        /// 查询view数据，不使用别名系统
        /// </summary>
        /// <param name="view"></param>
        /// <param name="columns"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        DataTable GetByViewNonAlias(string view, string columns, out int count, out int fcount, int start = 0, int length = 20, string filter = "", string sort = "Id");

        /// <summary>
        /// 根据存储过程名称和参数列表执行命令
        /// </summary>
        /// <param name="procname"></param>
        /// <param name="paras"></param>
        /// <returns>1: 执行成功  0: 执行失败</returns>
        int ExecuteProcedure(string procname, IDictionary<string, object> paras);

        /// <summary>
        /// 根据存储过程名称和参数列表执行命令,返回执行结果
        /// </summary>
        /// <param name="procname"></param>
        /// <param name="paras"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        int ExecuteProcedure(string procname, IDictionary<string, object> paras, out string msg);

        /// <summary>
        /// 获取存错过程参数列表
        /// </summary>
        /// <param name="procname"></param>
        /// <returns></returns>
        IList<string> GetParasByProcName(string procname);
    }
}
