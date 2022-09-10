using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntegration
{
	public class RGBEC_Oxygen : RGBEffectController
	{
		private int cachedOxygen = 0;

		public string GetEventName()
		{
			return "OXYGEN";
		}

		public void Update(RGBIntegration mod)
		{
			try
			{
				var pr = Locator.GetPlayerTransform().GetComponent<PlayerResources>();
				var currentOxygen = Math.Max(0, (int)(pr.GetOxygenFraction() * 100));
				if (currentOxygen != cachedOxygen)
				{
					cachedOxygen = currentOxygen;
					for (int i = 0; i < mod.ActiveInterfaces.Count; i++)
					{
						mod.ActiveInterfaces[i].UpdateValue(GetEventName(), cachedOxygen);
					}
				}
			}
			catch
			{

			}
		}
	}
}
