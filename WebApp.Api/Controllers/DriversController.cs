using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Auth.Authentication;
using WebApp.Domain.Models;

namespace WebApp.Api.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiAuthenticationFilter))]
    public class DriversController : ControllerBase
    {
        private static List<Driver> drivers = new List<Driver>();

        private readonly ILogger<DriversController> _logger;

        public DriversController(ILogger<DriversController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDrivers()
        {
            var items = drivers.Where(x => x.Status == 1).ToList();
            return Ok(items);
        }
        [HttpPost]
        public IActionResult CreateDriver(Driver data)
        {
            if (ModelState.IsValid)
            {
                drivers.Add(data);

                return CreatedAtAction("GetDriver", new { data.Id }, data);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public IActionResult GetDriver(Guid id)
        {
            var item = drivers.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDriver(Guid id, Driver item)
        {
            if (id != item.Id)
                return BadRequest();

            var existItem = drivers.FirstOrDefault(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            existItem.FirstName = item.FirstName;
            existItem.LastName = item.LastName;
            existItem.DriverNumber = item.DriverNumber;
            existItem.WorldChampionships = item.WorldChampionships;


            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDriver(Guid id)
        {
            var existItem = drivers.FirstOrDefault(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            existItem.Status = 0;

            return Ok(existItem);
        }
    }
}
