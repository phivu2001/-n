using DTO;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IUserService
    {
        bool Create(UserDTO input);
        bool Update(UserDTO input);
        bool Delete(long id);
        UserDTO GetById(long id);
        List<UserDTO> GetAll();
        UserDTO Login(string username, string pass);
    }
}
