using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Wei.Core;
using Wei.Core.Infrastructure;

namespace Wei.Web.Framework.NPOI
{
    public class ExcelReader
    {
        private IWorkbook _book;
        private IWorkContext _workContext;

        public ExcelReader(Stream stream)
        {
            _book = WorkbookFactory.Create(stream);
            _workContext = EngineContext.Current.Resolve<IWorkContext>();
        }
        public ExcelReader(string excelUrl)
        {
            _book = WorkbookFactory.Create(excelUrl);
            _workContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        public int SheetCount
        {
            get
            {
                return _book.NumberOfSheets;
            }
        }
        public bool Contain(string name)
        {
            var sheet = _book.GetSheet(name);
            if (sheet == null)
                return false;
            return true;
        }
        private IFormulaEvaluator _formulaEvaluator;
        public IFormulaEvaluator FormulaEvaluator
        {
            get
            {
                
                if (_formulaEvaluator == null)
                {
                    _formulaEvaluator = _book.GetCreationHelper().CreateFormulaEvaluator();
                    var sheet =_book.GetSheetAt(0);                    //_formulaEvaluator.re
                }
                return _formulaEvaluator;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<T> Read<T>(string sheetname, string tname, out string message)
        {
            StringBuilder sbmsg = new StringBuilder();
            IList<T> result = new List<T>();
            ISheet sheet = _book.GetSheet(sheetname);
            if (sheet == null)
                sheet = _book.GetSheetAt(0);
            
            Type t = typeof(T);
            var properties = t.GetProperties();
            // 获取表头
            var rowTitle = sheet.GetRow(0);
            var colDescription = _workContext.ColDescription[tname];
            // 动态生成对象
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null || row.LastCellNum < 2) continue;
                string name = "";

                var obj = Activator.CreateInstance(t);

                for (int j = 0; j <= row.LastCellNum; j++)
                {
                    var celltitle = rowTitle.GetCell(j);
                    var cell = row.GetCell(j);
                    if (celltitle == null || cell == null 
                        || celltitle.CellType != CellType.String || cell.CellType == CellType.Blank 
                        || !colDescription.ContainsKey(celltitle.StringCellValue))
                        continue;

                    string title = colDescription[celltitle.StringCellValue];
                    var property = properties.FirstOrDefault(x => x.Name.Equals(title, StringComparison.CurrentCultureIgnoreCase));
                    if(property != null)
                    {
                        // 基础数据类型  int decimal string DateTime
                        //var ptype = property.PropertyType;
                        try
                        {
                            GetCellValue(cell, property, obj);
                        }
                        catch 
                        {
                            sbmsg.AppendFormat("第{0}行 {1}的{2} 信息格式错误！<br/>", i + 1, name, celltitle.StringCellValue);
                        }
                    }
                    if ("名称".Equals(celltitle.StringCellValue, StringComparison.CurrentCultureIgnoreCase))
                        name = property.GetValue(obj).ToString();
                }
                name = "";
                result.Add((T)obj);
            }
            message = sbmsg.ToString();
            return result;
        }

        private void GetCellValue(ICell cell, PropertyInfo property, object obj)
        {
            Type ptype = property.PropertyType;
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (ptype == typeof(int) || ptype == typeof(int?))
                        property.SetValue(obj, cell.NumericCellValue.ToInt());
                    else if (ptype == typeof(string))
                        property.SetValue(obj, cell.NumericCellValue.ToString());
                    else if (ptype == typeof(decimal) || ptype == typeof(decimal?))
                        property.SetValue(obj, cell.NumericCellValue.ToDecimal());
                    else if(ptype == typeof(bool) || ptype == typeof(bool?))
                        property.SetValue(obj, cell.NumericCellValue > 0);
                    else if ((ptype == typeof(DateTime) || ptype == typeof(DateTime?)) && DateUtil.IsCellDateFormatted(cell))
                        property.SetValue(obj, cell.DateCellValue);
                    break;
                case CellType.String:
                    if (ptype == typeof(int) || ptype == typeof(int?))
                        property.SetValue(obj, cell.StringCellValue.ToInt());
                    else if (ptype == typeof(string))
                        property.SetValue(obj, cell.StringCellValue);
                    else if (ptype == typeof(decimal) || ptype == typeof(decimal?))
                        property.SetValue(obj, cell.StringCellValue.ToDecimal());
                    else if (ptype == typeof(bool) || ptype == typeof(bool?))
                        property.SetValue(obj, cell.StringCellValue.ToBoolean());
                    else if (ptype == typeof(DateTime) || ptype == typeof(DateTime?))
                        property.SetValue(obj, cell.StringCellValue.ToDateTime());
                    break;
                case CellType.Boolean:
                    if (ptype == typeof(int) || ptype == typeof(int?) || ptype == typeof(decimal) || ptype == typeof(decimal?))
                        property.SetValue(obj, cell.BooleanCellValue? 1:0);
                    else if (ptype == typeof(string))
                        property.SetValue(obj, cell.BooleanCellValue.ToString());
                    else if (ptype == typeof(bool) || ptype == typeof(bool?))
                        property.SetValue(obj, cell.BooleanCellValue);
                    break;
                case CellType.Formula:
                    CellValue value = null;
                    try
                    {
                        value = FormulaEvaluator.Evaluate(cell);
                    }
                    catch
                    {
                        try
                        {
                            if (ptype == typeof(int) || ptype == typeof(int?))
                                property.SetValue(obj, cell.NumericCellValue.ToInt());
                            else if (ptype == typeof(string))
                                property.SetValue(obj, cell.StringCellValue);
                            else if (ptype == typeof(decimal) || ptype == typeof(decimal?))
                                property.SetValue(obj, cell.NumericCellValue.ToDecimal());
                            else if (ptype == typeof(bool) || ptype == typeof(bool?))
                                property.SetValue(obj, cell.BooleanCellValue);
                        }
                        catch { }
                    }
                    if (value == null) return;
                    if (ptype == typeof(int) || ptype == typeof(int?))
                    {
                        switch (value.CellType)
                        {
                            case CellType.Numeric:
                                property.SetValue(obj, value.NumberValue.ToInt());
                                break;
                            case CellType.String:
                                property.SetValue(obj, value.StringValue.ToInt());
                                break;
                            case CellType.Boolean:
                                property.SetValue(obj, value.BooleanValue ? 1 : 0);
                                break;
                        }
                    }
                    else if (ptype == typeof(string))
                    {
                        switch (value.CellType)
                        {
                            case CellType.Numeric:
                                property.SetValue(obj, value.NumberValue.ToString());
                                break;
                            case CellType.String:
                                property.SetValue(obj, value.StringValue);
                                break;
                            case CellType.Boolean:
                                property.SetValue(obj, value.BooleanValue.ToString());
                                break;
                        }
                    }
                    else if (ptype == typeof(decimal) || ptype == typeof(decimal?))
                    {
                        switch (value.CellType)
                        {
                            case CellType.Numeric:
                                property.SetValue(obj, value.NumberValue.ToDecimal());
                                break;
                            case CellType.String:
                                property.SetValue(obj, value.StringValue.ToDecimal());
                                break;
                            case CellType.Boolean:
                                property.SetValue(obj, value.BooleanValue ? 1 : 0);
                                break;
                        }
                    }
                    else if (ptype == typeof(bool) || ptype == typeof(bool?))
                    {
                        switch (value.CellType)
                        {
                            case CellType.Numeric:
                                property.SetValue(obj, value.NumberValue > 0);
                                break;
                            case CellType.String:
                                property.SetValue(obj, value.StringValue.ToBoolean());
                                break;
                            case CellType.Boolean:
                                property.SetValue(obj, value.BooleanValue);
                                break;
                        }
                    }
                    else if (ptype == typeof(DateTime) || ptype == typeof(DateTime?))
                    {
                        if (value.CellType == CellType.Numeric)
                            property.SetValue(obj, new DateTime(value.NumberValue.ToLong()));
                    }
                    
                    break;
            }
            //if (ptype == typeof(int?) && cell.CellType == CellType.Numeric)
            //    property.SetValue(obj, (int?)cell.NumericCellValue);
            //else if (ptype == typeof(int?) && cell.CellType == CellType.Boolean)
            //    property.SetValue(obj, cell.BooleanCellValue ? 1 : 0);
            //else if (ptype == typeof(int?) && cell.CellType == CellType.String)
            //    property.SetValue(obj, cell.StringCellValue.ToInt());
            //else if (ptype == typeof(int) && cell.CellType == CellType.Numeric)
            //    property.SetValue(obj, (int)cell.NumericCellValue);
            //else if (ptype == typeof(int) && cell.CellType == CellType.Boolean)
            //    property.SetValue(obj, cell.BooleanCellValue ? 1 : 0);
            //else if (ptype == typeof(int) && cell.CellType == CellType.String)
            //    property.SetValue(obj, cell.StringCellValue.ToInt());
            //else if (ptype == typeof(decimal?) && cell.CellType == CellType.Numeric)
            //    property.SetValue(obj, (decimal?)cell.NumericCellValue);
            //else if (ptype == typeof(decimal?) && cell.CellType == CellType.String)
            //    property.SetValue(obj, cell.StringCellValue.ToDecimal());
            //else if (ptype == typeof(decimal) && cell.CellType == CellType.Numeric)
            //    property.SetValue(obj, (decimal)cell.NumericCellValue);
            //else if (ptype == typeof(decimal) && cell.CellType == CellType.String)
            //    property.SetValue(obj, cell.ToDecimal());
            //else if (ptype == typeof(DateTime?) && DateUtil.IsCellDateFormatted(cell))
            //    property.SetValue(obj, cell.DateCellValue == default(DateTime) ? null : (DateTime?)cell.DateCellValue);
            //else if (ptype == typeof(DateTime) && DateUtil.IsCellDateFormatted(cell))
            //    property.SetValue(obj, cell.DateCellValue);
            //else if (ptype == typeof(bool?) && cell.CellType == CellType.Boolean)
            //    property.SetValue(obj, cell.BooleanCellValue);
            //else if (ptype == typeof(bool) && cell.CellType == CellType.Boolean)
            //    property.SetValue(obj, cell.BooleanCellValue);
            //else if (ptype == typeof(string) && cell.CellType == CellType.String)
            //    property.SetValue(obj, cell.StringCellValue);
            //else if (ptype == typeof(string) && cell.CellType == CellType.Formula)
            //    property.SetValue(obj, cell.StringCellValue);
            //else if (ptype == typeof(string) && cell.CellType == CellType.Numeric)
            //    property.SetValue(obj, cell.NumericCellValue.ToString());
            //else if (ptype == typeof(string) && cell.CellType == CellType.Boolean)
            //    property.SetValue(obj, cell.BooleanCellValue ? "1" : "0");
            //else if (ptype == typeof(int?) && cell.CellType == CellType.Formula)
            //    // 临时方案，  下拉框公式暂时无法计算
            //    property.SetValue(obj, cell.NumericCellValue.ToInt());
            
        }
    }
}
