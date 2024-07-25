using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SalesSummary()
        {
            var data = new List<SalesSummary>();

            using (var context = new MyDbContext())
            {
                data = context.DataFruits
    .GroupBy(df => df.fruit)
    .Select(g => new SalesSummary
    {
        Item = g.Key,
        Revenue = g.Sum(df => df.unit_price ?? 0) // Handle nullable double
    }).ToList();
            }

            return Json(new { status = "OK", items = data }, JsonRequestBehavior.AllowGet);
        }
    }

    public class SalesSummary
    {
        public string Item { get; set; }
        public double Revenue { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("testAgitEntities") { }

        public DbSet<data_fruit> DataFruits { get; set; }
    }
}
