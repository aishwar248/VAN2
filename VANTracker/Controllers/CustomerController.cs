using Microsoft.AspNetCore.Mvc;
using VANTracker.DataAccess.Data;
using VANTracker.DataModels.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace VANTracker.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.RouteNumber = null; // Route is selected after login
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Login(string mobileNumber, string password)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.MobileNumber == mobileNumber && c.Password == password);
            if (customer != null)
            {
                HttpContext.Session.SetInt32("CustomerId", customer.Id);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials.";
            return View();
        }

        public IActionResult Dashboard()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToAction("Login");

            var customer = _context.Customers.Find(customerId);
            if (customer == null) return RedirectToAction("Login");

            // Provide list of routes to pick from (1–10)
            var allRoutes = Enumerable.Range(1, 10).Select(r => r.ToString()).ToList();
            ViewBag.AvailableRoutes = allRoutes;

            return View(customer);
        }

        [HttpPost]
        public IActionResult SelectRoute(string routeNumber)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToAction("Login");

            var customer = _context.Customers.Find(customerId);
            if (customer == null) return RedirectToAction("Login");

            customer.RouteNumber = routeNumber;
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ClearRoute()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToAction("Login");

            var customer = _context.Customers.Find(customerId);
            if (customer == null) return RedirectToAction("Login");

            customer.RouteNumber = null;
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}