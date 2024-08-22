using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration
{
    internal sealed class TshirtConfig : IEntityTypeConfiguration<Tshirt>
    {
        public void Configure(EntityTypeBuilder<Tshirt> builder)
        {
            builder.HasOne(x => x.PriceOffer)
                .WithOne(x => x.Tshirt)
                .HasForeignKey<PriceOffer>(x => x.TshirtId)
                .IsRequired();

            builder.HasMany(x => x.Reviews)
                .WithOne(x => x.Tshirt)
                .HasForeignKey(x => x.TshirtId)
                .IsRequired();
        }
    }
}
