using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.II
{
    public static class ResolvingStater
    {
        
        public static bool IsCloseTo_On(Point sub, Point main, int closeVal)
        {
            for (int i = 0; i <= closeVal; i++)
            {
                if (
                main.Equals(sub)
                ||
                main.ShiftLeft(i).Equals(sub)
                ||
                main.ShiftRight(i).Equals(sub)
                ||
                main.ShiftTop(i).Equals(sub)
                ||
                main.ShiftBottom(i).Equals(sub)
                ||
                main.ShiftLeft(i).ShiftTop().Equals(sub)
                ||
                main.ShiftLeft(i).ShiftBottom().Equals(sub)
                ||
                main.ShiftRight(i).ShiftTop().Equals(sub)
                ||
                main.ShiftRight(i).ShiftBottom().Equals(sub)
                )
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsCloseTo(Point sub, Point main)
        {
            if (
                main.Equals(sub)
                ||
                main.ShiftLeft().Equals(sub)
                ||
                main.ShiftRight().Equals(sub)
                ||
                main.ShiftTop().Equals(sub)
                ||
                main.ShiftBottom().Equals(sub)
                ||
                main.ShiftLeft().ShiftTop().Equals(sub)
                ||
                main.ShiftLeft().ShiftBottom().Equals(sub)
                ||
                main.ShiftRight().ShiftTop().Equals(sub)
                ||
                main.ShiftRight().ShiftBottom().Equals(sub)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsCloseTo(List<Point> sub, Point main)
        {
            for (int i = 0; i < sub.Count; i++)
            {


                if (
                    main.Equals(sub[i])
                    ||
                    main.ShiftLeft().Equals(sub[i])
                    ||
                    main.ShiftRight().Equals(sub[i])
                    ||
                    main.ShiftTop().Equals(sub[i])
                    ||
                    main.ShiftBottom().Equals(sub[i])
                    ||
                    main.ShiftLeft().ShiftTop().Equals(sub[i])
                    ||
                    main.ShiftLeft().ShiftBottom().Equals(sub[i])
                    ||
                    main.ShiftRight().ShiftTop().Equals(sub[i])
                    ||
                    main.ShiftRight().ShiftBottom().Equals(sub[i])
                    )
                {
                    return true;
                }
            }
                return false;
        }
        public static bool IsOnLineWith(Point sub, Point main)
        {
            if (sub.X == main.X || sub.Y == main.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsCloseOnLine(Point sub, Point main, int closeValue)
        {
            if (sub.X == main.X)
            {
                if (ModeledDifference(sub.Y, main.Y) <= closeValue)
                {
                    return true;
                }
            }
            if (sub.Y == main.Y)
            {
                if (ModeledDifference(sub.X, main.X) <= closeValue)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasCloseOnLineWith(List<Point> sub, Point main, int closeValue)
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

        public static int ModeledDifference(int first, int second)
        {
            if (first > second)
            {
                return first - second;
            }
            return second - first; 
        }
    }
}
