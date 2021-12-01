using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode2021 {
	public class Day01 : Day {
		public override string Name => "--- Day 1: Sonar Sweep ---";
		
		private int[] values;


		public override void ParseInput(string input) => Parse(input, out values);

		public override object PartOne() {
			int increments = 0;
			for (int i = 1; i < values.Length; i++) {
				var val0 = values[i - 1];
				var val1 = values[i - 0];

				if (val1 > val0) {
					increments++;
				}
			}

			return $"Total increments: {increments}";
		}

		public override object PartTwo() {
			int increments = 0;
			for (int i = 3; i < values.Length; i++) {
				var v00 = values[i - 3];
				var v01 = values[i - 2];
				var v02 = values[i - 1];
				var v03 = values[i - 0];

				var sum0 = v00 + v01 + v02;
				var sum1 = v01 + v02 + v03;

				if (sum1 > sum0) {
					increments++;
				}
			}
			return $"Total increments: {increments}";
		}
	}
}