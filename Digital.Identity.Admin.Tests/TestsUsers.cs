using Digital.Identity.Admin.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digital.Identity.Admin.Tests
{
    public static class TestsUsers
    {
        public static IQueryable<ApplicationUser> Users = (new List<ApplicationUser> {
               new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user1@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    UserName = "user2",
                    Email = "user2@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new ApplicationUser
                {
                    UserName = "user3",
                    Email = "user3@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    UserName = "user4",
                    Email = "user4@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new ApplicationUser
                {
                    UserName = "user5",
                    Email = "user5@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    UserName = "user6",
                    Email = "user6@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new ApplicationUser
                {
                    UserName = "user7",
                    Email = "user7@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    UserName = "user8",
                    Email = "user8@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new ApplicationUser
                {
                    UserName = "user9",
                    Email = "user9@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    UserName = "user10",
                    Email = "user10@email.com",
                    Id = Guid.NewGuid().ToString()
                }
        }).AsQueryable();
    }
}
