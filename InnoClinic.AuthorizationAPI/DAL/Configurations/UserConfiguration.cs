using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
             .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
           .IsRequired() 
           .HasMaxLength(255)
           .HasConversion(
               v => v.Trim().ToLower(), 
               v => v);

        builder.Property(u => u.Password)
           .IsRequired()
           .HasMaxLength(255);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
