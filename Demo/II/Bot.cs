using Bomberman.Api;
using Demo.II;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Bot
    {
        public MovementMask movementMask;
        public DangerMask dangerMask;
        public BenefitMask benefitMask;
        private MaskBuilder maskBuilder;
        private Point currentPoint;
        private KeyValuePair<Element, Point> currentAim;
        private bool destroyMode;
        public bool remoteControl;
        public bool noWay;
        int bombTimer = 0;
        public Bot()
        {
            destroyMode = false;
            currentAim = new KeyValuePair<Element, Point>(Element.Space, new Point(0, 0));
        }

        private List<Direction> ActBomb()
        {
            remoteControl = false;
            noWay = false;
            var res = new List<Direction>();
            if (bombTimer == 0)
            {
                bombTimer = 4;

                res.Add(Direction.Act);
                return res;
            }
            return res;
        }

        public List<Direction> NextStep(Board board)
        {
            if (bombTimer > 0)
            {
                bombTimer--;
            }
            int bombSize = 5;
            maskBuilder = new MaskBuilder(board);
            movementMask = new MovementMask(board, bombSize);
            currentPoint = board.GetBomberman();
            dangerMask = maskBuilder.GetDangerMask(movementMask, bombSize);
            benefitMask = maskBuilder.GetBenefitMask(movementMask, bombSize);
            List<Direction> directions = new List<Direction>();

            if (Kamikadze(board))
            {
                directions.AddRange(ActBomb());
                return directions;
            }

            if (destroyMode && benefitMask.IsClose(currentPoint, Element.DESTROYABLE_WALL))
            {
                directions.AddRange(ActBomb());
                currentAim = new KeyValuePair<Element, Point>(Element.Space, dangerMask.GetCloseSavePoint(currentPoint));
                directions.AddRange(movementMask.CalculateNextStep(currentAim.Value, true));
                Point temp = movementMask.NextPoint(directions, currentPoint);
                noWay = false;
                return directions;
            }
            else if (benefitMask.IsClose(currentPoint, Element.MEAT_CHOPPER))
            {
                directions.AddRange(ActBomb());
                currentAim = new KeyValuePair<Element, Point>(Element.Space, dangerMask.GetCloseSavePoint(currentPoint));
                directions.AddRange(movementMask.CalculateNextStep(currentAim.Value, true));
                Point temp = movementMask.NextPoint(directions, currentPoint);
                return directions;
            }
            else if(ResolvingStater.HasCloseOnLineWith(board.GetOtherBombermans(), currentPoint, 4))
            {
                directions.AddRange(ActBomb());
                currentAim = new KeyValuePair<Element, Point>(Element.Space, dangerMask.GetCloseSavePoint(currentPoint));
                directions.AddRange(movementMask.CalculateNextStep(currentAim.Value, true));
                Point temp = movementMask.NextPoint(directions, currentPoint);
                return directions;
            }
            if (dangerMask.IsOnBlast(currentPoint) || dangerMask.IsOnBomb(currentPoint))
            {
                currentAim = new KeyValuePair<Element, Point>(Element.Space, dangerMask.GetCloseSavePoint(currentPoint));
                directions.AddRange(movementMask.CalculateNextStep(currentAim.Value, true));
                Point temp = movementMask.NextPoint(directions, currentPoint);
                return directions;
            }



            if (ResolvingStater.HasCloseOnLineWith(board.GetMeatChoppers(), currentPoint, 2))
            {
                Point savePoint = dangerMask.GetCloseSavePoint(currentPoint);
                currentAim = new KeyValuePair<Element, Point>(Element.Space, savePoint);
                directions.AddRange(ActBomb());
            }
            else if (!noWay && benefitMask.HaveBonuses())
            {
                Point closest = benefitMask.GetClosest(benefitMask.GetBonusesAsElement());
                currentAim = new KeyValuePair<Element, Point>(board.GetAt(closest), closest);

            }
            else if (!noWay && benefitMask.HaveOtherBombers())
            {
                currentAim = new KeyValuePair<Element, Point>(Element.OTHER_BOMBERMAN, benefitMask.GetClosestBomberman());
            }
            else if (noWay/* || benefitMask.HaveDestroyableWalls()*/)
            {
                List<Element> temp = new List<Element>();
                temp.Add(Element.DESTROYABLE_WALL);
                Point closestWall = benefitMask.GetClosest(temp);
                currentAim = new KeyValuePair<Element, Point>(Element.DESTROYABLE_WALL, closestWall);
                destroyMode = true;
            }

            directions.AddRange(movementMask.CalculateNextStep(currentAim.Value));

            if (directions.Contains(Direction.Stop))
            {
                noWay = true;
            }

            //Element next = board.GetAt(movementMask.NextPoint(directions, currentPoint));
            //if (next == Element.DESTROYABLE_WALL)
            //{
            //    directions.Clear();
            //    directions.AddRange(ActBomb());
            //    directions.AddRange(movementMask.CalculateNextStep(currentAim.Value, true));
            //    return directions;
            //}

            if (benefitMask.IsNextElement(directions, Element.BOMB_REMOTE_CONTROL))
            {
                remoteControl = true;
            }
            return directions;


            //Point newSavePoint = dangerMask.GetCloseSavePoint(currentPoint);

            //if (newSavePoint != currentAim.Value && (dangerMask.IsOnBlast(currentPoint) || dangerMask.IsOnBomb(currentPoint)))
            //{
            //    bot = BotConfiguration.Hide;
            //    currentAim = new KeyValuePair<Element, Point>(Element.Space, newSavePoint);
            //}
            //else if (bot == BotConfiguration.Earn || bot == BotConfiguration.Defend)
            //{
            //    if (ResolvingStater.HasCloseOnLineWith(board.GetOtherBombermans(), currentPoint, 1)
            //       || ResolvingStater.HasCloseOnLineWith(board.GetMeatChoppers(), currentPoint, 1))
            //    {
            //        directions.AddRange(ActBomb());
            //        bot = BotConfiguration.Hide;
            //    }
            //    else if (currentAim.Key == Element.DESTROYABLE_WALL && ResolvingStater.IsCloseOnLine(currentPoint, currentAim.Value, 1))
            //    {
            //        directions.AddRange(ActBomb());
            //        bot = BotConfiguration.Hide;
            //    }
            //}
            //else if (board.GetAt(currentAim.Value) == currentAim.Key)
            //{
            //    if (board.GetAt(currentAim.Value) == currentAim.Key && bombTimer == 0)
            //    {
            //        bot = BotConfiguration.Earn;
            //    }
            //}


            //if (bot == BotConfiguration.Earn && !isActiveAim)
            //{
            //    if (benefitMask.HaveBonuses())
            //    {
            //        Point closest = benefitMask.GetClosest(benefitMask.GetBonusesAsElement());
            //        currentAim = new KeyValuePair<Element, Point>(board.GetAt(closest), closest);
            //    }
            //    else if (benefitMask.HaveDestroyableWalls())
            //    {
            //        List<Element> temp = new List<Element>();
            //        temp.Add(Element.DESTROYABLE_WALL);
            //        Point closestWall = benefitMask.GetClosest(temp);
            //        currentAim = new KeyValuePair<Element, Point>(Element.DESTROYABLE_WALL, closestWall);
            //    }
            //}
            //if (bot == BotConfiguration.Hide)
            //{
            //    Point savePoint = dangerMask.GetCloseSavePoint(currentPoint);
            //    currentAim = new KeyValuePair<Element, Point>(Element.Space, savePoint);
            //}
            //else if (bot == BotConfiguration.Attack)
            //{
            //    List<Element> temp = new List<Element>();
            //    temp.Add(Element.MEAT_CHOPPER);
            //    temp.Add(Element.OTHER_BOMBERMAN);
            //    temp.Add(Element.OTHER_BOMB_BOMBERMAN);
            //    Point closest = benefitMask.GetClosest(temp);
            //    currentAim = new KeyValuePair<Element, Point>(board.GetAt(closest), closest);
            //}
            //else if (bot == BotConfiguration.Defend)
            //{

            //    directions.Add(Direction.Act);
            //}


            //directions.AddRange(movementMask.CalculateNextStep(currentAim.Value));
            //Console.SetCursorPosition(50, 11);
            //Console.WriteLine(bot);
            //Console.WriteLine("My Pos:{0}", currentPoint);
            //Console.SetCursorPosition(50, 12);
            //Console.WriteLine("Aim Pos:{0}", currentAim.Value);
            //for (int i = 0; i < directions.Count; i++)
            //{
            //    Console.SetCursorPosition(50, 12 + i);
            //    Console.WriteLine(directions[i]);

            //}








            //if (bot == BotConfiguration.Hide)
            //{
            //    directions.AddRange(movementMask.CalculateNextStep(currentAim.Value));
            //    return directions;
            //}

            //do
            //{
            //    isChangingConfiguration = false;
            //    if (bot == BotConfiguration.Earn)
            //    {
            //        if (isActiveAim && currentPoint == currentAim.Value)
            //        {
            //            bot = BotConfiguration.Attack;
            //            isChangingConfiguration = true;
            //            isActiveAim = false;
            //        }
            //        if (isActiveAim && board.GetAt(currentAim.Value) != currentAim.Key)
            //        {
            //            isActiveAim = false;
            //        }
            //        if (!isActiveAim && !isChangingConfiguration && benefitMask.HaveBonuses())
            //        {
            //            Point closest = benefitMask.GetClosest(benefitMask.GetBonusesAsElement());
            //            currentAim = new KeyValuePair<Element, Point>(board.GetAt(closest), closest);
            //            isActiveAim = true;
            //        }
            //        else
            //        {
            //            if (!isActiveAim && !isChangingConfiguration)
            //            {
            //                bot = BotConfiguration.Attack;
            //                isChangingConfiguration = true;
            //            }
            //        }
            //    }
            //    else if (bot == BotConfiguration.Attack)
            //    {

            //        if (isActiveAim && movementMask.IfClose(currentPoint, currentAim.Value))
            //        {
            //            directions.Add(Direction.Act);
            //            bot = BotConfiguration.Hide;
            //            isChangingConfiguration = true;
            //            isActiveAim = false;
            //        }
            //        if (isActiveAim && movementMask.IfClose(currentPoint, currentAim.Value))
            //        {
            //            directions.Add(Direction.Act);
            //            bot = BotConfiguration.Hide;
            //            isChangingConfiguration = true;
            //            isActiveAim = false;
            //        }


            //        if (!isActiveAim && !isChangingConfiguration && benefitMask.HaveDestroyableWalls())
            //        {
            //            List<Element> temp = new List<Element>();
            //            temp.Add(Element.DESTROYABLE_WALL);
            //            Point closestWall = benefitMask.GetClosest(temp);
            //            currentAim = new KeyValuePair<Element, Point>(Element.DESTROYABLE_WALL, closestWall);
            //            isActiveAim = true;
            //        }
            //        else if (!isActiveAim && !isChangingConfiguration && benefitMask.HaveMeatChopers())
            //        {
            //            List<Element> temp = new List<Element>();
            //            temp.Add(Element.MEAT_CHOPPER);
            //            Point closestChopper = benefitMask.GetClosest(temp);
            //            currentAim = new KeyValuePair<Element, Point>(Element.MEAT_CHOPPER, closestChopper);
            //            isActiveAim = true;
            //        }
            //        else
            //        {
            //            if (!isActiveAim && !isChangingConfiguration)
            //            {
            //                bot = BotConfiguration.Earn;
            //                isChangingConfiguration = true;
            //            }
            //        }


            //    }
            //    else if (bot == BotConfiguration.Hide)
            //    {
            //        if (isActiveAim && currentPoint == currentAim.Value)
            //        {
            //            isActiveAim = false;
            //            bot = BotConfiguration.Earn;
            //            isChangingConfiguration = true;
            //        }

            //        if (!isActiveAim && !isChangingConfiguration)
            //        {
            //            Point savePoint = dangerMask.GetCloseSavePoint(currentPoint);
            //            currentAim = new KeyValuePair<Element, Point>(Element.Space, savePoint);
            //            isActiveAim = true;
            //        }
            //    }
            //    else if (bot == BotConfiguration.Run)
            //    {

            //    }

            //    if (!isActiveAim && directions.Contains(Direction.Act))
            //    {
            //        isChangingConfiguration = true;
            //        isActiveAim = false;
            //        bot = BotConfiguration.Hide;
            //    }
            //} while (isChangingConfiguration);


            //directions.AddRange(movementMask.CalculateNextStep(currentAim.Value));

            //if (dangerMask.IsDangerOnPoint(currentPoint))
            //{
            //    List<Direction> tempDirections = dangerMask.ReactOnDanger();
            //    return tempDirections;
            //}

            //List<Direction> directions = new List<Direction>();

            //for (int i = 0; i < maxBenefitTries; i++)
            //{
            //    Point benefitPoint = benefitMask.LookForBenefit();


            //    if (dangerMask.IsDangerOnPoint(movementMask.nextPoint))
            //    {
            //        return dangerMask.ReactOnDanger();
            //    }
            //    //else
            //    //{
            //    directions.AddRange(tempDirections);
            //        return directions;
            //    //}
            //}

            return directions;
        }


        private bool Kamikadze(Board board)
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

        //    if ((board.IsAt(currentPoint.ShiftRight(), Element.WALL) || board.IsAt(currentPoint.ShiftRight(), Element.DESTROYABLE_WALL))
        //    && (board.IsAt(currentPoint.ShiftLeft(), Element.WALL) || board.IsAt(currentPoint.ShiftLeft(), Element.DESTROYABLE_WALL))
        //    && (board.IsAt(currentPoint.ShiftTop(), Element.WALL) || board.IsAt(currentPoint.ShiftTop(), Element.DESTROYABLE_WALL))
        //    && (board.IsAt(currentPoint.ShiftBottom(), Element.WALL) || board.IsAt(currentPoint.ShiftBottom(), Element.DESTROYABLE_WALL)))
        //{
        //}

        private bool IsCloseTo(Point sub, Point main)
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


    }


}
