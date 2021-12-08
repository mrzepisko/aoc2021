using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021 {
	public class Day08 : Day {
		public override string Name => "--- Day 8: Seven Segment Search ---";
		private static readonly List<int> segmentCountForUniqueDigits = new() { 2, 3, 4, 7, };
		// private static readonly List<int> uniqueDigits = new() { 1, 4, 7, 8 };
		private static readonly char[] segments = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', };
		private SegmentConfig[] entries;
		public override void ParseInput(string input) {
			Parse(input, out entries, ParseEntry, valueSeparators: '|');
		}

		private SegmentConfig ParseEntry(string[] s) {
			Parse(s[0].Trim(), out var uq, ss => ss[0].Sort(), ' ');
			Parse(s[1].Trim(), out var res, ss => ss[0].Sort(), ' ');

			return new SegmentConfig(uq, res);
		}

		public override object PartOne() => entries.Sum(e => e.output.Count(
				s => segmentCountForUniqueDigits.Contains(s.Length)));

		public override object PartTwo() {
			var sum = 0;
			for (var iS = 0; iS < entries.Length; iS++) {
				// uniqueDigits.Clear();
				var segment = entries[iS];
				for (var iE = 0; iE < segment.entries.Length; iE++) {
					var entry = segment.entries[iE];
					if (entry.Length == 2) {
						segment.Save(1, entry);
					} else if (entry.Length == 3) {
						segment.Save(7, entry);
					} else if (entry.Length == 7) {
						segment.Save(8, entry);
					} else if (entry.Length == 4) {
						segment.Save(4, entry);
					}
				}


				for (var iE = 0; iE < segment.entries.Length; iE++) {
					var entry = segment.entries[iE];
					if (entry.Length == 6) {
						segment.Deduce069(entry);	
					}
				}
				


				for (var iE = 0; iE < segment.entries.Length; iE++) {
					var entry = segment.entries[iE];
					if (entry.Length == 5) {
						segment.Deduce235(entry);	
					}
				}

				segment.Invert();

				var length = segment.output.Length;
				var number = 0;
				for (int i = 0; i < length; i++) {
					var e = (int)Math.Pow(10, length - i - 1);
					number += e * segment.mapping[segment.output[i]];
				}

				sum += number;

			}

			return sum;
		}

		public class SegmentConfig {
			public string[] entries;
			public string[] output;
			public readonly Dictionary<string, int> mapping;
			public readonly Dictionary<int, List<char>> confirmedSegments;

			public SegmentConfig(string[] entries, string[] output) {
				this.entries = entries;
				this.output = output;

				confirmedSegments = new Dictionary<int, List<char>>();
				mapping = new Dictionary<string, int>();
			}

			
			
			
			public void Save(int value, string segments) {
				// if (uniqueDigits.Contains(value)) return;
				// uniqueDigits.Add(value);
				if (value < 0) throw new ArgumentOutOfRangeException();
				confirmedSegments.Add(value, new List<char>(segments));
			}

			public void Deduce069(string segments) {
					var and1 = confirmedSegments[1].Intersect(segments).Count();
					var and4 = confirmedSegments[4].Intersect(segments).Count();

					var is6 = and1 == 1 && and4 == 3;
					var is9 = and1 == 2 && and4 == 4;
					var is0 = and1 == 2 && and4 == 3;

					var key = -1;
					if (is6) {
						key = 6;
					} else if (is9) {
						key = 9;
					} else if (is0) {
						key = 0;
					}
					Save(key, segments);
			}

			public void Deduce235(string segments) {
					var and7 = confirmedSegments[7].Intersect(segments).Count();
					var and6 = confirmedSegments[6].Intersect(segments).Count();

					var is2 = and7 == 2 && and6 == 4;
					var is3 = and7 == 3 && and6 == 4;
					var is5 = and7 == 2 && and6 == 5;

					var key = -1;
					if (is2) {
						key = 2;
					} else if (is3) {
						key = 3;
					} else if (is5) {
						key = 5;
					}
					Save(key, segments);
			}

			public void Invert() {
				foreach (var kv in confirmedSegments) {
					mapping.Add(new string(kv.Value.ToArray()), kv.Key);
				}
			}
		}
	}
}