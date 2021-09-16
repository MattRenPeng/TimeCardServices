using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardServices.Controllers
{
    [EnableCors("any")]
    [Authorize]
    public class BaseController: ControllerBase
    {
        //test
    }
}
