using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QuanLyCTDT.Commons
{
    public class EnumCommon
    {
        public enum EGender
        {
            [Description("Nam")]
            Male = 1,
            [Description("Nữ")]
            FeMale = 2,
        }

        public enum ERole
        {
            [Description("Quản trị")]
            Admin = 1,
            [Description("Nhân viên")]
            Employee = 2,
        }
        public enum EStockStatus
        {
            [Description("Tốt")]
            Good = 1,
            [Description("Bình thường")]
            Normal = 2,
            [Description("Hư hỏng")]
            Bad = 3,
        }

        public enum EInvoiceStatus
        {
            [Description("Chờ duyệt")]
            Inprocessing = 0,
            [Description("Đã duyệt")]
            Approved = 1,
            [Description("Hủy bỏ")]
            Cancelled = 2
        }
        public enum EWarehouseStatus
        {
            [Description("Chờ lên hóa đơn")]
            Inprocessing = 0,
            [Description("Đã duyệt hóa đơn")]
            Approved = 1,
            [Description("Hủy bỏ")]
            Cancelled = 2
        }
    }
}