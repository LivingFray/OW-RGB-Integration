using OWML.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RGBIntegration.Razer
{
	public class RGB_Razer : RGB_Interface
	{
		private string URI = "";
		private RGBIntegration Mod;
		private float TimeSinceHeartbeat = 0.0f;
		private const float HEARTBEAT_RATE = 1.0f;
		private const float UPDATE_RATE = 0.05f;
		private float TimeSinceLastUpdate = UPDATE_RATE;
		private bool EventUpdated = false;
		private ChromaCustom chromaCustom;

		// Effects
		private Color background = new Color(1.0f, 158.0f / 255.0f, 7.0f / 255.0f);
		private Color flashlight = new Color(0.0f, 0.0f, 0.0f);
		private Color healthMaxColor = new Color(0.0f, 1.0f, 0.0f);
		private Color healthMinColor = new Color(1.0f, 0.0f, 0.0f);
		private Color oxygenColor = new Color(60.0f / 255.0f, 1.0f, 1.0f);
		private Color fuelColor = new Color(153.0f / 255.0f, 51.0f / 255.0f, 0.0f);
		private int health = 100;
		private int oxygen = 100;
		private int fuel = 100;

		public string GetName()
		{
			return "Razer Chroma";
		}

		public bool TryInitialise(RGBIntegration Mod)
		{
			this.Mod = Mod;

			var request = new UnityWebRequest("http://localhost:54235/" + "razer/chromasdk", "GET");
			byte[] bodyRaw = Encoding.UTF8.GetBytes("{}");
			request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");
			request.SendWebRequest();

			// Nasty, but I ain't rewriting things to be async until I know it actually works
			while (!request.isDone) { };

			if (request.responseCode != 200)
			{
				Mod.ModHelper.Console.WriteLine($"{request.error}", MessageType.Error);
				Mod.ModHelper.Console.WriteLine($"{request.downloadHandler.text}", MessageType.Error);
				return false;
			}

			return true;
		}

		public void RegisterEvents()
		{
			object init = new {
				title = "Outer Wilds",
				description = "Outer Wilds, supported by the RGB Integration mod by LivingFray",
				author = new {
					name = "Mobius Digital",
					contact = "www.mobiusdigitalgames.com"
				},
				device_supported = new []{
					"keyboard",
					"mouse"
				},
				category = "game"
			};

			URI = "http://localhost:54235/";

			UnityWebRequestAsyncOperation response = Send(Newtonsoft.Json.JsonConvert.SerializeObject(init));

			// I'll be honest. I don't know how to do async stuff in unity/c#
			while (!response.isDone) { };

			try
			{
				InitResponse initResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<InitResponse>(response.webRequest.downloadHandler.text);
				URI = initResponse.uri + "/";
			}
			catch
			{
				Mod.ModHelper.Console.WriteLine($"Error deserialising response: {response.webRequest.downloadHandler.text}", MessageType.Error);
				Mod.ModHelper.Console.WriteLine($"URI: {response.webRequest.uri}", MessageType.Error);
			}

			chromaCustom = new ChromaCustom() {
				effect = "CHROMA_CUSTOM",
				param = new int[6, 22]
			};

			UpdateEvent();
		}

		public void UnregisterEvents()
		{
			var request = UnityWebRequest.Delete("http://localhost:54235/" + "chromasdk");
			request.SetRequestHeader("Content-Type", "application/json");
			request.SendWebRequest();

		}

		public void Update(float DeltaTime)
		{
			TimeSinceHeartbeat += DeltaTime;
			TimeSinceLastUpdate += DeltaTime;
			if (TimeSinceHeartbeat > HEARTBEAT_RATE)
			{
				TimeSinceHeartbeat -= HEARTBEAT_RATE;

				Send("", "heartbeat", "PUT");
			}

			if (EventUpdated && TimeSinceLastUpdate > UPDATE_RATE)
			{
				UpdateEvent();
				EventUpdated = false;
				TimeSinceLastUpdate = 0.0f;
			}
		}

		public void UpdateColor(string name, Color color)
		{
			switch (name)
			{
				case "BACKGROUND":
					background = color;
					break;
				case "FLASHLIGHT":
					flashlight = color;
					break;
			}
			EventUpdated = true;
		}

		public void UpdateValue(string name, int value)
		{
			switch (name)
			{
				case "HEALTH":
					health = value;
					break;
				case "OXYGEN":
					oxygen = value;
					break;
				case "FUEL":
					fuel = value;
					break;
			}
			EventUpdated = true;
		}

		private void UpdateEvent()
		{
			for (int x = 0; x < 22; x++)
			{
				for (int y = 0; y < 6; y++)
				{
					chromaCustom.param[y, x] = ColorToBGR(background);
				}
			}

			Color currentHealthColor = Color.Lerp(healthMinColor, healthMaxColor, health / 100.0f);
			SetProgressBar(3, 0, 12, health, currentHealthColor);
			SetProgressBar(2, 1, 10, oxygen, oxygenColor);
			SetProgressBar(2, 2, 10, fuel, fuelColor);

			Send(Newtonsoft.Json.JsonConvert.SerializeObject(chromaCustom), "keyboard", "PUT");
			Mod.ModHelper.Console.WriteLine($"Update keyboard event: {Newtonsoft.Json.JsonConvert.SerializeObject(chromaCustom)}", MessageType.Debug);

			ChromaStatic mouse = new ChromaStatic() {
				effect = "CHROMA_STATIC",
				param = new Static() {
					color = ColorToBGR(flashlight)
				}
			};

			Send(Newtonsoft.Json.JsonConvert.SerializeObject(mouse), "mouse", "PUT");
			Mod.ModHelper.Console.WriteLine($"Update mouse event: {Newtonsoft.Json.JsonConvert.SerializeObject(mouse)}", MessageType.Debug);
		}

		private void SetProgressBar(int startX, int startY, int numkeys, int value, Color color)
		{
			float exactkeys = numkeys * (value / 100.0f);
			int NumFully = (int)exactkeys;
			float LastPercent = exactkeys - NumFully;

			for (int i = 0; i < numkeys; i++)
			{
				Color c = color;
				if (i == NumFully)
				{
					c *= LastPercent;
				}
				else if (i > NumFully)
				{
					c = Color.black;
				}
				chromaCustom.param[startY, startX + i] = ColorToBGR(c);
			}
		}

		private int ColorToBGR(Color color)
		{
			return (0 << 24) + ((int)(color.b * 255) << 16) + ((int)(color.g * 255) << 8) + (int)(color.r * 255);
		}

		private UnityWebRequestAsyncOperation Send(string bodyJsonString, string endpoint = "razer/chromasdk", string method = "POST")
		{
			var request = new UnityWebRequest(URI + endpoint, method);
			if (bodyJsonString != null && bodyJsonString.Length > 0)
			{
				byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
				request.SetRequestHeader("Content-Type", "application/json");
				request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			}
			request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			UnityWebRequestAsyncOperation response = request.SendWebRequest();

			response.completed += msg => {
				try
				{
					EffectResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<EffectResponse>(request.downloadHandler.text);
					if (request.responseCode != 200 || response.result != 0)
					{
						Mod.ModHelper.Console.WriteLine($"{request.error}", MessageType.Error);
						Mod.ModHelper.Console.WriteLine($"Bad request: {bodyJsonString}", MessageType.Error);
						Mod.ModHelper.Console.WriteLine($"{request.downloadHandler.text}", MessageType.Error);
					}
				}
				catch
				{
					Mod.ModHelper.Console.WriteLine($"Error deserialising response: {request.downloadHandler.text}", MessageType.Error);
					Mod.ModHelper.Console.WriteLine($"Bad request: {bodyJsonString}", MessageType.Error);
					Mod.ModHelper.Console.WriteLine($"URI: {request.uri}", MessageType.Error);
				}
			};

			return response;
		}
	}
}
