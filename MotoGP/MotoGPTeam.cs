using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MotoGP
{
    public class MotoGPTeam
    {
        [Key]
        public string Name { get; set; }

        public int Established { get; set; }
        public int Trophies { get; set; }
        public bool Registered { get; set; }
    }
}
