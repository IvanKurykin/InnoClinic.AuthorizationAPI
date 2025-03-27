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
               Name = RoleConstants.Admin,
               NormalizedName = RoleConstants.Admin.ToUpper()
           },
           new IdentityRole
           {
               Name = RoleConstants.Doctor,
               NormalizedName = RoleConstants.Doctor.ToUpper()
           },
           new IdentityRole
           {
               Name = RoleConstants.Patient,
               NormalizedName = RoleConstants.Patient.ToUpper()
           }
       );
    }
}
