using System;
using Solar.Scenarios;

namespace SolarForge.GalaxyChartFillings
{

	public class GalaxyChartFillingsSource
	{



		public string Name { get; set; }



		  
		public Solar.Scenarios.GalaxyChartFillings Fillings { get; set; } 
		  
         


		public bool ReadOnly { get; set; }
		 

		public override string ToString()
		{
            if (string.IsNullOrEmpty(this.Fillings.SourceDescription))
            {
                return this.Name;   
            }
            return this.Name + " (" + this.Fillings.SourceDescription + ")";
        }
    }
}
