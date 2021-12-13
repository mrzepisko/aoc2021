using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2021 {
	public class Day13 : Day {
		public override string Name => "--- Day 13: Transparent Origami ---";

		private readonly List<Vector2> dots;
		private readonly List<Vector2> folds;
		private int width, depth;
		private HashSet<Vector2> set;



		public Day13() {
			dots = new List<Vector2>();
			folds = new List<Vector2>();
		}

		public override void ParseInput(string input) {
			var L = 0;
			var lines = input.Split('\n');
			for (; L < lines.Length; L++) {
				var line = lines[L].Trim();

				if (string.IsNullOrEmpty(line)) break;

				var dot = Vector2.Parse(line);
				dots.Add(dot);
			}
			const string pattern = @"fold along (?>x=(?'x'\d+))?(?>y=(?'y'\d+))?";
			
			for (L++; L < lines.Length; L++) {
				var line = lines[L].Trim();
				var match = Regex.Match(line, pattern, RegexOptions.Multiline);
				var x = match.Groups[1].Value;
				var y = match.Groups[2].Value;

				int.TryParse(x, out var xx);
				int.TryParse(y, out var yy);
				
				var fold = new Vector2(xx, yy);
				folds.Add(fold);
			}
		}


		public override void PreTest() {
			width = int.MinValue;
			depth = int.MinValue;
			for (var i = 0; i < dots.Count; i++) {
				var dot = dots[i];
				width = dot.x > width ? dot.x : width;
				depth = dot.y > depth ? dot.y : depth;
			}
		}

		public override object PartOne() {
			var fold0 = folds[0];
			depth = fold0.y == 0 ? depth : fold0.y;
			width = fold0.x == 0 ? width : fold0.x;
			for (int i = 0; i < dots.Count; i++) {
				Fold(i, width, depth);
			}

			return dots.Distinct().Count(v => v.x >= 0 && v.y >= 0);
		}

		private void Fold(int dx, int width, int depth) {
			var dot = dots[dx];
			if (width > 0) {
				dot.x = width - Math.Abs(dot.x - width);
			}

			if (depth > 0) {
				dot.y = depth - Math.Abs(dot.y - depth);
			}

			dots[dx] = dot;
		}


		public override object PartTwo() {
			for (var f = 1; f < folds.Count; f++) {
				var fold = folds[f];
				depth = fold.y == 0 ? depth : fold.y;
				width = fold.x == 0 ? width : fold.x;
				for (int i = 0; i < dots.Count; i++) {
					Fold(i,fold.x, fold.y);
				}
			}

			char[] sb = Enumerable.Repeat('.', (1 + width) * depth).ToArray();
			for (var i = 0; i < dots.Count; i++) {
				var dot = dots[i];
				var idx = Id2Idx(dot);
				sb[idx] = '#';
			}

			for (int y = 0; y < depth; y++) {
				var idx = Id2Idx(width, y);
				sb[idx] = '\n';
			}

			return '\n' + new string(sb);
		}
		
		int Id2Idx(int x, int y) => x + (1 + width) * y;
		int Id2Idx(Vector2 id) => Id2Idx(id.x, id.y);
	}
}