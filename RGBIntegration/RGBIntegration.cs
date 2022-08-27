using OWML.Common;
using OWML.ModHelper;
using RGBIntegration.EffectControllers;
using RGBIntegration.Steelseries;
using System.Collections.Generic;
using UnityEngine;

namespace RGBIntegration
{
	public class RGBIntegration : ModBehaviour
	{
		// Replace this with an array when multiple are supported
		public RGB_Interface ActiveInterface;

		public List<RGBEffectController> EffectControllers;

		private bool isActive = false;

		private float LastDeltaTime;

		private void Awake()
		{
			LastDeltaTime = Time.realtimeSinceStartup;
		}

		private void Start()
		{
			// TODO: Support different interfaces and try to initialise each of them
			// TODO: Unload this stuff when in the menus
			// NB: multiple brands may return true, eg. if keyboard and mouse are different brands
			{
				RGB_SteelSeries steelSeries = new RGB_SteelSeries();
				if (steelSeries.TryInitialise(this))
				{
					ActiveInterface = steelSeries;
				}
			}

			if (ActiveInterface != null)
			{
				ModHelper.Console.WriteLine($"Interfaced with {ActiveInterface.GetName()}", MessageType.Success);
			}
			else
			{
				ModHelper.Console.WriteLine($"No supported RGB Interface found", MessageType.Error);
				return;
			}

			LoadManager.OnCompleteSceneLoad += (scene, loadScene) => {

				if (loadScene == OWScene.SolarSystem || loadScene == OWScene.EyeOfTheUniverse)
				{
					if (!isActive)
					{
						CreateEffectControllers();
						ActiveInterface.RegisterEvents();
						isActive = true;
						ModHelper.Console.WriteLine($"Enabled RGB effects", MessageType.Success);
					}
				}
				else
				{
					if (isActive)
					{
						ActiveInterface.UnregisterEvents();
						isActive = false;
						ModHelper.Console.WriteLine($"Disabled RGB effects", MessageType.Success);
					}
				}
			};
		}

		private void Update()
		{
			if (!isActive || ActiveInterface == null)
			{
				return;
			}

			foreach (RGBEffectController effectController in EffectControllers)
			{
				effectController.Update(this);
			}

			float Now = Time.realtimeSinceStartup;
			ActiveInterface.Update(Now - LastDeltaTime);
			LastDeltaTime = Now;
		}

		private void CreateEffectControllers()
		{
			EffectControllers = new List<RGBEffectController>() {
				new RGBEC_Background(),
				new RGBEC_Health(),
				new RGBEC_Oxygen(),
				new RGBEC_Fuel(),
			};
		}
	}
}
