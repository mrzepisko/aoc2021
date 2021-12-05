using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021 {
	public class Day05 : Day<Line> {
		public override string Name => "--- Day 5: Hydrothermal Venture ---";

		private readonly List<Line> lines;
		private int[,] map;
		private int xSize, ySize;
		private Vector2 offset;
		
		public Day05() {
			lines = new List<Line>();
		}

		private const string Pattern = @"(\d+),(\d+).*?(\d+),(\d+)";


		protected override Line ParseMatch(GroupCollection groups) {
			var r1 = groups[1].Value;
			var r2 = groups[2].Value;
			var r3 = groups[3].Value;
			var r4 = groups[4].Value;

			var v1 = int.Parse(r1);
			var v2 = int.Parse(r2);
			var v3 = int.Parse(r3);
			var v4 = int.Parse(r4);

			return new Line(v1, v2, v3, v4);
		}

		public override void ParseInput(string input) => RegexParse(input, Pattern, lines);


		public override void PreTest() {
			int xMin = int.MaxValue;
			int xMax = int.MinValue;
			
			int yMin = int.MaxValue;
			int yMax = int.MinValue;

			for (int i = 0; i < lines.Count; i++) {
				var line = lines[i];
				xMin = Math.Min(xMin, line.p0.x);
				xMin = Math.Min(xMin, line.p1.x);
				
				xMax = Math.Max(xMax, line.p0.x);
				xMax = Math.Max(xMax, line.p1.x);
				
				yMin = Math.Min(yMin, line.p0.y);
				yMin = Math.Min(yMin, line.p1.y);
				
				yMax = Math.Max(yMax, line.p0.y);
				yMax = Math.Max(yMax, line.p1.y);

			}

			xSize = xMax - xMin + 1;
			ySize = yMax - yMin + 1;
			offset = new Vector2(xMin, yMin);
			map = new int[1000,1000];
		}

		public override object PartOne() {
			Stopwatch s = new Stopwatch();
			s.Start();
			
			foreach (var line in lines) {
				var dir = line.dir;

				if (Math.Abs(dir.x) + Math.Abs(dir.y) >= 2) { //ignore diagonals
					Console.Out.WriteLine("Line {0} is diagonal ignored (dir {1}", line, line.dir);
					continue;
				}

				if (line.p0.x == line.p1.x && line.p0.y != line.p1.y) {
					FillVertical(line.p0, line.p1);
				} else if (line.p0.x != line.p1.x && line.p0.y == line.p1.y) {
					FillHorizontal(line.p0, line.p1);
				}
				
				
				
			}
			s.Stop();

			return $"\n\tResult: {map.Cast<int>().Select(v => v).Count(v => v > 1)}\n\tTime: {s.ElapsedMilliseconds}";
		}

		private void FillHorizontal(Vector2 from, Vector2 to) {
			var min = from.x < to.x ? from.x : to.x;
			var max = from.x > to.x ? from.x : to.x;

			if (from.y != to.y) throw new ArgumentOutOfRangeException("Tried to fill horizontal line with different y");
			
			for (int x = min; x <= max; x++) {
				map[x,from.y]++;
			}
		}

		public override object PartTwo() {
			Stopwatch s = new Stopwatch();
			foreach (var line in lines) {
				if (line.p0.x != line.p1.x && line.p0.y != line.p1.y) {
					FillDiagonal(line);
				}
			}
			s.Stop();
			return $"\n\tResult: {map.Cast<int>().Select(v => v).Count(v => v > 1)}\n\tTime: {s.ElapsedMilliseconds}";
		}

		private void FillDiagonal(Line line) {


			var dir = line.dir;
			var c = line.p0;
			var t = line.p1 + dir;
			while (c != t) {
				map[c.x, c.y]++;
				c += dir;
			}
		}

		private void FillVertical(Vector2 from, Vector2 to) {
			var min = from.y < to.y ? from.y : to.y;
			var max = from.y > to.y ? from.y : to.y;

			if (from.x != to.x) throw new ArgumentOutOfRangeException("Tried to fill vertical line with different x");
			
			for (int y = min; y <= max; y++) {
				var value = map[from.x,y];
				map[from.x, y] = value + 1;
			}
		}

		private bool MoveTowards(int x, int y, int tX, int tY, out int newX, out int newY) {
			var dx = Math.Sign(tX - x);
			newX = x + dx;
			var dy = Math.Sign(tY - y);
			newY = y + dy;
			return tX + dx != newX && tY + dy != newY;
		}
	}
}