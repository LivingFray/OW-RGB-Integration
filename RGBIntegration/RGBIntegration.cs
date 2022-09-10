using OWML.Common;
using OWML.ModHelper;
using RGBIntegration.EffectControllers;
using RGBIntegration.Razer;
using RGBIntegration.Steelseries;
using System.Collections.Generic;
using UnityEngine;

namespace RGBIntegration
{
	public class RGBIntegration : ModBehaviour
	{
		// Replace this with an array when multiple are supported
		public List<RGB_Interface> ActiveInterfaces;

		public List<RGBEffectController> EffectControllers;

		private bool isActive = false;

		private float LastDeltaTime;

		private void Awake()
		{
		}

		private void Start()
		{
			ActiveInterfaces = new List<RGB_Interface>();
			// TODO: Support different interfaces and try to initialise each of them
			// TODO: Unload this stuff when in the menus
			// NB: multiple brands may return true, eg. if keyboard and mouse are different brands
			{
				RGB_SteelSeries steelSeries = new RGB_SteelSeries();
				if (steelSeries.TryInitialise(this))
				{
					ActiveInterfaces.Add(steelSeries);
				}

				RGB_Razer razer = new RGB_Razer();
				if (razer.TryInitialise(this))
				{
					ActiveInterfaces.Add(razer);
				}

			}

			if (ActiveInterfaces.Count == 0)
			{
				ModHelper.Console.WriteLine($"No supported RGB Interface found", MessageType.Error);
				return;
			}
			for (int i = 0; i < ActiveInterfaces.Count; i++)
			{
				ModHelper.Console.WriteLine($"Interfaced with {ActiveInterfaces[i].GetName()}", MessageType.Success);
			}


			LoadManager.OnCompleteSceneLoad += (scene, loadScene) => {

				if (loadScene == OWScene.SolarSystem || loadScene == OWScene.EyeOfTheUniverse)
				{
					if (!isActive)
					{
						CreateEffectControllers();
						for (int i = 0; i < ActiveInterfaces.Count; i++)
						{
							ActiveInterfaces[i].RegisterEvents();
						}
						isActive = true;
						ModHelper.Console.WriteLine($"Enabled RGB effects", MessageType.Success);
						LastDeltaTime = Time.realtimeSinceStartup;
					}
				}
				else
				{
					if (isActive)
					{
						for (int i = 0; i < ActiveInterfaces.Count; i++)
						{
							ActiveInterfaces[i].UnregisterEvents();
						}
						isActive = false;
						ModHelper.Console.WriteLine($"Disabled RGB effects", MessageType.Success);
					}
				}
			};
		}

		private void Update()
		{
			if (!isActive || ActiveInterfaces.Count == 0)
			{
				return;
			}

			foreach (RGBEffectController effectController in EffectControllers)
			{
				effectController.Update(this);
			}

			float Now = Time.realtimeSinceStartup;
			for (int i = 0; i < ActiveInterfaces.Count; i++)
			{
				ActiveInterfaces[i].Update(Now - LastDeltaTime);
			}
			LastDeltaTime = Now;
		}

		private void CreateEffectControllers()
		{
			EffectControllers = new List<RGBEffectController>() {
				new RGBEC_Background(),
				new RGBEC_Health(),
				new RGBEC_Oxygen(),
				new RGBEC_Fuel(),
				new RGBEC_Flashlight(),
			};
		}
	}
}
