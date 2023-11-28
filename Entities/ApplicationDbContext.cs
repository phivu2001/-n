using Entities.Category;
using Entities.Customer;
using Entities.Invoice;
using Entities.Stock;
using Entities.User;
using Entities.Vendor;
using Entities.Warehouse;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=QLKDbContext")
        {
            //((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        public virtual DbSet<UserEntities> User { get; set; }
        public virtual DbSet<VendorEntities> Vendor { get; set; }
        public virtual DbSet<CustomerEntities> Customer { get; set; }
        public virtual DbSet<CategoryEntities> Category { get; set; }
        public virtual DbSet<StockEntities> Stock { get; set; }
        public virtual DbSet<InWarehouseEntities> InWarehouse { get; set; }
        public virtual DbSet<InWarehouseStockEntities> InWarehouseStock { get; set; }
        public virtual DbSet<OutWarehouseEntities> OutWarehouse { get; set; }
        public virtual DbSet<OutWarehouseStockEntities> OutWarehouseStock { get; set; }
        public virtual DbSet<InvoiceEntities> Invoice { get; set; }
        public virtual DbSet<InvoiceDetailEntities> InvoiceDetail { get; set; }
    }
}
