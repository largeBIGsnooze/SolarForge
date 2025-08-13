using System;
using System.Collections.Generic;
using Solar.Rendering;
using Solar.Simulations;

namespace SolarForge.Units
{

	public class SkinStageMeshTable
	{



		public Mesh BaseMesh { get; set; }




		public Dictionary<WeaponDefinitionName, TurretMeshes> WeaponNameToTurretMeshesMap { get; set; }




		public List<Mesh> ChildMeshes { get; set; }
	}
}
