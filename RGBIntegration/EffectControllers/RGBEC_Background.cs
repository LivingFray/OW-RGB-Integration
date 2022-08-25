using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RGBIntegration.EffectControllers
{
	public class RGBEC_Background : RGBEffectController
	{
		public string GetEventName()
		{
			return "BACKGROUND";
		}

		public void Update(RGBIntegration mod)
		{
			mod.ActiveInterface.UpdateColor("BACKGROUND", new Color(0, 0, 255));
		}
	}
}
