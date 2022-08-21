using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntergration
{
	public class RGBEC_Health : RGBEffectController
	{
		public string GetEventName() { return "HEALTH"; }

		private int cachedHealth = 0;

		public void Update(RGBIntergration mod)
		{
			try
			{
				var pr = Locator.GetPlayerTransform().GetComponent<PlayerResources>();
				if (pr._currentHealth != cachedHealth)
				{
					cachedHealth = (int)pr._currentHealth;
					mod.ActiveInterface.UpdateValue(GetEventName(), cachedHealth);
				}
			}
			catch
			{

			}
		}
	}
}
