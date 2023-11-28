using DTO;
using DTO.Category;
using DTO.Stock;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface ICategoryService
    {
        bool Create(CategoryDTO input);
        bool Update(CategoryDTO input);
        bool Delete(long id);
        CategoryDTO GetById(long id);
        List<CategoryDTO> GetAll();
    }
}
