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
	
	public class Iterate<T> : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		IDiffSpread<T> FInput;
		
		[Input("Vector Size", DefaultValue = 1.0, AsInt = true, MinValue = 1.0)]
		IDiffSpread<int> FSize;
		
		[Input("Frames Interval", DefaultValue = 1.0, AsInt = true, MinValue = 1.0)]
		IDiffSpread<double> FInterval;
		
		[Input("Reset", DefaultValue = 0.0, IsBang = true, IsSingle = true)]
		ISpread<bool> FReset;
		
		[Output("Output")]
		ISpread<T> FOutput;
		
		[Output("Former Slice")]
		ISpread<int> FFormerSlice;
		
		[Output("Progress")]
		ISpread<double> FProgress;
		
		[Import()]
		ILogger FLogger;
		
		int pointer = 0;
		int interval = 0;
		#endregion fields & pins
		

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			
			
//			bool  normalInput =( (FInput[0].GetType() != typeof(Matrix4x4)) || (FInput[0].GetType() != typeof(EnumEntry)) );
			bool  normalInput = !(FInput[0].GetType()==typeof(Matrix4x4) || FInput[0].GetType()==typeof(EnumEntry));
			//FLogger.Log(LogType.Debug, "type enum: " + TE);
			
			SpreadMax = (int)Math.Ceiling((double)FInput.SliceCount / (double)FSize[0])*FSize[0];
			if ( 
			(FInput.IsChanged && normalInput == true  ) ||
//			(FInput.IsChanged && FInput.GetType() != typeof(EnumEntry) ) || 
			FReset[0] || 
			FSize.IsChanged
			)
			{
				pointer = 0;
			}
			
			FOutput.SliceCount = FSize[0];
			FFormerSlice.SliceCount = FSize[0];
			
			if (pointer < SpreadMax)
			{
				for (int i=0;i<FSize[0];i++) {
					FOutput[i] = FInput[pointer + i];
					FFormerSlice[i] = pointer + i;
				}
				interval++;
				if (interval >= FInterval[0]) {
					pointer+=FSize[0];
					interval = 0;
				}
				FProgress[0] = (double)pointer / (double)FInput.SliceCount;
			}
			else
			{
				// do nothing
			}
			
			
			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}
	
	#region PluginInfo
	[PluginInfo(
	Name = "Iterate",
	Category = "Value",
	Help = "Iterate through Spreads",
	Tags = "",
	Author = "sebl")]
	#endregion PluginInfo
	public class IterateSpreads : Iterate<double> {}
	
	#region PluginInfo
	[PluginInfo(
	Name = "Iterate",
	Category = "String",
	Help = "Iterate through String",
	Tags = "",
	Author = "sebl")]
	#endregion PluginInfo
	public class IterateString : Iterate<string> {}
	
	#region PluginInfo
	[PluginInfo(
	Name = "Iterate",
	Category = "Color",
	Help = "Iterate through Colors",
	Tags = "",
	Author = "sebl")]
	#endregion PluginInfo
	public class IterateColor : Iterate<RGBAColor> {}
	
	#region PluginInfo
	[PluginInfo(
	Name = "Iterate",
	Category = "Transform",
	Help = "Iterate through Transforms",
	Tags = "",
	Author = "sebl")]
	#endregion PluginInfo
	public class IterateTransform : Iterate<Matrix4x4> {}
	
	#region PluginInfo
	[PluginInfo(
	Name = "Iterate",
	Category = "Enumerations",
	Help = "Iterate through Enums",
	Tags = "",
	Author = "sebl")]
	#endregion PluginInfo
	public class IterateEnum : Iterate<EnumEntry> {}
}
