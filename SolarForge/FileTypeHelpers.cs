using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarForge
{

	public static class FileTypeHelpers
	{

		public static string[] GetFileTypeExtensions(FileType fileType)
		{
			switch (fileType)
			{
			case FileType.ParticleEffect:
				return new string[]
				{
					".particle_effect"
				};
			case FileType.BeamEffect:
				return new string[]
				{
					".beam_effect"
				};
			case FileType.Mesh:
				return new string[]
				{
					".mesh",
					".mesh_json"
				};
			case FileType.Skybox:
				return new string[]
				{
					".skybox"
				};
			case FileType.Scenario:
				return new string[]
				{
					".scenario"
				};
			case FileType.Gui:
				return new string[]
				{
					".gui"
				};
			case FileType.Brush:
				return new string[]
				{
					".brush"
				};
			case FileType.Unit:
				return new string[]
				{
					".unit"
				};
			case FileType.Gltf:
				return new string[]
				{
					".gltf"
				};
			case FileType.Sins1Mesh:
				return new string[]
				{
					".sins1_mesh"
				};
			case FileType.DeathSequence:
				return new string[]
				{
					".death_sequence"
				};
			default:
				throw new NotImplementedException();
			}
		}


		public static string GetFileTypeName(FileType fileType)
		{
			switch (fileType)
			{
			case FileType.ParticleEffect:
				return "Particle Effect";
			case FileType.BeamEffect:
				return "Beam Effect";
			case FileType.Mesh:
				return "Mesh";
			case FileType.Skybox:
				return "Skybox";
			case FileType.Scenario:
				return "Scenario";
			case FileType.Gui:
				return "Gui";
			case FileType.Brush:
				return "Brush";
			case FileType.Unit:
				return "Unit";
			case FileType.Gltf:
				return "Gltf";
			case FileType.Sins1Mesh:
				return "Sins1 Mesh";
			case FileType.DeathSequence:
				return "Death Sequence";
			default:
				throw new NotImplementedException();
			}
		}


		public static FileType GetFileTypeFromEditorType(EditorType editorType)
		{
			switch (editorType)
			{
			case EditorType.ParticleEffect:
				return FileType.ParticleEffect;
			case EditorType.BeamEffect:
				return FileType.BeamEffect;
			case EditorType.Mesh:
				return FileType.Mesh;
			case EditorType.Skybox:
				return FileType.Skybox;
			case EditorType.Scenario:
				return FileType.Scenario;
			case EditorType.Gui:
				return FileType.Gui;
			case EditorType.Brush:
				return FileType.Brush;
			case EditorType.Unit:
				return FileType.Unit;
			case EditorType.DeathSequence:
				return FileType.DeathSequence;
			default:
				throw new NotImplementedException();
			}
		}


		public static string MakeFileTypeExtensionsListString(IEnumerable<string> extensions)
		{
			return string.Join(";", from ext in extensions
			select "*" + ext);
		}
	}
}
