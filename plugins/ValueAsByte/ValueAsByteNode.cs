#region usings
using System;
using System.ComponentModel.Composition;

//using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
//using VVVV.Utils.VColor;
//using VVVV.Utils.VMath;

//using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "AsByte", Category = "Value", Help = "Convert Values to ByteCode", Tags = "byte, value, compress")]
	#endregion PluginInfo
	public class ValueAsByteNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		IDiffSpread<double> FInput;
		
		[Output("Output Spread")]
		ISpread<string> FOutput;
		
		[Output("Output Concat")]
		ISpread<string> FOutputConcat;
		
//		[Import()]
//		ILogger FLogger;
		#endregion fields & pins
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if(FInput.IsChanged)
			
			{
				//FLogger.Log(LogType.Debug, "hi tty!");
				FOutput.SliceCount = SpreadMax;
				FOutputConcat.SliceCount = 1;
				
				for (int i = 0; i < SpreadMax; i++)
				{
					FOutput[i] = System.Text.Encoding.GetEncoding(1252).GetString(BitConverter.GetBytes((float)FInput[i]));
				}
				
				FOutputConcat[0] = string.Concat(FOutput);
			}
			
		}
	}
}

