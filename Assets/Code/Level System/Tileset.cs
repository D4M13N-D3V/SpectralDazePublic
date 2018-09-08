using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Levels
{
	[CreateAssetMenu(fileName = "Tileset", menuName = "Spectral Daze/Level Editor/Tileset")]
	public class Tileset : ScriptableObject
	{
		public GameObject[] WallPieces;
		public GameObject Wall, WallOuterCorner, WallInnerCorner, Floor;
		public Color FloorColor;
		public int TileSize = 1;
	}
}