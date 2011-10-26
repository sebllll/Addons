#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

//using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes

{
	#region PluginInfo
	[PluginInfo(Name = "Points2LineTransform", Category = "Value", Version = "1", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C1ValuePoints2LineTransformNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		ISpread<ISpread<Vector3D>> FInput;
		
		[Input("Width", DefaultValue = 0.005)]
		ISpread<double> FWidth;
		
		[Input("Closed")]
		ISpread<bool> FClosed;
		
		[Output("Transform")]
		ISpread<Matrix4x4> FTransform;

		//[Import()]
		//ILogger FLogger;
		#endregion fields & pins
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			var closed = 0;
			if (FClosed[0])
			{
				 closed = 0;
			}else
			{
				 closed = 1;
			}
			
			
			FTransform.SliceCount = 0;
			for (int b = 0; b < FInput.SliceCount; b++)
			{		
				for (int i = 0; i < FInput[b].SliceCount-closed; i++)
				{
					Vector3D line = line = FInput[b][i] - FInput[b][i+1];
				
					Vector3D angle = VMath.PolarVVVV( line  ) * VMath.RadToCyc;
					angle.y+=0.25;
					angle*= Math.PI*2;
					
					Vector3D pos = ( (FInput[b][i] + FInput[b][i+1]) / 2  );
					
					Matrix4x4 curM = new Matrix4x4(1,0,0,0,
												   0,1,0,0,
												   0,0,1,0,
												   0,0,0,1);
					curM *= VMath.Scale(!line,FWidth[b],1);
					curM *= VMath.Rotate(0,angle.y,angle.x);
					curM *= VMath.Translate( pos  );
					
					FTransform.Add(curM);
				}
			}
		}
	}
}

