using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace Wei.Core
{
    [Serializable]
    public class SortModel
    {
        public string Property { get; set; }
        public string Direction { get; set; }

        public IOrderedQueryable<T> GetSorted<T>(IOrderedQueryable<T> query) where T : BaseEntity
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            PropertyInfo pinfo = null;
            foreach (var prop in properties)
            {
                if (string.Equals(prop.Name, Property))
                {
                    pinfo = prop;
                    break;
                }
            }
            if (pinfo == null)
                return null;

            if (string.Equals(Direction, "desc", StringComparison.CurrentCultureIgnoreCase))
            {
                return query.ThenByDescending(x => pinfo.GetValue(x));
            }
            else if (string.Equals(Direction, "asc", StringComparison.CurrentCultureIgnoreCase))
            {
                return query.ThenBy(x => pinfo.GetValue(x));
            }
            return null;
        }

        public IOrderedQueryable<T> GetSorted<T>(IQueryable<T> query) where T : BaseEntity
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            PropertyInfo pinfo = null;
            foreach (var prop in properties)
            {
                if (string.Equals(prop.Name, Property))
                {
                    pinfo = prop;
                    break;
                }
            }
            if (pinfo == null)
                return null;
            
            if(string.Equals(Direction, "desc", StringComparison.CurrentCultureIgnoreCase))
            {
                return query.OrderByDescending(x => pinfo.GetValue(x));
            }
            else if(string.Equals(Direction, "asc", StringComparison.CurrentCultureIgnoreCase))
            {
                return query.OrderBy(x => pinfo.GetValue(x));
            }
            return null;
        }
    }
    [Serializable]
    public class FilterModel
    {
        public string Property { get; set; }
        public object Value { get; set; }
        public string Operator { get; set; }

        /// <summary>
        /// 执行过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        public IQueryable<T> GetFiltered<T>(IQueryable<T> query) where T : BaseEntity
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            PropertyInfo pinfo = null;
            foreach (var prop in properties)
            {
                if (string.Equals(prop.Name, Property))
                {
                    pinfo = prop;
                    break;
                }
            }
            if (pinfo == null)
                return query;

            query = query.Where(x => CompareValue(pinfo.GetValue(x), pinfo.PropertyType));
            return query;
            //switch (Operator)
            //{
            //    case "lt":
            //    case "<":
            //        //query = query.Where(x =>
            //        //{
            //        //    var value = pinfo.GetValue(x);
            //        //    //if(pinfo.PropertyType == typeof(int))
            //        //    //{
            //        //    //    return value.ToInt() < Value.ToInt();
            //        //    //}

            //        //    return x.ToString() == "";
            //        //});
            //        query = query.Where(x  => CompareValue(pinfo.GetValue(x), Value, pinfo.PropertyTypes));
            //        return query;
            //    case "gt":
            //    case ">":
            //        return string.Format(" and " + colname + " > '{1}' ", item.Property, item.Value);
            //    case "lteq":
            //    case "<=":
            //        return string.Format(" and " + colname + " <= '{1}' ", item.Property, item.Value);
            //    case "gteq":
            //    case ">=":
            //        return string.Format(" and " + colname + " >= '{1}' ", item.Property, item.Value);
            //    case "noteq":
            //    case "!=":
            //        return string.Format(" and " + colname + " != '{1}' ", item.Property, item.Value);
            //    case "like":
            //        return string.Format(" and " + colname + " like '%{1}%' ", item.Property, item.Value);
            //    case "in":
            //        string str = "";
            //        if (item.Value is JArray)
            //        {
            //            var filterarr = (JArray)item.Value;
            //            str = string.Format(" and " + colname + " in ({1}) ", item.Property, "'" + string.Join("','", filterarr) + "'");
            //            //}
            //        }
            //        return str; ;
            //    case "eq":
            //    case "=":
            //    default:
            //        DateTime time;
            //        if (DateTime.TryParse(item.Value.ToString(), out time))
            //            return string.Format(" and convert(varchar(10), " + colname + ", 120) = '{1}' ", item.Property, time.ToString("yyyy-MM-dd"));

            //        return string.Format(" and " + colname + " = '{1}' ", item.Property, item.Value);
            //}
        }

        public bool CompareValue(object value1, Type type)
        {
            switch (Operator)
            {
                case "lt":
                case "<":
                    if (type == typeof(int))
                        return value1.ToInt() < Value.ToInt();
                    if (type == typeof(long))
                        return value1.ToLong() < Value.ToLong();
                    if (type == typeof(decimal))
                        return value1.ToDecimal() < Value.ToDecimal();
                    if (type == typeof(double))
                        return value1.ToDouble() < Value.ToDouble();
                    if (type == typeof(float))
                        return value1.ToFloat() < Value.ToFloat();
                    if (type == typeof(DateTime))
                        return value1.ToDateTime() < Value.ToDateTime();
                    return false;
                case "gt":
                case ">":
                    if (type == typeof(int))
                        return value1.ToInt() > Value.ToInt();
                    if (type == typeof(long))
                        return value1.ToLong() > Value.ToLong();
                    if (type == typeof(decimal))
                        return value1.ToDecimal() > Value.ToDecimal();
                    if (type == typeof(double))
                        return value1.ToDouble() > Value.ToDouble();
                    if (type == typeof(float))
                        return value1.ToFloat() > Value.ToFloat();
                    if (type == typeof(DateTime))
                        return value1.ToDateTime() > Value.ToDateTime();
                    return false;
                case "lteq":
                case "<=":
                    if (type == typeof(int))
                        return value1.ToInt() <= Value.ToInt();
                    if (type == typeof(long))
                        return value1.ToLong() <= Value.ToLong();
                    if (type == typeof(decimal))
                        return value1.ToDecimal() <= Value.ToDecimal();
                    if (type == typeof(double))
                        return value1.ToDouble() <= Value.ToDouble();
                    if (type == typeof(float))
                        return value1.ToFloat() <= Value.ToFloat();
                    if (type == typeof(DateTime))
                        return value1.ToDateTime() <= Value.ToDateTime();
                    return false;
                case "gteq":
                case ">=":
                    if (type == typeof(int))
                        return value1.ToInt() >= Value.ToInt();
                    if (type == typeof(long))
                        return value1.ToLong() >= Value.ToLong();
                    if (type == typeof(decimal))
                        return value1.ToDecimal() >= Value.ToDecimal();
                    if (type == typeof(double))
                        return value1.ToDouble() >= Value.ToDouble();
                    if (type == typeof(float))
                        return value1.ToFloat() >= Value.ToFloat();
                    if (type == typeof(DateTime))
                        return value1.ToDateTime() >= Value.ToDateTime();
                    return false;
                case "noteq":
                case "!=":
                    if (type == typeof(int))
                        return value1.ToInt() != Value.ToInt();
                    if (type == typeof(long))
                        return value1.ToLong() != Value.ToLong();
                    if (type == typeof(decimal))
                        return value1.ToDecimal() != Value.ToDecimal();
                    if (type == typeof(double))
                        return value1.ToDouble() != Value.ToDouble();
                    if (type == typeof(float))
                        return value1.ToFloat() != Value.ToFloat();
                    if (type == typeof(DateTime))
                        return value1.ToDateTime() != Value.ToDateTime();
                    if (type == typeof(string))
                        return value1.ToStringN() != Value.ToStringN();
                    return false;
                case "like":
                    if (type == typeof(string))
                        return value1.ToStringN().IndexOf(Value.ToStringN(), StringComparison.CurrentCultureIgnoreCase) != -1;
                    return false;
                case "in":
                    if (Value is Newtonsoft.Json.Linq.JArray)
                    {
                        var filterarr = (Newtonsoft.Json.Linq.JArray)Value;
                        foreach(JToken i in filterarr)
                        {
                            if (string.Equals(i.ToStringN(), Value.ToStringN(), StringComparison.CurrentCultureIgnoreCase))
                                return true;
                        }
                    }
                    return false; ;
                case "eq":
                case "=":
                default:
                    if (type == typeof(int))
                        return value1.ToInt() == Value.ToInt();
                    if (type == typeof(long))
                        return value1.ToLong() == Value.ToLong();
                    if (type == typeof(decimal))
                        return value1.ToDecimal() == Value.ToDecimal();
                    if (type == typeof(double))
                        return value1.ToDouble() == Value.ToDouble();
                    if (type == typeof(float))
                        return value1.ToFloat() == Value.ToFloat();
                    if (type == typeof(DateTime))
                        return value1.ToDateTime() == Value.ToDateTime();
                    if (type == typeof(string))
                        return string.Equals(value1.ToStringN(), Value.ToStringN(), StringComparison.CurrentCultureIgnoreCase);
                    return false;
            }
            return false;
        }
    }
}
