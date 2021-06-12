using System;
using System.Linq;
using FootballersCatalogue.Models;

namespace FootballersCatalogue
{
    public static class SampleData
    {
        public static void Initialize(CatalogueContext context)
        {
            if (!context.Teams.Any())
            {
                context.Teams.AddRange(
                    new Team("Zenith"), new Team("Arsenal"), new Team("Nottingham")
                );
                context.SaveChanges();
            }
        }
    }
}