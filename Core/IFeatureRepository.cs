using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IFeatureRepository
    {
        void Add(Feature feature);
        Task<Feature> GetFeature(int id, bool includeRelated = true);
        Task<List<Feature>> GetAllFeatures();
        void Remove(Feature feature);
        void Update(Feature feature);
    }
}