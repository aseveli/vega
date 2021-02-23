using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IModelRepository
    {
        void Add(Model model);
        Task<Model> GetModel(int id);
        Task<List<Model>> GetAllModels();
        void Remove(Model model);
        void Update(Model model);
    }
}