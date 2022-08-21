using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntergration
{
	interface RGB_Interface
	{
		// Attempt to interface with the API and return true if successful
		bool TryInitialise(RGBIntergration Mod);

		// The name of the API (e.g. Razor Synapse, SteelSeries GameSense)
		string GetName();

		void Update(float DeltaTime);

		void UpdateValue(string name, int value);
	}
}
