using System.Collections.Generic;

namespace AdventOfCode2021 {
	public class Day09 : Day{
		public override string Name => "--- Day 9: Smoke Basin ---";

		private static readonly Vector2 North = new Vector2(0, 1);
		private static readonly Vector2 South = new Vector2(0, -1);
		private static readonly Vector2 West = new Vector2(-1, 0);
		private static readonly Vector2 East = new Vector2(1, 0);
		
		private int widthCount, depthCount;
		private int[][] grid;
		private readonly List<Vector2> lowPoints;
		private readonly List<HashSet<Vector2>> basins;

		private int this[Vector2 v] => grid[v.y][v.x];

		public Day09() {
			lowPoints = new List<Vector2>();
			basins = new List<HashSet<Vector2>>();
		}

		public override void ParseInput(string input) {
			var lines = input.Split('\n');
			grid = new int[lines.Length][];
			for (var z = 0; z < grid.Length; z++) {
				var line = lines[z].Trim();
				grid[z] = new int[line.Length];
				for (int x = 0; x < grid[z].Length; x++) {
					grid[z][x] = line[x] - '0';
				}
			}

			depthCount = grid.Length;
			widthCount = grid[0].Length;
		}

		public override object PartOne() {
			int riskPoint = 0;
			for (int z = 0; z < depthCount; z++) {
				for (int x = 0; x < widthCount; x++) {
					var lowest = true;
					lowest &= NeighbourIsGreater(x, z, -1, 0);
					lowest &= NeighbourIsGreater(x, z, +1, 0);
					lowest &= NeighbourIsGreater(x, z, 0, -1);
					lowest &= NeighbourIsGreater(x, z, 0, +1);

					if (lowest) {
						lowPoints.Add(new Vector2(x, z));
						riskPoint += 1 + grid[z][x];
					}
				}
			}

			return riskPoint;
		}

		private bool NeighbourIsGreater(int x, int z, int ox, int oz) {
			var nx = x + ox;
			var nz = z + oz;
			if (OutOfBounds(nx, nz)) {
				return true;
			}

			return grid[z][x] < grid[nz][nx];
		}

		private bool OutOfBounds(int nx, int nz) => nx < 0 || nx >= widthCount || nz < 0 || nz >= depthCount;
		private bool OutOfBounds(Vector2 v) => OutOfBounds(v.x, v.y);

		public override object PartTwo() {
			foreach (var lowPoint in lowPoints) {
				var set = new HashSet<Vector2>();
				Flood(lowPoint, set);
				basins.Add(set);
			}
			
			basins.Sort(BasinsSort);
			return basins[0].Count * basins[1].Count * basins[2].Count;
		}

		private int BasinsSort(HashSet<Vector2> x, HashSet<Vector2> y) => y.Count.CompareTo(x.Count);

		private void Flood(Vector2 seed, HashSet<Vector2> list) {
			if (list.Contains(seed)) return;
			if (OutOfBounds(seed)) return;
			if (this[seed] == 9) return;
			list.Add(seed);
			
			Flood(seed + North, list);
			Flood(seed + South, list);
			Flood(seed + West, list);
			Flood(seed + East, list);
		}
	}
}