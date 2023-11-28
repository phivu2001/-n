using DTO.Invoice;
using DTO.Stock;
using DTO.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IWarehouseService
    {
        #region Thông tin hóa đơn
        List<int> GetInvoiceSendMail();
        InvoiceDTO InVoiceDetail(long id);
        #endregion
        #region Thay đổi trạng thái xuất nhập kho
        bool ChangeStatusInWarehouse(long InWarehouseId, int status);
        bool ChangeStatusOutWarehouse(long OutWarehouseId, int status);
        #endregion
        #region Tạo hóa đơn mua bán hàng
        bool PurchaseCreate(long inWarehouseId);
        bool SellCreate(long outWarehouseId);
        #endregion
        #region Check tồn kho
        bool CheckOutOffStock(List<OutWarehousDetailDTO> input);
        #endregion
        #region In stock
        bool InWarehouseCreate(InWarehouseDTO input);
        bool InWarehouseUpdate(InWarehouseDTO input);
        bool InWarehouseDelete(long id);
        InWarehouseDTO InWarehouseGetById(long id);
        List<InWarehouseDTO> InWarehouseGetAll();
        #endregion

        #region Out stock
        bool OutWarehouseCreate(OutWarehouseDTO input);
        bool OutWarehouseUpdate(OutWarehouseDTO input);
        bool OutWarehouseDelete(long id);
        OutWarehouseDTO OutWarehouseGetById(long id);
        List<OutWarehouseDTO> OutWarehouseGetAll();
        #endregion
    }
}
