#region usings
using System;
using System.ComponentModel.Composition;

//using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
//using VVVV.Utils.VColor;
//using VVVV.Utils.VMath;
//
//using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "AsValue", Category = "Byte", Help = "Convert ByteCode to Values", Tags = "byte, value, compress")]
	#endregion PluginInfo
	public class ByteAsValueNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultString = "hello c#")]
		ISpread<string> FInput;

		[Output("Output")]
		ISpread<double> FOutput;

//		[Import()]
//		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{

			FOutput.SliceCount = SpreadMax;

			for (int i = 0; i < SpreadMax; i++)
			FOutput[i] = BitConverter.ToSingle(System.Text.Encoding.GetEncoding(1252).GetBytes(FInput[i]),0);

			//FOutput2.AssignFrom(s);
			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}
}
