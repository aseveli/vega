using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using System.Collections.Generic;

namespace vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IFeatureRepository _featureRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModelRepository _modelRepository;
        public VehiclesController(IMapper mapper,
            IVehicleRepository vehicleRepository,
            IFeatureRepository featureRepository,
            IModelRepository modelRepository,
            IUnitOfWork unitOfWork)
        {
            this._featureRepository = featureRepository;
            this._mapper = mapper;
            this._modelRepository = modelRepository;
            this._unitOfWork = unitOfWork;
            this._vehicleRepository = vehicleRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource saveVehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await this._modelRepository.GetModel(saveVehicleResource.ModelId);

            if (model == null)
            {
                ModelState.AddModelError("ModelId", "Model Id is invalid.");

                return BadRequest(ModelState);
            }

            var vehicle = this._mapper.Map<SaveVehicleResource, Vehicle>(saveVehicleResource);

            //go through our features from the save vehicle resource and add to vehicle resource
            foreach (int featureId in saveVehicleResource.Features)
            {
                var newFeature = await this._featureRepository.GetFeature(featureId);

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

            this._vehicleRepository.Add(vehicle);

            await this._unitOfWork.CompleteAsync();

            vehicle = await this._vehicleRepository.GetVehicle(vehicle.Id);

            var result = this._mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource saveVehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await this._vehicleRepository.GetVehicle(id);

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
                var newFeature = await this._featureRepository.GetFeature(newFeatureId);

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

            //this._vehicleRepository.Update(vehicle);

            await this._unitOfWork.CompleteAsync();

            vehicle = await this._vehicleRepository.GetVehicle(vehicle.Id);

            var result = this._mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await this._vehicleRepository.GetVehicle(id, includeRelated: false);

            if (vehicle == null)
            {
                return NotFound();
            }

            this._vehicleRepository.Remove(vehicle);

            await this._unitOfWork.CompleteAsync();

            return Ok(id);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await this._vehicleRepository.GetVehicle(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var vehicleResource = this._mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(vehicleResource);
        }
        /// <summary>
        /// Get all vehicles.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<VehicleResource>> GetVehicles(VehicleQueryResource vehicleQueryResource)
        {
            var filter = this._mapper.Map<VehicleQueryResource, VehicleQuery>(vehicleQueryResource);

            var vehicles = await this._vehicleRepository.GetVehicles(filter);

            return this._mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleResource>>(vehicles);
        }
    }
}