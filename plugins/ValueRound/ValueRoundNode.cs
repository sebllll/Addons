#region usings
using System;
using System.IO;
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
		IDiffSpread<double> FInput;
		
		[Input("Input Stream", DefaultValue = 1.0)]
		IInStream<double> FStreamIn;
		
		[Input("Digits", DefaultValue = 0, MinValue=0, MaxValue=15)]
		IDiffSpread<int> FDigits;
		
		[Input("Digits Stream", DefaultValue = 0, MinValue=0, MaxValue=15)]
		IInStream<int> FDigitsStream;
		
		[Input("method", DefaultEnumEntry = "round")]
		IDiffSpread<Method> FMethod;
		
		[Output("Output")]
		ISpread<double> FOutput;
		
		[Output("Output Stream")]
		IOutStream<double> FStreamOut;
		
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		
		//called when all inputs and outputs defined above are assigned from the host
//		public void OnImportsSatisfied()
//		{
//			//start with an empty stream output
//			FStreamOut.SliceCount = 0;
//		}
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = SpreadMax;
			//FStreamOut.SetLength(SpreadMax);
			//FStreamOut.SetLengthBy(this, FStreamIn);
			
			var maxN = FStreamIn.CombineWith(FDigitsStream);
			var bufferInput = new double[1024];
			var bufferdigits = new int[1024];
			var bufferOut = new double[1024];
			
			var reader1 = FStreamIn.GetCyclicReader();
			var reader2 = FDigitsStream.GetCyclicReader();
			try
			{
				var numSlicesToRead = maxN;
				while (numSlicesToRead > 0)
				{
					var blockSize = Math.Min(numSlicesToRead, bufferInput.Length);
					reader1.Read(bufferInput, 0, blockSize);
					reader2.Read(bufferdigits, 0, blockSize);
					for (int i = 0; i < blockSize; i++)
					{
						FOutput[i] = Math.Round(bufferInput[i], bufferdigits[i]);
						// put it into bufferOut, so that this one can be copied into the Pin atr the end?
					}
					numSlicesToRead -= blockSize;
				}
			}
			finally
			{
				reader1.Dispose();
				reader2.Dispose();
			}
			
			//FStreamOut.Flush(true);
			//FStreamOut.Write( bufferOut,0, bufferOut.Length );
			//FStreamOut.AssignFrom();
			
			
			// the simple variant with the use of spreads
			
//			if (FInput.IsChanged || FMethod.IsChanged || FDigits.IsChanged)
//			{
//				for (int i = 0; i < SpreadMax; i++)
//				{
//					if((int)FMethod[i] == 0)
//					{
//						FOutput[i] = Math.Round(FInput[i], Math.Abs(FDigits[i]));
//					}
//					else if((int)FMethod[i] == 1)
//					{
//						FOutput[i] = Math.Ceiling(FInput[i]);
//					}
//					else if((int)FMethod[i] == 2)
//					{
//						FOutput[i] = Math.Floor(FInput[i]);
//					}
//				}
//			}
		}
	}
}
