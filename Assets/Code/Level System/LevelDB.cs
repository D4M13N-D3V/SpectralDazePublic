using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpectralDaze.Levels
{
	public static class LevelDB
	{
		public static Level[] Levels;
		public static Tileset[] Tilesets;

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			Levels = Resources.LoadAll<Level>("");
			Tilesets = Resources.LoadAll<Tileset>("");
		}
	}
}