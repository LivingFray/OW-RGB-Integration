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
		float TimeSinceUpdated = 0.0f;
		const float UpdateFrequency = 0.5f;

		public string GetEventName()
		{
			return "BACKGROUND";
		}

		public void Update(RGBIntegration mod)
		{
			TimeSinceUpdated += Time.deltaTime;
			if (TimeSinceUpdated > UpdateFrequency)
			{
				TimeSinceUpdated -= UpdateFrequency;
				SunController sun = Locator.GetSunController();
				if (!sun)
				{
					return;
				}

				// TODO: Occluded places (GD, DB, QM, stranger, etc) get own effects

				if (!sun._collapseStarted)
				{
					mod.ActiveInterface.UpdateColor("BACKGROUND", sun._atmosphereColor.Evaluate(Mathf.InverseLerp(sun._progressionStartTime, sun._progressionEndTime, TimeLoop.GetMinutesElapsed())));
				}
				else
				{
					mod.ActiveInterface.UpdateColor("BACKGROUND", sun._collapseAtmosphereColor.Evaluate(sun._collapseT));
				}
			}
		}
	}
}
