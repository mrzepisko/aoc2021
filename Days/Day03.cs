using System;
using System.Collections.Generic;

namespace AdventOfCode2021 {
	public class Day03 : Day {
		public override string Name => "--- Day 3: Binary Diagnostic ---";
		private int[] values;
		
		
		public override void ParseInput(string input) => ParseBinary(input, out values);

		public override object PartOne() {
			var size = 12;
			var eps = 0;
			var gamma = 0;
			for (int shift = 0; shift < size; shift++) {
				var total = 0;
				for (var i = 0; i < values.Length; i++) {
					var val = values[i];
					total += (val >> shift) & 1;
				}

				var rate = 1 << shift;
				if (total > values.Length / 2) {
					eps |= rate;
				} else {
					gamma |= rate;
				}
			}

			return $"{eps * gamma}";
		}

		public override object PartTwo() {
			var size = 12;
			var ox = new List<int>(values);
			var co = new List<int>(values);
			
			BitTest(ox, 1);
			BitTest(co, 0);


			return $"{ox[0] * co[0]}";
		}

		private void BitTest(List<int> input, int testFor, int shift = 11) {
			while (input.Count > 1) {
				var total = 0;
				for (int i = input.Count - 1; i >= 0; i--) {
					var value = input[i];
					total += 1 - ((value >> shift) ^ testFor) & 1;
				}


				var bitRemove = total >= input.Count / 2 ? testFor ^ 1 : testFor;
				input.RemoveAll(v => (((v >> shift) ^ bitRemove) & 1) == 0);

				shift--;
			}

			return;
		}
	}
}