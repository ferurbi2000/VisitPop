using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Domain.Entities;

namespace VisitPop.Infrastructure.Persistence.Contexts
{
    public class VisitPopDbContext: DbContext
    {
        public VisitPopDbContext(DbContextOptions<VisitPopDbContext> options)
            :base(options)
        {
        }

        #region DBSet Region - Do Not Delete
        public DbSet<Person> People { get; set; }

        #endregion
    }
}
