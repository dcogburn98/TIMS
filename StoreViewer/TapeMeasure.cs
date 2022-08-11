using System;
using System.Collections.Generic;
using StereoKit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreViewer
{
	public class TapeMeasure
	{
		public static Pose tapeMeasurePose = new Pose(0, -0.5f, -0.3f, Quat.FromAngles(0, 0, 0));
		public Model tapeMeasure = Model.FromFile("Models/Tape Measure.stl");

		public Mesh tape = Mesh.GenerateCube(new Vec3(U.mm * 20.0f, U.cm * 8.08f, U.mm * 1.0f));
		public List<Pose> tapeLinks = new List<Pose>();
		public Pose tapePose = new Pose(0, -0.5f, -0.3f, Quat.FromAngles(0, 0, 0));
		public bool beingHeld = false;
		public float distancePulled = 0.0f;

		public TapeMeasure()
		{

		}

		public void DrawTapeMeasure()
		{
			if (UI.Handle("TapeMeasure", ref tapeMeasurePose, tapeMeasure.Bounds * 0.001f))
				beingHeld = true;
			else
				beingHeld = false;
			tapeMeasure.Draw(tapeMeasurePose.ToMatrix(0.001f));

			if (beingHeld)
			{
				
			}
		}
	}
}
