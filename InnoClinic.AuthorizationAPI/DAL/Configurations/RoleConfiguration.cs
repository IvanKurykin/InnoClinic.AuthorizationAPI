using DAL.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
           new IdentityRole
           {
               Name = Roles.Admin,
               NormalizedName = Roles.Admin.ToUpper()
           },
           new IdentityRole
           {
               Name = Roles.Doctor,
               NormalizedName = Roles.Doctor.ToUpper()
           },
           new IdentityRole
           {
               Name = Roles.Patient,
               NormalizedName = Roles.Patient.ToUpper()
           }
       );
    }
}
