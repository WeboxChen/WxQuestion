using Wei.Core.Domain.Sys;

namespace Wei.Services.Sys
{
    public interface IDBService
    {
        DBTable GetDBTable(string tname);
    }
}
