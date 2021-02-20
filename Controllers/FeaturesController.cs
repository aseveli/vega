using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Models;
using vega.Persistence;

namespace vega.Controllers {
    public class FeaturesController: Controller {
        private readonly IMapper _mapper;
        private readonly VegaDbContext _context;
        public FeaturesController(VegaDbContext context, IMapper mapper) {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("/api/features")]
        public async Task < IEnumerable < KeyValuePairResource >> GetFeatures() {
            var features = await this._context.Features.ToListAsync();

            return this._mapper.Map < List < Feature > , List < KeyValuePairResource >> (features);
        }
    }
}