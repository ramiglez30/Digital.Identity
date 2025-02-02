﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Models.Api
{
    public class UserDto
    {
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string ConcurrencyStamp { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string Email { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Id { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }
    }
}
