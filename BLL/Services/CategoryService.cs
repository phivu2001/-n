using BLL.IServices;
using DTO;
using DTO.Category;
using DTO.Stock;
using Entities;
using Entities.Category;
using Entities.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public List<CategoryDTO> GetAll()
        {
            var stock = _dbContext.Category.AsQueryable();
            return stock.Select(s => new CategoryDTO()
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
        public bool Create(CategoryDTO input)
        {
            var Class = new CategoryEntities()
            {
                Name = input.Name
            };
            _dbContext.Category.Add(Class);
            return _dbContext.SaveChanges() > 0;
        }
        public bool Delete(long id)
        {
            var Stock = _dbContext.Category.FirstOrDefault(x => x.Id == id);
            if (Stock == null)
            {
                throw new Exception("Không tìm thấy nhóm vật tư");
            }
            _dbContext.Category.Remove(Stock);
            return _dbContext.SaveChanges() > 0;
        }

        public CategoryDTO GetById(long id)
        {
            var Class = _dbContext.Category.FirstOrDefault(x => x.Id == id);
            if (Class == null)
            {
                throw new Exception("Không tìm thấy nhóm vật tư");
            }
            var result = new CategoryDTO()
            {
                Id = Class.Id,
                Name = Class.Name
            };
            return result;
        }

        public bool Update(CategoryDTO input)
        {
            var Stock = _dbContext.Category.FirstOrDefault(x => x.Id == input.Id);
            Stock.Name = input.Name;
            return _dbContext.SaveChanges() > 0;
        }
    }
}
