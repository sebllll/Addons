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
	[PluginInfo(Name = "DeciTime", Category = "Value", Help = "hours, minutes, seconds to decimal time", Tags = "")]
	#endregion PluginInfo
	public class ValueDeciTimeNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Hour", DefaultValue = 12.0)]
		IDiffSpread<int> FHour;
		
		[Input("Minute", DefaultValue = 0.0)]
		IDiffSpread<int> FMinute;
		
		[Input("Second", DefaultValue = 0.0)]
		IDiffSpread<double> FSecond;
		
		[Output("Decimal")]
		ISpread<double> FDec;
		
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		
		public void Evaluate(int SpreadMax)
		{
			FDec.SliceCount = SpreadMax;
			if (FHour.IsChanged || FMinute.IsChanged || FSecond.IsChanged)
			{
				for (int i = 0; i < SpreadMax; i++)
				{
					string Time = Convert.ToString(FHour[i]) +":"+ Convert.ToString(FMinute[i]) +":"+ Convert.ToString(FSecond[i]);
					
					FDec[i] = (double)Convert.ToDecimal(TimeSpan.Parse(Time).TotalHours);
				}
			}
			
			
			//FLogger.Log(LogType.Debug, "hi tty!");
		}
		
		
		
		
		
	}
}
