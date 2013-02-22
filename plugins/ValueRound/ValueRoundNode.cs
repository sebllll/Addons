#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using VVVV.Utils.Streams;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	public enum Method
	{
		round,
		ceil,
		floor
	}
	
	#region PluginInfo
	[PluginInfo(Name = "Round", Category = "Value", Help = "mathematical rounbd with optional ceiling or floor behaviour", Tags = "")]
	#endregion PluginInfo
	public class ValueRoundNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		protected IDiffSpread<double> FInput;
		
		[Input("Input Stream", DefaultValue = 1.0)]
		protected IInStream<double> FInStream;
		
		[Input("Digits", DefaultValue = 0, MinValue=0, MaxValue=15)]
		protected IDiffSpread<int> FDigits;
		
		[Input("method", DefaultEnumEntry = "round")]
		protected IDiffSpread<Method> FMethod;
		
		[Output("Output")]
		protected ISpread<double> FOutput;
		
		[Output("Output Stream")]
		protected IOutStream<double> FOutStream;
		
		readonly byte[] FBuffer = new byte[1024];
		
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = SpreadMax;
			
			if (FInput.IsChanged || FMethod.IsChanged || FDigits.IsChanged)
			{
				for (int i = 0; i < SpreadMax; i++)
				{
					if((int)FMethod[i] == 0)
					{
						FOutput[i] = Math.Round(FInput[i], Math.Abs(FDigits[i]));
					}
					else if((int)FMethod[i] == 1)
					{
						FOutput[i] = Math.Ceiling(FInput[i]);
					}
					else if((int)FMethod[i] == 2)
					{
						FOutput[i] = Math.Floor(FInput[i]);
					}
				}
			}
		}
	}
}
