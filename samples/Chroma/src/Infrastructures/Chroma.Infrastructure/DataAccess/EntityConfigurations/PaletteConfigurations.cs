using Chroma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chroma.Infrastructure.DataAccess.EntityConfigurations;

public class PaletteConfigurations : IEntityTypeConfiguration<Palette>
{
    public void Configure(EntityTypeBuilder<Palette> builder)
    {
        builder.OwnsMany(palette => palette.Colors, color =>
        {
            color.ToTable("PaletteColors");
            
            // Explicit FK to owner
            color.WithOwner().HasForeignKey("PaletteId");

            // Add an Id for the dependent entries (or define a composite key)
            color.Property<long>("PaletteColorId").ValueGeneratedOnAdd();
            color.HasKey("PaletteColorId");
            
            color.Property(c => c.RedPigment).HasColumnName("R");
            color.Property(c => c.GreenPigment).HasColumnName("G");
            color.Property(c => c.BluePigment).HasColumnName("B");
            color.Property(c => c.Opacity).HasColumnName("A");
        });
    }
}