using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Models;
using vega.Persistence;

namespace vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly VegaDbContext _context;
        public VehiclesController(IMapper mapper, VegaDbContext context)
        {
            this._context = context;
            this._mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource saveVehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await this._context.Models.FindAsync(saveVehicleResource.ModelId);

            if (model == null)
            {
                ModelState.AddModelError("ModelId", "Model Id is invalid.");

                return BadRequest(ModelState);
            }

            var vehicle = this._mapper.Map<SaveVehicleResource, Vehicle>(saveVehicleResource);

            //go through our features from the save vehicle resource and add to vehicle resource
            foreach (int featureId in saveVehicleResource.Features)
            {
                var newFeature = await this._context.Features.FindAsync(featureId);

                if (newFeature != null)
                {
                    vehicle.Features.Add(newFeature);
                }
                else
                {
                    ModelState.AddModelError("feature", $"Feature Id:{featureId} is not Valid.");

                    return BadRequest(ModelState);
                }
            }

            vehicle.LastUpdate = DateTime.Now;

            this._context.Vehicles.Add(vehicle);

            await this._context.SaveChangesAsync();

            var result = this._mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource saveVehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await this._context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            this._mapper.Map<SaveVehicleResource, Vehicle>(saveVehicleResource, vehicle);

            //remove unselected features
            var removedFeatures = vehicle.Features.Where(f => !saveVehicleResource.Features.Contains(f.Id)).ToList();

            foreach (var f in removedFeatures)
            {
                vehicle.Features.Remove(f);
            }

            //add new features
            var addedFeatures = saveVehicleResource.Features.Where(sf => !vehicle.Features.Any(f => sf == f.Id)).ToList();

            foreach (int newFeatureId in addedFeatures)
            {
                var newFeature = await this._context.Features.FindAsync(newFeatureId);

                if (newFeature != null)
                {
                    vehicle.Features.Add(newFeature);
                }
                else
                {
                    ModelState.AddModelError("feature", $"Feature Id:{newFeatureId} is not Valid.");

                    return BadRequest(ModelState);
                }
            }

            vehicle.LastUpdate = DateTime.Now;

            this._context.Vehicles.Update(vehicle);

            await this._context.SaveChangesAsync();

            var result = this._mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await this._context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            this._context.Vehicles.Remove(vehicle);
            await this._context.SaveChangesAsync();

            return Ok(id);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await this._context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var saveVehicleResource = this._mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

            return Ok(saveVehicleResource);
        }
    }
}