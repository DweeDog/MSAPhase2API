using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContactBookAPI.Models
{
    public class ContactBookAPIContext : DbContext
    {
        public ContactBookAPIContext (DbContextOptions<ContactBookAPIContext> options)
            : base(options)
        {
        }

        public DbSet<ContactBookAPI.Models.ContactItem> ContactItem { get; set; }
    }
}
