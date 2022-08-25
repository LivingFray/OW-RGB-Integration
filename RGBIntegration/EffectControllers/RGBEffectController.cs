using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntegration
{
	public interface RGBEffectController
	{
		public string GetEventName();
		public void Update(RGBIntegration mod);
	}
}
