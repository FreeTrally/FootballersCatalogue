using System;
using System.Collections.Generic;

namespace FootballersCatalogue.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; } 

        public Team (string name)
        {
            Name = name;
        }
    }
}