using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2021;
using AStarSharp;

namespace AdventOfCode2021 {
	public class Day15 : Day {
		public override string Name => "--- Day 15: Chiton ---";

		private static readonly Vector2[] Neighbours = new[]
			{ new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) };
		private int[][] grid;
		public override void ParseInput(string input) => ParseDigitArray(input, out grid);
		
		public override object PartOne() {
			var astar = new Astar(grid);
			var start = new Vector2(0, 0);
			var end = new Vector2(grid.Length - 1, grid[grid.Length - 1].Length - 1);
			var path = astar.FindPath(start, end);
			return path.Sum(v => grid[v.Position.x][v.Position.y]);
		}

		public override object PartTwo() {
			var grid = Extend(this.grid, 5);
			var astar = new Astar(grid);
			var start = new Vector2(0, 0);
			var end = new Vector2(grid.Length - 1, grid[grid.Length - 1].Length - 1);
			var path = astar.FindPath(start, end);
			return path.Sum(v => grid[v.Position.x][v.Position.y]);
		}

		private int[][] Extend(int[][] grid, int size) {
			var outGrid = new int[grid.Length * size][];
			for (int x = 0; x < outGrid.Length; x++) {
				outGrid[x] = new int[outGrid.Length];
			}
			
			for (int h = 0; h < size; h++) {
				for (int v = 0; v < size; v++) {
					CopyGrid(grid, outGrid, h + v, grid.Length * h, grid.Length * v);
				}
			}
			return outGrid;
		}

		private void CopyGrid(int[][] source, int[][] target, int offset, int x0, int y0) {
			for (int x = 0; x < source.Length; x++) {
				for (int y = 0; y < source[x].Length; y++) {
					var nx = x + x0;
					var ny = y + y0;
					target[nx][ny] = 1 + (source[x][y] - 1 + offset) % 9;
				}
			}
		}
	}
}