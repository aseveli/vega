using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;

namespace vega.Controllers
{
    public class MakesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMakeRepository _makeRepository;
        public MakesController(IMapper mapper, IMakeRepository makeRepository)
        {
            this._makeRepository = makeRepository;
            this._mapper = mapper;
        }

        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var makes = await this._makeRepository.GetAllMakes(includeRelated: true);

            return _mapper.Map<List<Make>, List<MakeResource>>(makes);
        }
    }
}