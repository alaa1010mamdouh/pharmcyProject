using Microsoft.EntityFrameworkCore;
using Pharmcy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pharmcy.Repository.Data
{
    public class PharmcyContext: DbContext
    {
        public PharmcyContext(DbContextOptions<PharmcyContext> options):base(options)
        {

        }
        
        public DbSet<Medicine> medicines { get; set; }
        
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<InvoiceItem> invoiceItems { get; set; }
        public DbSet<Prescription> prescriptions { get; set; }
        public DbSet<PrescriptionItem> prescriptionItems { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
