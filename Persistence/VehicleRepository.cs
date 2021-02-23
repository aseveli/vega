using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;

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