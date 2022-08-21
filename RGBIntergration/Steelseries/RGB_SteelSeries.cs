using OWML.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RGBIntergration.Steelseries
{
	class RGB_SteelSeries : RGB_Interface
	{
		private float TimeSinceHeartbeat = 0.0f;
		private const float HEARTBEAT_RATE = 1.0f;
		private RGBIntergration Mod;
		private const string _ServerPropsPath = "%PROGRAMDATA%/SteelSeries/SteelSeries Engine 3/coreProps.json";
		private string URI;
		private readonly string heartbeat = "{\"game\":\"OUTER_WILDS\"}";

		public string GetName()
		{
			return "SteelSeries GameSense";
		}

		public bool TryInitialise(RGBIntergration Mod)
		{
			this.Mod = Mod;
			try
			{
				ReadServerProps();
				BindEvents();
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

			BindGameEvent bindGameEvent = new BindGameEvent { 
				game = "OUTER_WILDS",
				eventName = "HEALTH",
				min_value = 0,
				max_value = 100,
				handlers = new List<ColorHandler> { 
					new ColorHandler() {
						device_type = "keyboard",
						zone = "function-keys",
						color = new StaticColorDefinition() {
							red = 255,
							green = 255,
							blue = 255
						},
						mode = "percent"
					}
				}
			};

			ColorHandler colorHandler = new ColorHandler {
				device_type = "keyboard",
				zone = "function-keys",
				color = new StaticColorDefinition() {
					red = 255,
					green = 255,
					blue = 255
				},
				mode = "percent"
			};

			// TODO: Programatically get these from Mod obj
			Post(Newtonsoft.Json.JsonConvert.SerializeObject(bindGameEvent), "bind_game_event");
			Mod.ModHelper.Console.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(bindGameEvent)}", MessageType.Error);
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
			GameEvent gameEvent = new GameEvent() {
				game = "OUTER_WILDS",
				eventName = name,
				data = new EventData() {
					value = value
				}
			};

			Mod.ModHelper.Console.WriteLine($"Update request: {Newtonsoft.Json.JsonConvert.SerializeObject(gameEvent)}", MessageType.Message);

			Post(Newtonsoft.Json.JsonConvert.SerializeObject(gameEvent), "game_event");
		}
	}
}
