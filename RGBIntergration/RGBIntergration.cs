using OWML.Common;
using OWML.ModHelper;
using RGBIntergration.Steelseries;
using UnityEngine;

namespace RGBIntergration
{
    public class RGBIntergration : ModBehaviour
    {
        // Replace this with an array when multiple are supported
        RGB_Interface ActiveInterface;

        

        private void Awake()
        {
            // TODO: Support different interfaces and try to initialise each of them
            // NB: multiple brands may return true, eg. if keyboard and mouse are different brands
            //{
               // RGB_SteelSeries steelSeries = new RGB_SteelSeries();
               // if (steelSeries.TryInitialise(this))
               // {
               //     ActiveInterface = steelSeries;
               // }
            //}
        }

        private void Start()
        {
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

            ActiveInterface.UpdateValue("HEALTH", 100);
        }

        private void Update()
        {
            if (ActiveInterface == null)
            {
                return;
            }

            ActiveInterface.Update(Time.deltaTime);
            ActiveInterface.UpdateValue("HEALTH", (int)Locator.GetPlayerTransform().GetComponent<PlayerResources>()._currentHealth);
        }
    }
}
