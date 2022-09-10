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
		private float TimeSinceEnteredCloak = 0.0f;
		private float[] FlickerSwitchTimes = { 0.1f, 0.15f, 0.3f, 0.35f, 0.45f, 0.5f, 0.6f };

		private Color LastColor;

		public string GetEventName()
		{
			return "BACKGROUND";
		}

		public void Update(RGBIntegration mod)
		{
			Color ColorToSend;

			SunController sun = Locator.GetSunController();
			if (sun)
			{
				if (PlayerState.InGiantsDeep())
				{
					ColorToSend = new Color(116.0f / 255.0f, 112.0f / 255.0f, 43.0f / 255.0f) * sun._sunLight.sunIntensity;
				}
				else if (PlayerState.OnQuantumMoon())
				{
					// TODO: Change based on quantum location
					ColorToSend = new Color(0.2f, 0.0f, 0.2f);
				}
				else if (PlayerState.InCloakingField())
				{
					PlayerSectorDetector sectorDetector = Locator.GetPlayerSectorDetector();

					bool InsideRingworld = false;

					if (sectorDetector)
					{
						foreach (Sector sector in sectorDetector._sectorList)
						{
							if (sector.GetIDString() == "RingWorldInterior")
							{
								InsideRingworld = true;
								break;
							}
						}
					}

					if (InsideRingworld)
					{
						Light fakeSun = GameObject.Find("RingWorld_Body/Sector_RingInterior/Lights_RingInterior/IP_SunLight").GetComponent<Light>();
						ColorToSend = fakeSun.color * fakeSun.intensity;
					}
					else if (PlayerState.InDreamWorld())
					{
						DreamLanternItem playerLantern = Locator.GetDreamWorldController().GetPlayerLantern();
						if (playerLantern.GetLanternController().IsConcealed())
						{
							ColorToSend = new Color(0.0f, 0.0f, 0.0f);
						}
						else
						{
							ColorToSend = new Color(44.0f / 255.0f, 192.0f / 255.0f, 120.0f / 255.0f);
						}
					}
					else
					{
						TimeSinceEnteredCloak += Time.deltaTime;

						int flickerIndex = 0;
						for (int i = 0; i < FlickerSwitchTimes.Length; i++)
						{
							if (TimeSinceEnteredCloak < FlickerSwitchTimes[i])
							{
								break;
							}
							flickerIndex = i + 1;
						}
						ColorToSend = flickerIndex % 2 == 0 ? CurrentSunColor(sun) : new Color(0.0f, 0.0f, 0.0f);
					}
				}
				else if (PlayerState.InBrambleDimension())
				{
					// TODO: Fog
					ColorToSend = new Color(83.0f / 255.0f, 99.0f / 255.0f, 87.0f / 255.0f);
				}
				else
				{
					ColorToSend = CurrentSunColor(sun);
				}

				if (!PlayerState.InCloakingField())
				{
					TimeSinceEnteredCloak = 0.0f;
				}
			}
			else
			{
				// Presumably we are at the eye
				ColorToSend = new Color(0.2f, 0.0f, 0.2f);
			}

			if (ColorToSend != LastColor)
			{
				for (int i = 0; i < mod.ActiveInterfaces.Count; i++)
				{
					mod.ActiveInterfaces[i].UpdateColor("BACKGROUND", ColorToSend);
				}
				LastColor = ColorToSend;
			}
		}

		Color CurrentSunColor(SunController sun)
		{

			if (!sun._collapseStarted)
			{
				return sun._atmosphereColor.Evaluate(Mathf.InverseLerp(sun._progressionStartTime, sun._progressionEndTime, TimeLoop.GetMinutesElapsed()));
			}
			else
			{
				return sun._collapseAtmosphereColor.Evaluate(sun._collapseT);
			}
		}
	}
}
