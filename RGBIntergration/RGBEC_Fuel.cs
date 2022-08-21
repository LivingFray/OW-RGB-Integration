using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntergration
{
	public class RGBEC_Fuel : RGBEffectController
	{
		private int cachedFuel = 0;

		public string GetEventName()
		{
			return "FUEL";
		}

		public void Update(RGBIntergration mod)
		{
			try
			{
				var pr = Locator.GetPlayerTransform().GetComponent<PlayerResources>();
				var currentFuel = (int)(pr.GetFuelFraction() * 100);
				if (currentFuel != cachedFuel)
				{
					cachedFuel = currentFuel;
					mod.ActiveInterface.UpdateValue(GetEventName(), cachedFuel);
				}
			}
			catch
			{

			}
		}
	}
}
