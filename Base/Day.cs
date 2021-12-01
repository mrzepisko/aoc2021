using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2021 {
	public abstract class Day {
		public abstract string Name { get; }
		protected virtual string Input => LoadFromFile();

		public abstract void ParseInput(string input);
		public abstract object PartOne();
		public abstract object PartTwo();
		
		public virtual void PreTest() { }

		public void Init() {
			ParseInput(Input);
		}
        
		protected string LoadFromFile() => File.ReadAllText($"../../Data/{GetType().Name}.txt");

		protected void Parse<T>(string input, out T[] values, Func<string[], T> lineParser, char entrySeparator = '\n', params char[] valueSeparators /*char separator = '\n', char valueSeparator = ' '*/) 
			=> values = input.Split(entrySeparator).Select(s => s.Split(valueSeparators)).Select(lineParser.Invoke).ToArray();
		protected void Parse(string input, out int[] intLines, char separator = '\n') {
			var list = new List<int>();
			var invalid = new List<string>();

			foreach (var line in input.Split(separator)) {
				if (string.IsNullOrEmpty(line)) continue;
				var valid = int.TryParse(line, out var value);
				if (valid) {
					list.Add(value);
				} else {
					invalid.Add(line);
				}
			}

			ListInvalidValues(invalid);
			intLines = list.ToArray();
		}

		protected void ParseBinary(string input, out int[] values) =>
			values = input.Split('\n').Select(s => s.Trim())
				.Select(l => Convert.ToInt32(l, 2))
				.ToArray();

		protected void ParseDigitArray(string input, out int[][] output) {
			var lines = input.Split('\n');
			output = new int[lines.Length][];
			for (int i = 0; i < lines.Length; i++) {
				var line = lines[i].Trim();

				output[i] = new int[line.Length];
				for (int c = 0; c < line.Length; c++) {
					output[i][c] = (int)line[c] - '0';
				}
			}
		}
        
		
		
		[Conditional("DEBUG")]
		private static void ListInvalidValues(List<string> invalid) {
			if (invalid.Count > 0) {
				var sb = new StringBuilder("Could not parse all lines, invalid entries:");
				foreach (var line in invalid) {
					sb.Append("\n\t(").Append(line.Trim()).Append(")");
				}

				Console.WriteLine(sb);
			}
		}
	}
	
	
	public abstract class Day<T> : Day {
		protected void RegexParse(string input, string pattern, List<T> output) 
			=> output.AddRange(Regex.Matches(input, pattern, RegexOptions.Multiline)
				.Cast<Match>()
				.Select(m => m.Groups)
				.Select(ParseMatch));

		protected abstract T ParseMatch(GroupCollection group);

	}
}