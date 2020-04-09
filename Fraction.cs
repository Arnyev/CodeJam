using System.Numerics;

namespace CodeJam
{
	public class Fraction
	{
		public BigInteger p;
		public BigInteger q;
		public Fraction(long p)
		{
			this.p = p;
			q = 1;
		}

		public Fraction(BigInteger p, BigInteger q)
		{
			var gcd = BigInteger.GreatestCommonDivisor(p, q);
			this.p = p / gcd;
			this.q = q / gcd;
		}

		public Fraction Invert()
		{
			return new Fraction(q, p);
		}

		public static implicit operator double(Fraction f)
		{
			return (double)f.p / (double)f.q;
		}

		public Fraction LimitDenominator(BigInteger maxDenominator)
		{
			if (q <= maxDenominator)
				return new Fraction(p, q);

			BigInteger p0 = 0;
			BigInteger q0 = 1;
			BigInteger p1 = 1;
			BigInteger q1 = 0;

			BigInteger n = p;
			BigInteger d = q;

			while (true)
			{
				var a = n / d;
				var q2 = q0 + a * q1;
				if (q2 > maxDenominator)
					break;

				var tmpP0 = p0;
				p0 = p1;
				q0 = q1;
				p1 = tmpP0 + a * p1;
				q1 = q2;
				var tmpn = n;
				n = d;
				d = tmpn - a * d;
			}

			var k = (maxDenominator - q0) / q1;

			var bound1 = new Fraction(p0 + k * p1, q0 + k * q1);
			var bound2 = new Fraction(p1, q1);

			var d1 = bound2 - this;
			var d2 = bound1 - this;
			d1.p = BigInteger.Abs(d1.p);
			d1.q = BigInteger.Abs(d1.q);
			d2.p = BigInteger.Abs(d2.p);
			d1.q = BigInteger.Abs(d1.q);

			if (d1 <= d2)
				return bound2;
			else
				return bound1;
		}

		public static Fraction operator +(Fraction a, Fraction b)
		{
			var p = a.p * b.q + b.p * a.q;
			var q = a.q * b.q;
			return new Fraction(p, q);
		}

		public static Fraction operator -(Fraction a, Fraction b)
		{
			var p = a.p * b.q - b.p * a.q;
			var q = a.q * b.q;
			return new Fraction(p, q);
		}

		public static Fraction operator *(Fraction a, Fraction b)
		{
			var p = a.p * b.p;
			var q = a.q * b.q;
			return new Fraction(p, q);
		}

		public static Fraction operator /(Fraction a, Fraction b)
		{
			var p = a.p * b.q;
			var q = a.q * b.p;
			return new Fraction(p, q);
		}

		public static bool operator <(Fraction a, Fraction b)
		{
			if (a.q == 0)
				return a.p < 0;
			if (b.q == 0)
				return b.p > 0;

			return a.p * b.q < b.p * a.q;
		}
		public static bool operator <=(Fraction a, Fraction b)
		{
			return a == b || a < b;
		}

		public static bool operator >=(Fraction a, Fraction b)
		{
			return !(a < b);
		}
		public static bool operator ==(Fraction a, Fraction b)
		{
			return a.p == b.p && (a.p == 0 || a.q == b.q);
		}
		public static bool operator !=(Fraction a, Fraction b)
		{
			return !(a == b);
		}

		public static bool operator >(Fraction a, Fraction b)
		{
			return !(a <= b);
		}

		public override string ToString()
		{
			return $"{p} {q}";
		}

		public override bool Equals(object obj)
		{
			var fraction = obj as Fraction;
			return p.Equals(fraction.p) && q.Equals(fraction.q);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 31 + p.GetHashCode();
				hash = hash * 31 + q.GetHashCode();
				return hash;
			}
		}
	};
}
