using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurtleChallenge
{
    public class TurtleChallengeGame
    {
        private readonly BoardDimension BoardDimension;
        private readonly List<Position> MineLocations;
        private List<Action> Actions;
        private Position TurtlePosition;
        private Position ExitPosition;
        private Direction TurtleDirection;

        public TurtleChallengeGame(string actions,
                                   string boardDimension,
                                   int objects)
        {
            BoardDimension = new BoardDimension(boardDimension);
            TurtlePosition = new Position(0, 1);
            TurtleDirection = Direction.North;
            MineLocations = GenerateMines(objects);
            ExitPosition = GenerateExitPosition();
            Actions = ReadActions(actions);




            Console.WriteLine($"Board Dimension: {BoardDimension.Width}X{BoardDimension.Height}");
            Console.WriteLine($"Exit Position: {ExitPosition.X},{ExitPosition.Y}");
            if (MineLocations.Any())
            {
                StringBuilder sbMine = new StringBuilder();
                foreach (var mine in MineLocations)
                {
                    sbMine.Append($"({mine.X},{mine.Y}) ");
                }
                Console.WriteLine($"Mine Locations: {sbMine}");
            }

            StartGame();
        }

        public void StartGame()
        {
            foreach (var action in Actions)
            {
                if (action == Action.Rotate)
                {
                    Rotate();
                    Console.WriteLine($"Turtle rotate to {TurtleDirection}");
                }
                else
                {
                    Move();
                    Console.WriteLine($"Turtle move to {TurtlePosition.X},{TurtlePosition.Y}");
                    if (!MonitorTurtleMove())
                        break;
                }

            }
        }

        private bool MonitorTurtleMove()
        {
            var status = CheckPosition();
            switch (status)
            {
                case TurtleStatus.MineHit:
                    {
                        Console.WriteLine("Turtle is dead!");
                        return false;
                    }
                case TurtleStatus.Normal:
                    {
                        return true;
                    }
                case TurtleStatus.IsOutOfBounds:
                    {
                        Console.WriteLine("Turtle is out of bound position!");
                        return false;
                    }
                case TurtleStatus.Exit:
                    {
                        Console.WriteLine("Turtle reach its destination!");
                        return false;
                    }
                case TurtleStatus.InDanger:
                    {
                        Console.WriteLine("Turtle is in danger!");
                        return true;
                    }
            }
            return true;
        }

        private List<Action> ReadActions(string actions)
        {
            var actionList = new List<Action>();
            var actionParts = actions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < actionParts.Length; i++)
            {
                var action = actionParts[i];
                if (action == "m")
                    actionList.Add(Action.Move);
                else
                    actionList.Add(Action.Rotate);
            }
            return actionList;
        }

        private List<Position> GenerateMines(int objects)
        {
            var locations = new List<Position>();
            Random rnd = new Random();
            for (int i = 0; i < objects; i++)
            {
                locations.Add(new Position(rnd.Next(0, BoardDimension.Width - 1), rnd.Next(0, BoardDimension.Height - 1)));
            }
            return locations;
        }

        private Position GenerateExitPosition()
        {
            Random rnd = new Random();
            return new Position(BoardDimension.Width - 1, rnd.Next(0, BoardDimension.Height - 1));
        }

        private void Rotate()
        {
            if (TurtleDirection == Direction.West)
                TurtleDirection = Direction.North;
            else
                TurtleDirection = TurtleDirection + 1;
        }

        private void Move()
        {
            switch (TurtleDirection)
            {
                case Direction.North:
                    TurtlePosition.Y--;
                    break;
                case Direction.East:
                    TurtlePosition.X++;
                    break;
                case Direction.South:
                    TurtlePosition.Y++;
                    break;
                case Direction.West:
                    TurtlePosition.X--;
                    break;
            }
        }

        private TurtleStatus CheckPosition()
        {
            if (TurtlePosition.Y < 0 || TurtlePosition.X < 0)
            {
                return TurtleStatus.IsOutOfBounds;
            }
            if (MineLocations.Any(m => m.Equals(TurtlePosition)))
            {
                return TurtleStatus.MineHit;
            }
            if (ExitPosition.Equals(TurtlePosition))
            {
                return TurtleStatus.Exit;
            }
            if (CheckIfIsInDanger())
            {
                return TurtleStatus.InDanger;
            }
            return TurtleStatus.Normal;
        }

        private bool CheckIfIsInDanger()
        {
            if (TurtlePosition.X == 0 ||
                TurtlePosition.Y == 0 ||
                TurtlePosition.X == BoardDimension.Width - 1 ||
                TurtlePosition.Y == BoardDimension.Height - 1)
                return true;


            foreach (var m in MineLocations)
            {
                if (m.X + 1 == TurtlePosition.X ||
                    m.X - 1 == TurtlePosition.X ||
                    m.Y + 1 == TurtlePosition.Y ||
                    m.Y + 1 == TurtlePosition.Y)
                    return true;
            }
            return false;
        }
    }
}
