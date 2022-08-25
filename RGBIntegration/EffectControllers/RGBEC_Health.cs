using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntegration
{
	public class RGBEC_Health : RGBEffectController
	{
		public string GetEventName() { return "HEALTH"; }

		private int cachedHealth = 0;

		public void Update(RGBIntegration mod)
		{
			try
			{
				var pr = Locator.GetPlayerTransform().GetComponent<PlayerResources>();
				int roundedHealth = Math.Max(0, (int)(pr.GetHealthFraction() * 100));
				if (roundedHealth != cachedHealth)
				{
					cachedHealth = roundedHealth;
					mod.ActiveInterface.UpdateValue(GetEventName(), cachedHealth);
				}
			}
			catch
			{

			}
		}
	}
}
