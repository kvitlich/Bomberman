using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.II
{
    public class BenefitMask
    {
        public Board board { get; private set; }
        public MovementMask movementMask { get; private set; }
        public int bombWaveSize { get; private set; }

        private bool IsOnBlast(Point point)
        {
            List<Point> blasts = board.GetFutureBlasts(null, bombWaveSize);
            return blasts.Contains(point);
        }
        public BenefitMask(MovementMask movementMask, Board board, int bombWaveSize)
        {
            Random random = new Random();
            this.board = board;
            this.movementMask = movementMask;
            this.bombWaveSize = bombWaveSize;
        }

        public  bool IsNextElement(List<Direction> directions, Element element)
        {
            Point point = board.GetBomberman();
            if (directions.Contains(Direction.Down))
            {
                return board.GetAt(point.ShiftBottom()) == element;
            }
            if (directions.Contains(Direction.Up))
            {
                return board.GetAt(point.ShiftTop()) == element;
            }
            if (directions.Contains(Direction.Right))
            {
                return board.GetAt(point.ShiftRight()) == element;
            }
            if (directions.Contains(Direction.Left))
            {
                return board.GetAt(point.ShiftLeft()) == element;
            }
            return false;
        }

        public Point GetClosestBomberman()
        {
            Random random = new Random();
            var bombers = board.GetOtherBombermans();
            return bombers[random.Next(0, bombers.Count)];    
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
                if (walked.Count >= 1024)
                {
                    break;
                }
            }
            return new Point(1, 1);
        }

        public bool IsClose(Point currentPoint, Element element)
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
        public bool HaveOtherBombers()
        {
            List<Point> otherBombers = board.GetOtherBombermans();
            if (otherBombers.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool HaveBonuses()
        {
            List<Point> bonuses = GetBonuses();
            if (bonuses.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool HaveDestroyableWalls()
        {
            List<Point> walls = board.GetDestroyableWalls();
            if (walls.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool HaveMeatChopers()
        {
            List<Point> meat = board.GetMeatChoppers();
            if (meat.Count > 0)
            {
                return true;
            }
            return false;
        }

        public List<Element> GetBonusesAsElement()
        {
            List<Element> result = new List<Element>();
            result.Add(Element.BOMB_BLAST_RADIUS_INCREASE);
            result.Add(Element.BOMB_COUNT_INCREASE);
            result.Add(Element.BOMB_IMMUNE);
            result.Add(Element.BOMB_REMOTE_CONTROL);
            return result;
        }

        private List<Point> GetBonuses()
        {
            List<Point> result = new List<Point>();
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    Element temp = board.GetAt(new Point(i, j));
                    if (temp == Element.BOMB_BLAST_RADIUS_INCREASE
                       || temp == Element.BOMB_COUNT_INCREASE
                       || temp == Element.BOMB_IMMUNE
                       || temp == Element.BOMB_REMOTE_CONTROL)
                    {
                        result.Add(new Point(i, j));
                    }
                }
            }
            return result;
        }
    }
}
