using Chroma.Domain.Entities;
using Chroma.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chroma.Infrastructure.DataAccess.EntityConfigurations;

public class PaletteConfigurations : IEntityTypeConfiguration<Palette>
{
    public void Configure(EntityTypeBuilder<Palette> builder)
    {
        builder.HasKey(p => p.PaletteId);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

        builder.OwnsMany(palette => palette.Colors, colorBuilder =>
        {
            colorBuilder.ToTable("PaletteColors");

            colorBuilder.Property<long>("PaletteColorId").ValueGeneratedOnAdd();
            colorBuilder.HasKey("PaletteColorId");

            colorBuilder.WithOwner().HasForeignKey(nameof(Palette.PaletteId));

            colorBuilder.Property(c => c.R).HasColumnName("R");
            colorBuilder.Property(c => c.G).HasColumnName("G");
            colorBuilder.Property(c => c.B).HasColumnName("B");
            colorBuilder.Property(c => c.A).HasColumnName("A").HasColumnType("decimal(18,2)");

            colorBuilder.HasIndex(
                nameof(Palette.PaletteId),
                nameof(Color.R),
                nameof(Color.G),
                nameof(Color.B),
                nameof(Color.A));
        });
    }
}