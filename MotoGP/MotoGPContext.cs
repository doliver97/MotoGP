using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MotoGP
{
    class MotoGPContext : DbContext
    {
        public MotoGPContext() : base("MotoGPDB")
        {

        }

        public DbSet<MotoGPTeam> Teams { get; set; }
    }
}
