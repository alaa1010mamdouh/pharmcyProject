using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmcy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmcy.Repository.Data.Configuration
{
    public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
    {
        public void Configure(EntityTypeBuilder<Medicine> builder)
        {
            builder.ToTable("Medicines");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Description)
                .HasMaxLength(500);

            builder.Property(m => m.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(m => m.StockQuantity)
                .IsRequired();

            builder.Property(m => m.ExpiryDate)
                .IsRequired();

            builder.Property(m => m.Category)
                .HasMaxLength(50)
                .HasDefaultValue("General");

            builder.Property(m => m.Manufacturer)
                .HasMaxLength(100);

            builder.Property(m => m.Barcode)
                .HasMaxLength(20);

            // Index for Barcode (Unique)
            builder.HasIndex(m => m.Barcode)
                .IsUnique()
                .HasFilter("[Barcode] IS NOT NULL");
        }
    }
}
