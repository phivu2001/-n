using BLL.IServices;
using DTO;
using DTO.User;
using Entities;
using Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<UserDTO> GetAll()
        {
            var subjects = _dbContext.User.AsQueryable();
            return subjects.Select(s => new UserDTO()
            {
                Id = s.Id,
                FullName = s.FullName,
                UserName = s.UserName,
                Phone = s.Phone,
                Email = s.Email,
                BirthDay = s.BirthDay,
                Password = s.Password,
                Address = s.Address,
                Role = s.Role
            }).ToList();
        }
        public bool Create(UserDTO input)
        {
            var User = new UserEntities()
            {
                Id = input.Id,
                FullName = input.FullName,
                UserName = input.UserName,
                Phone = input.Phone,
                Email = input.Email,
                BirthDay = input.BirthDay,
                Password = input.Password,
                Address = input.Address,
                Role = input.Role,
            };
            _dbContext.User.Add(User);
            return _dbContext.SaveChanges() > 0;
        }

        public bool Delete(long id)
        {
            var User = _dbContext.User.FirstOrDefault(x => x.Id == id);
            if (User == null)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            _dbContext.User.Remove(User);
            return _dbContext.SaveChanges() > 0;
        }

        public UserDTO GetById(long id)
        {
            var User = _dbContext.User.FirstOrDefault(x => x.Id == id);
            var result = new UserDTO()
            {
                Id = User.Id,
                FullName = User.FullName,
                UserName = User.UserName,
                Phone = User.Phone,
                Email = User.Email,
                BirthDay = User.BirthDay,
                Password = User.Password,
                Address = User.Address
            };
            return result;
        }

        public bool Update(UserDTO input)
        {
            var userEntity = _dbContext.User.FirstOrDefault(x => x.Id == input.Id);
            if (userEntity != null)
            {
                userEntity.FullName = input.FullName;
                userEntity.UserName = input.UserName;
                userEntity.Phone = input.Phone;
                userEntity.Email = input.Email;
                userEntity.BirthDay = input.BirthDay;
                userEntity.Password = input.Password;
                userEntity.Address = input.Address;
                userEntity.Role = input.Role;
                return _dbContext.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }
        public UserDTO Login(string username, string pass)
        {
            return _dbContext.User.AsQueryable().Where(w => (w.UserName.ToLower() == username.Trim().ToLower() || w.Email.ToLower() == username.Trim().ToLower()) && w.Password == pass)
                .Select(s => new UserDTO()
            {
                Id = s.Id,
                FullName = s.FullName,
                UserName = s.UserName,
                Phone = s.Phone,
                Email = s.Email,
                BirthDay = s.BirthDay,
                Password = s.Password,
                Address = s.Address,
                Role = s.Role,
            }).FirstOrDefault();
        }
    }
}
