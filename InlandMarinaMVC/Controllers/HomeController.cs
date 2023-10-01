// This application outputs the Home, Contact, and Slips page. Also you are able to log in or register to be able to view all slips you have leased currently and the ability to lease new slips
// Created by Meetkamal Grewal, February 2023
using InlandMarinaData;
using InlandMarinaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace InlandMarinaMVC.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            string? message = TempData["Message"]?.ToString();
            ViewBag.Message = message;
            return View();
        }
        // shows contact page
        public ActionResult Contact()
        {
            return View();
        }
        // shows all avalible slips that can be purchased with a filer option
        public ActionResult Slip()
        {
            InlandMarinaContext db = new InlandMarinaContext();
            List<Slip> slips = null;
            List<Dock> docks = InlandMarinaManager.GetDocks(db);
            var list = new SelectList(docks, "ID", "ID").ToList();
            list.Insert(0, new SelectListItem("All", "All")); // add first element for all avalible docks
            ViewBag.Docks = list;
            try
            {
                slips = InlandMarinaManager.GetSlips(db);
            }
            catch (Exception)
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
            }
            return View(slips);
        }
       // post request to filter slips for the desired dock id
        [HttpPost]
        public ActionResult Slip(string DockID)
        {
            InlandMarinaContext db = new InlandMarinaContext();
            List<Dock> docks = InlandMarinaManager.GetDocks(db);
            var list = new SelectList(docks, "ID", "ID").ToList();
            list.Insert(0, new SelectListItem("All", "All")); // add first element for all genres
            // retain selected value
            foreach (var item in list) // find the selected item
            {
                if (item.Value == DockID)
                {
                    item.Selected = true;
                    break;
                }
            }
            ViewBag.Docks = list;

            List<Slip> slips = InlandMarinaManager.GetSlips(db);
            if (DockID == "All") // all docks
            {
                slips = InlandMarinaManager.GetSlips(db);
            }
            else // filter by Dock ID
            {
                slips = InlandMarinaManager.GetSlipsByDocks(db, int.Parse(DockID));
            }
            return View(slips);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}