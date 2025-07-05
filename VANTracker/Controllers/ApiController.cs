using Microsoft.AspNetCore.Mvc;
using VANTracker.DataAccess.Data;
using System.Linq;

namespace VANTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("update")]
        public IActionResult UpdateLocation([FromBody] LocationUpdateDto dto)
        {
            var driver = _context.Drivers.FirstOrDefault(d => d.MobileNumber == dto.MobileNumber);
            if (driver != null)
            {
                driver.Latitude = dto.Lat;
                driver.Longitude = dto.Lng;
                driver.LastUpdated = DateTime.UtcNow;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("{routeNumber}")]
        public IActionResult GetDriverLocation(string routeNumber)
        {
            var driver = _context.Drivers.FirstOrDefault(d => d.RouteNumber == routeNumber);
            if (driver == null || driver.Latitude == null || driver.Longitude == null)
                return NotFound();

            return Ok(new
            {
                driver.Latitude,
                driver.Longitude
            });
        }
    }
    public class LocationUpdateDto
    {
        public string MobileNumber { get; set; }
        public string RouteNumber { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
