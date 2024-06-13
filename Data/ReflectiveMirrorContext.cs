using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReflectiveMirror.Models;

namespace ReflectiveMirror.Data
{
    public class ReflectiveMirrorContext : DbContext
    {
        public ReflectiveMirrorContext (DbContextOptions<ReflectiveMirrorContext> options)
            : base(options)
        {
        }

        public DbSet<ReflectiveMirror.Models.Mirror> Mirror { get; set; } = default!;
    }
}
