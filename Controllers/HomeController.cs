using FootballersCatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FootballersCatalogue.Controllers
{
    public class HomeController : Controller
    {
        CatalogueContext db;

        public HomeController(CatalogueContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Phones.ToList());
        }

        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.PhoneId = id;
            return View(db.Orders.Select(order => order.ContactPhone).Distinct().ToList());
        }

        [HttpPost]
        public IActionResult Buy(Order order)
        {
            db.Orders.Add(order);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return RedirectToAction("Orders");
        }

        [HttpGet]
        public IActionResult Orders()
        {
            return View(db.Orders.ToList());
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {           
            //if (orderId != null)
            //{
            //    var order = await db.Orders.FirstOrDefaultAsync(p => p.OrderId == orderId);
            //    if (order != null)
            //        return View(order);
            //}
            //if (id == null) return RedirectToAction("Orders");
            ViewBag.OrderId = id;
            return View(db.Orders.FirstOrDefault(p => p.OrderId == id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? orderId)
        {
            if (orderId != null)
            {
                var order = await db.Orders.FirstOrDefaultAsync(p => p.OrderId == orderId);
                if (order != null)
                {
                    db.Orders.Remove(order);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Orders");
                }
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var order = await db.Orders.FirstOrDefaultAsync(p => p.OrderId == id);
                if (order != null)
                    return View(order);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            
            db.Orders.Update(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Orders");
        }

        [HttpGet]
        public IActionResult GetTeamsDatalist()
        {
            Debug.WriteLine(db.Orders.Count());
            Debug.WriteLine("GetTeams");
            return PartialView(db.Orders.ToList());
        }
    }
}
