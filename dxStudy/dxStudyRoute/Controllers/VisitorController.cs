using dxStudyRoute.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dxStudyRoute.Controllers
{
    public class VisitorController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Route("/visitor")]
        [Route("visitor")]   
        public ActionResult GetVisitor()
        {
            return Content("Ask for specific user on URL: /visitor/{visitorId}");
        }

        [HttpPost]
        [Route("visitor/createnew")]
        public ActionResult CreateVisitor(Visitor visitor)
        {
            //var visitorId = CreateVisitor(visitor);
            return Content("Visitor creted!");
        }
    }
}