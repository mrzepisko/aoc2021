using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021 {
	public class Day10 : Day {
		public override string Name => "--- Day 10: Syntax Scoring ---";

		private string[] lines;
		
		public override void ParseInput(string input) => lines = input.Split('\n').Select(s => s.Trim()).ToArray();

		private StringBuilder sb = new StringBuilder();
		private Dictionary<char, uint> syntaxPoints = new() {{')', 3}, {']', 57}, {'}', 1197}, {'>', 25137} };
		private Dictionary<char, uint> autocompletePoints = new() {{')', 1}, {']', 2}, {'}', 3}, {'>', 4} };

		private List<ULongULong> autocompleteScore = new();
		private List<string> notCorrupted = new();

		private Dictionary<char, char> openings = new() {
			{ ')', '(' },
			{ '}', '{' },
			{ '>', '<' },
			{ ']', '[' },
		};
		private Dictionary<char, char> closings = new() {
			{ '(', ')' },
			{ '{', '}' },
			{ '<', '>' },
			{ '[', ']' },
		};

		public override object PartOne() {
			Stack<char> stack = new Stack<char>();
			var score = 0u;
			foreach (var line in lines) {
				 stack.Clear();
				 bool valid = true;
				 for (int i = 0; i < line.Length; i++) {
					 var c = line[i];

					 if (closings.ContainsValue(c)) {
						 if (stack.Peek() == openings[c]) {
							 //valid
							 stack.Pop();
						 } else {
							 score += syntaxPoints[c];
							 valid = false;
							 break;
						 }
					 } else {
						 stack.Push(c);
					 }
				 }

				 if (valid && stack.Count > 0) {
					 var completed = Autocomplete(stack, line);
					 notCorrupted.Add(line);
				 }
			}

			return score;
		}

		private string Autocomplete(Stack<char> stack, string line) {
			if (stack.Count == 0) return line;

			ULongULong score = 0u;
			sb.Length = 0;
			sb.EnsureCapacity(line.Length + stack.Count);
			sb.Append(line);
			while (stack.Count > 0) {
				var toAppend = closings[stack.Pop()]; 
				sb.Append(toAppend);
				score = score * 5 + autocompletePoints[toAppend];
			}

			autocompleteScore.Add(score);
			return sb.ToString();
		}

		public override object PartTwo() {
			autocompleteScore.Sort();
			return autocompleteScore[autocompleteScore.Count / 2];
		}


	}
}