using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode2021 {
	public class Day06 : Day {

		private byte[] values;
		private long[] fishes;

		public override string Name => "--- Day 6: Lanternfish ---";
		public override void ParseInput(string input) => Parse(input, out values, 
			s => byte.Parse(s[0]), ',');

		

		public override void PreTest() {
			fishes = new long[10];

			for (int i = 0; i < values.Length; i++) {
				var fishCycle = values[i];
				fishes[fishCycle]++;
			}
		}

		public override object PartOne() {

			long fishCount = fishes.Sum(v => v);
			for (int day = 0; day < 256; day++) {
				var cycle0 = GetCycleIdx(day, 0);
				var cycle6 = GetCycleIdx(day, 6);
				fishes[cycle6] += fishes[7];
				fishes[7] = fishes[8];
				fishes[8] = fishes[9];
				fishes[9] = 0;

				fishCount += fishes[9] += fishes[cycle0];

				Console.Out.WriteLine("Day {9} ({10})\t\tc0:{0}\tc1:{1}\tc2:{2}\tc3:{3}\tc4:{4}\tc5:{5}\tc6:{6}\t||\tc7:{7}\tc8:{8},",
					fishes[GetCycleIdx(day, 0)], 
					fishes[GetCycleIdx(day, 1)], 
					fishes[GetCycleIdx(day, 2)],
					fishes[GetCycleIdx(day, 3)], 
					fishes[GetCycleIdx(day, 4)], 
					fishes[GetCycleIdx(day, 5)],
					fishes[GetCycleIdx(day, 6)], 
					fishes[7], fishes[8], day, fishCount);
			}

			return fishCount;
		}

		private static int GetCycleIdx(int day, int cycle) {
			return (day + cycle) % 7;
		}

		public override object PartTwo() {
			return "";
		}

		struct LanternfishGroup {
			public int count;
		}
	}
}