using OWML.Common;
using OWML.ModHelper;
using RGBIntergration.Steelseries;
using System.Collections.Generic;
using UnityEngine;

namespace RGBIntergration
{
	public class RGBIntegration : ModBehaviour
	{
		// Replace this with an array when multiple are supported
		public RGB_Interface ActiveInterface;

		public List<RGBEffectController> EffectControllers;

		private bool isActive = false;

		private void Awake()
		{

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

			ActiveInterface.Update(Time.deltaTime);

			foreach (RGBEffectController effectController in EffectControllers)
			{
				effectController.Update(this);
			}
		}

		private void CreateEffectControllers()
		{
			EffectControllers = new List<RGBEffectController>() {
				new RGBEC_Health(),
				new RGBEC_Oxygen(),
				new RGBEC_Fuel(),
			};
		}
	}
}
