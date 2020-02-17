using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Watermango.Models;
using Watermango.Repository;
using System.Threading;

namespace Watermango.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class PlantController : ControllerBase 
    {
        private PlantRepository repository;

        public PlantController(PlantRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public List<Plant> Get() {
            return repository.GetPlants();
        }

        [HttpGet("{id}")]
        public ActionResult<Plant> GetById(long id) 
        {
            Plant p = repository.GetPlantById(id);

            if (p == null) 
            {
                // Error 404 not found
                return NotFound();
            }

            return p;
        }

        [HttpPost("{id}/start")]
        public ActionResult<Plant> StartWatering(long id) 
        {
            Plant p = this.repository.GetPlantById(id);

            // Checking existing and in valid state before starting...
            if (p == null) 
            {
                return NotFound();
            }
            else if (p.State == PlantStatus.Watering || p.State == PlantStatus.Resting) 
            {
                // Internal server error
                return StatusCode(500, "Cannot water plant at this time");
            } 
            else 
            {
                p = this.repository.UpdateStatus(id, PlantStatus.Watering);
            }
            
            return StatusCode(200, p.Name + " has started watering.");
        }

        [HttpPost("{id}/stop")]
        public ActionResult<Plant> StopWatering(long id) 
        {
            // Update state of plant to idle
            Plant p = this.repository.GetPlantById(id);

            if (p == null) 
            {
                return NotFound();
            } 
            else if (p.State == PlantStatus.Idle || p.State == PlantStatus.Resting) {
                return StatusCode(500, "Cannot stop watering plant at this time.");
            }
            else 
            {
                p = this.repository.UpdateStatus(id, PlantStatus.Resting);
            }

            return StatusCode(200, p.Name + " is now resting.");
        } 
    }
}