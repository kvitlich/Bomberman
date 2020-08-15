using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.II
{
    public class MovementMask
    {
        public List<List<int>> boardMask { get; private set; }
        public Board board { get; private set; }
        public Point nextPoint { get; private set; }
        public int bombWaveSize { get; private set; }

        private bool IsOnBlast(Point point)
        {
            List<Point> blasts = board.GetFutureBlasts(null, bombWaveSize);
            return blasts.Contains(point);
        }

        public bool IfClose(Point pointFir, Point pointSec)
        {
            if (pointFir.ShiftRight() == pointSec
                || pointFir.ShiftLeft() == pointSec
                || pointFir.ShiftBottom() == pointSec
                || pointFir.ShiftTop() == pointSec
                 )
            {
                return true;
            }
            return false;
        }

        public Point NextPoint(List<Direction> directions, Point point)
        {
            if (directions.Contains(Direction.Down)) {
                return point.ShiftBottom();
            }
            if (directions.Contains(Direction.Up)) {
                return point.ShiftTop();
            }
            if (directions.Contains(Direction.Right)) {
                return point.ShiftRight();
            }
            if (directions.Contains(Direction.Left)) {
                return point.ShiftLeft();
            }
            return point;
        }
        public List<Direction> GetNext(Point point)
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
        public bool IfCanPass(Point point, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    point = point.ShiftLeft();
                    break;
                case Direction.Right:
                    point = point.ShiftRight();
                    break;
                case Direction.Up:
                    point = point.ShiftTop();
                    break;
                case Direction.Down:
                    point = point.ShiftBottom();
                    break;
                case Direction.Stop:
                    point = point;
                    break;
            }

            switch (board.GetAt(point))
            {
                case Element.BOMBERMAN:
                    return true;
                    break;
                case Element.BOMB_BOMBERMAN:
                    return false;
                    break;
                case Element.DEAD_BOMBERMAN:
                    return true;
                    break;
                case Element.OTHER_BOMBERMAN:
                    return false;
                    break;
                case Element.OTHER_BOMB_BOMBERMAN:
                    return false;
                    break;
                case Element.OTHER_DEAD_BOMBERMAN:
                    return true;
                    break;
                case Element.BOMB_TIMER_5:
                    return false;
                    break;
                case Element.BOMB_TIMER_4:
                    return false;
                    break;
                case Element.BOMB_TIMER_3:
                    return false;
                    break;
                case Element.BOMB_TIMER_2:
                    return false;
                    break;
                case Element.BOMB_TIMER_1:
                    return false;
                    break;
                case Element.BOOM:
                    return true;
                    break;
                case Element.WALL:
                    return false;
                    break;
                case Element.DESTROYABLE_WALL:
                    return false;
                    break;
                case Element.DestroyedWall:
                    return true;
                    break;
                case Element.MEAT_CHOPPER:
                    return false;
                    break;
                case Element.DeadMeatChopper:
                    return true;
                    break;
                case Element.Space:
                    return true;
                    break;
                case Element.BOMB_BLAST_RADIUS_INCREASE:
                    return true;
                    break;
                case Element.BOMB_COUNT_INCREASE:
                    return true;
                    break;
                case Element.BOMB_REMOTE_CONTROL:
                    return true;
                    break;
                case Element.BOMB_IMMUNE:
                    return true;
                    break;
            }
            return true;
        }
        public List<Direction> CalculateNextStep(Point benefitPoint, bool isHiding = false)
        {
            if (isHiding)
            {
                this.GenerateHideMasking(benefitPoint);
            }
            else
            {
                this.GenerateMasking(benefitPoint);
            }
            Point currentPoint = board.GetBomberman();
            List<Direction> temp = this.GetNext(currentPoint);
            switch (temp[0])
            {
                case Direction.Left:
                    nextPoint = currentPoint.ShiftLeft();
                    break;
                case Direction.Right:
                    nextPoint = currentPoint.ShiftRight();
                    break;
                case Direction.Up:
                    nextPoint = currentPoint.ShiftTop();
                    break;
                case Direction.Down:
                    nextPoint = currentPoint.ShiftBottom();
                    break;
                case Direction.Stop:
                    nextPoint = currentPoint;
                    break;
            }
            return temp;
        }

        private void GenerateHideMasking(Point benefitPoint)
        {
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
        private void GenerateMasking(Point benefitPoint)
        {
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
                    if (walked[i].X + 1 >= 0 && walked[i].Y < board.Size && walked[i].X + 1 < board.Size && walked[i].Y >= 0 && !IsOnBlast(walked[i].ShiftRight()))
                    {
                        if (this.boardMask[walked[i].X + 1][walked[i].Y] == 0)
                        {
                            this.boardMask[walked[i].X + 1][walked[i].Y] = d_i;
                            walked.Add(new Point(walked[i].X + 1, walked[i].Y));
                            ret = true;
                        }
                    }
                    if (walked[i].X - 1 >= 0 && walked[i].Y < board.Size && walked[i].X - 1 < board.Size && walked[i].Y >= 0 && !IsOnBlast(walked[i].ShiftLeft()))
                    {
                        if (this.boardMask[walked[i].X - 1][walked[i].Y] == 0)
                        {
                            this.boardMask[walked[i].X - 1][walked[i].Y] = d_i;
                            walked.Add(new Point(walked[i].X - 1, walked[i].Y));
                            ret = true;
                        }
                    }
                    if (walked[i].X >= 0 && walked[i].Y + 1 < board.Size && walked[i].X < board.Size && walked[i].Y + 1 >= 0 && !IsOnBlast(walked[i].ShiftTop()))
                    {
                        if (this.boardMask[walked[i].X][walked[i].Y + 1] == 0)
                        {
                            this.boardMask[walked[i].X][walked[i].Y + 1] = d_i;
                            walked.Add(new Point(walked[i].X, walked[i].Y + 1));
                            ret = true;
                        }
                    }
                    if (walked[i].X >= 0 && walked[i].Y - 1 < board.Size && walked[i].X + 1 < board.Size && walked[i].Y - 1 >= 0 && !IsOnBlast(walked[i].ShiftBottom()))
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

        public MovementMask(Board board, int bombWaveSize)
        {
            this.board = board;
            this.boardMask = new List<List<int>>();
            this.bombWaveSize = bombWaveSize;

   
        }

    }
}
