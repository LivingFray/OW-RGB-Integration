using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RGBIntegration
{
	public interface RGB_Interface
	{
		// Attempt to interface with the API and return true if successful
		bool TryInitialise(RGBIntegration Mod);

		void RegisterEvents();

		void UnregisterEvents();

		// The name of the API (e.g. Razor Synapse, SteelSeries GameSense)
		string GetName();

		// Update with the real time that elapsed since last frame (i.e. still ticks during pause)
		void Update(float DeltaTime);

		// Set the value for an event
		void UpdateValue(string name, int value);

		// Set the color for an event
		void UpdateColor(string name, Color color);
	}
}
