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
        private readonly IUnitOfWork _unitOfWork;
        public ManufactureController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<Manufacture>> Get()
        {
            var random = new Random();
            var _randomName = random.Next(1, 100);
            var carName = "BMW " + _randomName;
            var result = await _unitOfWork.GetRepository<IManufactureRepository>(true).GetAllAsync(); //await _manufactureRepository.GetAllAsync();
            //return result;
            var modelToDelete = result.Where(x=>x.Id != default).FirstOrDefault();
            
            modelToDelete.Name = "modelToDelete.Name "+ _randomName;
           // Console.WriteLine(modelToDelete.Name);

             var deleteAysnc = await _unitOfWork.GetRepository<IManufactureRepository>(true).DeleteAsync(modelToDelete);
            var id = Guid.NewGuid();
            
            var model = new Manufacture
            {
               Id = id,
               Name = carName,
               Description = carName,
               CreatedBy = "Menzi Manqele",
               DateCreated = DateTime.Now
            };

            var categoryModel = new Category
            {
                Id = 2,
                Name = "Politics" + _randomName ,
                DisplayOrder = 7
            };
           // await _unitOfWork.GetRepository<ICategoryRepository>(false).AddSync(categoryModel);
           
           
           // await _unitOfWork.GetRepository<IManufactureRepository>(false).AddSync(model);
            await _unitOfWork.GetRepository<IManufactureRepository>(false).UpdateAsync(modelToDelete);
            await _unitOfWork.SaveChangesAsync();

            /*await _unitOfWork.RollbackTransactionAsync();
            await _unitOfWork.CommitTransactionAsync();*/
            var categoriesResults = await _unitOfWork.GetRepository<ICategoryRepository>(false).GetAllAsync();
           
            return new List<Manufacture>();
        }
    }
}

