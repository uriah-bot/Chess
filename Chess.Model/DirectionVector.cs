namespace Chess.Model
{
    public class DirectionVector
    {
        public readonly static DirectionVector Up = new DirectionVector(-1, 0);
        public readonly static DirectionVector Down = new DirectionVector(1, 0);
        public readonly static DirectionVector Left = new DirectionVector(0, -1);
        public readonly static DirectionVector Right = new DirectionVector(0, 1);
        public readonly static DirectionVector UpLeft = Up + Left;
        public readonly static DirectionVector UpRight = Up + Right;
        public readonly static DirectionVector DownLeft = Down + Left;
        public readonly static DirectionVector DownRight = Down + Right;
        public int RowDelta { get; }
        public int ColumnDelta { get; }
        public DirectionVector(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        public static DirectionVector operator +(DirectionVector vector1, DirectionVector vector2)
        {
            return new DirectionVector(vector1.RowDelta + vector2.RowDelta, vector1.ColumnDelta + vector2.ColumnDelta);
        }

        public static DirectionVector operator *(DirectionVector vector, int scalar)
        {
            return new DirectionVector(vector.RowDelta * scalar, vector.ColumnDelta * scalar);
        }
    }
}
