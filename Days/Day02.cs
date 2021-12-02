using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode2021 {
	public class Day02 : Day {
		public override string Name => "--- Day 2: Dive! ---";

		private Vector2[] values;


		public override void ParseInput(string input) => Parse(input, out values, ParseEntry, valueSeparators: ' ');

		private Vector2 ParseEntry(params string[] args) {
			var value = int.Parse(args[1]);
			switch (args[0]) {
				case "up":
					return new Vector2(0, -value);
				case "down":
					return new Vector2(0, value);
				case "forward":
					return new Vector2(value, 0);
				default:
					Console.WriteLine($"Direction not recognized ({args[0]}");
					return default;
			}
		}

		public override object PartOne() {
			Vector2 pos = default;
			for (var i = 0; i < values.Length; i++) {
				var v = values[i];
				pos += v;
			}

			return $"{pos.x * pos.y}";
		}

		public override object PartTwo() {
			long posx = default;
			long posy = default;
			long aim = default;
			for (var i = 0; i < values.Length; i++) {
				var v = values[i];
				aim -= v.y;

				posx += v.x;
				posy -= v.x * aim;
			}

			return $"{posx * posy}";
		}
	}
}