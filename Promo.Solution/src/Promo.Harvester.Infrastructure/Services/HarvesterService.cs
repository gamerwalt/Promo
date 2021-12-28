using Promo.Harvester.Infrastructure;
using System.Threading.Tasks;

namespace Promo.Harvester.Infrastruture.Services
{
    //ideally, we need to put this in a queue to make things faster
    //from the queue, we add it to the database 
    //but then also make sure we send out an event that will allow us to get that result into individual campaign databases/tables
    //but the campaign service needs to know about this
    public interface IHarvesterService
    {
        Task AddAsync(string json);
    }

    public class HarvesterService : IHarvesterService
    {
        private readonly IHarvesterRepository _repository;

        public HarvesterService(IHarvesterRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(string json)
        {
            await _repository.CreateHarvestedRecordAsync(json);
        }
    }
}
