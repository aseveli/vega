using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IMakeRepository
    {
        void Add(Make make);
        Task<Make> GetMake(int id, bool includeRelated = true);
        Task<List<Make>> GetAllMakes(bool includeRelated = true);
        void Remove(Make make);
        void Update(Make make);
    }
}