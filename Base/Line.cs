using System;

namespace AdventOfCode2021 {
	public readonly struct Line {
		public readonly Vector2 p0, p1;
		public Vector2 dir {
			get { 
				var diff = p1 - p0;
				var nx = Math.Sign(diff.x);
				var ny = Math.Sign(diff.y);
				return new Vector2(nx, ny); 
			}
		}

		public Line(Vector2 p0, Vector2 p1) {
			this.p0 = p0;
			this.p1 = p1;
		}
		
		public Line(int x0, int y0, int x1, int y1) : this(new Vector2(x0, y0), new Vector2(x1, y1)) { }

		public override string ToString() => $"L{p0}:{p1}";

		// public static Vector2 operator +(Vector2 v0, Vector2 v1) => new Vector2(v0.x + v1.x, v0.y + v1.y);
		// public static Vector2 operator -(Vector2 v0, Vector2 v1) => new Vector2(v0.x - v1.x, v0.y - v1.y);
		// public static Vector2 operator *(Vector2 v0, int l) => new Vector2(v0.x * l, v0.y * l);
		// public static Vector2 operator /(Vector2 v0, int l) => new Vector2(v0.x / l, v0.y / l);
	}
	
}