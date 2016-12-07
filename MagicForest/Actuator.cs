using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public static class Actuator
    {
        /*
        public static void TurnLelf(Hero p_hHero)
        {
            int iCurrentDirectionIndex = p_hHero.m_asPossibleDirections.ToList().IndexOf(p_hHero.DirectionFacing);
            int iNewDirection = (iCurrentDirectionIndex - 1) < 0 ? iCurrentDirectionIndex + 3 : iCurrentDirectionIndex - 1;
            p_hHero.DirectionFacing = p_hHero.m_asPossibleDirections[iNewDirection];
            //p_hHero.Score--;
        }

        public static void TurnRight(Hero p_hHero)
        {
            int iCurrentDirectionIndex = p_hHero.m_asPossibleDirections.ToList().IndexOf(p_hHero.DirectionFacing);
            p_hHero.DirectionFacing = p_hHero.m_asPossibleDirections[(iCurrentDirectionIndex + 1) % 4];
            //p_hHero.Score--;
        }

        public static void GoForward(Hero p_hHero)
        {
            p_hHero.CurrentCell = p_hHero.getFrontCell();
            p_hHero.CurrentCell.AlreadyVisited = true;
            p_hHero.Score--;
        }

        public static void GoBackward(Hero p_hHero)
        {
            p_hHero.CurrentCell = p_hHero.getBackCell();
            p_hHero.CurrentCell.AlreadyVisited = true;
            p_hHero.Score--;
        }
        */

        public static void Move(Hero p_hHero, ForestCell p_fcDestinationCell)
        {
            int cost = 1/*CalculateCost(p_hHero.CurrentCell, p_fcDestinationCell)*/;
            p_hHero.CurrentCell = p_fcDestinationCell;
            p_hHero.CurrentCell.AlreadyVisited = true;
            p_hHero.Score-= cost;
        }

        /*
        public static int CalculateCost(ForestCell p_fcStartingCell, ForestCell p_fcDestinationCell)
        {

        }

        private List<ForestCell> GetAdjacentSafeCells(Hero p_hHero, ForestCell p_fcCell)
        {
            List<ForestCell> fcResult = new List<ForestCell>();
            List<ForestCell> fcNeighboorsList = p_fcCell.getAdjacentCells();
            foreach (ForestCell fcItem in fcNeighboorsList)
            {
                if (p_fcCell.Closed)
                {
                    continue;
                }
                if (p_hHero.CellsOK.Contains(fcItem) || p_hHero.CellsSuspicous.Contains(fcItem))
                {
                    fcResult.Add(fcItem);
                }
            }
            return fcResult;
        }

        private static int CalculateCost(ForestCell p_fcCell, ForestCell p_fcParentCell)
        {

            return 1;
        }

        private static bool Search(Hero p_hHero, ForestCell p_fcCell)
        {
            p_fcCell.Closed = true;
            List<ForestCell> nextCells = GetAdjacentSafeCells(p_hHero, p_fcCell);
            nextCells.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextCells)
            {
                if (Search(nextNode)) // Note: Recurses back into Search(Node)
                    return true;
            }
            return false;
        }
        */

        public static void ThrowRock(Hero p_hHero, string direction)
        {
            ForestCell fcTargetCell = p_hHero.getFrontCell();
            fcTargetCell.HasMonster = false;

            List<ForestCell> lfcAdjacentForectCells = fcTargetCell.getAdjacentCells();
            foreach (ForestCell fcForectCell in lfcAdjacentForectCells)
            {
                fcForectCell.HasPoop = false;
            }
            p_hHero.Score -= 10;
        }

        public static void Exit(Hero p_hHero)
        {
            //TODO...
        }

    }
}
