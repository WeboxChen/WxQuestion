﻿using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Wei.Core
{
    public static class DataConvertHelper
    {
        public static readonly DateTime MinDate = new DateTime(2000, 1, 1);

        public static string ToStringN(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return "";
            return obj.ToString();
        }

        public static int ToInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;
            int i = 0;
            int.TryParse(obj.ToString(), out i);
            return i;
        }

        public static long ToLong(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;
            long i = 0;
            long.TryParse(obj.ToString(), out i);
            return i;
        }

        public static float ToFloat(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;
            float i = 0;
            float.TryParse(obj.ToString(), out i);
            return i;
        }

        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return default(DateTime);
            DateTime time;
            if (DateTime.TryParse(obj.ToString(), out time))
                return time;
            return default(DateTime);
        }

        public static bool ToBoolean(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return default(bool);
            bool b;
            bool.TryParse(obj.ToString(), out b);
            return b;
        }

        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;
            double i = 0;
            double.TryParse(obj.ToString(), out i);
            return i;
        }

        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;
            decimal i = 0;
            decimal.TryParse(obj.ToString(), out i);
            return i;
        }

        //public static T ToBaseEntity<T>(this JObject jobject)
        //{
        //    T entity = Activator.CreateInstance<T>();
        //    var properties = typeof(T).GetProperties();
        //    var jproperties = jobject.Properties();
        //    foreach(var prop in properties)
        //    {
        //        var jprop = jproperties.FirstOrDefault(x => string.Equals(x.Name, prop.Name, StringComparison.CurrentCultureIgnoreCase));
        //        if (jprop == null)
        //            continue;

        //        switch (jprop.Value.Type)
        //        {
        //            case JTokenType.String:
        //                prop.SetValue(entity, jprop.Value.Value<string>());
        //                break;
        //            case JTokenType.Integer:
        //                prop.SetValue(entity, jprop.Value.Value<int>());
        //                break;
        //            case JTokenType.Float:
        //                prop.SetValue(entity, jprop.Value.Value<float>());
        //                break;
        //            case JTokenType.Null:
        //                continue;
        //            case JTokenType.Object:
        //                break;
        //            case JTokenType.Date:
        //                var tmpdate = jprop.Value.Value<DateTime>();
        //                if (tmpdate > new DateTime(2000, 1, 1))
        //                    prop.SetValue(entity, tmpdate);
        //                break;
        //            case JTokenType.Boolean:
        //                prop.SetValue(entity, jprop.Value.Value<bool>());
        //                break;
        //            case JTokenType.Bytes:
        //                prop.SetValue(entity, jprop.Value.Value<byte[]>());
        //                break;

        //        }
               
        //    }
        //    return entity;
        //}
    }
}
