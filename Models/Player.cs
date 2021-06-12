using System;

namespace FootballersCatalogue.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public string TeamName { get; set; }
        public string Country { get; set; }
    }
}