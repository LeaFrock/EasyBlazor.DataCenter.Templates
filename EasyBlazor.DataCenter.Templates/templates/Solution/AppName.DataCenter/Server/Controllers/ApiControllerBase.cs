using Microsoft.AspNetCore.Mvc;
using AppName.DataCenter.Server.Data;

namespace AppName.DataCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        public ApiControllerBase(
            ILogger logger,
            AppNameDbContext dbContext)
        {
            Logger = logger;
            EFContext = dbContext;
        }

        protected ILogger Logger { get; }

        protected AppNameDbContext EFContext { get; }
    }
}