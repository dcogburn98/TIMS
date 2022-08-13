using System;
using System.Collections.Generic;
using System.Drawing;
using StereoKit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel.Planogram.Shelving;

namespace StoreViewer
{
	public class GondolaShelfSK
	{
		public GondolaRow gondola;
		public List<Pose> shelfPoses;
		public List<Pose> uprightPoses;
		public List<Pose> backboardPoses;
		public Mesh backboardMesh;
		public Model uprightModel = Model.FromFile("Models/Gondola Upright 6ft.stl");
		public Model shelfModel = Model.FromFile("Models/Gondola4ft18in.stl");
		public Material pegboardMaterial = Material.Default.Copy();


		public GondolaShelfSK(Point position)
		{
			gondola = new GondolaRow(position);
			gondola.orientation = GondolaRow.Orientation.Widthwise;
			for (int j = 0; j != 3; j++)
				gondola.AddGondola();

			shelfPoses = new List<Pose>();
			uprightPoses = new List<Pose>();
			backboardPoses = new List<Pose>();

			pegboardMaterial[MatParamName.DiffuseTex] = Tex.FromFile("pegboard.jpg");
			pegboardMaterial[MatParamName.TexScale] = 3.0f;

			int i = 0;
			foreach (Gondola shelfSection in gondola.gondolas)
			{
				int offset = shelfSection.width == GondolaShelf.ShelfWidths.in48 ? 48 :
					shelfSection.width == GondolaShelf.ShelfWidths.in36 ? 36 :
					shelfSection.width == GondolaShelf.ShelfWidths.in24 ? 24 : 0;

				
				foreach (GondolaShelf shelf in shelfSection.shelfPointsLeftSide)
				{
					if (gondola.orientation == GondolaRow.Orientation.Widthwise)
						shelfPoses.Add(new Pose(
							(offset * U.inch) * i, 
							World.BoundsPose.position.y + (U.inch * shelf.uprightPoint), 
							U.cm * -2.5f, 
							Quat.FromAngles(-90, 0, 0)));
					else
						shelfPoses.Add(new Pose(
							U.cm * -2.5f, 
							World.BoundsPose.position.y + U.inch * shelf.uprightPoint, 
							0 - (offset * U.inch) * i, 
							Quat.FromAngles(-90, 90, 0)));
					
				}
				foreach (GondolaShelf shelf in shelfSection.shelfPointsRightSide)
				{
					if (gondola.orientation == GondolaRow.Orientation.Widthwise)
						shelfPoses.Add(new Pose(
							U.ft * 4 + (offset * U.inch) * i, 
							World.BoundsPose.position.y + U.inch * shelf.uprightPoint, 
							U.cm * 2.5f, 
							Quat.FromAngles(-90, 180, 0)));
					else
						shelfPoses.Add(new Pose(
							U.cm * 2.5f, 
							World.BoundsPose.position.y + U.inch * shelf.uprightPoint, 
							0 - (U.ft * 4 + (offset * U.inch) * i), 
							Quat.FromAngles(-90, 270, 0)));
				}

				if (gondola.orientation == GondolaRow.Orientation.Widthwise)
				{
					if (shelfSection.baseShelfLeftSide != null)
						shelfPoses.Add(new Pose(
							(offset * U.inch) * i, 
							World.BoundsPose.position.y + U.inch * 6, 
							U.cm * -2.5f, 
							Quat.FromAngles(-90, 0, 0)));
					if (shelfSection.baseShelfRightSide != null)
						shelfPoses.Add(new Pose(
							U.ft*4 + (offset * U.inch) * i, 
							World.BoundsPose.position.y + U.inch * 6, 
							U.cm * 2.5f, 
							Quat.FromAngles(-90, 180, 0)));
				}
				else
				{
					if (shelfSection.baseShelfLeftSide != null)
						shelfPoses.Add(new Pose(
							U.cm * -2.5f, 
							World.BoundsPose.position.y + U.inch * 6,
							0 - (offset * U.inch) * i, 
							Quat.FromAngles(-90, 90, 0)));
					if (shelfSection.baseShelfRightSide != null)
						shelfPoses.Add(new Pose(
							U.cm * 2.5f, 
							World.BoundsPose.position.y + U.inch * 6, 
							0 - (U.ft * 4 + (offset * U.inch) * i), 
							Quat.FromAngles(-90, 270, 0)));
				}

				if (gondola.orientation == GondolaRow.Orientation.Widthwise)
				{
					if (i == 0)
						uprightPoses.Add(new Pose(
							position.X + (offset * U.inch) * i, 
							World.BoundsPose.position.y, 
							position.Y, 
							Quat.FromAngles(-90, 0, 0)));
					uprightPoses.Add(new Pose(
						position.X + 1.2192f + (offset * U.inch) * i, 
						World.BoundsPose.position.y, 
						position.Y, 
						Quat.FromAngles(-90, 0, 0)));
				}
				else
				{
					if (i == 0)
						uprightPoses.Add(new Pose(
							position.X, 
							World.BoundsPose.position.y, 
							position.Y - (offset * U.inch) * i, 
							Quat.FromAngles(-90, 90, 0)));
					uprightPoses.Add(new Pose(
						position.X, 
						World.BoundsPose.position.y, 
						(position.Y - U.ft*4) - (offset * U.inch) * i, 
						Quat.FromAngles(-90, 90, 0)));
				}

				if (gondola.orientation == GondolaRow.Orientation.Widthwise)
				{
					backboardMesh = Mesh.GenerateCube(new Vec3(
						U.ft * 3.9f,
						U.ft * 6.0f,
						U.inch * 1.5f));
					backboardPoses.Add(new Pose(
						((U.ft * 4.0f) * i) + (2.0f*U.ft),
						(-8.0f * U.inch),
						0.0f,
						Quat.FromAngles(0, 0, 0)));
				}
				else
				{
					backboardMesh = Mesh.GenerateCube(new Vec3(
						U.ft * 6.0f,
						U.ft * 3.9f,
						U.inch * 1.5f));
					backboardPoses.Add(new Pose(
						0.0f * (U.ft * 4),
						0.0f,
						(U.ft * 4.0f) * i,
						Quat.FromAngles(-90, 90, 0)));
				}
				i++;
			}
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
			foreach (Pose pose in backboardPoses)
			{
				backboardMesh.Draw(pegboardMaterial, pose.ToMatrix(1.0f));
			}
		}
	}
}
