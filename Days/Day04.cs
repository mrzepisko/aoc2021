using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2021 {
	public class Day04 : Day {
		public override string Name => "--- Day 4: Giant Squid ---";
		private int[] numbers;
		private List<Board> boards;

		public override void ParseInput(string input) {
			var lines = input.Split('\n');
			
			Parse(lines[0], out numbers, ',');
			boards = new List<Board>();
			for (int i = 2; i < lines.Length; i += 6) {
				var board = new Board(5);
				for (int b = 0; b < 5; b++) {
					Parse(lines[i + b], out var line, ' ');
					board.values[b] = line;
				}

				boards.Add(board);
			}
		}

		public override object PartOne() {
			for (int i = 0; i < numbers.Length; i++) {
				foreach (var board in boards) {
					var v = board.RegisterValue(numbers[i]);
					if (v >= 0) {
						return v;
					}
				}
			}

			return string.Empty;
		}

		public override object PartTwo() {
			for (var i = 0; i < boards.Count; i++) {
				var board = boards[i];
				board.Reset();
			}

			var toRemove = new List<Board>();
			for (int i = 0; i < numbers.Length; i++) {
				foreach (var board in boards) {
					var v = board.RegisterValue(numbers[i]);
					if (v >= 0) {
						if (boards.Count > 1) {
							toRemove.Add(board);
						} else {
							return v;
						}
					}
				}

				boards.RemoveAll(toRemove.Contains);
				toRemove.Clear();
			}
			
			

			return string.Empty;
		}

		
	}public struct Board {
		private readonly int size;
		public int[][] values;
		public bool[][] marked;
		public Board(int size) {
			this.size = size;
			values = new int[size][];
			marked = new bool[size][];
			for (int i = 0; i < size; i++) {
				marked[i] = new bool[size];
			}
		}

		public int RegisterValue(int value) {
			for (int y = 0; y < size; y++) {
				for (int x = 0; x < size; x++) {
					var val = values[x][y];
					if (val == value) {
						marked[x][y] = true;
					}
				}	
			}

			if (Validate()) {
				var sum = 0;
				for (int y = 0; y < size; y++) {
					for (int x = 0; x < size; x++) {
						if (marked[x][y]) continue;

						sum += values[x][y];
					}
				}

				sum *= value;
				return sum;
			} else {
				return -1;
			}

		}

		private bool Validate() {
			//vertical
			for (int x = 0; x < size; x++) {
				var match = true;
				for (int y = 0; y < size; y++) {
					match &= marked[x][y];
					if (!match) break;
				}

				if (match) return true;
			}
				
			//horizontal
			for (int y = 0; y < size; y++) {
				var match = true;
				for (int x = 0; x < size; x++) {
					match &= marked[x][y];
					if (!match) break;
				}

				if (match) return true;
			}

			return false;
		}

		public void Reset() {
			for (int x = 0; x < size; x++) {
				for (int y = 0; y < size; y++) {
					marked[x][y] = false;
				}
			}
		}
	}
}