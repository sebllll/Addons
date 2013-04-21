#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "TimeString", Category = "String", Help = "gives a Time String based on the given Format String", Tags = "Time, Astronomy", Author = "sebl")]
	#endregion PluginInfo
	public class StringTimeStringNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Format", DefaultString = "yyyy-MM-dd-HH-mm-ss-ff")]
		ISpread<string> FFormat;
		
		[Output("TimeString")]
		ISpread<string> FOutput;
		
		[Output("Ticks")]
		ISpread<double> FTicks;
		
		[Output("Decimal")]
		ISpread<double> FDec;
		
		
		//		[Output("errors")]
		//		ISpread<string> FError;
		
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = SpreadMax;
			//			FError.SliceCount = SpreadMax;
			
			for (int i = 0; i < SpreadMax; i++)
			{
				try
				{
					FOutput[i] = DateTime.Now.ToString(FFormat[i]);
					FTicks[i] = (double)DateTime.Now.Ticks;
					//					FFrac[i] =
					
					TimeSpan tspan = TimeSpan.FromHours( (double) 1.50);
//					FFrac[i] = tspan.ToString();
//					Convert.ToDecimal(TimeSpan.Parse("11:30").TotalHours);
					
					FDec[i] = (double)Convert.ToDecimal(TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss")).TotalHours);
					
				}
				catch (FormatException e)
				{
					FLogger.Log(LogType.Debug, "exception: " + e);
					//FError[i] = e.ToString();
				}
			}
			
			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}
}
