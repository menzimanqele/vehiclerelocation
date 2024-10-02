using Microsoft.AspNetCore.Mvc;
using VehicleRelocation.Api.Domain.Entities;
using VehicleRelocation.Api.Domain.Interfaces;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VehicleRelocation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManufactureController : ControllerBase
    {
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ManufactureController(IManufactureRepository manufactureRepository, IUnitOfWork unitOfWork)
        {
            _manufactureRepository = manufactureRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<Manufacture>> Get()
        {
            var random = new Random();
            var _randomName = random.Next(1, 100);
            var carName = "BMW " + _randomName;
            var result = await _unitOfWork.GetRepository<IManufactureRepository>(false).GetAllAsync(); //await _manufactureRepository.GetAllAsync();
            //return result;
            var modelToDelete = result.First();
            Console.WriteLine(modelToDelete.Name);

            var deleteAysnc = await _unitOfWork.GetRepository<IManufactureRepository>(true).DeleteAysnc(modelToDelete);
             //var d = await _manufactureRepository.DeleteAysnc(modelToDelete);
             
             
           // await _manufactureRepository.SaveChangesAsync();
           
            var model = new Manufacture
            {
               Id = Guid.NewGuid(),
               Name = carName,
               Description = carName,
               CreatedBy = "Menzi Manqele",
               DateCreated = DateTime.Now
            };

             await _unitOfWork.GetRepository<IManufactureRepository>(false).AddSync(model);
          //  _manufactureRepository.AddSync(model);
             //await _manufactureRepository.SaveChangesAsync();
           //if(result is not null)
           // {
           //     return result.ToList();
           // }
           
           await _unitOfWork.SaveChangesAsync();

           
            return new List<Manufacture>();
        }
    }
}

