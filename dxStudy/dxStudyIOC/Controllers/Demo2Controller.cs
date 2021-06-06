using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxStudyIOC.Controllers
{
    public class Demo2Controller : Controller
    {
        private readonly IDemo2Service _demoService;

        public Demo2Controller(IDemo2Service demoService)
        {
            _demoService = demoService;
        }

        public IActionResult Index()
        {
            return Json(_demoService.Test());
        }
    }
}
