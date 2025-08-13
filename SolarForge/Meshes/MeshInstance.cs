using System;
using Solar.Math;
using Solar.Rendering;

namespace SolarForge.Meshes
{

	public class MeshInstance
	{



		public string Name { get; set; }




		public Mesh Mesh { get; set; }




		public Basis Basis { get; set; }




		public int? PickedTriangleIndex { get; set; }
	}
}
