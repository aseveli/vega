using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;

namespace vega.Persistence
{
    public class ModelRepository : IModelRepository
    {
        private readonly VegaDbContext _context;
        public ModelRepository(VegaDbContext context)
        {
            this._context = context;
        }
        public void Add(Model model)
        {
            this._context.Add(model);
        }
        public async Task<List<Model>> GetAllModels()
        {
            return await this._context.Models.ToListAsync();
        }
        public async Task<Model> GetModel(int id)
        {
            return await this._context.Models.FindAsync(id);
        }
        public void Remove(Model model)
        {
            this._context.Remove(model);
        }
        public void Update(Model model)
        {
            this._context.Update(model);
        }
    }
}