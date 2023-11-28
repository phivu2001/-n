using BLL.IServices;
using DTO;
using DTO.Stock;
using Entities;
using Entities.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _dbContext;

        public StockService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public List<StockDTO> GetAll()
        {
            var stock = _dbContext.Stock.AsQueryable();
            var cate = _dbContext.Category.AsQueryable();
            return stock.Select(s => new StockDTO()
            {
                Id = s.Id,
                Code= s.Code,
                Name = s.Name,
                Price= s.Price,
                ImageURL= s.ImageURL,
                CategoryId= s.CategoryId,
                CategoryName = cate.FirstOrDefault(f=>f.Id == s.CategoryId).Name,
                Qty = _dbContext.InWarehouseStock.Where(w=>w.StockId == s.Id).Sum(m=>m.Quantity)
            }).ToList();
        }
        public bool Create(StockDTO input)
        {
            var Class = new StockEntities()
            {
                Code = input.Code,
                Name = input.Name,
                Price= input.Price,
                ImageURL= input.ImageURL,
                CategoryId= input.CategoryId,
            };
            _dbContext.Stock.Add(Class);
            return _dbContext.SaveChanges() > 0;
        }
        public bool Delete(long id)
        {
            var Stock = _dbContext.Stock.FirstOrDefault(x => x.Id == id);
            if (Stock == null)
            {
                throw new Exception("Không tìm thấy kho");
            }
            _dbContext.Stock.Remove(Stock);
            return _dbContext.SaveChanges() > 0;
        }

        public StockDTO GetById(long id)
        {
            var Class = _dbContext.Stock.FirstOrDefault(x => x.Id == id);
            if (Class == null)
            {
                throw new Exception("Không tìm thấy kho");
            }
            var result = new StockDTO()
            {
                Id = Class.Id,
                Code = Class.Code,
                Name = Class.Name,
                Price = Class.Price,
                ImageURL = Class.ImageURL,
                CategoryId = Class.CategoryId,
                CategoryName = _dbContext.Category.FirstOrDefault(f => f.Id == Class.CategoryId)?.Name,
            };
            return result;
        }

        public bool Update(StockDTO input)
        {
            var Stock = _dbContext.Stock.FirstOrDefault(x => x.Id == input.Id);
            Stock.Code = input.Code;
            Stock.Name = input.Name;
            Stock.Price = input.Price;
            Stock.ImageURL = input.ImageURL;
            Stock.CategoryId = input.CategoryId;
            return _dbContext.SaveChanges() > 0;
        }
    }
}
