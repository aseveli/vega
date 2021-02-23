using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;

namespace vega.Persistence
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly VegaDbContext _context;
        public FeatureRepository(VegaDbContext context)
        {
            this._context = context;
        }
        public void Add(Feature feature)
        {
            this._context.Add(feature);
        }
        public async Task<Feature> GetFeature(int id, bool includeRelated = true)
        {
            return await this._context.Features.FindAsync(id);
        }
        public async Task<List<Feature>> GetAllFeatures()
        {
            return await this._context.Features.ToListAsync();
        }
        public void Remove(Feature feature)
        {
            this._context.Remove(feature);
        }
        public void Update(Feature feature)
        {
            this._context.Update(feature);
        }
    }
}