using Bomberman.Api;
using Demo.MarkSerion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.II
{
    public class Mark1
    {
        private Board board;

        private DangerCoordinator dangerCoordinator;

        private Coordinator coordinator;

        private KeyValuePair<Element, Point> wallFinder;

        private bool remoteControl = false;

        private int remoteControlCount = 0;

        private int incrCount = 1;

        private Point mist = new Point(-1, -1);

        private int bombWaveSize = 3;

        private Point currentPoint;

        public Mark1()
        {
            wallFinder = new KeyValuePair<Element, Point>(Element.BOOM, new Point(0, 0));
        }

        public List<Direction> ActivateBomb()
        {
            if (remoteControlCount == 0)
            {
                remoteControl = false;
            }
            else
            {
                remoteControlCount--;
            }
            List<Direction> directions = new List<Direction>();
            directions.Add(Direction.Act);
            Point savePoint = dangerCoordinator.GetCloseSavePoint(currentPoint);
            wallFinder = new KeyValuePair<Element, Point>(Element.Space, savePoint);
            return directions;
        }

        private bool Kamikadze(Point currentPoint)
        {
            if ((board.IsAt(currentPoint.ShiftRight(), Element.WALL) || board.IsAt(currentPoint.ShiftRight(), Element.DESTROYABLE_WALL))
            && (board.IsAt(currentPoint.ShiftLeft(), Element.WALL) || board.IsAt(currentPoint.ShiftLeft(), Element.DESTROYABLE_WALL))
            && (board.IsAt(currentPoint.ShiftTop(), Element.WALL) || board.IsAt(currentPoint.ShiftTop(), Element.DESTROYABLE_WALL))
            && (board.IsAt(currentPoint.ShiftBottom(), Element.WALL) || board.IsAt(currentPoint.ShiftBottom(), Element.DESTROYABLE_WALL)))
            {
                return true;
            }
            return false;
        }


        
        public List<Direction> GetStep(Board board)
        {
            this.board = board;
            dangerCoordinator = new DangerCoordinator(board, bombWaveSize);
            coordinator = new Coordinator(board, dangerCoordinator);
            currentPoint = board.GetBomberman();
            Point savePoint;
            List<Point> benefits;
            List<Direction> directions = new List<Direction>();
            List<Element> otherPlayerElements = new List<Element>();
            List<Element> walls = new List<Element>();
            List<Element> bonuses = new List<Element>();
            bonuses.Add(Element.BOMB_BLAST_RADIUS_INCREASE);
            bonuses.Add(Element.BOMB_COUNT_INCREASE);
            bonuses.Add(Element.BOMB_COUNT_INCREASE);
            bonuses.Add(Element.BOMB_IMMUNE);
            bonuses.Add(Element.BOMB_REMOTE_CONTROL);
            otherPlayerElements.Add(Element.OTHER_BOMBERMAN);
            otherPlayerElements.Add(Element.MEAT_CHOPPER);
            walls.Add(Element.DESTROYABLE_WALL);
            if (Kamikadze(currentPoint))
            {
                directions.Add(Direction.Act);
                return directions;
            }

            if (incrCount > 1)
            {
                directions.Add(Direction.Act);
            }

            if (dangerCoordinator.blasts.Contains(currentPoint) || coordinator.IsCloseCircly(currentPoint, Element.MEAT_CHOPPER))
            {   
                savePoint = dangerCoordinator.GetCloseSavePoint(currentPoint);
                if (savePoint == mist)
                {

                    directions.AddRange(ActivateBomb());
                    return directions;
                }
                var res = coordinator.CalculateNextStep(savePoint, true);
                if (remoteControl && coordinator.IsCloseFrontal(currentPoint, savePoint))
                {
                    res.Add(Direction.Act);
                }
                directions.AddRange(res);
                if (directions.Contains(Direction.Stop))
                {
                    directions.Remove(Direction.Stop);
                    directions.AddRange(GetWallAsPoint());

                }
                if (coordinator.IsCloseCircly(currentPoint, Element.MEAT_CHOPPER))
                {
                    directions.AddRange(ActivateBomb());
                }
                return directions;
            }
            else
            {
                if (remoteControl)
                {
                    directions.AddRange(ActivateBomb());
                }
            }

            if (coordinator.IsOnLineWith(otherPlayerElements, currentPoint, bombWaveSize))
            {
                directions.AddRange(KillPlayer());
            }
            else if (coordinator.IsOnLineWith(otherPlayerElements, currentPoint, bombWaveSize))
            {
                directions.AddRange(KillPlayer());
            }
            else if (coordinator.IsOnLineWith(Element.DESTROYABLE_WALL, currentPoint, 2, 2))
            {
                directions.AddRange(ActivateBomb());
            }
            else if (coordinator.IsCloseFrontal(currentPoint, Element.DESTROYABLE_WALL, 2))
            {
                directions.AddRange(ActivateBomb());
            }
            else if (coordinator.IsCloseFrontal(currentPoint, Element.DESTROYABLE_WALL))
            {
                directions.AddRange(ActivateBomb());
            }
            
            if (BenefitCoordinator.TryGetBonuses(board, out benefits))
            {

                var res = coordinator.CalculateNextStep(coordinator.GetClosest(bonuses), false);
                directions.AddRange(res);
            }
            else if (BenefitCoordinator.TryGetBombermans(board, out benefits))
            {
                if (coordinator.IsOnLineWith(walls, currentPoint, 2))
                {
                    directions.AddRange(GetWallAsPoint());
                }
                else
                {
                    directions.AddRange(GetWallAsPoint());
                    if (directions.Contains(Direction.Stop))
                    {
                        Point closest = benefits[benefits.Count - 1];
                        if (closest == new Point(0, 0))
                        {
                            directions.AddRange(GetWallAsPoint());
                        }
                        var res = coordinator.CalculateNextStep(closest, false);
                        directions.AddRange(res);
                        if (res.Contains(Direction.Stop))
                        {
                            directions.Clear();
                            directions.Add(Direction.Act);
                        }
                    }
                   
                }
            }

            else
            {
                directions.AddRange(GetWallAsPoint());
            }





            Point nextPoint = coordinator.GetPointByDirections(directions);


        






            if (dangerCoordinator.DangerCheckOnPoint(nextPoint))
            {
                directions.Clear();
                savePoint = dangerCoordinator.GetCloseSavePoint(currentPoint);
                wallFinder = new KeyValuePair<Element, Point>(Element.Space, savePoint);
                if (savePoint == mist)
                {
                    directions.Add(Direction.Act);
                    return directions;
                }
                var res = coordinator.CalculateNextStep(savePoint, false);
                if (remoteControl && coordinator.IsCloseFrontal(currentPoint, savePoint))
                {
                    directions.AddRange(ActivateBomb());
                }
                directions.AddRange(res);
            }
            else
            {
                if (remoteControl && nextPoint == wallFinder.Value)
                {
                    directions.AddRange(ActivateBomb());    
                }
            }

            if (directions.Contains(Direction.Stop))
            {
                directions.Clear();
                directions.AddRange(GetWallAsPoint());
            }

            if (coordinator.IsOnLineWith(bonuses, currentPoint, bombWaveSize))
            {
                directions.Remove(Direction.Act);

            }
            if (coordinator.IsCloseFrontal(nextPoint, Element.BOMB_REMOTE_CONTROL))
            {
                directions.Remove(Direction.Act);
                remoteControl = true;
                remoteControlCount = 3;
            }
            else if (coordinator.IsCloseFrontal(nextPoint, Element.BOMB_COUNT_INCREASE))
            {
                directions.Remove(Direction.Act);
                incrCount++;
            }
            else if (coordinator.IsCloseFrontal(nextPoint, Element.BOMB_BLAST_RADIUS_INCREASE))
            {
                directions.Remove(Direction.Act);
                bombWaveSize += 2;
            }
            return directions;

            //if (dangerCoordinator.blasts.Contains(currentPoint) || coordinator.IsCloseCircly(currentPoint, Element.MEAT_CHOPPER) || coordinator.IsOnLineWith(Element.MEAT_CHOPPER, currentPoint, 3))
            //{
            //    directions.Clear();
            //    savePoint = dangerCoordinator.GetCloseSavePoint(currentPoint);
            //    if (savePoint == mist)
            //    {
            //        directions.Add(Direction.Act);
            //        return directions;
            //    }
            //    wallFinder = new KeyValuePair<Element, Point>(Element.Space, savePoint/*coordinator.GetClosest(otherPlayerElements)*/);
            //    var res = coordinator.CalculateNextStep(savePoint, true);
            //    if (remoteControl && coordinator.IsCloseFrontal(currentPoint, savePoint))
            //    {
            //        res.Add(Direction.Act);
            //    }
            //    directions.AddRange(res);
            //    if (directions.Contains(Direction.Stop))
            //    {
            //        List<Element> elements = new List<Element>();
            //        elements.Add(Element.DESTROYABLE_WALL);
            //        if (board.GetAt(wallFinder.Value) != wallFinder.Key)
            //        {
            //            wallFinder = new KeyValuePair<Element, Point>(Element.DESTROYABLE_WALL, dangerCoordinator.destroyableWalls[dangerCoordinator.destroyableWalls.Count - 1]);
            //        }
            //        directions.AddRange(coordinator.CalculateNextStep(wallFinder.Value, true));

            //    }
            //    return directions;
            //}
            //else if (BenefitCoordinator.TryGetBonuses(board, out benefits))
            //{
            //    if (coordinator.IsOnLineWith(otherPlayerElements, currentPoint, bombWaveSize))
            //    {
            //        directions.AddRange(KillPlayer());
            //    }
            //    var res = coordinator.CalculateNextStep(coordinator.GetClosest(bonuses), false);
            //    directions.AddRange(res);
            //}
            //else if (BenefitCoordinator.TryGetBombermans(board, out benefits))
            //{
            //    if (coordinator.IsOnLineWith(otherPlayerElements, currentPoint, bombWaveSize))
            //    {
            //        directions.AddRange(KillPlayer());
            //    }
            //    else if (coordinator.IsOnLineWith(Element.DESTROYABLE_WALL, currentPoint, 1))
            //    {
            //        savePoint = dangerCoordinator.GetCloseSavePointFrom(currentPoint, dangerCoordinator.GenerateBlastFrom(currentPoint, bombWaveSize));

            //        wallFinder = new KeyValuePair<Element, Point>(Element.OTHER_BOMBERMAN, coordinator.GetClosest(otherPlayerElements)/*coordinator.getclosest(otherplayerelements)*/);
            //        directions.Add(Direction.Act);
            //        directions.AddRange(coordinator.CalculateNextStep(wallFinder.Value, true));
            //    }
            //    else
            //    {
            //        List<Element> elements = new List<Element>();
            //        elements.Add(Element.DESTROYABLE_WALL);
            //        if (board.GetAt(wallFinder.Value) != wallFinder.Key)
            //        {
            //            wallFinder = new KeyValuePair<Element, Point>(Element.DESTROYABLE_WALL, coordinator.GetClosest(elements));
            //        }
            //        directions.AddRange(coordinator.CalculateNextStep(wallFinder.Value, false));
            //    }

            //}
            //else
            //{
            //    if (coordinator.IsOnLineWith(Element.DESTROYABLE_WALL, currentPoint, 2))
            //    {
            //        savePoint = dangerCoordinator.GetCloseSavePointFrom(currentPoint, dangerCoordinator.GenerateBlastFrom(currentPoint, 5));
            //        wallFinder = new KeyValuePair<Element, Point>(Element.Space, savePoint/*coordinator.GetClosest(otherPlayerElements)*/);
            //        directions.Add(Direction.Act);
            //        directions.AddRange(coordinator.CalculateNextStep(savePoint, true));
            //    }
            //    else
            //    {
            //        List<Element> elements = new List<Element>();
            //        elements.Add(Element.DESTROYABLE_WALL);
            //        if (board.GetAt(wallFinder.Value) != wallFinder.Key)
            //        {
            //            wallFinder = new KeyValuePair<Element, Point>(Element.DESTROYABLE_WALL, coordinator.GetClosest(elements));
            //        }
            //        directions.AddRange(coordinator.CalculateNextStep(wallFinder.Value, false));
            //    }
            //}
            //Point nextPoint = coordinator.GetPointByDirections(directions);


            //if (coordinator.IsCloseFrontal(nextPoint, Element.BOMB_REMOTE_CONTROL))
            //{
            //    remoteControl = true;
            //    remoteControlCount = 3;
            //}
            //else if (coordinator.IsCloseFrontal(nextPoint, Element.BOMB_COUNT_INCREASE))
            //{
            //    incrCount++;
            //}
            //else if (coordinator.IsCloseFrontal(nextPoint, Element.BOMB_BLAST_RADIUS_INCREASE))
            //{
            //    bombWaveSize += 2;
            //}

            //else if (incrCount > 1)
            //{
            //    directions.Add(Direction.Act);
            //}


           

            //if (dangerCoordinator.DangerCheckOnPoint(nextPoint))
            //{
            //    directions.Clear();
            //    savePoint = dangerCoordinator.GetCloseSavePoint(nextPoint);
            //    if (savePoint == mist)
            //    {
            //        directions.Add(Direction.Act);
            //        directions.Add(Direction.Act);
            //        return directions;
            //    }
            //    wallFinder = new KeyValuePair<Element, Point>(Element.Space, savePoint/*coordinator.GetClosest(otherPlayerElements)*/);
            //    var res = coordinator.CalculateNextStep(savePoint, false);
            //    if (remoteControl && coordinator.IsCloseFrontal(currentPoint, savePoint))
            //    {
            //        res.Add(Direction.Act);
            //    }
            //    directions.AddRange(res);
            //    return directions;
            //}

            ////if (remoteControl)
            ////{
            ////    if (remoteControlCount == 0)
            ////    {
            ////        remoteControl = false;
            ////    }
            ////    else
            ////    {
            ////        remoteControlCount--;
            ////    }
            ////    directions.Add(Direction.Act);
            ////}
            //return directions;
        }

        public List<Direction> KillPlayer()
        {
            List<Direction> directions = new List<Direction>();
            Point savePoint = dangerCoordinator.GetCloseSavePointFrom(currentPoint, dangerCoordinator.GenerateBlastFrom(currentPoint, 5));
            wallFinder = new KeyValuePair<Element, Point>(Element.Space, savePoint/*coordinator.GetClosest(otherPlayerElements)*/);
            directions.Add(Direction.Act);
            directions.AddRange(coordinator.CalculateNextStep(savePoint, true));
            return directions;
        }
        public List<Direction> GetWallAsPoint()
        {
            Point savePoint;
            List<Direction> directions = new List<Direction>();
            if (coordinator.IsCloseFrontal(currentPoint, Element.DESTROYABLE_WALL))
            {
                savePoint = dangerCoordinator.GetCloseSavePointFrom(currentPoint, dangerCoordinator.GenerateBlastFrom(currentPoint, 5));
                wallFinder = new KeyValuePair<Element, Point>(Element.Space, savePoint);
                directions.Add(Direction.Act);
                directions.AddRange(coordinator.CalculateNextStep(savePoint, true));
            }
            else
            {
                List<Element> elements = new List<Element>();
                elements.Add(Element.DESTROYABLE_WALL);
                wallFinder = new KeyValuePair<Element, Point>(Element.DESTROYABLE_WALL, coordinator.GetClosest(elements));
                directions.AddRange(coordinator.CalculateNextStep(wallFinder.Value, false));
            }
            return directions;
        }

    }
}
