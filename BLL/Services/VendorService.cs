using BLL.IServices;
using DTO;
using DTO.User;
using DTO.Vendor;
using Entities;
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
    public class VendorService : IVendorService
    {
        private readonly ApplicationDbContext _dbContext;

        public VendorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<VendorDTO> GetAll()
        {
            var subjects = _dbContext.Vendor.AsQueryable();
            return subjects.Select(s => new VendorDTO()
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
        public bool Create(VendorDTO input)
        {
            var User = new VendorEntities()
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
            _dbContext.Vendor.Add(User);
            return _dbContext.SaveChanges() > 0;
        }

        public bool Delete(long id)
        {
            var User = _dbContext.Vendor.FirstOrDefault(x => x.Id == id);
            if (User == null)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            _dbContext.Vendor.Remove(User);
            return _dbContext.SaveChanges() > 0;
        }

        public VendorDTO GetById(long id)
        {
            var User = _dbContext.Vendor.FirstOrDefault(x => x.Id == id);
            var result = new VendorDTO()
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

        public bool Update(VendorDTO input)
        {
            var userEntity = _dbContext.Vendor.FirstOrDefault(x => x.Id == input.Id);
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
