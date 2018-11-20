using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NotepadAPI.Models
{
    public class NotepadAPIContext : DbContext
    {
        public NotepadAPIContext (DbContextOptions<NotepadAPIContext> options)
            : base(options)
        {
        }

        public DbSet<NotepadAPI.Models.NotepadItem> NotepadItem { get; set; }
    }
}
