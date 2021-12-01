using System;

namespace AdventOfCode2021 {
	public readonly struct ULongULong : IComparable<ULongULong> {
		public readonly ulong value;
		public readonly ulong overflows;


		public ULongULong(ulong overflows, ulong value) {
			this.value = value;
			this.overflows = overflows;
		}

		public static ULongULong operator +(ULongULong self, int add) => self + (uint)add;
		public static ULongULong operator +(ULongULong self, uint add) {
			ULongULong newValue = default;

			var overflow = self.overflows;
			ulong value;
			try {
				value = checked(self.value + add);
			} catch (OverflowException) {
				value = self.value + add;
				overflow++;
			}

			return new ULongULong(overflow, value);
		}


		public static ULongULong operator *(ULongULong self, uint mul) {
			ULongULong newValue = default;

			var overflows = self.overflows * mul;
			ulong value;
			try {
				value = checked(self.value * mul);
			} catch (OverflowException) {
				value = self.value * mul;
				overflows++;
			}

			return new ULongULong(overflows, value);
		}

		public int CompareTo(ULongULong other) {
			return overflows == other.overflows
				? value.CompareTo(other.value)
				: overflows.CompareTo(other.overflows);
		}

		public static implicit operator ULongULong(uint value) => new(0, value);
		public static implicit operator ULongULong(int value) => new(0, value < 0 ? 0 : (uint)value);
		public static implicit operator ULongULong(long value) => new(0, (ulong)value);
		public static implicit operator ULongULong(ulong value) => new(0, value);


		public override string ToString() {
			return $"2^64*{overflows}+{value}";
		}
	}
}