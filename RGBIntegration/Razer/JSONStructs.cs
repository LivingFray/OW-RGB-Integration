using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 0649
namespace RGBIntegration.Razer
{
	[System.Serializable]
	public struct InitResponse
	{
		public int sessionid;
		public string uri;
	}

	[System.Serializable]
	public struct EffectResponse
	{
		public int result;
		public string id;
	}


	[System.Serializable]
	public struct ChromaCustom
	{
		public string effect;
		public int[,] param;
	}

	[System.Serializable]
	public struct Static
	{
		public int color;
	}

	[System.Serializable]
	public struct ChromaStatic
	{
		public string effect;
		public Static param;
	}
}
#pragma warning restore 0649