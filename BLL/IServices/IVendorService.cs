using DTO;
using DTO.User;
using DTO.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IVendorService
    {
        bool Create(VendorDTO input);
        bool Update(VendorDTO input);
        bool Delete(long id);
        VendorDTO GetById(long id);
        List<VendorDTO> GetAll();
    }
}
