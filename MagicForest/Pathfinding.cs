using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public class Pathfinding
    {

        // A* search for path cost

        public static void InitCost(Hero p_hHero, ForestCell p_fcDestinationCell)
        {
            for (int i = 0; i < MainWindow.ForestSize; i++)
            {
                for (int j = 0; j < MainWindow.ForestSize; j++)
                {
                    MainWindow.Forest[i, j].H = Math.Sqrt(Math.Pow(p_fcDestinationCell.LineIndex - i, 2) + Math.Pow(p_fcDestinationCell.ColumnIndex - j, 2));
                    MainWindow.Forest[i, j].G = Math.Abs(p_fcDestinationCell.LineIndex - p_hHero.CurrentForestCell.LineIndex) +
                        Math.Abs(p_fcDestinationCell.ColumnIndex - p_hHero.CurrentForestCell.ColumnIndex);
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
                    MainWindow.Forest[i, j].Closed = false;
                }
            }
        }

        private static List<ForestCell> GetAdjacentSafeCells(Hero p_hHero, ForestCell p_fcCell)
        {
            List<ForestCell> fcResult = new List<ForestCell>();

            List<MemoryCell> fcNeighboorsList = Hero.Memory[p_fcCell.LineIndex, p_fcCell.ColumnIndex].getAdjacentMemoryCells();

            //List<ForestCell> fcNeighboorsList = p_fcCell.getAdjacentCells();
            foreach (MemoryCell mcItem in fcNeighboorsList)
            {
                if (MainWindow.Forest[mcItem.LineIndex, mcItem.ColumnIndex].Closed)
                {
                    continue;
                }
                if (mcItem.HasNoHole == 1 && mcItem.HasNoMonster == 1)
                {
                    double gTemp = p_fcCell.F;

                    ForestCell fcItem = MainWindow.Forest[mcItem.LineIndex, mcItem.ColumnIndex];

                    if (gTemp > fcItem.F)
                    {
                        fcItem.ParentCell = p_fcCell;
                        fcResult.Add(fcItem);
                    }
                }
                else
                {
                    ForestCell fcItem = MainWindow.Forest[mcItem.LineIndex, mcItem.ColumnIndex];
                    fcItem.ParentCell = p_fcCell;
                    fcItem.Closed = false;
                    fcResult.Add(fcItem);
                }
            }
            return fcResult.Distinct().ToList();
        }

        private static bool Search(Hero p_hHero, ForestCell p_fcStartingCell, ForestCell p_fcDestinationCell)
        {
            p_fcStartingCell.Closed = true;
            List<ForestCell> nextCells = GetAdjacentSafeCells(p_hHero, p_fcStartingCell);
            nextCells.Sort((cell1, cell2) => cell1.F.CompareTo(cell2.F));
            foreach (ForestCell nextCell in nextCells)
            {
                if (nextCell.Equals(p_fcDestinationCell))
                {
                    return true;
                }
                else
                {
                    if (Search(p_hHero, nextCell, p_fcDestinationCell))
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

    }
}
