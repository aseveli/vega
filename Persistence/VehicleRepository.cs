using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using vega.Extensions;

namespace vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;
        public VehicleRepository(VegaDbContext context)
        {
            this._context = context;
        }
        public void Add(Vehicle vehicle)
        {
            this._context.Add(vehicle);
        }
        public async Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery qryObject)
        {
            var query = this._context.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .AsQueryable();

            if (qryObject.MakeId.HasValue)
            {
                query = query.Where(v => v.Model.MakeId == qryObject.MakeId.Value);
            }

            if (qryObject.ModelId.HasValue)
            {
                query = query.Where(v => v.Model.Id == qryObject.ModelId.Value);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,
                ["id"] = v => v.Id,
            };

            query = query.ApplyOrdering(qryObject, columnsMap);

            return await query.ToListAsync();
        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            Vehicle vehicle = null;

            if (!includeRelated)
            {
                vehicle = await this._context.Vehicles.FindAsync(id);
            }
            else
            {
                vehicle = await this._context.Vehicles
                    .Include(v => v.Features)
                    .Include(v => v.Model)
                        .ThenInclude(m => m.Make)
                    .SingleOrDefaultAsync(v => v.Id == id);
            }

            return vehicle;
        }
        public async Task<Vehicle> GetVehicleWithModel(int id)
        {
            var vehicle = await this._context.Vehicles
                    .Include(v => v.Model)
                    .SingleOrDefaultAsync(v => v.Id == id);

            return vehicle;
        }
        public void Update(Vehicle vehicle)
        {
            this._context.Update(vehicle);
        }
        public void Remove(Vehicle vehicle)
        {
            this._context.Remove(vehicle);
        }
    }
}