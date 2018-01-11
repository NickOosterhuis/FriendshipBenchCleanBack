using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class BenchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Info(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            ViewBag.id = id;
            ViewBag.title = "Bench info";
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}