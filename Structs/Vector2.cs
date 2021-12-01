using System;
using System.Diagnostics;

namespace AdventOfCode2021 {
	public struct Vector2 : IComparable<Vector2> {
		public int x, y;

		public Vector2(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public static Vector2 operator +(Vector2 v0, Vector2 v1) => new Vector2(v0.x + v1.x, v0.y + v1.y);
		public static Vector2 operator -(Vector2 v0, Vector2 v1) => new Vector2(v0.x - v1.x, v0.y - v1.y);
		public static Vector2 operator *(Vector2 v0, int l) => new Vector2(v0.x * l, v0.y * l);
		public static Vector2 operator /(Vector2 v0, int l) => new Vector2(v0.x / l, v0.y / l);
		
		public static bool operator ==(Vector2 v0, Vector2 v1) => v0.x == v1.x && v0.y == v1.y;
		public static bool operator !=(Vector2 v0, Vector2 v1) => v0.x != v1.x || v0.y != v1.y;

		
		public override string ToString() => $"v({x};{y})";

		public bool Equals(Vector2 other) {
			return x == other.x && y == other.y;
		}

		public override bool Equals(object obj) {
			return obj is Vector2 other && Equals(other);
		}

		public override int GetHashCode() {
			unchecked {
				return (x * 397) ^ y;
			}
		}

		public static Vector2 Parse(string value) {
			var split = value.Split(',');
			var x = split[0].Trim();
			var y = split[1].Trim();
			return new Vector2(int.Parse(x), int.Parse(y));
		}

		public int CompareTo(Vector2 other) {
			var xComparison = x.CompareTo(other.x);
			if (xComparison != 0) {
				return xComparison;
			}

			return y.CompareTo(other.y);
		}
	}
	
}