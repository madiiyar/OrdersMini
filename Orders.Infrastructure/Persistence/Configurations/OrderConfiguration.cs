using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> b)
        {
            b.ToTable("Orders");
            b.HasKey(x => x.Id);
            b.Property(x => x.CustomerName).IsRequired().HasMaxLength(200);
            b.Property(x => x.TotalAmount).HasPrecision(18, 2);
            b.Property(x => x.CreatedAt)
             .HasDefaultValueSql("now() at time zone 'utc'")
             .IsRequired();
            b.Property(x => x.Status).HasConversion<string>().IsRequired();
        }
    }
}
