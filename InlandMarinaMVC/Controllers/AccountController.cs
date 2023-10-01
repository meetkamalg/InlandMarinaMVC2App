using InlandMarinaData;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InlandMarinaMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = "")
        {
            if (returnUrl != null)
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Customer customer) // customer comes from login form
        {
            InlandMarinaContext db = new InlandMarinaContext();
            Customer cust = CustomerManager.Authenticate(customer.Username, customer.Password);
            if (cust == null) //authentication failed
            {
                return View(); //stay on login page
            }
            //cust is not null - customer is authenticated

            //if the customer has slips, add customerid to session state

            HttpContext.Session.SetInt32("CurrentCustomer", cust.ID);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cust.Username),
                new Claim("FullName", cust.FirstName + " " + cust.LastName),
                new Claim("Password", cust.Password)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme); //"Cookies"
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            //get authentication ticket
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (string.IsNullOrEmpty(TempData["ReturnUrl"].ToString()))
            {
                return RedirectToAction("Index", "Home"); //by default go to the main page
            }
            else
            {
                return Redirect(TempData["ReturnUrl"].ToString());
            }
        }
        public async Task<IActionResult> LogoutAsync()
        {
            // return the authentication ticket
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("CurrentCustomer");
            return RedirectToAction("Index", "Home"); //go to the main page after logout
        }
        public ActionResult Registeration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registeration(Customer customer)
        {
            InlandMarinaContext db = new InlandMarinaContext();

            Customer newCustomer = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                City = customer.City,
                Phone = customer.Phone,
                Username = customer.Username,
                Password = customer.Password
            };
            db.Customers.Add(newCustomer);
            db.SaveChanges();

            // Pass TempData to next request after redirect to show registration confirmation
            TempData["Message"] = "Account was created successfully";
            return RedirectToAction("Index", "Home");

        }
    }
}
