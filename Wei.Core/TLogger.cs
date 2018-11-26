using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Wei.Core
{
    public class TLogger
    {
        /// <summary>
        /// sql error
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="e"></param>
        public static void WriteLog(string sql, Exception e, string DBAddress)
        {
            string ErrorLog = "在执行Sql:" + sql;
            ErrorLog += "时出错，错误信息为:" + e.Message;
            WriteLog(ErrorLog, DBAddress);
        }

        /// <summary>
        /// 集合 error
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="htbParam"></param>
        /// <param name="e"></param>
        public static void WriteLog(string sql, System.Collections.Hashtable htbParam, Exception e, string DBAddress)
        {
            string ErrorLog = "在执行Sql:" + sql + " 参数：";
            foreach (DictionaryEntry de in htbParam)
            {
                ErrorLog += " key:" + ((de.Key == null) ? string.Empty : (de.Key.ToString()));
                ErrorLog += " value:" + ((de.Value == null) ? string.Empty : (de.Value.ToString()));
            }
            ErrorLog += "时出错，错误信息为:" + e.Message;
            WriteLog(ErrorLog, DBAddress);
        }

        /// <summary>
        /// 详细异常
        /// </summary>
        /// <param name="e"></param>
        /// <param name="Remark"></param>
        public static void WriteLog(Exception e, string Remark, string DBAddress)
        {
            System.Text.StringBuilder str = new StringBuilder();
            str.Append(Remark + " e:" + e.Message + " ex.Source:" + e.Source + " ex.Data:" + e.Data + " e.StackTrace:" + e.StackTrace);
            WriteLog(str.ToString(), DBAddress);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="Remark">日志信息</param>
        public static void WriteLog(string Remark, string DBAddress)
        {
            string LogFilePath = Path.Combine(GetLogDirectoryPath(), DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            System.IO.StreamWriter w = null;

            try
            {
                w = new System.IO.StreamWriter(LogFilePath, true, System.Text.Encoding.Default);
            }
            catch
            {
                return;
            }

            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append("\r\n-------------------------------------------------------------------------------\r\n");
            str.Append("时间:" + DateTime.Now.ToString() + "\r\n");
            str.Append("\r\n");

            try
            {
                string ipAddress = Dns.GetHostName().ToString();
                str.Append("用户名:" + ipAddress + "\r\n");
                str.Append("\r\n");

                IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress ip in arrIPAddresses)
                {
                    if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                    {
                        ipAddress = ip.ToString();
                    }
                }

                str.Append("用户IP:" + ipAddress + "\r\n");
                str.Append("请求数据地址:" + DBAddress + "\r\n");
            }
            catch { }

            str.Append(Remark);
            str.Append("\r\n-------------------------------------------------------------------------------\r\n");
            try
            {
                w.WriteLine(str.ToString());//将信息写入日志　
                w.Flush();
            }
            catch
            {
            }
            finally
            {
                w.Close();
            }
        }

        /// <summary>
        /// 获取日志路径
        /// </summary>
        /// <returns></returns>
        public static string GetLogDirectoryPath()
        {
            string value = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(value))
                Directory.CreateDirectory(value);
            return value;
        }
    }
}
