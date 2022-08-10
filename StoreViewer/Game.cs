using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

using TIMSServerModel.Planogram.Shelving;

namespace StoreViewer
{
	class Light
	{
		public Pose pose;
		public Vec3 color;
	}
	enum LightMode
	{
		Lights,
		Image,
	}

	class DemoSky : ITest
	{
		static List<Light> lights = new List<Light>();
		static LightMode mode = LightMode.Lights;
		static Pose floorPose = new Pose(0, World.BoundsPose.position.y, -0.3f, Quat.FromAngles(0, 0, 0));

		Material floorMaterial = Material.Default.Copy();
		Mesh floorMesh = Mesh.GeneratePlane(new Vec2(10));
		Mesh lightMesh = Mesh.GenerateSphere(1);
		Material lightSrcMat = new Material(Default.ShaderUnlit);

		TapeMeasure tape = new TapeMeasure();
		GondolaShelfSK shelfSK = new GondolaShelfSK();

		public void Initialize() 
		{
			floorMaterial[MatParamName.DiffuseTex] = Tex.FromFile("devgrid.jpg");
			floorMaterial[MatParamName.TexScale] = 10.0f;
		}
		public void Shutdown() => Platform.FilePickerClose();
		public void Update()
		{
			tape.DrawTapeMeasure();
			shelfSK.DrawGondolaShelf();

			floorMesh.Draw(floorMaterial, floorPose.ToMatrix(1.0f));

			if (Input.Key(Key.Esc) == BtnState.Active)
			{
				Environment.Exit(0);
			}
		}

		bool LightHandle(int i)
		{
			UI.PushId("window" + i);
			bool dirty = UI.HandleBegin("Color", ref lights[i].pose, new Bounds(Vec3.One * 3 * U.cm));
			UI.LayoutArea(new Vec3(6, -3, 0) * U.cm, new Vec2(10, 0) * U.cm);
			if (lights[i].pose.position.Length > 0.5f)
				lights[i].pose.position = lights[i].pose.position.Normalized * 0.5f;

			lightMesh.Draw(lightSrcMat, Matrix.TS(Vec3.Zero, 3 * U.cm), Color.HSV(lights[i].color));

			dirty = UI.HSlider("H", ref lights[i].color.v.X, 0, 1, 0, 10 * U.cm) || dirty;
			dirty = UI.HSlider("S", ref lights[i].color.v.Y, 0, 1, 0, 10 * U.cm) || dirty;
			dirty = UI.HSlider("V", ref lights[i].color.v.Z, 0, 1, 0, 10 * U.cm) || dirty;

			UI.HandleEnd();
			Lines.Add(
				lights[i].pose.position, Vec3.Zero,
				Color.HSV(lights[i].color) * LightIntensity(lights[i].pose.position) * 0.5f,
				U.mm);

			UI.PopId();
			return dirty;
		}

		void UpdateLights()
		{
			SphericalHarmonics lighting = SphericalHarmonics.FromLights(lights
				.ConvertAll(a => new SHLight
				{
					directionTo = a.pose.position.Normalized,
					color = Color.HSV(a.color) * LightIntensity(a.pose.position)
				})
				.ToArray());

			Renderer.SkyTex = Tex.GenCubemap(lighting);
			Renderer.SkyLight = lighting;
		}

		float LightIntensity(Vec3 pos)
		{
			return Math.Max(0, 2 - pos.Magnitude * 4);
		}

		void InitGondolaRow()
		{
			Gondola gondola = new Gondola();
			gondola.baseShelf = new GondolaShelf(true);
			gondola.shelfPoints.Add(new GondolaShelf(false) { shelfHeight = 12 });
			gondola.shelfPoints.Add(new GondolaShelf(false) { shelfHeight = 36 });
			gondola.shelfPoints.Add(new GondolaShelf(false) { shelfHeight = 48 });

			List<Pose> shelfPoses = new List<Pose>();
			foreach (GondolaShelf shelf in gondola.shelfPoints)
			{
				shelfPoses.Add(new Pose(0, shelf.shelfHeight * 0.1f, 0, Quat.FromAngles(90, 0, 0)));
			}
		}
	}

	public class GondolaShelfSK
	{
		public Gondola gondola;
		public List<Pose> shelfPoses;
		public List<Pose> uprightPoses;
		public Model uprightModel = Model.FromFile("Models/Gondola Upright 6ft.stl");
		public Model shelfModel = Model.FromFile("Models/Gondola4ft18in.stl");

		public GondolaShelfSK()
		{
			gondola = new Gondola();
			gondola.baseShelf = new GondolaShelf(true) { shelfHeight = 6};
			gondola.width = 48;

			gondola.shelfPoints.Add(new GondolaShelf(false) { shelfHeight = 12 });
			gondola.shelfPoints.Add(new GondolaShelf(false) { shelfHeight = 36 });
			gondola.shelfPoints.Add(new GondolaShelf(false) { shelfHeight = 48 });

			shelfPoses = new List<Pose>();
			foreach (GondolaShelf shelf in gondola.shelfPoints)
			{
				shelfPoses.Add(new Pose(0, shelf.shelfHeight * 0.04f - 1.2f, U.cm * -2.5f, Quat.FromAngles(-90, 0, 0)));
			}
			shelfPoses.Add(new Pose(0, gondola.baseShelf.shelfHeight * 0.04f - 1.2f, U.cm * -2.5f, Quat.FromAngles(-90, 0, 0)));

			uprightPoses = new List<Pose>();
			uprightPoses.Add(new Pose(0, World.BoundsPose.position.y, 0, Quat.FromAngles(-90, 0, 0)));
			uprightPoses.Add(new Pose(1.2192f, World.BoundsPose.position.y, 0, Quat.FromAngles(-90, 0, 0)));
		}

		public void DrawGondolaShelf()
		{
			foreach (Pose shelf in shelfPoses)
			{
				shelfModel.Draw(shelf.ToMatrix(0.001f));
			}
			foreach (Pose upright in uprightPoses)
			{
				uprightModel.Draw(upright.ToMatrix(0.001f));
			}
		}
	}

	public class TapeMeasure
	{
		public static Pose tapeMeasurePose = new Pose(0, -0.5f, -0.3f, Quat.FromAngles(0,0,0));
		public Model tapeMeasure = Model.FromFile("Models/Tape Measure.stl");

		public Mesh tape = Mesh.GenerateCube(new Vec3(U.cm * 16.0f, U.mm * 1.0f, U.cm * 5.08f));
		public List<Pose> tapeLinks = new List<Pose>();
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
				int linksCreated = (int)Math.Floor((decimal)(distancePulled / (U.cm * 5.08f))) + 1;
				int i = 0;
				while (tapeLinks.Count < linksCreated)
				{
					tapeLinks.Add(new Pose(tapeMeasurePose.position, tapeMeasurePose.orientation));
					i++;
				}
				foreach (Pose p in tapeLinks)
				{
					Pose pp = tapeLinks[tapeLinks.IndexOf(p)];
					UI.Handle("TapeLink" + i, ref pp, tape.Bounds);
					tape.Draw(Material.Default, tapeLinks[i].ToMatrix(1.0f));
				}
			}
			else
			{
				tapeLinks.Clear();
			}

		}
	}
}