/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberman.Api
{
    public class Board
    {

        private String BoardString { get; }
        private LengthToXY LengthXY;

        public Board(String boardString)
        {
            BoardString = boardString.Replace("\n", "");
            LengthXY = new LengthToXY(Size);
        }

        /// <summary>
        /// GameBoard size (actual board size is Size x Size cells)
        /// </summary>
        public int Size {
            get {
                return (int)Math.Sqrt(BoardString.Length);
            }
        }

        public Point GetBomberman()
        {
            return Get(Element.BOMBERMAN)
                    .Concat(Get(Element.BOMB_BOMBERMAN))
                    .Concat(Get(Element.DEAD_BOMBERMAN))
                    .Single();
        }
        public List<Point> GetSpaces()
        {
            return Get(Element.Space);
        }

        public List<Point> GetOtherBombermans()
        {
            return Get(Element.OTHER_BOMBERMAN)
                .Concat(Get(Element.OTHER_BOMB_BOMBERMAN))
                .Concat(Get(Element.OTHER_DEAD_BOMBERMAN))
                .ToList();
        }

        public bool isMyBombermanDead {
            get {
                return BoardString.Contains((char)Element.DEAD_BOMBERMAN);
            }
        }

        public Element GetAt(Point point)
        {
            if (point.IsOutOf(Size))
            {
                return Element.WALL;
            }
            return (Element)BoardString[LengthXY.GetLength(point.X, point.Y)];
        }

        public bool IsAt(Point point, Element element)
        {
            if (point.IsOutOf(Size))
            {
                return false;
            }

            return GetAt(point) == element;
        }

        public string BoardAsString()
        {
            string result = "";
            for (int i = 0; i < Size; i++)
            {
                result += BoardString.Substring(i * Size, Size);
                result += "\n";
            }
            return result;
        }

        /// <summary>
        /// gets board view as string
        /// </summary>
        public string ToString()
        {
            return string.Format("{0}\n" +
                     "Bomberman at: {1}\n" +
                     "Other bombermans at: {2}\n" +
                     "Meat choppers at: {3}\n" +
                     "Destroy walls at: {4}\n" +
                     "Bombs at: {5}\n" +
                     "Blasts: {6}\n" +
                     "Expected blasts at: {7}\n" +
                     "Perks at: {8}",
                     BoardAsString(),
                     GetBomberman(),
                     ListToString(GetOtherBombermans()),
                     ListToString(GetMeatChoppers()),
                     ListToString(GetDestroyableWalls()),
                     ListToString(GetBombs()),
                     ListToString(GetBlasts()),
                     ListToString(GetFutureBlasts()),
                     ListToString(GetPerks()));
        }

        private string ListToString(List<Point> list)
        {
            return string.Join(",", list.ToArray());
        }

        public List<Point> GetBarrier()
        {
            return GetMeatChoppers()
                .Concat(GetWalls())
                .Concat(GetBombs())
                .Concat(GetDestroyableWalls())
                .Concat(GetOtherBombermans())
                .Distinct()
                .ToList();
        }

    

        public List<Point> GetMeatChoppers()
        {
            return Get(Element.MEAT_CHOPPER);
        }

        public List<Point> Get(Element element)
        {
            List<Point> result = new List<Point>();

            for (int i = 0; i < Size * Size; i++)
            {
                Point pt = LengthXY.GetXY(i);

                if (IsAt(pt, element))
                {
                    result.Add(pt);
                }
            }

            return result;
        }

        public List<Point> GetWalls()
        {
            return Get(Element.WALL);
        }

        public List<Point> GetDestroyableWalls()
        {
            return Get(Element.DESTROYABLE_WALL);
        }

        public List<Point> GetBombs()
        {
            return Get(Element.BOMB_TIMER_1)
                .Concat(Get(Element.BOMB_TIMER_2))
                .Concat(Get(Element.BOMB_TIMER_3))
                .Concat(Get(Element.BOMB_TIMER_4))
                .Concat(Get(Element.BOMB_TIMER_5))
                .Concat(Get(Element.BOMB_BOMBERMAN))
                .Concat(Get(Element.OTHER_BOMB_BOMBERMAN))
                .ToList();
        }

        public List<Point> GetPerks()
        {
            return Get(Element.BOMB_BLAST_RADIUS_INCREASE)
                .Concat(Get(Element.BOMB_COUNT_INCREASE))
                .Concat(Get(Element.BOMB_IMMUNE))
                .Concat(Get(Element.BOMB_REMOTE_CONTROL))
                .ToList();
        }

        public List<Point> GetBlasts()
        {
            return Get(Element.BOOM);
        }
        public List<Point> GetBlasts_1(int wave)
        {
            return GetFutureBlasts(Get(Element.BOMB_TIMER_1).ToList(), wave);
        }
        public List<Point> GetBlasts_2(int wave)
        {
            return GetFutureBlasts(Get(Element.BOMB_TIMER_2).ToList(), wave);
        }
        public List<Point> GetBlasts_5(int wave)
        {
            return GetFutureBlasts(Get(Element.BOMB_TIMER_5).ToList(), wave);
        }
        public List<Point> GetFutureBlasts(List<Point> bombs = null, int wave = 3)
        { 
            if (bombs == null)
            {
                bombs = GetBombs();
            }
            var result = new List<Point>();
            bool leftFlag, rightFlag, topFlag, bottomFlag;
            for(int j = 0; j < bombs.Count; j++)
            {
                leftFlag = rightFlag = topFlag = bottomFlag = true;
                result.Add(bombs[j]);
                for (int i = 1; i <= wave; i++)
                {
                    if (this.GetAt(bombs[j].ShiftLeft(i)) == Element.WALL || this.GetAt(bombs[j].ShiftLeft(i)) == Element.DESTROYABLE_WALL)
                    {
                        leftFlag = false;
                    }
                    if (this.GetAt(bombs[j].ShiftRight(i)) == Element.WALL || this.GetAt(bombs[j].ShiftRight(i)) == Element.DESTROYABLE_WALL)
                    {
                        rightFlag = false;
                    }
                    if (this.GetAt(bombs[j].ShiftTop(i)) == Element.WALL || this.GetAt(bombs[j].ShiftTop(i)) == Element.DESTROYABLE_WALL)
                    {
                        topFlag = false;

                    }
                    if (this.GetAt(bombs[j].ShiftBottom(i)) == Element.WALL || this.GetAt(bombs[j].ShiftBottom(i)) == Element.DESTROYABLE_WALL)
                    {
                        bottomFlag = false;
                    }

                    if (!bombs[j].ShiftLeft(i).IsOutOf(Size) && leftFlag)
                    {
                        result.Add(bombs[j].ShiftLeft(i));
                    }
                    if (!bombs[j].ShiftRight(i).IsOutOf(Size) && rightFlag)
                    {
                        result.Add(bombs[j].ShiftRight(i));
                    }
                    if (!bombs[j].ShiftBottom(i).IsOutOf(Size) && bottomFlag)
                    {
                        result.Add(bombs[j].ShiftBottom(i));
                    }
                    if (!bombs[j].ShiftTop(i).IsOutOf(Size) && topFlag)
                    {
                        result.Add(bombs[j].ShiftTop(i));
                    }
                }
            }

            return result.ToList();
        }

        public bool IsAnyOfAt(Point point, params Element[] elements)
        {
            return elements.Any(elem => IsAt(point, elem));
        }

        public bool IsNear(Point point, Element element, int near = 1)
        {
            if (point.IsOutOf(Size))
                return false;

            for (int i = 1; i <= near; i++)
            {
                if (IsAt(point.ShiftLeft(i), element))
                {
                    return true;
                }
                if (IsAt(point.ShiftRight(i), element))
                {
                    return true;
                }
                if (IsAt(point.ShiftTop(i), element))
                {
                    return true;

                }
                if (IsAt(point.ShiftBottom(i), element))
                {
                    return true;

                }
            }
            return true;
        }

        public bool IsBarrierAt(Point point)
        {
            return GetBarrier().Contains(point);
        }
        public int CountNear(Point point, Element element)
        {
            if (point.IsOutOf(Size))
                return 0;

            int count = 0;
            if (IsAt(point.ShiftLeft(), element)) count++;
            if (IsAt(point.ShiftRight(), element)) count++;
            if (IsAt(point.ShiftTop(), element)) count++;
            if (IsAt(point.ShiftBottom(), element)) count++;
            return count;
        }
    }
}
