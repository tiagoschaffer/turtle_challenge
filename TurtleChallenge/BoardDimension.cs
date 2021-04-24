namespace TurtleChallenge
{
    public struct BoardDimension
    {
        public int Width { get; private set; }
        public int Height { get; private set; }


        public BoardDimension(string dimension)
        {
            if (!string.IsNullOrWhiteSpace(dimension) && dimension.Contains("x"))
            {
                var dimensionParts = dimension.Split(new char[] { 'x', 'X' }, System.StringSplitOptions.RemoveEmptyEntries);
                Width = int.Parse(dimensionParts[0]);
                Height = int.Parse(dimensionParts[1]);
            }
            else
            {
                Width = 5;
                Height = 4;
            }
        }
    }
}
