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

            colorBuilder.Property(c => c.RedPigment).HasColumnName("R");
            colorBuilder.Property(c => c.GreenPigment).HasColumnName("G");
            colorBuilder.Property(c => c.BluePigment).HasColumnName("B");
            colorBuilder.Property(c => c.Opacity).HasColumnName("A").HasColumnType("decimal(18,2)");

            colorBuilder.HasIndex(
                nameof(Palette.PaletteId),
                nameof(Color.RedPigment),
                nameof(Color.GreenPigment),
                nameof(Color.BluePigment),
                nameof(Color.Opacity));
        });
    }
}