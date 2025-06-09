using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionsApi.Models;

namespace TransactionsApi.Mappings
{
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(c => c.Name)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnType("varchar(250)")
                .IsRequired();

            builder.Property(c => c.PasswordHash)
                .HasColumnType("varchar(320)")
                .IsRequired();

            builder.HasOne(c => c.Wallet)
                .WithOne(w => w.Client)
                .HasForeignKey<Wallet>(w => w.ClientId);

        }
    }
}
