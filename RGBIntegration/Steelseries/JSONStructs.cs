using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 0649
namespace RGBIntegration.Steelseries
{
	// coreProps.json holds current address for steelseries API
	[System.Serializable]
	public struct CoreProps
	{
		public string address;
	}

	[System.Serializable]
	public struct MultipleEvents
	{
		public string game;
		public List<GameEvent> events;
	}

	[System.Serializable]
	public struct ColorHandler
	{
		[JsonProperty("device-type")]
		public string device_type;
		public string zone;
		public string mode;
		public object color;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public object rate;
		[JsonProperty("context-frame-key", NullValueHandling = NullValueHandling.Ignore)]
		public string context_frame_key;
	}

	[System.Serializable]
	public struct BindGameEvent
	{
		public string game;
		[JsonProperty("event")]
		public string eventName;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? min_value;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? max_value;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? icon_id;
		public bool value_optional;
		public List<ColorHandler> handlers;
	}

	[System.Serializable]
	public struct EventData
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? value;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Frame frame;
	}

	public struct Frame
	{
		[JsonProperty("zone-one-color", NullValueHandling = NullValueHandling.Ignore)]
		public object zone_one_color;
	}

	[System.Serializable]
	public struct GameEvent
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string game;
		[JsonProperty("event")]
		public string eventName;
		public EventData data;
	}

	[System.Serializable]
	public struct StaticColorDefinition
	{
		public int red;
		public int green;
		public int blue;
	}

	[System.Serializable]
	public struct GradientColorDefinition
	{
		public Gradient gradient;
	}

	[System.Serializable]
	public struct Gradient
	{
		public StaticColorDefinition zero;
		public StaticColorDefinition hundred;
	}

	[System.Serializable]
	public struct RangeColorDefinition
	{
		public StaticColorDefinition low;
		public StaticColorDefinition high;
		public object color;
	}
}
#pragma warning restore 0649
