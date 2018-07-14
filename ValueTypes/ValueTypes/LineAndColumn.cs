using System;

namespace ValueTypes
{
    public readonly struct LineAndColumn : IEquatable<LineAndColumn>
    {
        public int Line { get; }
        public int Column { get; }

        public LineAndColumn(int line, int column)
            => (Line, Column) = (line, column);

        public override int GetHashCode()
            => (Line, Column).GetHashCode();

        public override bool Equals(object otherObject)
            => otherObject is LineAndColumn otherLineAndColumn && Equals(otherLineAndColumn);

        public bool Equals(LineAndColumn other)
            => (Line, Column) == (other.Line, other.Column);
    }
}
