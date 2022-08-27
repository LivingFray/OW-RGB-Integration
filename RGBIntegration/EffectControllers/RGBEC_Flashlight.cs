using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RGBIntegration.EffectControllers
{
	public class RGBEC_Flashlight : RGBEffectController
	{
		public string GetEventName()
		{
			return "FLASHLIGHT";
		}

		private Color LastColor;

		public void Update(RGBIntegration mod)
		{
			Color NewColor;
			if (PlayerState.InDreamWorld())
			{
				DreamLanternItem playerLantern = Locator.GetDreamWorldController().GetPlayerLantern();
				if (playerLantern && !playerLantern.GetLanternController().IsConcealed() && playerLantern.GetLanternController().IsHeldByPlayer())
				{
					mod.ModHelper.Console.WriteLine(playerLantern.GetLanternController().GetLight().GetLight().color.ToString(), OWML.Common.MessageType.Message);
					NewColor = new Color(44.0f / 255.0f, 192.0f / 255.0f, 120.0f / 255.0f);
				}
				else
				{
					NewColor = new Color(0.0f, 0.0f, 0.0f);
				}
			}
			else
			{
				if (PlayerState.IsFlashlightOn())
				{
					NewColor = new Color(1.0f, 1.0f, 1.0f);
				}
				else
				{
					NewColor = new Color(0.0f, 0.0f, 0.0f);
				}
			}


			if (LastColor != NewColor)
			{
				mod.ActiveInterface.UpdateColor("FLASHLIGHT", NewColor);
				LastColor = NewColor;
			}
		}
	}
}
