using System;
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
						value_optional = true,
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
						value_optional = true,
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
						value_optional = true,
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
					// Steelseries, would you care to explain why there is no easy way to set every zone of a zoned keyboard to one colour?

					List<ColorHandler> allHandlers = new List<ColorHandler>()
					{
						new ColorHandler() 
						{
							device_type = "mouse",
							zone = "logo",
							color = new StaticColorDefinition()
							{
								red = 255,
								green = 158,
								blue = 7
							},
							mode = "context-color",
							context_frame_key = "zone-one-color"
						},
						new ColorHandler() 
						{
							device_type = "keyboard",
							zone = "all",
							color = new StaticColorDefinition()
							{
								red = 255,
								green = 158,
								blue = 7
							},
							mode = "context-color",
							context_frame_key = "zone-one-color"
						}
					};

					string[] zones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty", "twenty - one", "twenty - two", "twenty - three", "twenty - four", "twenty - five", "twenty - six", "twenty - seven", "twenty - eight", "twenty - nine", "thirty", "thirty - one", "thirty - two", "thirty - three", "thirty - four", "thirty - five", "thirty - six", "thirty - seven", "thirty - eight", "thirty - nine", "forty", "forty - one", "forty - two", "forty - three", "forty - four", "forty - five", "forty - six", "forty - seven", "forty - eight", "forty - nine", "fifty", "fifty - one", "fifty - two", "fifty - three", "fifty - four", "fifty - five", "fifty - six", "fifty - seven", "fifty - eight", "fifty - nine", "sixty", "sixty - one", "sixty - two", "sixty - three", "sixty - four", "sixty - five", "sixty - six", "sixty - seven", "sixty - eight", "sixty - nine", "seventy", "seventy - one", "seventy - two", "seventy - three", "seventy - four", "seventy - five", "seventy - six", "seventy - seven", "seventy - eight", "seventy - nine", "eighty", "eighty - one", "eighty - two", "eighty - three", "eighty - four", "eighty - five", "eighty - six", "eighty - seven", "eighty - eight", "eighty - nine", "ninety", "ninety - one", "ninety - two", "ninety - three", "ninety - four", "ninety - five", "ninety - six", "ninety - seven", "ninety - eight", "ninety - nine", "one - hundred", "one - hundred - one", "one - hundred - two", "one - hundred - three" };

					for (int i = 0; i < zones.Length; i++)
					{
						allHandlers.Add(new ColorHandler()
						{
							device_type = "keyboard",
							zone = zones[i],
							color = new StaticColorDefinition() 
							{
								red = 255,
								green = 158,
								blue = 7
							},
							mode = "context-color",
							context_frame_key = "zone-one-color"
						});
					}

					return new BindGameEvent() 
					{
						game = "OUTER_WILDS",
						eventName = "BACKGROUND",
						value_optional = true,
						handlers = allHandlers
					};
				case "FLASHLIGHT":
					return new BindGameEvent() {
						game = "OUTER_WILDS",
						eventName = "FLASHLIGHT",
						value_optional = true,
						handlers = new List<ColorHandler>
						{
							new ColorHandler() {
								device_type = "mouse",
								zone = "wheel",
								color = new StaticColorDefinition()
								{
									red = 0,
									green = 0,
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
