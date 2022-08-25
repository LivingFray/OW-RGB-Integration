using OWML.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RGBIntegration.Steelseries
{
	class RGB_SteelSeries : RGB_Interface
	{
		private float TimeSinceHeartbeat = 0.0f;
		private const float HEARTBEAT_RATE = 1.0f;
		private RGBIntegration Mod;
		private const string _ServerPropsPath = "%PROGRAMDATA%/SteelSeries/SteelSeries Engine 3/coreProps.json";
		private string URI;
		private readonly string heartbeat = "{\"game\":\"OUTER_WILDS\"}";

		public string GetName()
		{
			return "SteelSeries GameSense";
		}

		public bool TryInitialise(RGBIntegration Mod)
		{
			this.Mod = Mod;
			try
			{
				ReadServerProps();
			}
			catch (Exception e)
			{
				Mod.ModHelper.Console.WriteLine($"{e.Message}", MessageType.Error);
				return false;
			}
			return true;
		}

		public void Update(float DeltaTime)
		{
			TimeSinceHeartbeat += DeltaTime;
			if (TimeSinceHeartbeat > HEARTBEAT_RATE)
			{
				TimeSinceHeartbeat -= HEARTBEAT_RATE;

				Post(heartbeat, "game_heartbeat");
			}
		}

		private void ReadServerProps()
		{
			string props = System.IO.File.ReadAllText(Environment.ExpandEnvironmentVariables(_ServerPropsPath));
			CoreProps cp = JsonUtility.FromJson<CoreProps>(props);
			URI = $"http://{cp.address}/";
		}

		private void BindEvents()
		{
			foreach (RGBEffectController controller in Mod.EffectControllers)
			{
				Post(Newtonsoft.Json.JsonConvert.SerializeObject(EffectDefinitions.GetBindGameEvent(controller)), "bind_game_event");
			}
		}

		private void Post(string bodyJsonString, string endpoint = "game_event")
		{
			var request = new UnityWebRequest(URI + endpoint, "POST");
			byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
			request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");
			request.SendWebRequest().completed += msg => {
				if (request.responseCode != 200)
				{
					Mod.ModHelper.Console.WriteLine($"{request.error}", MessageType.Error);
					Mod.ModHelper.Console.WriteLine($"Bad request: {bodyJsonString}", MessageType.Message);
					Mod.ModHelper.Console.WriteLine($"{request.downloadHandler.text}", MessageType.Message);
				}
			};
		}

		public void UpdateValue(string name, int value)
		{
			GameEvent gameEvent = new GameEvent() 
			{
				game = "OUTER_WILDS",
				eventName = name,
				data = new EventData() 
				{
					value = value
				}
			};

			Mod.ModHelper.Console.WriteLine($"Update request: {Newtonsoft.Json.JsonConvert.SerializeObject(gameEvent)}", MessageType.Message);

			Post(Newtonsoft.Json.JsonConvert.SerializeObject(gameEvent), "game_event");
		}

		public void RegisterEvents()
		{
			BindEvents();
		}

		public void UnregisterEvents()
		{
			//Debug
			Post("{\"game\":\"OUTER_WILDS\"}", "remove_game");
			Post("{\"game\":\"OUTER_WILDS\"}", "stop_game");
		}

		public void UpdateColor(string name, Color color)
		{
			GameEvent gameEvent = new GameEvent() 
			{
				game = "OUTER_WILDS",
				eventName = name,
				data = new EventData() 
				{ 
					value = 1,
					frame = new Frame() 
					{
						zone_one_color = new StaticColorDefinition() 
						{
							red = (int)color.r,
							green = (int)color.g,
							blue = (int)color.b
						}
					}
				}
			};

			Mod.ModHelper.Console.WriteLine($"Update Color request: {Newtonsoft.Json.JsonConvert.SerializeObject(gameEvent)}", MessageType.Message);
			Post(Newtonsoft.Json.JsonConvert.SerializeObject(gameEvent), "game_event");
		}
	}
}
