using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AdventOfCode2021 {
	public class Day14 : Day {
		public override string Name => "--- Day 14: Extended Polymerization ---";


		private string polymer;
		private readonly Dictionary<int, char> rules;
		private readonly Dictionary<int, long> tempInsertions;
		private readonly Dictionary<int, long> tempRemoves;
		private readonly Dictionary<int, long> pairCount;
		private readonly Dictionary<char, long> count;


		public Day14() {
			rules = new Dictionary<int, char>();
			count = new Dictionary<char, long>();
			tempInsertions = new Dictionary<int, long>();
			tempRemoves = new Dictionary<int, long>();
			pairCount = new Dictionary<int, long>();
		}

		public override void ParseInput(string input) {
			var lines = input.Split('\n');

			polymer = lines[0].Trim();
		
			for (int i = 2; i < lines.Length; i++) {
				var line = lines[i];

				var c1 = line[0];
				var c2 = line[1];
				var insertion = line[6];

				var hash = Hash(c1, c2);
				rules.Add(hash, insertion);
			}
		}

		public override object PartOne() {
			InitialisePolymer();
			for (int i = 0; i < 10; i++) {
				Step();
			}

			var orderedCount = count.Values.OrderBy(d => d).ToList();
			return orderedCount[orderedCount.Count - 1] - orderedCount[0];
		}

		private void InitialisePolymer() {
			pairCount.Clear();
			count.Clear();
			for (int i = 1; i < polymer.Length; i++) {
				var c1 = polymer[i - 1];
				var c2 = polymer[i - 0];
				var hash = Hash(c1, c2);

				AddCount(pairCount, hash, 1);
			}

			for (int i = 0; i < polymer.Length; i++) {
				AddCount(count, polymer[i], 1);
			}
		}

		private static void AddCount<T>(Dictionary<T, long> target, T key, long count) {
			if (!target.ContainsKey(key)) {
				target.Add(key, count);
			} else {
				target[key] += count;
			}
		}
		

		private void Step() {
			tempRemoves.Clear();
			tempInsertions.Clear();
			foreach (var kv in pairCount) {
				if (rules.TryGetValue(kv.Key, out var toInsert)) {
					Pair(kv.Key, out var c1, out var c2);

					var hash1 = Hash(c1, toInsert);
					var hash2 = Hash(toInsert, c2);
					
					AddCount(count, toInsert, kv.Value);

					AddCount(tempInsertions, hash1, kv.Value);
					AddCount(tempInsertions, hash2, kv.Value);
					
					AddCount(tempInsertions, kv.Key, -kv.Value);

				}
			}
			
			foreach (var kv in tempInsertions) {
				AddCount(pairCount, kv.Key, kv.Value);
			}
		}

		public override object PartTwo() {
			InitialisePolymer();
			for (int i = 0; i < 40; i++) {
				Step();
			}

			var orderedCount = count.Values.OrderBy(d => d).ToList();
			return orderedCount[orderedCount.Count - 1] - orderedCount[0];
		}


		int Hash(char c1, char c2) => (c1 << 8) + c2;

		void Pair(int hash, out char c1, out char c2) {
			c2 = (char)(hash & 255);
			c1 = (char)(hash >> 8);
		}

	}
}