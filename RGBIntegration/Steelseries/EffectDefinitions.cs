﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBIntegration.Steelseries
{
	public class EffectDefinitions
	{
		public static BindGameEvent GetBindGameEvent(RGBEffectController controller)
		{
			switch(controller.GetEventName())
			{
				case "HEALTH":
					return new BindGameEvent 
					{
						game = "OUTER_WILDS",
						eventName = "HEALTH",
						min_value = 0,
						max_value = 100,
						handlers = new List<ColorHandler> 
						{
							new ColorHandler() {
								device_type = "keyboard",
								zone = "function-keys",
								color = new GradientColorDefinition() 
								{
									gradient = new Gradient() 
									{
										zero = new StaticColorDefinition() 
										{
											red = 255,
											green = 0,
											blue = 0
										},
										hundred = new StaticColorDefinition()
										{
											red = 0,
											green = 255,
											blue = 0
										},
									}
								},
								mode = "percent"
							}
						}
					};
				case "OXYGEN":
					return new BindGameEvent 
					{
						game = "OUTER_WILDS",
						eventName = "OXYGEN",
						min_value = 0,
						max_value = 100,
						handlers = new List<ColorHandler> 
						{
							new ColorHandler() {
								device_type = "keyboard",
								zone = "number-keys",
								color = new StaticColorDefinition() 
								{
									red = 60,
									green = 255,
									blue = 255
								},
								mode = "percent"
							}
						}
					};
				case "FUEL":
					return new BindGameEvent 
					{
						game = "OUTER_WILDS",
						eventName = "FUEL",
						min_value = 0,
						max_value = 100,
						handlers = new List<ColorHandler>
						{
							new ColorHandler() {
								device_type = "keyboard",
								zone = "q-row",
								color = new StaticColorDefinition()
								{
									red = 153,
									green = 51,
									blue = 0
								},
								mode = "percent"
							}
						}
					};
				case "BACKGROUND":
					//TODO: Actual background, multiple binds per event
					return new BindGameEvent() 
					{
						game = "OUTER_WILDS",
						eventName = "BACKGROUND",
						value_optional = true,
						handlers = new List<ColorHandler> 
						{
							new ColorHandler() {
								device_type = "mouse",
								zone = "logo",
								color = new StaticColorDefinition() 
								{
									red = 255,
									green = 127,
									blue = 0
								},
								mode = "context-color",
								context_frame_key = "zone-one-color"
							}
						}
					};
			}

			return new BindGameEvent();
		}
	}
}