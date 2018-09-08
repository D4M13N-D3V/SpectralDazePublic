using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpectralDaze.Levels
{
	public class LevelBuilder : MonoBehaviour
	{
		public Level ReferenceLevel;

		[ContextMenu("Test Generate")]
		private void Start()
		{
			if (ReferenceLevel != null)
				BuildMesh();
		}

		private void BuildMesh()
		{
			ReferenceLevel.MapData = new int[16]
			{
				0,0,0,0,
				0,-1,-1,0,
				0,0,-1,0,
				-1,0,0,0
			};

			if (transform.childCount > 0)
			{
				for (int i = transform.childCount; i-- > 0;)
					DestroyImmediate(transform.GetChild(i).gameObject);
			}

			DoFloorPass(transform);
			DoWallPass(transform);
			DoCornerPass(transform);
		}

		private void DoFloorPass(Transform parent)
		{
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					if (ReferenceLevel[x, y] == -1)
						continue;
					// Make a basic quad

					float tileSize = LevelDB.Tilesets[ReferenceLevel[x, y]].TileSize;
					Vector3 spawnPos = new Vector3(x, 0, y) * tileSize;
					GameObject newFloor = GameObject.Instantiate(LevelDB.Tilesets[ReferenceLevel[x, y]].Floor, spawnPos, Quaternion.Euler(-90, 0, 0), parent);
					newFloor.name = "{" + x + ", " + y + "}";
				}
			}
		}

		/// <summary>
		/// Tuple for holding WallDirection Rotation values
		/// Item1	=	Wall Side Neighbor index
		/// Item2	=	Wall sub-bitmask neighbor index, left
		/// Item3	=	Wall sub-bitmask neighbor index, right
		/// Item4	=	Wall placement direction
		/// Item5	=	Wall placement rotation
		/// </summary>
		private static readonly Tuple<int, int, int, Vector3, Quaternion>[] WallDirectionRotationValues =
		{
			Tuple.Create(4, 2, 7, Vector3.forward, Quaternion.Euler(-90,0,-90)),
			Tuple.Create(1, 0, 2, Vector3.left, Quaternion.Euler(-90,0,-180)),
			Tuple.Create(6, 7, 5, Vector3.right, Quaternion.Euler(-90,0,0)),
			Tuple.Create(3, 5, 0, Vector3.back, Quaternion.Euler(-90,0,90)),
		};
		private void DoWallPass(Transform parent)
		{
			int[] neighborBuffer = new int[8];
			int sampleTileID = -1;
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					sampleTileID = ReferenceLevel[x, y];
					if (sampleTileID == -1) continue;

					ReferenceLevel.GetNeighborsNonAlloc(x, y, neighborBuffer);
					Tileset tileset = LevelDB.Tilesets[sampleTileID];

					foreach (var idr in WallDirectionRotationValues)
					{
						// If the neighbor in said direction has the same tile type as us, do nothing.
						if (neighborBuffer[idr.Item1] == sampleTileID) continue;

						//Debug.Log(sampleTileID);
						Vector3 spawnPos = new Vector3(x* tileset.TileSize, 0, y* tileset.TileSize);
						spawnPos += idr.Item4 * (tileset.TileSize);
						Vector3 spawnScale = Vector3.one;
						Quaternion spawnRot = idr.Item5;
						Transform spawnParent = parent.Find("{" + x + ", " + y + "}");

						//GameObject wall = GameObject.Instantiate(tileset.Wall, spawnPos, spawnRot, spawnParent);

						// Spawn walls
						bool leftSide = neighborBuffer[idr.Item2] == sampleTileID;
						bool rightSide = neighborBuffer[idr.Item3] == sampleTileID;

						GameObject.Instantiate(tileset.WallPieces[1], spawnPos, spawnRot, spawnParent);
						if (!leftSide)
							GameObject.Instantiate(tileset.WallPieces[0], spawnPos, spawnRot, spawnParent);
						if (!rightSide)
							GameObject.Instantiate(tileset.WallPieces[2], spawnPos, spawnRot, spawnParent);
					}
				}
			}
		}

		private static readonly Tuple<int, int, int, Quaternion, Vector3>[] CornerRotationValues =
		{
			Tuple.Create(2, 1, 4, Quaternion.Euler(0, 0, 180), new Vector3(-1, 0, 1)),
			Tuple.Create(7, 4, 6, Quaternion.Euler(0, 0, -90), new Vector3(1, 0, 1)),
			Tuple.Create(5, 6, 3, Quaternion.Euler(0, 0, 0), new Vector3(1, 0, -1)),
			Tuple.Create(0, 3, 1, Quaternion.Euler(0, 0, 90), new Vector3(-1, 0, -1)),
		};
		private void DoCornerPass(Transform parent)
		{
			int[] neighborBuffer = new int[8];
			int sampleTileID = -1;
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					sampleTileID = ReferenceLevel[x, y];
					if (sampleTileID == -1) continue;

					ReferenceLevel.GetNeighborsNonAlloc(x, y, neighborBuffer);
					Tileset tileset = LevelDB.Tilesets[sampleTileID];

					foreach (var cornerMask in CornerRotationValues)
					{
						// If the corner neighbor itself is our type, we won't need to place a corner here.
						if (neighborBuffer[cornerMask.Item1] == sampleTileID)
							continue;

						// If either of the 'sides' of the corner aren't the correct type, assume we don't need to place a corner.
						if (neighborBuffer[cornerMask.Item2] != sampleTileID || neighborBuffer[cornerMask.Item3] != sampleTileID)
							continue;

						// Position is that of the corner tile

						Vector3 spawnPos = new Vector3(x * tileset.TileSize, 0, y * tileset.TileSize) + (cornerMask.Item5 * tileset.TileSize);
						Quaternion spawnRot = cornerMask.Item4;
						Transform spawnParent = parent.Find("{" + x + ", " + y + "}");

						GameObject corner = GameObject.Instantiate(tileset.WallOuterCorner, spawnPos, spawnRot);
						corner.transform.parent = spawnParent;
						corner.transform.localRotation = spawnRot;
					}
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (ReferenceLevel == null)
				return;
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					if (ReferenceLevel[x, y] == -1)
						Gizmos.color = Color.red;
					else
						Gizmos.color = Color.green;
					Gizmos.DrawCube(new Vector3(x, 0, y), Vector3.one * 0.25f);
				}
			}
		}
	}
}