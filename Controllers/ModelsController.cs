using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;

namespace vega.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IModelRepository _modelRepository;
        public ModelsController(IMapper mapper, IModelRepository modelRepository)
        {
            this._modelRepository = modelRepository;
            this._mapper = mapper;
        }

        [HttpGet("/api/models")]
        public async Task<IEnumerable<ModelResource>> GetModels()
        {
            var models = await this._modelRepository.GetAllModels();

            return _mapper.Map<List<Model>, List<ModelResource>>(models);
        }
    }
}