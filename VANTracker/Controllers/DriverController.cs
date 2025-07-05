using Microsoft.AspNetCore.Mvc;
using VANTracker.DataAccess.Data;
using VANTracker.DataModels.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace VANTracker.Controllers
{
    public class DriverController : Controller
    {
        private readonly AppDbContext _context;

        public DriverController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(Driver driver)
        {
            if (ModelState.IsValid)
            {
                driver.RouteNumber = null; // Route selection comes after login
                _context.Drivers.Add(driver);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(driver);
        }

        [HttpPost]
        public IActionResult Login(string mobileNumber, string password)
        {
            var driver = _context.Drivers.FirstOrDefault(d => d.MobileNumber == mobileNumber && d.Password == password);
            if (driver != null)
            {
                HttpContext.Session.SetInt32("DriverId", driver.Id);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials.";
            return View();
        }

        public IActionResult Dashboard()
        {
            int? driverId = HttpContext.Session.GetInt32("DriverId");
            if (driverId == null)
                return RedirectToAction("Login");

            var driver = _context.Drivers.Find(driverId);
            if (driver == null)
                return RedirectToAction("Login");

            // Get all taken routes except this driver
            var takenRoutes = _context.Drivers
                .Where(d => d.RouteNumber != null && d.Id != driver.Id)
                .Select(d => d.RouteNumber)
                .ToList();

            // Get available routes (1 to 10)
            var allRoutes = Enumerable.Range(1, 10).Select(r => r.ToString()).ToList();
            var availableRoutes = allRoutes.Except(takenRoutes).ToList();

            ViewBag.AvailableRoutes = availableRoutes;
            return View(driver);
        }

        [HttpPost]
        public IActionResult SelectRoute(string routeNumber)
        {
            int? driverId = HttpContext.Session.GetInt32("DriverId");
            if (driverId == null)
                return RedirectToAction("Login");

            var driver = _context.Drivers.Find(driverId);
            if (driver == null)
                return RedirectToAction("Login");

            // Check if the route is already taken
            bool isTaken = _context.Drivers.Any(d => d.RouteNumber == routeNumber && d.Id != driver.Id);
            if (!isTaken)
            {
                driver.RouteNumber = routeNumber;
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ClearRoute()
        {
            int? driverId = HttpContext.Session.GetInt32("DriverId");
            if (driverId == null)
                return RedirectToAction("Login");

            var driver = _context.Drivers.Find(driverId);
            if (driver == null)
                return RedirectToAction("Login");

            driver.RouteNumber = null;
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