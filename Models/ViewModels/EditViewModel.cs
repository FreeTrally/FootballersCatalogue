using System;
using System.Collections.Generic;

namespace FootballersCatalogue.Models
{
    public class EditViewModel
    {
        public Player Player { get; set; }
        public IEnumerable<Team> Teams { get; set; }

        public EditViewModel(Player player, IEnumerable<Team> teams)
        {
            Player = player;
            Teams = teams;
        }
    }
}