using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;

namespace vega.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFeatureRepository _featureRepository;
        public FeaturesController(IMapper mapper, IFeatureRepository featureRepository)
        {
            this._featureRepository = featureRepository;
            this._mapper = mapper;
        }

        [HttpGet("/api/features")]
        public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
        {
            var features = await this._featureRepository.GetAllFeatures();
            //var features = await this._context.Features.ToListAsync();

            return this._mapper.Map<List<Feature>, List<KeyValuePairResource>>(features);
        }
    }
}