using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;

namespace vega.Persistence
{
    public class MakeRepository : IMakeRepository
    {
        private readonly VegaDbContext _context;
        public MakeRepository(VegaDbContext context)
        {
            this._context = context;
        }
        public void Add(Make make)
        {
            this._context.Add(make);
        }
        public async Task<Make> GetMake(int id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await this._context.Makes.FindAsync(id);
            }
            else
            {
                return await this._context.Makes.Include(m => m.Models).SingleOrDefaultAsync(m => m.Id == id);
            }
        }
        public async Task<List<Make>> GetAllMakes(bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await this._context.Makes.ToListAsync();
            }
            else
            {
                return await this._context.Makes.Include(m => m.Models).ToListAsync();
            }
        }
        public void Remove(Make make)
        {
            this._context.Remove(make);
        }
        public void Update(Make make)
        {
            this._context.Update(make);
        }
    }
}