namespace TurtleChallenge
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            Position p = (Position)obj;
            return p.X == this.X && p.Y == this.Y;

        }
    }
}
