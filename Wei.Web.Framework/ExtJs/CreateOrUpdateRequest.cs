using System.Collections.Generic;
using Wei.Core;

namespace Wei.Web.Framework.ExtJs
{
    public class CreateOrUpdateRequest : RequestBase
    {
        public IList<FilterModel> Data { get; set; }

        //public string extraCreate { get; set; }
    }
}
