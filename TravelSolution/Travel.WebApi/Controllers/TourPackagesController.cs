using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Data.Contexts;
using Travel.Domain.Entities;

namespace Travel.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourPackagesController:ControllerBase
    {
        private TravelDbContext _travelDbContext;

        public TourPackagesController(TravelDbContext travelDbContext)
        {
            _travelDbContext = travelDbContext;        
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_travelDbContext.TourPackages);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TourPackage tourPackage)
        {
            await _travelDbContext.TourPackages.AddAsync(tourPackage);
            await _travelDbContext.SaveChangesAsync();

            return Ok(tourPackage);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tourPackage = await _travelDbContext.TourPackages.SingleOrDefaultAsync(s => s.Id == id);

            if (tourPackage == null)
                return NotFound();

            _travelDbContext.TourPackages.Remove(tourPackage);
            await _travelDbContext.SaveChangesAsync();

            return Ok(tourPackage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TourPackage tourPackage)
        {
            _travelDbContext.Update(tourPackage);

            await _travelDbContext.SaveChangesAsync();

            return Ok(tourPackage);
        }

    }
}
 