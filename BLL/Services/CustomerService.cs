using BLL.IServices;
using DTO;
using DTO.Customer;
using DTO.User;
using DTO.Vendor;
using Entities;
using Entities.Customer;
using Entities.User;
using Entities.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<CustomerDTO> GetAll()
        {
            var subjects = _dbContext.Customer.AsQueryable();
            return subjects.Select(s => new CustomerDTO()
            {
                Id = s.Id,
                FullName = s.FullName,
                UserName = s.UserName,
                Phone = s.Phone,
                Email = s.Email,
                BirthDay = s.BirthDay,
                Password = s.Password,
                Address = s.Address,
            }).ToList();
        }
        public bool Create(CustomerDTO input)
        {
            var User = new CustomerEntities()
            {
                Id = input.Id,
                FullName = input.FullName,
                UserName = input.UserName,
                Phone = input.Phone,
                Email = input.Email,
                BirthDay = input.BirthDay,
                Password = input.Password,
                Address = input.Address,
            };
            _dbContext.Customer.Add(User);
            return _dbContext.SaveChanges() > 0;
        }

        public bool Delete(long id)
        {
            var User = _dbContext.Customer.FirstOrDefault(x => x.Id == id);
            if (User == null)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            _dbContext.Customer.Remove(User);
            return _dbContext.SaveChanges() > 0;
        }

        public CustomerDTO GetById(long id)
        {
            var User = _dbContext.Customer.FirstOrDefault(x => x.Id == id);
            var result = new CustomerDTO()
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

        public bool Update(CustomerDTO input)
        {
            var userEntity = _dbContext.Customer.FirstOrDefault(x => x.Id == input.Id);
            if (userEntity != null)
            {
                userEntity.FullName = input.FullName;
                userEntity.UserName = input.UserName;
                userEntity.Phone = input.Phone;
                userEntity.Email = input.Email;
                userEntity.BirthDay = input.BirthDay;
                userEntity.Password = input.Password;
                userEntity.Address = input.Address;
                return _dbContext.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
