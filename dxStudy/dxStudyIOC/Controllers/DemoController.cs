using Microsoft.AspNetCore.Mvc;

namespace dxStudyIOC.Controllers
{
    public class DemoController : Controller
    {
        private readonly DemoService _demoService;

        public DemoController(DemoService demoService)
        {
            _demoService = demoService;
        }

        public IActionResult Index()
        {
            return Json(_demoService.Test());
        }
    }
}
