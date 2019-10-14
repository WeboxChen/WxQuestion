using System.Collections;
using System.Collections.Generic;

namespace Wei.Core.Domain.Sys
{
    public class Property : BaseEntity
    {
        private ICollection<PropertyValue> _propertyValueList;

        public string PCode { get; set; }
        public string Desc { get; set; }
        public int SelectValueType { get; set; }

        public virtual ICollection<PropertyValue> PropertyValueList
        {
            get { return _propertyValueList ?? (_propertyValueList = new List<PropertyValue>()); }
            set { _propertyValueList = value; }
        }
    }
}
