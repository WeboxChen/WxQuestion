using System;
using System.Linq;
using Wei.Core.Data;
using Wei.Core.Domain.Sys;

namespace Wei.Services.Sys
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepository<Property> _propertyRepository;

        public PropertyService(IRepository<Property> propertyRepository)
        {
            this._propertyRepository = propertyRepository;
        }

        /// <summary>
        /// 根据code获取值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public string GetValue(string code, int sortType = 0)
        {
            if (string.IsNullOrEmpty(code))
                throw new System.ArgumentException("property ");

            var property = this._propertyRepository.Table.FirstOrDefault(x => string.Equals(x.PCode, code));

            if(property == null)
            {
                throw new Exception("property " + code);
            }

            switch (sortType)
            {
                case 1:
                    Random r = new Random();
                    var num = r.Next(0, property.PropertyValueList.Count - 1);
                    return property.PropertyValueList.ToList()[num].Text;
                case 2:
                    break;
            }
            var def = property.PropertyValueList.OrderBy(x => x.Sort).FirstOrDefault(x => x.IsDef == 1);
            if (def != null)
                return def.Text;
            return property.PropertyValueList.OrderBy(x => x.Sort).First().Text;
        }

        /// <summary>
        /// 验证code和值是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckCodeValue(string code, string value)
        {
            var property = this._propertyRepository.Table.FirstOrDefault(x => string.Equals(x.PCode, code));
            if(property != null)
            {
                return property.PropertyValueList.Any(x => string.Equals(x.Text, value, StringComparison.CurrentCultureIgnoreCase));
            }
            return false;
        }
    }
}
