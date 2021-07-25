using com.allcard.institution.models;
using com.allcard.institution.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.allcard.institution
{
    public static class SeedData
    {
        public static async Task AddInitialdata(InstitutionContext context)
        {


            var benildeInstitution = new Institution();
            benildeInstitution.Name = "Rustans";
            benildeInstitution.Code = "Rustans";
            await context.Institution.AddAsync(benildeInstitution);
            await context.SaveChangesAsync();

            var benchain = new Chain();
            benchain.InstitutionID = benildeInstitution.GUID;
            benchain.Name = "Rustans";
            benchain.Code = "Rustans";
            await context.Chain.AddAsync(benchain);



            var groupBen = new Group();
            groupBen.InstitutionID = benildeInstitution.GUID;
            groupBen.ChainID = benchain.ID;
            groupBen.Name = "Rustans";
            groupBen.Code = "Rustans";
            await context.Group.AddAsync(groupBen);
            await context.SaveChangesAsync();

            var benMer = new Merchant();
            benMer.InstitutionID = benildeInstitution.GUID;
            benMer.GroupID = groupBen.ID;
            benMer.Name = "Rustans";
            benMer.Code = "Rustans";
            await context.Merchant.AddAsync(benMer);
            await context.SaveChangesAsync();


            var benBranch = new Branch();
            benBranch.InstitutionID = benildeInstitution.GUID;
            benBranch.MerchantID = benMer.ID;
            benBranch.Name = "Rockwell makati";
            benBranch.Code = "Rockwell makati";
            await context.Branch.AddAsync(benBranch);


            var benBranch2 = new Branch();
            benBranch2.InstitutionID = benildeInstitution.GUID;
            benBranch2.MerchantID = benMer.ID;
            benBranch2.Name = "Rockwell makati 2";
            benBranch2.Code = "Rockwell makati 2";
            await context.Branch.AddAsync(benBranch2);

            await context.SaveChangesAsync();



            try
            {
                var role1 = new Role { Name = "System Administrator", IsActive = true };
                var role2 = new Role { Name = "Administrator", IsActive = true };
                var role3 = new Role { Name = "Guard", IsActive = true };

                await context.Role.AddAsync(role1);
                await context.Role.AddAsync(role2);
                await context.Role.AddAsync(role3);


                var user1 = new UsersProfile { Username = "guard1", Password = "p@ssw0rd", BranchID = benBranch.ID, DisplayName = "" };
                var user2 = new UsersProfile { Username = "guard2", Password = "p@ssw0rd", BranchID = benBranch2.ID, DisplayName = "" };
                await context.UsersProfile.AddAsync(user1);
                await context.UsersProfile.AddAsync(user2);


                var userRole1 = new UserRole { UserProfileID = user1.ID, RoleID = role3.ID };
                var userRole2 = new UserRole { UserProfileID = user2.ID, RoleID = role3.ID };
                await context.UserRole.AddAsync(userRole1);
                await context.UserRole.AddAsync(userRole2);


                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
