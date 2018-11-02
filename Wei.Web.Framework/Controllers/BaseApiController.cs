using System.Web.Http;

namespace Wei.Web.Framework.Controllers
{
    [WebApiExceptionFilter]
    [UserApiAuthorize]
    public class BaseApiController : ApiController
    {
        
    }
}
