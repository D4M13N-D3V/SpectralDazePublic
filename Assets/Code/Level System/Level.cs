using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpectralDaze.Levels
{
	[CreateAssetMenu(fileName = "Level", menuName = "Spectral Daze/Level Editor/Level")]
	public class Level : ScriptableObject
	{
		public int[] MapData = new int[16]
		{
			0,0,0,0,
			0,-1,-1,0,
			0,0,-1,0,
			-1,0,0,0
		};

		public int MapWidth = 4;
		public int MapHeight = 4;

		public int GetTile(int x, int y)
		{
			if (x >= MapWidth || x < 0 || y >= MapHeight || y < 0)
				return -1;
			return MapData[x + y * 4];
		}

		/// <summary>
		/// Gets the neighbors surrounding a specific tile.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int[] GetNeighbors(int x, int y)
		{
			int[] neighborsBuffer = new int[8];
			int i = 0;
			for (int ix = -1; ix < 2; ix++)
			{
				for (int iy = -1; iy < 2; iy++)
				{
					if (iy == 0 && ix == 0)
						continue;
					neighborsBuffer[i] = GetTile(x + ix, y + iy);
					i++;
				}
			}

			return neighborsBuffer;
		}

		/// <summary>
		/// Gets the neighbors surrounding a specific tile without creating a new temporary array.
		/// Instead, a predetermined array is used, thus saving memory allocations.
		/// </summary>
		/// <remarks>
		///	Layout:
		///		2 4 7
		///		1 - 6
		///		0 3 5
		/// </remarks>
		/// <param name="x">X position of test tile</param>
		/// <param name="y">Y position of test tile</param>
		/// <param name="buffer">A buffer to store the neighbor tile IDs in, must have a length of atleast 8.</param>
		public void GetNeighborsNonAlloc(int x, int y, int[] buffer)
		{
			if (buffer.Length < 8)
				throw new ArgumentException("Buffer size was not large enough to hold all neighbor values, must be atleast 8!", nameof(buffer));

			int i = 0;
			for (int ix = -1; ix < 2; ix++)
			{
				for (int iy = -1; iy < 2; iy++)
				{
					if (iy == 0 && ix == 0)
						continue;
					buffer[i] = GetTile(x + ix, y + iy);
					i++;
				}
			}
		}

		public int this[int x, int y]
		{
			get { return GetTile(x, y); }
		}
	}
}