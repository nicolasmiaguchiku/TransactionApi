using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionsApi.Models;

namespace TransactionsApi.Mappings
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder) 
        {
            builder.Property(t => t.Title)
                .HasColumnType("varchar(250)")
                .IsRequired();

            builder.Property(t => t.Type)
                .HasColumnType("varchar(250)")
                .IsRequired();

            builder.Property(t => t.Descriptor)
                .HasColumnType("varchar(250)");

            builder.Property(t => t.Date)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(t => t.Amount)
               .HasColumnType("numeric(38,2)")
               .IsRequired(); 
        }
    }
}
