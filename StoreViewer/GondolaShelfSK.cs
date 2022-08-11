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
		public List<Mesh> backboardMeshes;
		public Model uprightModel = Model.FromFile("Models/Gondola Upright 6ft.stl");
		public Model shelfModel = Model.FromFile("Models/Gondola4ft18in.stl");



		public GondolaShelfSK(Point position)
		{
			gondola = new GondolaRow(position);
			gondola.orientation = GondolaRow.Orientation.Lengthwise;

			shelfPoses = new List<Pose>();
			uprightPoses = new List<Pose>();
			backboardMeshes = new List<Mesh>();

			foreach (Gondola shelfSection in gondola.gondolas)
			{
				foreach (GondolaShelf shelf in shelfSection.shelfPointsLeftSide)
				{
					if (gondola.orientation == GondolaRow.Orientation.Widthwise)
						shelfPoses.Add(new Pose(0, World.BoundsPose.position.y + U.inch * shelf.uprightPoint, U.cm * -2.5f, Quat.FromAngles(-90, 0, 0)));
					else
						shelfPoses.Add(new Pose(U.cm * -2.5f, World.BoundsPose.position.y + U.inch * shelf.uprightPoint, 0, Quat.FromAngles(-90, 90, 0)));
				}
				foreach (GondolaShelf shelf in shelfSection.shelfPointsRightSide)
				{
					if (gondola.orientation == GondolaRow.Orientation.Widthwise)
						shelfPoses.Add(new Pose(U.ft * 4, World.BoundsPose.position.y + U.inch * shelf.uprightPoint, U.cm * 2.5f, Quat.FromAngles(-90, 180, 0)));
					else
						shelfPoses.Add(new Pose(U.cm * 2.5f, World.BoundsPose.position.y + U.inch * shelf.uprightPoint, 0 - U.ft * 4, Quat.FromAngles(-90, 270, 0)));
				}

				if (gondola.orientation == GondolaRow.Orientation.Widthwise)
				{
					if (shelfSection.baseShelfLeftSide != null)
						shelfPoses.Add(new Pose(0, World.BoundsPose.position.y + U.inch * 6, U.cm * -2.5f, Quat.FromAngles(-90, 0, 0)));
					if (shelfSection.baseShelfRightSide != null)
						shelfPoses.Add(new Pose(U.ft*4, World.BoundsPose.position.y + U.inch * 6, U.cm * -2.5f, Quat.FromAngles(-90, 180, 0)));
				}
				else
				{
					if (shelfSection.baseShelfLeftSide != null)
						shelfPoses.Add(new Pose(U.cm * -2.5f, World.BoundsPose.position.y + U.inch * 6, 0, Quat.FromAngles(-90, 90, 0)));
					if (shelfSection.baseShelfRightSide != null)
						shelfPoses.Add(new Pose(U.cm * 2.5f, World.BoundsPose.position.y + U.inch * 6, 0 - U.ft * 4, Quat.FromAngles(-90, 270, 0)));
				}

				if (gondola.orientation == GondolaRow.Orientation.Widthwise)
				{
					uprightPoses.Add(new Pose(position.X, World.BoundsPose.position.y, position.Y, Quat.FromAngles(-90, 0, 0)));
					uprightPoses.Add(new Pose(position.X + 1.2192f, World.BoundsPose.position.y, position.Y, Quat.FromAngles(-90, 0, 0)));
				}
				else
				{
					uprightPoses.Add(new Pose(position.X, World.BoundsPose.position.y, position.Y, Quat.FromAngles(-90, 90, 0)));
					uprightPoses.Add(new Pose(position.X, World.BoundsPose.position.y, position.Y - U.ft*4, Quat.FromAngles(-90, 90, 0)));
				}

				if (gondola.orientation == GondolaRow.Orientation.Widthwise)
				{
					backboardMeshes.Add(Mesh.GenerateCube(new Vec3(U.ft * 1.5f, U.ft * 6.0f, U.inch * 3.9f)));
				}
				else
				{
					backboardMeshes.Add(Mesh.GenerateCube(new Vec3(U.ft * 3.9f, U.ft * 6.0f, U.inch * 1.5f)));
				}
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
			foreach (Mesh mesh in backboardMeshes)
			{
				mesh.Draw(Material.Default, new Pose()
			}
		}
	}
}
