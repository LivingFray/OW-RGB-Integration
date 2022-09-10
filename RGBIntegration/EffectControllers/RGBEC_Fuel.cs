using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntegration
{
	public class RGBEC_Fuel : RGBEffectController
	{
		private int cachedFuel = 0;

		public string GetEventName()
		{
			return "FUEL";
		}

		public void Update(RGBIntegration mod)
		{
			try
			{
				var pr = Locator.GetPlayerTransform().GetComponent<PlayerResources>();
				var currentFuel = Math.Max(0, (int)(pr.GetFuelFraction() * 100));
				if (currentFuel != cachedFuel)
				{
					cachedFuel = currentFuel;
					for (int i = 0; i < mod.ActiveInterfaces.Count; i++)
					{
						mod.ActiveInterfaces[i].UpdateValue(GetEventName(), cachedFuel);
					}
				}
			}
			catch
			{

			}
		}
	}
}
