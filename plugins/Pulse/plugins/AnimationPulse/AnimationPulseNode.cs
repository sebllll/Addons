#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using VVVV.Utils.Animation;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Pulse", Category = "Animation")]
	#endregion PluginInfo
	public class AnimationPulseNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		ISpread<Vector3D> FInput;

		[Input("Bang", IsSingle = true)]
		ISpread<bool> FBang;

		[Input("Max Lifetime", IsSingle = true)]
		ISpread<double> FMaxLifeTime;

		[Output("Output")]
		ISpread<Vector3D> FOutput;

		[Output("Age")]
		ISpread<double> FAge;

		[Import()]
		ILogger FLogger;

		[Import()]
		IHDEHost FHDEHost;

		List<Particle> FParticles = new List<Particle>();
		Random FRandom = new Random();
		#endregion fields & pins

		//called each frame by vvvv
		public void Evaluate(int SpreadMax)
		{
			//update particles and remove timed out particles
			for (int i = FParticles.Count - 1; i >= 0; i--)
				if (!FParticles[i].Update(FHDEHost.GetCurrentTime()))
					FParticles.RemoveAt(i);

			//add new particles
			if (FBang[0]) {
					var vel = new Vector3D(0, 0, 0);
					var acc = new Vector3D(0, 0, 0);
					FParticles.Add(new Particle(FHDEHost.GetCurrentTime(), FMaxLifeTime[0], FInput[0], vel, acc));
			}

			//set outputs
			FOutput.SliceCount = FParticles.Count;
			FAge.SliceCount = FParticles.Count;
			for (int i = 0; i < FParticles.Count; i++) {
				FOutput[i] = FParticles[i].Position;
				FAge[i] = FParticles[i].Age;
			}
		}
	}
}
