using StereoKit;
using StereoKit.Framework;
using System;
using System.Drawing;
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
		static Pose floorPose = new Pose(0, World.BoundsPose.position.y, -0.3f, Quat.FromAngles(0, 0, 0));
		Material floorMaterial = Material.Default.Copy();
		Mesh floorMesh = Mesh.GeneratePlane(new Vec2(U.ft*80));

		TapeMeasure tape = new TapeMeasure();
		GondolaShelfSK shelfSK = new GondolaShelfSK(new Point(0,0));

		public void Initialize() 
		{
			floorMaterial[MatParamName.DiffuseTex] = Tex.FromFile("256x256devgrid.jpg");
			floorMaterial[MatParamName.TexScale] = 80.0f;
		}
		
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

		public void Shutdown() => Platform.FilePickerClose();
	}
}