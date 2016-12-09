using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicForest
{
    /// <summary>
    /// Pathfinding class to find the fastest safe path between 2 cells with A* search.
    /// </summary>
    public class Pathfinding
    {
        /// <summary>
        /// Init cell distance to destination cell.
        /// </summary>
        /// <param name="p_hHero"> hero. </param>
        /// <param name="p_fcDestinationCell"> Destination cell. </param>
        public static void InitCost(Hero p_hHero, ForestCell p_fcDestinationCell)
        {
            for (int i = 0; i < MainWindow.ForestSize; i++)
            {
                for (int j = 0; j < MainWindow.ForestSize; j++)
                {
                    MainWindow.Forest[i, j].Distance = Math.Sqrt(Math.Pow(p_fcDestinationCell.LineIndex - i, 2) + Math.Pow(p_fcDestinationCell.ColumnIndex - j, 2));
                }
            }
        }

        /// <summary>
        /// Reset cell value after search.
        /// </summary>
        public static void ResetGridCost()
        {
            for (int i = 0; i < MainWindow.ForestSize; i++)
            {
                for (int j = 0; j < MainWindow.ForestSize; j++)
                {
                    MainWindow.Forest[i, j].Distance = 0;
                    MainWindow.Forest[i, j].ParentCell = null;
                    MainWindow.Forest[i, j].Closed = false;
                }
            }
        }

        /// <summary>
        /// Get adjacent safe cells from a given cell.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        /// <param name="p_fcCell"> Current cell. </param>
        /// <returns> The safe neighboors. </returns>
        private static List<ForestCell> GetAdjacentSafeCells(Hero p_hHero, ForestCell p_fcCell)
        {
            List<ForestCell> fcResult = new List<ForestCell>();
            List<MemoryCell> fcNeighboorsList = Hero.Memory[p_fcCell.LineIndex, p_fcCell.ColumnIndex].getAdjacentMemoryCells();

            foreach (MemoryCell mcItem in fcNeighboorsList)
            {
                if (MainWindow.Forest[mcItem.LineIndex, mcItem.ColumnIndex].Closed)
                {
                    continue;
                }
                if (mcItem.HasNoHole == 1 && mcItem.HasNoAlien == 1)
                {
                    double fTemp = p_fcCell.Distance;

                    ForestCell fcItem = MainWindow.Forest[mcItem.LineIndex, mcItem.ColumnIndex];

                    // If new distance is less than distance found before.
                    if (fTemp > fcItem.Distance)
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

        /// <summary>
        /// Recursive search to find the path.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        /// <param name="p_fcStartingCell"> Starting cell. </param>
        /// <param name="p_fcDestinationCell"> Destination cell. </param>
        /// <returns></returns>
        private static bool Search(Hero p_hHero, ForestCell p_fcStartingCell, ForestCell p_fcDestinationCell)
        {
            p_fcStartingCell.Closed = true;
            List<ForestCell> lfcNextCells = GetAdjacentSafeCells(p_hHero, p_fcStartingCell);
            lfcNextCells.Sort((cell1, cell2) => cell1.Distance.CompareTo(cell2.Distance));
            foreach (ForestCell nextCell in lfcNextCells)
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

        /// <summary>
        /// Run the A* search for the given parameters.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        /// <param name="p_fcDestinationCell"> Destination cell. </param>
        /// <returns> The fastest safest path if found. </returns>
        public static List<ForestCell> FindPath(Hero p_hHero, ForestCell p_fcDestinationCell)
        {
            List<ForestCell> lfcPath = new List<ForestCell>();
            bool bSuccess = Search(p_hHero, p_hHero.CurrentForestCell, p_fcDestinationCell);
            if (bSuccess)
            {
                ForestCell node = p_fcDestinationCell;
                while (node.ParentCell != null)
                {
                    lfcPath.Add(node);
                    node = node.ParentCell;
                }
                lfcPath.Reverse();
            }
            return lfcPath;
        }
    }
}
