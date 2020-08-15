using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.MarkSerion
{
    public class Coordinator
    {
        private Board board;

        private DangerCoordinator dangerCoordinator;
        public List<List<int>> boardMask { get; private set; }

        public Coordinator(Board board, DangerCoordinator dangerCoordinator)
        {
            this.board = board;
            this.dangerCoordinator = dangerCoordinator;
            boardMask = new List<List<int>>();
        }

        public int GetTicksTo(Point from, Point to)
        {
            List<Point> walked = new List<Point>(50);
            int d_i = 1;
            Point point;
            int old_size = walked.Count;
            walked.Add(from);
            for (int i = 0; i < old_size; i++)
            {
                if (d_i > 5)
                {
                    return 0;
                }
                point = walked[i].ShiftRight();
                if (!dangerCoordinator.meatChopers_1.Contains(point) && board.GetAt(point) != Element.WALL && board.GetAt(point) != Element.DESTROYABLE_WALL && board.GetAt(point) != Element.BOMBERMAN && board.GetAt(point) != Element.OTHER_BOMB_BOMBERMAN)
                {
                    if (point == to)
                    {
                        return d_i;
                    }
                    walked.Add(point);
                }
                point = walked[i].ShiftLeft();
                if (!dangerCoordinator.meatChopers_1.Contains(point) && board.GetAt(point) != Element.WALL && board.GetAt(point) != Element.DESTROYABLE_WALL && board.GetAt(point) != Element.BOMBERMAN && board.GetAt(point) != Element.OTHER_BOMB_BOMBERMAN)
                {
                    if (point == to)
                    {
                        return d_i;
                    }
                    walked.Add(point);
                }
                point = walked[i].ShiftTop();
                if (!dangerCoordinator.meatChopers_1.Contains(point) && board.GetAt(point) != Element.WALL && board.GetAt(point) != Element.DESTROYABLE_WALL && board.GetAt(point) != Element.BOMBERMAN && board.GetAt(point) != Element.OTHER_BOMB_BOMBERMAN)
                {
                    if (point == to)
                    {
                        return d_i;
                    }
                    walked.Add(point);
                }
                point = walked[i].ShiftBottom();
                if (!dangerCoordinator.meatChopers_1.Contains(point) && board.GetAt(point) != Element.WALL && board.GetAt(point) != Element.DESTROYABLE_WALL && board.GetAt(point) != Element.BOMBERMAN && board.GetAt(point) != Element.OTHER_BOMB_BOMBERMAN)
                {
                    if (point == to)
                    {
                        return d_i;
                    }
                    walked.Add(point);
                }
                d_i++;
            }
            return -1;
        }

        public bool IsOnLineWith(Element sub, Point main, int closeValue, int count)
        {
            int resCount = 0;
            bool side_1 = true;
            bool side_2 = true;
            bool side_3 = true;
            bool side_4 = true;
            for (int j = 1; j <= closeValue; j++)
            {
                Element temp = board.GetAt(new Point(main.X + j, main.Y));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_1) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_1))
                {
                    if (sub == board.GetAt(new Point(main.X + j, main.Y)))
                    {
                        resCount++;
                    }
                }
                else { side_1 = false; }
                temp = board.GetAt(new Point(main.X - j, main.Y));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_2) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_2))
                {
                    if (sub == board.GetAt(new Point(main.X - j, main.Y)))
                    {
                        resCount++;

                    }
                }
                else { side_2 = false; }
                temp = board.GetAt(new Point(main.X, main.Y + j));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_3) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_3))
                {
                    if (sub == board.GetAt(new Point(main.X, main.Y + j)))
                    {
                        resCount++;

                    }
                }
                else { side_3 = false; }
                temp = board.GetAt(new Point(main.X, main.Y - j));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_4) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_4))
                {
                    if (sub == board.GetAt(new Point(main.X, main.Y - j)))
                    {
                        resCount++;

                    }
                }
                else { side_4 = false; }
            }
            if (resCount >= count)
            {
                return true;
            }
            return false;
        }
        public bool IsOnLineWith(Element sub, Point main, int closeValue)
        {
            bool side_1 = true;
            bool side_2 = true;
            bool side_3 = true;
            bool side_4 = true;
            for (int j = 1; j <= closeValue; j++)
            {
                Element temp = board.GetAt(new Point(main.X + j, main.Y));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_1) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_1))
                {
                    if (sub == board.GetAt(new Point(main.X + j, main.Y)))
                    {
                        return true;
                    }
                }
                else { side_1 = false; }
                temp = board.GetAt(new Point(main.X - j, main.Y));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_2) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_2))
                {
                    if (sub == board.GetAt(new Point(main.X - j, main.Y)))
                    {
                        return true;
                    }
                }
                else { side_2 = false; }
                temp = board.GetAt(new Point(main.X, main.Y + j));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_3) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_3))
                {
                    if (sub == board.GetAt(new Point(main.X, main.Y + j)))
                    {
                        return true;
                    }
                }
                else { side_3 = false; }
                temp = board.GetAt(new Point(main.X, main.Y - j));
                if ((temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_4) || (temp != Element.WALL && sub == Element.DESTROYABLE_WALL && side_4))
                {
                    if (sub == board.GetAt(new Point(main.X, main.Y - j)))
                    {
                        return true;
                    }
                }
                else { side_4 = false; }
            }
            return false;
        }
        public bool IsOnLineWith(List<Element> sub, Point main, int closeValue)
        {
            bool side_1 = true;
            bool side_2 = true;
            bool side_3 = true;
            bool side_4 = true;
            for (int i = 0; i < sub.Count; i++)
            {
                for (int j = 1; j <= closeValue; j++)
                {
                    for (int k = 0; k < sub.Count; k++)
                    {
                        Element temp = board.GetAt(new Point(main.X + j, main.Y));
                        if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_1)
                        {
                            if (sub[i] == board.GetAt(new Point(main.X + j, main.Y)))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            side_1 = false;
                        }
                        temp = board.GetAt(new Point(main.X - j, main.Y));
                        if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_2)
                        {
                            if (sub[i] == board.GetAt(new Point(main.X - j, main.Y)))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            side_2 = false;
                        }
                        temp = board.GetAt(new Point(main.X, main.Y + j));
                        if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_3)
                        {
                            if (sub[i] == board.GetAt(new Point(main.X, main.Y + j)))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            side_3 = false;
                        }
                        temp = board.GetAt(new Point(main.X, main.Y - j));
                        if (temp != Element.WALL && temp != Element.DESTROYABLE_WALL && side_4)
                        {
                            if (sub[i] == board.GetAt(new Point(main.X, main.Y - j)))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            side_4 = false;
                        }
                    }
                }
            }
            return false;
        }
        public bool IsOnLineWith(List<Point> sub, Point main, int closeValue)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                for (int j = 1; j <= closeValue; j++)
                {
                    if (sub[i].X == main.X)
                    {
                        if (ModeledDifference(sub[i].Y, main.Y) <= j)
                        {
                            return true;
                        }
                    }
                    if (sub[i].Y == main.Y)
                    {
                        if (ModeledDifference(sub[i].X, main.X) <= j)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private int ModeledDifference(int first, int second)
        {
            if (first > second)
            {
                return first - second;
            }
            return second - first;
        }
        public Point GetPointByDirections(List<Direction> directions)
        {
            Point point = board.GetBomberman();
            if (directions.Contains(Direction.Down))
            {
                point = point.ShiftBottom();
            }
            else if (directions.Contains(Direction.Up))
            {
                point = point.ShiftTop();
            }
            else if (directions.Contains(Direction.Right))
            {
                point = point.ShiftRight();
            }
            else if (directions.Contains(Direction.Left))
            {
                point = point.ShiftLeft();
            }
            return point;
        }

        public List<Point> GetCloseCircly(Point currentPoint, Element element)
        {
            List<Point> res = new List<Point>();
            if (board.GetAt(currentPoint.ShiftBottom()) == element)
            {
                res.Add(currentPoint.ShiftBottom());
            }
            if (board.GetAt(currentPoint.ShiftLeft()) == element)
            {
                res.Add(currentPoint.ShiftLeft());
            }
            if (board.GetAt(currentPoint.ShiftRight()) == element)
            {
                res.Add(currentPoint.ShiftRight());
            }
            if (board.GetAt(currentPoint.ShiftTop()) == element)
            {
                res.Add(currentPoint.ShiftTop());
            }
            if (board.GetAt(new Point(currentPoint.X + 1, currentPoint.Y - 1)) == element)
            {
                res.Add(new Point(currentPoint.X + 1, currentPoint.Y));
                res.Add(new Point(currentPoint.X, currentPoint.Y - 1));
            }
            if (board.GetAt(new Point(currentPoint.X - 1, currentPoint.Y - 1)) == element)
            {
                res.Add(new Point(currentPoint.X, currentPoint.Y - 1));
                res.Add(new Point(currentPoint.X - 1, currentPoint.Y));
            }
            if (board.GetAt(new Point(currentPoint.X - 1, currentPoint.Y - 1)) == element)
            {
                res.Add(new Point(currentPoint.X - 1, currentPoint.Y));
                res.Add(new Point(currentPoint.X, currentPoint.Y - 1));
            }
            if (board.GetAt(new Point(currentPoint.X + 1, currentPoint.Y + 1)) == element)
            {
                res.Add(new Point(currentPoint.X, currentPoint.Y + 1));
                res.Add(new Point(currentPoint.X + 1, currentPoint.Y));
            }
            return res;
        }

        public bool IsCloseCircly(Point currentPoint, Element element)
        {
            if (board.GetAt(currentPoint.ShiftBottom()) == element
                || board.GetAt(currentPoint.ShiftLeft()) == element
                || board.GetAt(currentPoint.ShiftRight()) == element
                || board.GetAt(currentPoint.ShiftTop()) == element
                || board.GetAt(new Point(currentPoint.X + 1, currentPoint.Y)) == element
                || board.GetAt(new Point(currentPoint.X - 1, currentPoint.Y)) == element
                || board.GetAt(new Point(currentPoint.X, currentPoint.Y - 1)) == element
                || board.GetAt(new Point(currentPoint.X, currentPoint.Y + 1)) == element
                )
            {
                return true;
            }
            return false;
        }
        public bool IsCloseFrontal(Point currentPoint, Element element)
        {
            if (board.GetAt(currentPoint.ShiftBottom()) == element
                || board.GetAt(currentPoint.ShiftLeft()) == element
                || board.GetAt(currentPoint.ShiftRight()) == element
                || board.GetAt(currentPoint.ShiftTop()) == element)
            {
                return true;
            }
            return false;
        }
        public bool IsCloseFrontal(Point currentPoint, Element element, int count)
        {
            int r = 0;
            if (board.GetAt(currentPoint.ShiftBottom()) == element)
            {
                r++;
            }
            if (board.GetAt(currentPoint.ShiftLeft()) == element)
            {
                r++;
                
            }
            if (board.GetAt(currentPoint.ShiftRight()) == element)
            {
                r++;

            }
            if (board.GetAt(currentPoint.ShiftTop()) == element)
            {
                r++;
            }
            if (r >= count)
            {
                return true;
            }
            return false;
        }
        public bool IsCloseFrontal(Point currentPoint, Point point)
        {
            if (currentPoint.ShiftBottom() == point
                || currentPoint.ShiftLeft() == point
                || currentPoint.ShiftRight() == point
                || currentPoint.ShiftTop() == point)
            {
                return true;
            }
            return false;
        }
        public Point GetClosest(List<Element> elements)
        {
            List<Point> walked = new List<Point>();
            Point currentPoint = board.GetBomberman();
            walked.Add(currentPoint);
            int index = 0;
            while (true)
            {
                int i;
                int old_size = walked.Count;
                for (i = index; i < old_size; i++)
                {
                    if ((board.GetAt(walked[i].ShiftRight()) != Element.WALL && board.GetAt(walked[i].ShiftRight()) != Element.DESTROYABLE_WALL)
                        || (board.GetAt(walked[i].ShiftRight()) != Element.WALL && elements.Contains(Element.DESTROYABLE_WALL)))
                    {
                        walked.Add(walked[i].ShiftRight());
                        if (elements.Contains(board.GetAt(walked[i].ShiftRight())))
                        {
                            return walked[i].ShiftRight();
                        }
                    }
                    if ((board.GetAt(walked[i].ShiftLeft()) != Element.WALL && board.GetAt(walked[i].ShiftLeft()) != Element.DESTROYABLE_WALL)
                        || (board.GetAt(walked[i].ShiftLeft()) != Element.WALL && elements.Contains(Element.DESTROYABLE_WALL)))
                    {
                        walked.Add(walked[i].ShiftLeft());
                        if (elements.Contains(board.GetAt(walked[i].ShiftLeft())))
                        {
                            return walked[i].ShiftLeft();
                        }
                    }
                    if ((board.GetAt(walked[i].ShiftTop()) != Element.WALL && board.GetAt(walked[i].ShiftTop()) != Element.DESTROYABLE_WALL)
                        || (board.GetAt(walked[i].ShiftTop()) != Element.WALL && elements.Contains(Element.DESTROYABLE_WALL)))
                    {
                        walked.Add(walked[i].ShiftTop());

                        if (elements.Contains(board.GetAt(walked[i].ShiftTop())))
                        {
                            return walked[i].ShiftTop();
                        }

                    }
                    if ((board.GetAt(walked[i].ShiftBottom()) != Element.WALL && board.GetAt(walked[i].ShiftBottom()) != Element.DESTROYABLE_WALL)
                        || (board.GetAt(walked[i].ShiftBottom()) != Element.WALL && elements.Contains(Element.DESTROYABLE_WALL)))
                    {
                        walked.Add(walked[i].ShiftBottom());
                        if (elements.Contains(board.GetAt(walked[i].ShiftBottom())))
                        {
                            return walked[i].ShiftBottom();
                        }

                    }
                }
                index = i;

                if (walked.Count > 2048)
                {
                    break;
                }
            }
            return new Point(0, 0);
        }
        public List<Direction> CalculateNextStep(Point benefitPoint, bool isHiding = false)
        {
            if (isHiding)
            {
                this.TrassBoardHide(benefitPoint);
            }
            else
            {
                this.TrassBoard(benefitPoint);
            }
            Point currentPoint = board.GetBomberman();
            List<Direction> temp = this.GetNext(currentPoint);
            return temp;
        }

        private List<Direction> GetNext(Point point)
        {
            List<Direction> directions = new List<Direction>();
            if (this.boardMask[point.X - 1][point.Y] < this.boardMask[point.X][point.Y] && this.boardMask[point.X - 1][point.Y] != -1)
            {
                directions.Add(Direction.Left);
                return directions;
            }
            else if (this.boardMask[point.X + 1][point.Y] < this.boardMask[point.X][point.Y] && this.boardMask[point.X + 1][point.Y] != -1)
            {
                directions.Add(Direction.Right);
                return directions;
            }
            else if (this.boardMask[point.X][point.Y - 1] < this.boardMask[point.X][point.Y] && this.boardMask[point.X][point.Y - 1] != -1)
            {
                directions.Add(Direction.Down);
                return directions;
            }
            else if (this.boardMask[point.X][point.Y + 1] < this.boardMask[point.X][point.Y] && this.boardMask[point.X][point.Y + 1] != -1)
            {
                directions.Add(Direction.Up);
                return directions;
            }
            directions.Add(Direction.Stop);
            return directions;
        }
        public void TrassBoardHide(Point benefitPoint)
        {
            for (int i = 0; i < board.Size; i++)
            {
                this.boardMask.Add(new List<int>());
                for (int j = 0; j < board.Size; j++)
                {
                    switch (board.GetAt(new Point(i, j)))
                    {
                        case Element.BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.DEAD_BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.OTHER_BOMBERMAN:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.OTHER_BOMB_BOMBERMAN:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.OTHER_DEAD_BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_TIMER_5:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_4:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_3:

                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_2:

                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_1:

                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOOM:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.WALL:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.DESTROYABLE_WALL:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.DestroyedWall:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.MEAT_CHOPPER:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.DeadMeatChopper:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.Space:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_BLAST_RADIUS_INCREASE:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_COUNT_INCREASE:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_REMOTE_CONTROL:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_IMMUNE:
                            this.boardMask[i].Add(0);
                            break;
                        default:
                            this.boardMask[i].Add(0);
                            break;
                    }

                }
            }
            Point currentPoint = board.GetBomberman();


            //for (int i = 0; i < dangerCoordinator.meatChopers_1.Count; i++)
            //{
            //    dangerCoordinator.meatChopers_1[i] = 
            //}
            if (IsCloseCircly(currentPoint, Element.MEAT_CHOPPER))
            {
                GetCloseCircly(currentPoint, Element.MEAT_CHOPPER).ForEach(x => this.boardMask[x.X][x.X] = -1);

            }
            //for (int i = 0; i < dangerCoordinator.blasts.Count; i++)
            //{
            //    if (!dangerCoordinator.bombs.Contains(dangerCoordinator.blasts[i]))
            //    {
            //        int ticks = GetTicksTo(currentPoint, dangerCoordinator.blasts[i]);
            //        if (ticks == 1 && IsOnLineWith(Element.BOMB_TIMER_1, currentPoint, dangerCoordinator.bombWave))
            //        {
            //            boardMask[dangerCoordinator.blasts[i].X][dangerCoordinator.blasts[i].Y] = -1;
            //        }
            //        else if (ticks == 2 && IsOnLineWith(Element.BOMB_TIMER_2, currentPoint, dangerCoordinator.bombWave))
            //        {
            //            boardMask[dangerCoordinator.blasts[i].X][dangerCoordinator.blasts[i].Y] = -1;
            //        }
            //        else if (ticks == 3 && IsOnLineWith(Element.BOMB_TIMER_3, currentPoint, dangerCoordinator.bombWave))
            //        {
            //            boardMask[dangerCoordinator.blasts[i].X][dangerCoordinator.blasts[i].Y] = -1;
            //        }
            //        else if (ticks == 4 && IsOnLineWith(Element.BOMB_TIMER_4, currentPoint, dangerCoordinator.bombWave))
            //        {
            //            boardMask[dangerCoordinator.blasts[i].X][dangerCoordinator.blasts[i].Y] = -1;
            //        }
            //        else
            //        {
            //            boardMask[dangerCoordinator.blasts[i].X][dangerCoordinator.blasts[i].Y] = 0;
            //        }
            //    }
            //    this.boardMask[dangerCoordinator.blasts[0].X][dangerCoordinator.blasts[0].Y] = 0;
            //}

            List<Point> walked = new List<Point>();
            this.boardMask[benefitPoint.X][benefitPoint.Y] = 1;
            walked.Add(benefitPoint);
            int d_i = 2;
            int index = 0;

            while (true)
            {
                bool ret = false;
                int old_size = walked.Count;
                int i;
                for (i = index; i < old_size; i++)
                {
                    if (walked[i].X + 1 >= 0 && walked[i].Y < board.Size && walked[i].X + 1 < board.Size && walked[i].Y >= 0)
                    {
                        if (this.boardMask[walked[i].X + 1][walked[i].Y] == 0)
                        {
                            this.boardMask[walked[i].X + 1][walked[i].Y] = d_i;
                            walked.Add(new Point(walked[i].X + 1, walked[i].Y));
                            ret = true;
                        }
                    }
                    if (walked[i].X - 1 >= 0 && walked[i].Y < board.Size && walked[i].X - 1 < board.Size && walked[i].Y >= 0)
                    {
                        if (this.boardMask[walked[i].X - 1][walked[i].Y] == 0)
                        {
                            this.boardMask[walked[i].X - 1][walked[i].Y] = d_i;
                            walked.Add(new Point(walked[i].X - 1, walked[i].Y));
                            ret = true;
                        }
                    }
                    if (walked[i].X >= 0 && walked[i].Y + 1 < board.Size && walked[i].X < board.Size && walked[i].Y + 1 >= 0)
                    {
                        if (this.boardMask[walked[i].X][walked[i].Y + 1] == 0)
                        {
                            this.boardMask[walked[i].X][walked[i].Y + 1] = d_i;
                            walked.Add(new Point(walked[i].X, walked[i].Y + 1));
                            ret = true;
                        }
                    }
                    if (walked[i].X >= 0 && walked[i].Y - 1 < board.Size && walked[i].X + 1 < board.Size && walked[i].Y - 1 >= 0)
                    {
                        if (this.boardMask[walked[i].X][walked[i].Y - 1] == 0)
                        {
                            this.boardMask[walked[i].X][walked[i].Y - 1] = d_i;
                            walked.Add(new Point(walked[i].X, walked[i].Y - 1));
                            ret = true;
                        }
                    }
                }
                index = i;
                d_i++;
                if (!ret)
                {
                    break;
                }
            }
        }
        public void TrassBoard(Point benefitPoint)
        {
            for (int i = 0; i < board.Size; i++)
            {
                this.boardMask.Add(new List<int>());
                for (int j = 0; j < board.Size; j++)
                {
                    switch (board.GetAt(new Point(i, j)))
                    {
                        case Element.BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.DEAD_BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.OTHER_BOMBERMAN:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.OTHER_BOMB_BOMBERMAN:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.OTHER_DEAD_BOMBERMAN:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_TIMER_5:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_4:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_3:

                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_2:

                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOMB_TIMER_1:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.BOOM:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.WALL:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.DESTROYABLE_WALL:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.DestroyedWall:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.MEAT_CHOPPER:
                            this.boardMask[i].Add(-1);
                            break;
                        case Element.DeadMeatChopper:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.Space:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_BLAST_RADIUS_INCREASE:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_COUNT_INCREASE:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_REMOTE_CONTROL:
                            this.boardMask[i].Add(0);
                            break;
                        case Element.BOMB_IMMUNE:
                            this.boardMask[i].Add(0);
                            break;
                        default:
                            this.boardMask[i].Add(0);
                            break;
                    }

                }
            }

            //for (int i = 0; i < dangerCoordinator.blasts.Count; i++)
            //{
            //    this.boardMask[dangerCoordinator.blasts[0].X][dangerCoordinator.blasts[0].Y] = -1;
            //}

            for (int i = 0; i < dangerCoordinator.blasts.Count; i++)
            {
                boardMask[dangerCoordinator.blasts[i].X][dangerCoordinator.blasts[i].Y] = -1;

            }

            List<Point> walked = new List<Point>();
            this.boardMask[benefitPoint.X][benefitPoint.Y] = 1;
            walked.Add(benefitPoint);
            int d_i = 2;
            int index = 0;

            while (true)
            {
                bool ret = false;
                int old_size = walked.Count;
                int i;
                for (i = index; i < old_size; i++)
                {
                    if (walked[i].X + 1 >= 0 && walked[i].Y < board.Size && walked[i].X + 1 < board.Size && walked[i].Y >= 0 && !dangerCoordinator.blasts.Contains(walked[i].ShiftRight()))
                    {
                        if (this.boardMask[walked[i].X + 1][walked[i].Y] == 0)
                        {
                            this.boardMask[walked[i].X + 1][walked[i].Y] = d_i;
                            walked.Add(new Point(walked[i].X + 1, walked[i].Y));
                            ret = true;
                        }
                    }
                    if (walked[i].X - 1 >= 0 && walked[i].Y < board.Size && walked[i].X - 1 < board.Size && walked[i].Y >= 0 && !dangerCoordinator.blasts.Contains(walked[i].ShiftLeft()))
                    {
                        if (this.boardMask[walked[i].X - 1][walked[i].Y] == 0)
                        {
                            this.boardMask[walked[i].X - 1][walked[i].Y] = d_i;
                            walked.Add(new Point(walked[i].X - 1, walked[i].Y));
                            ret = true;
                        }
                    }
                    if (walked[i].X >= 0 && walked[i].Y + 1 < board.Size && walked[i].X < board.Size && walked[i].Y + 1 >= 0 && !dangerCoordinator.blasts.Contains(walked[i].ShiftTop()))
                    {
                        if (this.boardMask[walked[i].X][walked[i].Y + 1] == 0)
                        {
                            this.boardMask[walked[i].X][walked[i].Y + 1] = d_i;
                            walked.Add(new Point(walked[i].X, walked[i].Y + 1));
                            ret = true;
                        }
                    }
                    if (walked[i].X >= 0 && walked[i].Y - 1 < board.Size && walked[i].X + 1 < board.Size && walked[i].Y - 1 >= 0 && !dangerCoordinator.blasts.Contains(walked[i].ShiftBottom()))
                    {
                        if (this.boardMask[walked[i].X][walked[i].Y - 1] == 0)
                        {
                            this.boardMask[walked[i].X][walked[i].Y - 1] = d_i;
                            walked.Add(new Point(walked[i].X, walked[i].Y - 1));
                            ret = true;
                        }
                    }
                }
                index = i;
                d_i++;
                if (!ret)
                {
                    break;
                }
            }
        }

    }
}
