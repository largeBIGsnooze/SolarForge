using System;

namespace SolarForge.Utility
{

	public interface IDataSourcePathResolver
	{

		string ResolveDataSourceSavePath(string path);
	}
}
