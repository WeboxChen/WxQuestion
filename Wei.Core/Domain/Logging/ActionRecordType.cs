using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wei.Core.Domain.Logging
{
    public enum ActionRecordType
    {
        Delete  = -1,
        Create  = 1,
        Update  = 2,
        Import  = 3,
        Upload  = 4
    }
}
