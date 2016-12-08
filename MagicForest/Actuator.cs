using System;
using System.Collections.Generic;

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

        public delegate void dlgMove(ForestCell prevForestCell, ForestCell NewForestCell);
        public static dlgMove OnMove;

        public static void Move(Hero p_hHero, ForestCell p_fcDestinationCell)
        {
            //CalculateHCost(p_fcDestinationCell);
            int cost = 1/*CalculateCost(p_hHero.CurrentCell, p_fcDestinationCell)*/;

            p_hHero.PreviousForestCell = p_hHero.CurrentForestCell;
            p_hHero.CurrentForestCell = p_fcDestinationCell;
            p_hHero.CurrentMemoryCell = Hero.Memory[p_fcDestinationCell.LineIndex, p_fcDestinationCell.ColumnIndex];

            p_hHero.CurrentForestCell.AlreadyVisited = true;
            p_hHero.Score -= cost;

            OnMove(p_hHero.PreviousForestCell, p_hHero.CurrentForestCell);

            //ResetGridCost();
        }

        /*************************************************/

        public static void CalculateHCost(ForestCell p_fcDestinationCell)
        {
            for (int i = 0; i < MainWindow.ForestSize; i++)
            {
                for (int j = 0; j < MainWindow.ForestSize; j++)
                {
                    MainWindow.Forest[i, j].H = Math.Sqrt(Math.Pow(p_fcDestinationCell.LineIndex - i, 2) + Math.Pow(p_fcDestinationCell.ColumnIndex - j, 2));
                }
            }
        }

        public static void ResetGridCost()
        {
            for (int i = 0; i < MainWindow.ForestSize; i++)
            {
                for (int j = 0; j < MainWindow.ForestSize; j++)
                {
                    MainWindow.Forest[i, j].H = 0;
                    MainWindow.Forest[i, j].F = 0;
                    MainWindow.Forest[i, j].G = 0;
                    MainWindow.Forest[i, j].ParentCell = null;
                }
            }
        }

        private static List<ForestCell> GetAdjacentSafeCells(Hero p_hHero, ForestCell p_fcCell)
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
                    //float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    double gTemp = p_fcCell.G++;
                    if (gTemp < fcItem.G)
                    {
                        fcItem.ParentCell = p_fcCell;
                        fcResult.Add(fcItem);
                    }
                    //fcResult.Add(fcItem);
                }
                else
                {
                    fcItem.ParentCell = p_fcCell;
                    fcItem.Closed = false;
                    fcResult.Add(fcItem);
                }
            }
            return fcResult;
        }

        private static bool Search(Hero p_hHero, ForestCell p_fcStartingCell, ForestCell p_fcDestinationCell)
        {
            p_fcStartingCell.Closed = true;
            List<ForestCell> nextCells = GetAdjacentSafeCells(p_hHero, p_fcStartingCell);
            nextCells.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (ForestCell nextCell in nextCells)
            {
                if (nextCell.Equals(p_fcDestinationCell))
                {
                    return true;
                }
                else
                {
                    if (Search(p_hHero, nextCell, p_fcDestinationCell)) // Note: Recurses back into Search(Node)
                        return true;
                }
            }
            return false;
        }

        public static List<ForestCell> FindPath(Hero p_hHero, ForestCell p_fcDestinationCell)
        {
            List<ForestCell> path = new List<ForestCell>();
            bool success = Search(p_hHero, p_hHero.CurrentForestCell, p_fcDestinationCell);
            if (success)
            {
                ForestCell node = p_fcDestinationCell;
                while (node.ParentCell != null)
                {
                    path.Add(node);
                    node = node.ParentCell;
                }
                path.Reverse();
            }
            return path;
        }

        /*************************************************/

        public static void ThrowRockLeft(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.LineIndex - 1 > 0)
            {
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex - 1, p_hHero.CurrentForestCell.ColumnIndex];
                p_fcTarget.RemoveMonsterOnCell();
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoMonster = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].ContainMonster = -1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainMonster = -1;
                p_hHero.CellsOK.Add(p_fcTarget);
                p_hHero.Score -= 10;
            }
        }

        public static void ThrowRockRight(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.LineIndex + 1 > 0)
            {
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex + 1, p_hHero.CurrentForestCell.ColumnIndex];
                p_fcTarget.RemoveMonsterOnCell();
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoMonster = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].ContainMonster = -1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainMonster = -1;
                p_hHero.Score -= 10;
            }
        }

        public static void ThrowRockTop(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.ColumnIndex - 1 > 0)
            {
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex, p_hHero.CurrentForestCell.ColumnIndex - 1];
                p_fcTarget.RemoveMonsterOnCell();
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoMonster = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].ContainMonster = -1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainMonster = -1;
                p_hHero.Score -= 10;
            }
        }

        public static void ThrowRockBottom(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.ColumnIndex + 1 > 0)
            {
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex, p_hHero.CurrentForestCell.ColumnIndex + 1];
                p_fcTarget.RemoveMonsterOnCell();
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoMonster = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].ContainMonster = -1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainMonster = -1;
                p_hHero.Score -= 10;
            }
        }

        // New delegate event for exit (portal found)
        public delegate void dlgExit();
        public static dlgExit OnExit;

        public static void Exit(Hero p_hHero)
        {
            if (OnExit != null)
            {
                OnExit();
            }
        }
    }
}
