using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InlandMarinaData;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TravelExpertsMVC.Controllers
{
    public class BookingController : Controller
    {
        // Shows all slips that are leased by the current customer that is logged in
        [Authorize]
        public IActionResult MySlips()
        {
            string? message = TempData["Message"]?.ToString();
            ViewBag.Message = message;

            InlandMarinaContext db = new InlandMarinaContext();
            int? currentCustomerId = HttpContext.Session.GetInt32("CurrentCustomer");

            if (currentCustomerId != null)
            {
                List<Slip> leasedSlips = InlandMarinaManager.GetLeasedSlips(currentCustomerId);
                return View(leasedSlips);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            } 
        }

        // pass data to dropdown list of only avalible slips that can be leased
        [Authorize]
        public IActionResult Lease() 
        {
            InlandMarinaContext db = new InlandMarinaContext();
            var slips = (InlandMarinaManager.GetSlips(db)).ToList();

            return View(slips);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ConfirmLease(int SlipID)
        {
            InlandMarinaContext db = new InlandMarinaContext();

            // Retrieve the current customer ID from the session
            int? currentCustomerId = HttpContext.Session.GetInt32("CurrentCustomer");

            // Check if the customer ID was found in the session
            if (currentCustomerId == null)
            {
                // If the customer ID is not found, redirect to the login page
                return RedirectToAction("Login", "Account");
            }

            // Add a new lease record to the database
            Lease newLease = new Lease
            {
                SlipID = SlipID,
                CustomerID = currentCustomerId.Value
            };
            db.Leases.Add(newLease);
            db.SaveChanges();

            // Pass TempData to next request after redirect to show booking confirmation
            TempData["Message"] = "Slip was leased successfully";
            return RedirectToAction("MySlips", "Booking");
        }

    }
}
