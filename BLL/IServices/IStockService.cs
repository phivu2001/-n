using DTO;
using DTO.Stock;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IStockService
    {
        bool Create(StockDTO input);
        bool Update(StockDTO input);
        bool Delete(long id);
        StockDTO GetById(long id);
        List<StockDTO> GetAll();
    }
}
