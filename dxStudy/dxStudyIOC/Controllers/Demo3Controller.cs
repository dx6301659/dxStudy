using dxStudyIOCByAssembly.Contract;
using Microsoft.AspNetCore.Mvc;

namespace dxStudyIOC.Controllers
{
    public class Demo3Controller : Controller
    {
        private readonly IDemo3Service _demoService;

        public Demo3Controller(IDemo3Service demoService)
        {
            _demoService = demoService;
        }

        public IActionResult Index()
        {
            return Json(_demoService.Test());
        }
    }
}
