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
    public class TourListsController: ControllerBase
    {
        private TravelDbContext _dbContext;

        public TourListsController(TravelDbContext travelDbContext)
        {
            _dbContext = travelDbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.TourLists);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TourList tourList)
        {
            await _dbContext.TourLists.AddAsync(tourList);
            await _dbContext.SaveChangesAsync();

            return Ok(tourList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tourList = await _dbContext.TourLists.SingleOrDefaultAsync(t => t.Id == id);

            if (tourList == null)
                return NotFound();

            _dbContext.TourLists.Remove(tourList);
            await _dbContext.SaveChangesAsync();

            return Ok(tourList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] TourList tourList)
        {
            _dbContext.Update(tourList);

            await _dbContext.SaveChangesAsync();

            return Ok(tourList);
        }



    }
}
