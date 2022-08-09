using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

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
		static Tex cubemap = null;
		static bool cubelightDirty = false;
		static Pose previewPose = new Pose(0, -0.1f, -0.3f, Quat.LookDir(-Vec3.Forward));

		Model previewModel = Model.FromFile("Gondola4ft18in.stl");
		Mesh lightMesh = Mesh.GenerateSphere(1);
		Material lightProbeMat = Default.Material;
		Material lightSrcMat = new Material(Default.ShaderUnlit);

		public void Initialize() 
		{ 
		}
		public void Shutdown() => Platform.FilePickerClose();
		public void Update()
		{
			//lightMesh.Draw(lightProbeMat, Matrix.TS(Vec3.Zero, 0.04f));
			previewPose.orientation.w = 0.5f;
			previewPose.orientation.x = 0.5f;
			UI.Handle("Preview", ref previewPose, previewModel.Bounds * 0.001f);
			previewModel.Draw(previewPose.ToMatrix(0.001f));

			if (mode == LightMode.Lights)
			{
				bool needsUpdate = false;
				for (int i = 0; i < lights.Count; i++)
				{
					needsUpdate = LightHandle(i) || needsUpdate;
				}
				if (needsUpdate)
					UpdateLights();
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

		void DrawShelves()
		{

		}
	}
}