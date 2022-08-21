using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 0649
namespace RGBIntergration.Steelseries
{
	// coreProps.json holds current address for steelseries API
	[System.Serializable]
	struct CoreProps
	{
		public string address;
	}

	[System.Serializable]
	struct ColorHandler
	{
		[JsonProperty("device-type")]
		public string device_type;
		public string zone;
		public string mode;
		public object color;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public object rate;
		[JsonProperty("context_frame_key", NullValueHandling = NullValueHandling.Ignore)]
		public string context_frame_key;
	}

	[System.Serializable]
	struct BindGameEvent
	{
		public string game;
		[JsonProperty("event")]
		public string eventName;
		public int min_value;
		public int max_value;
		public int icon_id;
		public bool value_optional;
		public List<ColorHandler> handlers;
	}

	[System.Serializable]
	struct EventData
	{
		public int value;
	}

	[System.Serializable]
	struct GameEvent
	{
		public string game;
		[JsonProperty("event")]
		public string eventName;
		public EventData data;
	}

	[System.Serializable]
	struct StaticColorDefinition
	{
		public int red;
		public int green;
		public int blue;
	}

	[System.Serializable]
	struct GradientColorDefinition
	{
		public StaticColorDefinition zero;
		public StaticColorDefinition hundred;
	}

	[System.Serializable]
	struct RangeColorDefinition
	{
		public StaticColorDefinition low;
		public StaticColorDefinition high;
		public object color;
	}
}
#pragma warning restore 0649
