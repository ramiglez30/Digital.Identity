using Digital.Identity.Admin.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Tests
{
    public static class TestsUsers
    {
        public static IList<UserDto> Users = new List<UserDto> {
               new UserDto
                {
                    UserName = "user1",
                    Email = "user1@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new UserDto
                {
                    UserName = "user2",
                    Email = "user2@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new UserDto
                {
                    UserName = "user3",
                    Email = "user3@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new UserDto
                {
                    UserName = "user4",
                    Email = "user4@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new UserDto
                {
                    UserName = "user5",
                    Email = "user5@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new UserDto
                {
                    UserName = "user6",
                    Email = "user6@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new UserDto
                {
                    UserName = "user7",
                    Email = "user7@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new UserDto
                {
                    UserName = "user8",
                    Email = "user8@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                   new UserDto
                {
                    UserName = "user9",
                    Email = "user9@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new UserDto
                {
                    UserName = "user10",
                    Email = "user10@email.com",
                    Id = Guid.NewGuid().ToString()
                }
        };
    }
}
