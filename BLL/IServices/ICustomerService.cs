using DTO;
using DTO.Customer;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface ICustomerService
    {
        bool Create(CustomerDTO input);
        bool Update(CustomerDTO input);
        bool Delete(long id);
        CustomerDTO GetById(long id);
        List<CustomerDTO> GetAll();
    }
}
