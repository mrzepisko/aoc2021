using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021.Data {
	public class Day11 : Day {
		public override string Name => "--- Day 11: Dumbo Octopus ---";

		private int[][] octopussies;
		private readonly Stack<Vector2>  stack = new();
		public override void ParseInput(string input) {
			ParseDigitArray(input, out octopussies);
		}
		
		public override object PartOne() {
			var flashes = 0;
			Console.Out.WriteLine("Starting configuration:");
			Print();
			for (int i = 0; i < 100; i++) {
				var f = Step();
				flashes += f;
				Console.Out.WriteLine("After step {0} ({1} flashes, {2} total)", i + 1, f, flashes);
				Print();
			}
			
			return flashes;
		}

		private int Step() {
			stack.Clear();
			for (int x = 0; x < octopussies.Length; x++) {
				for (int y = 0; y < octopussies[x].Length; y++) {
					octopussies[x][y]++;
					if (octopussies[x][y] == 10) {
						stack.Push(new Vector2(x, y));
					}
				}
			}

			while (stack.Count > 0) {
				var flash = stack.Pop();
				octopussies[flash.x][flash.y]++;
				for (int x = Math.Max(0, flash.x - 1); x <= flash.x + 1 && x < octopussies.Length; x++) {
					for (int y = Math.Max(0, flash.y - 1); y <= flash.y + 1 && y < octopussies[x].Length; y++) {
						octopussies[x][y]++;
						if (octopussies[x][y] == 10) {
							stack.Push(new Vector2(x, y));
						}
					}
				}
			}


			int flashes = 0;
			for (int x = 0; x < octopussies.Length; x++) {
				for (int y = 0; y < octopussies[x].Length; y++) {
					if (octopussies[x][y] > 9) {
						octopussies[x][y] = 0;
						flashes++;
					}
				}
			}

			return flashes;
		}


		private StringBuilder sb = new StringBuilder();
		private void Print() {
			sb.Length = 0;
			var depth = octopussies[0].Length;
			var width = octopussies.Length;
				for (int x = 0; x < width; x++) {
					for (int y = 0; y < depth; y++) {
					sb.Append(octopussies[x][y])
						.Append(' ');
				}

				sb.Append('\n');
			}

			Console.Out.WriteLine(sb.ToString());
		}
		
		public override object PartTwo() {
			int step = 100;
			var f = 0;
			var flashes = 1601;
			var width = octopussies.Length;
			var depth = octopussies[0].Length;
			var count = width * depth;
			while (f < count) {
				f = Step();
				flashes += f;
				step++;
				Console.Out.WriteLine("After step {0} ({1} flashes, {2} total)", step, f, flashes);
				Print();
			}

			return step;
		}
	}
}