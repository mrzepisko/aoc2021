using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021 {
	public class Day07 : Day {
		public override string Name => "--- Day 7: The Treachery of Whales ---";
		public override void ParseInput(string input) => Parse(input, out values, ',');

		private int[] values;
		private readonly List<long> sumCache = new(new []{0L, 1L});
		private readonly List<int> seriesCache = new List<int>();
		public override void PreTest() {
			Array.Sort(values);
		}

		public override object PartOne() {
			var target = values[values.Length / 2];
			var sum = CalculateFuel(target, MovingCostConst);
			return sum;
		}

		private long CalculateFuel(int target, Func<int, int, long> func) {
			var sum = values.Sum(v => func(v, target));
			return sum;
		}

		public override object PartTwo() {
			var min = values[0];
			var max = values[values.Length - 1];
			long minSum = long.MaxValue;
			for (int i = min; i <= max; i++) {
				var sum = values.Sum(v => MovingCostLinear(v, i));
				if (sum < minSum) {
					minSum = sum;
				}
			}

			return minSum;
		}

		long MovingCostConst(int from, int to) => Math.Abs(to - from);
		double MovingCostLinear(double from, double to) => from - to == 0 ? 0 : Sum(Math.Abs(to - from));
		long MovingCostLinear(int from, int to) => from - to == 0 ? 0 : (long)Math.Round(Sum(Math.Abs(to - from)));
		double Sum(double value) => value * (value + 1) / 2;
	}
}