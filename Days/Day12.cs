using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode2021 {
	public class Day12 : Day {
		public override string Name => "--- Day 12: Passage Pathing ---";

		private Dictionary<string, List<string>> graph = new();

		private volatile Stack<NavData> asyncWorker = new();
		private volatile int activeThreads = 0;

		void ThreadingWorker() {
			// asyncWorker.Clear();
			activeThreads = 0;
			int ThreadLimit = 2048;
			while (asyncWorker.Count > 0) {

				if (asyncWorker.Count == 0 || activeThreads >= ThreadLimit) {
					continue;
				}

				List<Thread> toStart = new List<Thread>();
				for (int i = activeThreads; i < ThreadLimit && asyncWorker.Count > 0; i++) {
					var node = asyncWorker.Pop();
					if (node.node == null) continue;
					Thread t = new Thread(() =>
						node.action(node.node, string.Empty, node.path, node.paths, node.allowedVisits));
					toStart.Add(t);
				}

				foreach (var thread in toStart) {
					activeThreads++;
					thread.Start();
				}
				
				Console.Out.WriteLine("Waiting: {1}, activeThreads = {0}, eligible paths: {2}", toStart.Count, asyncWorker.Count, paths.Count);
				
				Thread.Sleep(1);
			}
		}
		
		public override void ParseInput(string input) {
			graph.Clear();
			var lines = input.Split('\n');
			for (var i = 0; i < lines.Length; i++) {
				var line = lines[i].Trim();
				var nodes = line.Split('-');
				var n0 = nodes[0];
				var n1 = nodes[1];
				GetConnections(n0, out var list);
				list.Add(n1);
				GetConnections(n1, out list);
				list.Add(n0);
			}
		}

		private void GetConnections(string n0, out List<string> list) {
			if (!graph.TryGetValue(n0, out list)) {
				graph[n0] = list = new List<string>();
			}
		}

		public override void PreTest() {
			foreach (var list in graph.Values) {
				list.Sort();
			}
		}

		public override object PartOne() {
			List<string> paths = new List<string>();
			Navigate("start", string.Empty, string.Empty, paths);
			foreach (var path in paths) {
				Console.Out.WriteLine(path);
			}

			return $"total: {paths.Count}";
		}


		private delegate void NavigateAction(string node, string cameFrom, string path, List<string> paths,
			int allowedVisits = 1);
		
		struct NavData {
			public string node;
			public string path;
			public List<string> paths;
			public int allowedVisits;
			public NavigateAction action;

		}
		
		void Navigate(string node, string cameFrom, string path, List<string> paths, int allowedVisits = 1) {
			path += "," + node;
			if (node == "end") {
				lock (paths) {
					paths.Add(path);
				}

				return;
			}

			var connections = graph[node];
			// if (connections.Count == 1 && IsLowercase(cameFrom)) return;
			foreach (var conn in connections) {
				if (conn == "start") continue;
				if (!IsValid(path, allowedVisits, conn)) { //small cave visited more than n times
					continue;
				}

				if (IsLoop(path, conn, allowedVisits)) {
					continue; //invalid loop
				}

				Navigate(conn, node, path, paths, allowedVisits);
			}
		}


		void NavigateAsyncStep(NavData data) {
			NavigateAsync(data.node, string.Empty, data.path, data.paths, data.allowedVisits);
		}
		
		void NavigateAsync(string node, string cameFrom, string path, List<string> paths, int allowedVisits = 1) {
			path += "," + node;
			if (node == "end") {
				paths.Add(path);
				activeThreads--;
				return;
			}

			var connections = graph[node];
			// if (connections.Count == 1 && IsLowercase(cameFrom)) return;
			foreach (var conn in connections) {
				if (conn == "start") continue;
				if (!IsValid(path, allowedVisits, conn)) { //small cave visited more than n times
					continue;
				}

				if (IsLoop(path, conn, allowedVisits)) {
					continue; //invalid loop
				}

				var data = new NavData() {
					node = conn,
					path = path,
					paths = paths,
					allowedVisits = allowedVisits,
					action = NavigateAsync,
				};
				
				asyncWorker.Push(data);
			}

			activeThreads--;
		}

		
		
		private bool IsValid(string path, int allowedVisits, string conn) {
			if (IsLeaf(conn)) return true;
			if (!IsLowercase(conn)) return true;
			var nodes = path.Split(',');
			var matches = nodes.Where(c => c == conn);
			var count = matches.Count();
			var valid = count + 1 <= allowedVisits;
			return valid;
		}

		private static bool IsLowercase(string c) => c[0] >= 'a';
		private bool IsLeaf(string s) => s.Length == 0 || s is "start" or "end";
		private bool NotALeaf(string s) => !IsLeaf(s);

		private bool IsLoop(string path, string nextNode, int allowedVisits) {
			var nodes = path.Split(',');
			var last = nodes.Length - 1;
			var lookFor = nodes[last];

			var first = Array.IndexOf(nodes, lookFor);
			int visits = 0;
			for (int i = first; i < last; i++) {
				if (nodes[i] == lookFor) {
					if (nodes[i + 1] == nextNode) {
						if (++visits > allowedVisits) {
							return true;
						}
					}
				}
			}

			return false;
		}

		private volatile List<string> paths;
		
		public override object PartTwo() {
			paths = new List<string>();

			var validPaths = new List<string>();
			
			var navThread = new Thread(() => Navigate("start", string.Empty, string.Empty, paths, 2));
			var filterThread = new Thread(() => FilterPaths(validPaths, 1000, false));
			navThread.Start();
			filterThread.Start();

			while (navThread.IsAlive) {
				Console.Out.WriteLine("Eligible paths = {0}", paths.Count);
				Thread.Sleep(500);
			}

			
			filterThread.Join(100);
			FilterPaths(validPaths, 0, true);
			
			return $"total: {validPaths.Count}";
		}

		private List<string> copy = new List<string>();

		private void FilterPaths(List<string> validPaths, int chunkSize, bool single) {
			do {
				copy.Clear();

				while (!single && paths.Count < chunkSize) {
					Thread.Sleep(100);
				}

				lock (paths) {
					copy.AddRange(paths);
					paths.Clear();
				}

				foreach (var path in copy) {
					var smallCaves = path
						.Split(',')
						.Where(NotALeaf)
						.Where(IsLowercase)
						.ToList();
					var d = smallCaves.GroupBy(s => s)
						.Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))
						.ToList();

					var valid = true;
					var visitedTwice = 0;
					foreach (var visits in d) {
						if (visits.Value > 1) {
							if (visits.Value == 2) visitedTwice++;
							if (visitedTwice > 1 || visits.Value > 2) valid = false;
						}
					}

					if (valid) {
						validPaths.Add(path);
					}
				}
			} while (!single);
		}
	}
}