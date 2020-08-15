using Bomberman.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.MarkSerion
{
    public static class BenefitCoordinator
    {
        public static bool TryGetBonuses(Board board, out List<Point> points)
        {
            points = new List<Point>();
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
                        points.Add(new Point(i, j));
                    }
                }
            }
            if (points.Count > 0)
            { return true; }
            return false;
        }

        public static bool TryGetBombermans(Board board, out List<Point> points)
        {
            points = new List<Point>();
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    Element temp = board.GetAt(new Point(i, j));
                    if (temp == Element.OTHER_BOMBERMAN)
                    {
                        points.Add(new Point(i, j));
                    }
                }
            }
            if (points.Count > 0)
            { return true; }
            return false;
        }


        public static bool TryGetWalls(Board board, out List<Point> points)
        {
            points = new List<Point>();
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
                        points.Add(new Point(i, j));
                    }
                }
            }
            if (points.Count > 0)
            { return true; }
            return false;
        }
    }
}
