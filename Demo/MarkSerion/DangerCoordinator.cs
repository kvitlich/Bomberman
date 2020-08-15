using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.MarkSerion
{
    public class DangerCoordinator
    {
        public Board board { get; private set; }

        public List<Point> bombs { get; private set; }

        public List<Point> meatChopers { get; private set; }
        public List<Point> meatChopers_1 { get; private set; }
        public List<Point> barries { get; private set; }

        public List<Point> blasts { get; private set; }
        public List<Point> blasts_1 { get; private set; }
        public List<Point> blasts_2 { get; private set; }
        public List<Point> blasts_5 { get; private set; }

        public List<Element> statBarries { get; private set; }

        public List<Point> destroyableWalls { get; private set; }

        public int bombWave;
        public DangerCoordinator(Board board, int bombWave)
        {
            this.board = board;
            barries = board.GetBarrier();
            bombs = board.GetBombs();
            blasts = board.GetFutureBlasts(bombs, bombWave);
            blasts_1 = board.GetBlasts_1(bombWave);
            blasts_2 = board.GetBlasts_2(bombWave);
            blasts_5 = board.GetBlasts_5(bombWave);
            destroyableWalls = board.GetDestroyableWalls();
            barries = board.GetBarrier();
            meatChopers = GetMeatChopersDangers(board.GetMeatChoppers(), 2);
            meatChopers_1 = GetMeatChopersDangers(board.GetMeatChoppers(), 1);
            this.board = board;
            this.bombWave = bombWave;
            statBarries = new List<Element>();
            statBarries.Add(Element.MEAT_CHOPPER);
            statBarries.Add(Element.DestroyedWall);
            statBarries.Add(Element.OTHER_BOMBERMAN);
        }
        public bool DangerCheckOnPoint(Point point)
        {
            if (meatChopers_1.Contains(point))
            {
                return true;
            }
            if (blasts.Contains(point))
            {
                return true;
            }
            if (bombs.Contains(point))
            {
                return true;
            }
            if (blasts_5.Contains(point))
            {
                return true;
            }
            return false;
        }
        public List<Point> GenerateBlastFrom(Point point, int wave)
        {
            List<Point> points = new List<Point>();
            points.Add(point);
            for (int i = 1; i <= wave; i++)
            {
                Point temp = point.ShiftLeft(i);
                if (board.GetAt(temp) != Element.DESTROYABLE_WALL && board.GetAt(temp) != Element.WALL)
                {
                    points.Add(point.ShiftLeft(i));
                }
                temp = point.ShiftRight(i);
                if (board.GetAt(temp) != Element.DESTROYABLE_WALL && board.GetAt(temp) != Element.WALL)
                {
                    points.Add(point.ShiftRight(i));
                }
                temp = point.ShiftTop(i);
                if (board.GetAt(temp) != Element.DESTROYABLE_WALL && board.GetAt(temp) != Element.WALL)
                {
                    points.Add(point.ShiftTop(i));
                }
                temp = point.ShiftBottom(i);
                if (board.GetAt(temp) != Element.DESTROYABLE_WALL && board.GetAt(temp) != Element.WALL)
                {
                    points.Add(point.ShiftBottom(i));
                }
            }
            return points;
        }
        public Point GetCloseSavePointFrom(Point currentPoint, List<Point> danger)
        {
            List<Point> walked = new List<Point>();
            walked.Add(currentPoint);
            int index = 0;
            while (true)
            {
                int i;
                int old_size = walked.Count;
                for (i = index; i < old_size; i++)
                {
                    if (!blasts.Contains(walked[i]) && !bombs.Contains(walked[i]) && !meatChopers.Contains(walked[i]) && !danger.Contains(walked[i]))
                    {
                        return walked[i];
                    }
                    if (board.GetAt(walked[i].ShiftRight()) != Element.WALL && board.GetAt(walked[i].ShiftRight()) != Element.DESTROYABLE_WALL && board.GetAt(walked[i].ShiftRight()) != Element.MEAT_CHOPPER)
                    {
                        if (!blasts.Contains(walked[i].ShiftRight()) && !bombs.Contains(walked[i].ShiftRight()) && !meatChopers.Contains(walked[i].ShiftRight()) && !danger.Contains(walked[i]) && IsCanPass(walked[i].ShiftRight(), Direction.Left))
                        {
                            return walked[i].ShiftRight();
                        }
                        walked.Add(walked[i].ShiftRight());
                    }
                    if (board.GetAt(walked[i].ShiftTop()) != Element.WALL && board.GetAt(walked[i].ShiftTop()) != Element.DESTROYABLE_WALL && board.GetAt(walked[i].ShiftTop()) != Element.MEAT_CHOPPER)
                    {
                        if (!blasts.Contains(walked[i].ShiftTop()) && !bombs.Contains(walked[i].ShiftTop()) && !meatChopers.Contains(walked[i].ShiftTop()) && !danger.Contains(walked[i]) && IsCanPass(walked[i].ShiftTop(), Direction.Down))
                        {
                            return walked[i].ShiftTop();
                        }
                        walked.Add(walked[i].ShiftTop());
                    }
                    if (board.GetAt(walked[i].ShiftLeft()) != Element.WALL && board.GetAt(walked[i].ShiftLeft()) != Element.DESTROYABLE_WALL && board.GetAt(walked[i].ShiftLeft()) != Element.MEAT_CHOPPER)
                    {
                        if (!blasts.Contains(walked[i].ShiftLeft()) && !bombs.Contains(walked[i].ShiftLeft()) && !meatChopers.Contains(walked[i].ShiftLeft()) && !danger.Contains(walked[i]) && IsCanPass(walked[i].ShiftLeft(), Direction.Right))
                        {
                            return walked[i].ShiftLeft();
                        }
                        walked.Add(walked[i].ShiftLeft());
                    }

                    if (board.GetAt(walked[i].ShiftBottom()) != Element.WALL && board.GetAt(walked[i].ShiftBottom()) != Element.DESTROYABLE_WALL && board.GetAt(walked[i].ShiftBottom()) != Element.MEAT_CHOPPER)
                    {
                        if (!blasts.Contains(walked[i].ShiftBottom()) && !bombs.Contains(walked[i].ShiftBottom()) && !meatChopers.Contains(walked[i].ShiftBottom()) && !danger.Contains(walked[i]) && IsCanPass(walked[i].ShiftBottom(), Direction.Up))
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

        private bool IsCanPass(Point point, Direction fromDirection)
        {
            if (!barries.Contains(point.ShiftBottom()) && !meatChopers_1.Contains(point.ShiftBottom()) && fromDirection != Direction.Down)
            {
                return true;
            }
            else if (!barries.Contains(point.ShiftTop()) && !meatChopers_1.Contains(point.ShiftTop()) && fromDirection != Direction.Up)
            {
                return true;
            }
            else if (!barries.Contains(point.ShiftLeft()) && !meatChopers_1.Contains(point.ShiftLeft()) && fromDirection != Direction.Left)
            {
                return true;
            }
            else if (!barries.Contains(point.ShiftRight()) && !meatChopers_1.Contains(point.ShiftRight()) && fromDirection != Direction.Right)
            {
                return true;
            }
            return false;
        }
        public Point GetCloseSavePoint(Point currentPoint)
        {
            List<Point> walked = new List<Point>();
            walked.Add(currentPoint);
            int index = 0;
            while (true)
            {
                int i;
                int old_size = walked.Count;
                if (index == old_size)
                {
                    break;
                }
                for (i = index; i < old_size; i++)
                {
                    if (!blasts.Contains(walked[i]) && !bombs.Contains(walked[i]) && !meatChopers.Contains(walked[i]))
                    {
                        return walked[i];
                    }
                    if (!barries.Contains(walked[i].ShiftRight()))
                    {
                        if (!blasts.Contains(walked[i].ShiftRight()) && !bombs.Contains(walked[i].ShiftRight()) && !meatChopers.Contains(walked[i].ShiftRight()) && IsCanPass(walked[i].ShiftRight(), Direction.Left))
                        {
                            return walked[i].ShiftRight();
                        }
                        walked.Add(walked[i].ShiftRight());
                    }
                    if (!barries.Contains(walked[i].ShiftTop()))
                    {
                        if (!blasts.Contains(walked[i].ShiftTop()) && !bombs.Contains(walked[i].ShiftTop()) && !meatChopers.Contains(walked[i].ShiftTop()) && IsCanPass(walked[i].ShiftTop(), Direction.Down))
                        {
                            return walked[i].ShiftTop();
                        }
                        walked.Add(walked[i].ShiftTop());
                    }
                    if (!barries.Contains(walked[i].ShiftLeft()))
                    {
                        if (!blasts.Contains(walked[i].ShiftLeft()) && !bombs.Contains(walked[i].ShiftLeft()) && !meatChopers.Contains(walked[i].ShiftLeft()) && IsCanPass(walked[i].ShiftLeft(), Direction.Right))
                        {
                            return walked[i].ShiftLeft();
                        }
                        walked.Add(walked[i].ShiftLeft());
                    }

                    if (!barries.Contains(walked[i].ShiftBottom()))
                    {
                        if (!blasts.Contains(walked[i].ShiftBottom()) && !bombs.Contains(walked[i].ShiftBottom()) && !meatChopers.Contains(walked[i].ShiftBottom()) && IsCanPass(walked[i].ShiftBottom(), Direction.Up))
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
            return new Point(-1, -1);
        }

        private List<Point> GetMeatChopersDangers(List<Point> chopers, int frontalDanger)
        {
            List<Point> points = new List<Point>(chopers.Count);
            for (int i = 0; i < chopers.Count; i++)
            {
                Element temp;
                for (int j = 1; j <= frontalDanger; j++)
                {
                    temp = board.GetAt(chopers[i].ShiftLeft(j));
                    if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                    {
                        points.Add(chopers[i].ShiftLeft(j));
                    }
                    temp = board.GetAt(chopers[i].ShiftRight(j));
                    if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                    {
                        points.Add(chopers[i].ShiftRight(j));
                    }
                    temp = board.GetAt(chopers[i].ShiftTop(j));
                    if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                    {
                        points.Add(chopers[i].ShiftTop(j));
                    }
                    temp = board.GetAt(chopers[i].ShiftBottom(j));
                    if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                    {
                        points.Add(chopers[i].ShiftBottom(j));
                    }
                
                }
                points.Add(chopers[i]);
                temp = board.GetAt(chopers[i].ShiftLeft().ShiftTop());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftLeft().ShiftTop());
                }
                temp = board.GetAt(chopers[i].ShiftRight().ShiftBottom());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftRight().ShiftBottom());
                }
                temp = board.GetAt(chopers[i].ShiftTop().ShiftRight());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftTop().ShiftRight());
                }
                temp = board.GetAt(chopers[i].ShiftBottom().ShiftLeft());
                if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL)
                {
                    points.Add(chopers[i].ShiftBottom().ShiftLeft());
                }
            }

            return points;
        }
    }
}

