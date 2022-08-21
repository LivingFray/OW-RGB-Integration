using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntergration
{
	public class RGBEC_Oxygen : RGBEffectController
	{
		private int cachedOxygen = 0;

		public string GetEventName()
		{
			return "OXYGEN";
		}

		public void Update(RGBIntergration mod)
		{
			try
			{
				var pr = Locator.GetPlayerTransform().GetComponent<PlayerResources>();
				var currentOxygen = (int)(pr.GetOxygenFraction() * 100);
				if (currentOxygen != cachedOxygen)
				{
					cachedOxygen = currentOxygen;
					mod.ActiveInterface.UpdateValue(GetEventName(), cachedOxygen);
				}
			}
			catch
			{

			}
		}
	}
}
