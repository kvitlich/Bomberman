using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.II
{
    public class DangerMask
    {
        public List<List<int>> boardMask { get; private set; }
        public Board board { get; private set; }
        public MovementMask movementMask { get; private set; }
        public int bombWaveSize { get; private set; }
        public bool inOnBombWave { get; private set; }
        public bool ifNeedToBoom { get; private set; }
        public DangerMask(MovementMask movementMask, Board board, int bombWaveSize)
        {
            this.movementMask = movementMask;
            this.board = board;
            this.bombWaveSize = bombWaveSize;
            boardMask = new List<List<int>>();
        }
        public Point GetCloseSavePoint(Point currentPoint)
        {
            List<Point> bombs = board.GetBombs();
            List<Point>blasts = board.GetFutureBlasts(null, 5);
            List<Point> walked = new List<Point>();
            List<Point> barries = board.GetBarrier();
            List<Point> meatChopers = GetMeatChopersDangers(board.GetMeatChoppers());
            bombs.Add(currentPoint);
            walked.Add(currentPoint);
            int index = 0;
            while (true)
            {
                int i;
                int old_size = walked.Count;
                for (i = index; i < old_size; i++)
                {
                    if (!blasts.Contains(walked[i]) && !bombs.Contains(walked[i]) && !meatChopers.Contains(walked[i]))
                    {
                        return walked[i];
                    }
                    if (!barries.Contains(walked[i].ShiftRight()))
                    {
                        if (!blasts.Contains(walked[i].ShiftRight()) && !bombs.Contains(walked[i].ShiftRight()) && !meatChopers.Contains(walked[i].ShiftRight()))
                        {
                            return walked[i].ShiftRight();
                        }
                        walked.Add(walked[i].ShiftRight());
                    }
                    if (!barries.Contains(walked[i].ShiftTop()))
                    {
                        if (!blasts.Contains(walked[i].ShiftTop()) && !bombs.Contains(walked[i].ShiftTop()) && !meatChopers.Contains(walked[i].ShiftTop()))
                        {
                            return walked[i].ShiftTop();
                        }
                        walked.Add(walked[i].ShiftTop());
                    }
                    if (!barries.Contains(walked[i].ShiftLeft()))
                    {
                        if (!blasts.Contains(walked[i].ShiftLeft()) && !bombs.Contains(walked[i].ShiftLeft()) && !meatChopers.Contains(walked[i].ShiftLeft()))
                        {
                            return walked[i].ShiftLeft();
                        }
                        walked.Add(walked[i].ShiftLeft());
                    }

                    if (!barries.Contains(walked[i].ShiftBottom()))
                    {
                        if (!blasts.Contains(walked[i].ShiftBottom()) && !bombs.Contains(walked[i].ShiftBottom()) && !meatChopers.Contains(walked[i].ShiftBottom()))
                        {
                            return walked[i].ShiftBottom();
                        }
                        walked.Add(walked[i].ShiftBottom());
                    }
                }
                index = i;
                if (walked.Count >= 220)
                {
                    break;
                }
            }
            return new Point(1, 1);
        }
        public bool IsOnBlast(Point point)
        {
            List<Point> blasts = board.GetFutureBlasts(null, bombWaveSize);
            return blasts.Contains(point);
        }
        public bool IsOnBomb(Point point)
        {
            List<Point> bombs = board.GetBombs();
            return bombs.Contains(point);
        }

        public bool IsDangerOnPoint(Point point)
        {
            bool res = false;
            List<Point> blasts = board.GetFutureBlasts(null, 5);
            if (blasts.Contains(point))
            {
                return true;
            }
            if (board.IsNear(point, Element.MEAT_CHOPPER, 2))
            {
                ifNeedToBoom = true;
                return true;
            }

            return false;
        }

        public List<Direction> ReactOnDanger()
        {
            List<Direction> directions = new List<Direction>();

            if (ifNeedToBoom)
            {
                directions.Add(Direction.Act);
            }

            Point currentPosition = board.GetBomberman();


            Point pointSave = GetCloseSavePoint(currentPosition);
            directions.AddRange(movementMask.CalculateNextStep(pointSave));


            return directions;
        }

        private List<Point> GetMeatChopersDangers(List<Point> chopers)
        {
            List<Point> points = new List<Point>(chopers.Count);
            int old_size = chopers.Count;
            for (int i = 0; i < old_size; i++)
            {
                points.Add(chopers[i]);
                Element temp = board.GetAt(chopers[i].ShiftLeft());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftLeft());
                }
                temp = board.GetAt(chopers[i].ShiftRight());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftRight());
                }
                temp = board.GetAt(chopers[i].ShiftTop());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftTop());
                }
                temp = board.GetAt(chopers[i].ShiftBottom());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftBottom());
                }
            }
            return points;
        }
        public bool ContainsSameX(List<Point> points, Point point)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X == point.X)
                    return true;
                }
            return false;
        }
        public bool ContainsSameY(List<Point> points, Point point)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y == point.Y)
                    return true;
            }
            return false;
        }

        private bool HasCommonWith(Point current, List<Point> with)
        {
            for (int i = 0; i < with.Count; i++)
            {
                if (with[i].Equals(current))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsInBombWaveDanger(Point point)
        {
            List<Point> sides = new List<Point>();
            Element element = board.GetAt(point);
            bool res = false;
            if (element == Element.BOMB_TIMER_1
                   || element == Element.BOMB_TIMER_2
                   || element == Element.BOMB_TIMER_3
                   || element == Element.BOMB_TIMER_4
                   || element == Element.BOMB_TIMER_5
                   || element == Element.BOMB_BOMBERMAN
                   || element == Element.OTHER_BOMB_BOMBERMAN)
            {
                return true;
            }

            sides.Add(point.ShiftRight());
            sides.Add(point.ShiftLeft());
            sides.Add(point.ShiftTop());
            sides.Add(point.ShiftBottom());
            for (int i = 0; i < bombWaveSize; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    element = board.GetAt(sides[j]);
                    if (element == Element.BOMB_TIMER_1
                   || element == Element.BOMB_TIMER_2
                   || element == Element.BOMB_TIMER_3
                   || element == Element.BOMB_TIMER_4
                   || element == Element.BOMB_TIMER_5
                   || element == Element.BOMB_BOMBERMAN
                   || element == Element.OTHER_BOMB_BOMBERMAN)
                    {
                        return true;
                    }
                }

                sides[0] = point.ShiftRight();
                sides[1] = point.ShiftLeft();
                sides[2] = point.ShiftTop();
                sides[3] = point.ShiftBottom();
            }

            return false;
        }
        private bool ScanBombsWaveDanger(Point point)
        {
            List<Point> blasts = board.GetFutureBlasts(board.GetBombs(), bombWaveSize);
            for (int i = 0; i < blasts.Count; i++)
            {
                if (blasts[i] == point)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsElementByPointInArea(Point point, Element element, int area = 1)
        {
            List<Point> sides = new List<Point>();
            Element onPoint = board.GetAt(point);
            if (element == Element.BOMB_TIMER_1
                   || element == Element.BOMB_TIMER_2
                   || element == Element.BOMB_TIMER_3
                   || element == Element.BOMB_TIMER_4
                   || element == Element.BOMB_TIMER_5
                   || element == Element.BOMB_BOMBERMAN
                   || element == Element.OTHER_BOMB_BOMBERMAN)
            {
                return true;
            }

            sides.Add(point.ShiftRight());
            sides.Add(point.ShiftLeft());
            sides.Add(point.ShiftTop());
            sides.Add(point.ShiftBottom());
            for (int i = 1; i < +area; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    onPoint = board.GetAt(sides[j]);
                    if (onPoint == element)
                    {
                        return true;
                    }
                }


                sides[0] = point.ShiftRight();
                sides[1] = point.ShiftLeft();
                sides[2] = point.ShiftTop();
                sides[3] = point.ShiftBottom();
            }

            return false;
        }
    }
}
