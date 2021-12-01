using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2021 {
	public static class DataUtils {
		public static T[] Sort<T>(this T[] self) {
			Array.Sort(self);
			return self;
		}
		
		public static string Sort(this string self) => new (self.ToCharArray().Sort());
	}
}