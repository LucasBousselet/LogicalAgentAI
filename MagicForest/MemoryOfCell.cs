using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public class MemoryOfCell
    {
        /* for all integers :
         * -1 -> false
         * 1 -> true
         * 0 -> unknown */ 
        public int m_iIsSafe, m_iMayContainMonster, m_iMayContainHole, m_iContainMonster, m_iContainHole = 0;

        public int m_iLineIndex;
        public int m_iColumnIndex;

        /// <summary>
        /// Find the MemoryCells directly on the left/right/up/down of the current cell
        /// </summary>
        /// <returns>A list of the cells (up to 4) neighbors of the current cell</returns>
        public List<MemoryOfCell> getAdjacentMemoryCells()
        {
            List<MemoryOfCell> lmcResult = new List<MemoryOfCell>();

            if (m_iLineIndex - 1 >= 0)
            {
                lmcResult.Add(Hero.MatrixOfMemoryCells[m_iLineIndex - 1, m_iColumnIndex]);
            }
            if (m_iLineIndex + 1 < MainWindow.ForestSize)
            {
                lmcResult.Add(Hero.MatrixOfMemoryCells[m_iLineIndex + 1, m_iColumnIndex]);
            }
            if (m_iColumnIndex - 1 >= 0)
            {
                lmcResult.Add(Hero.MatrixOfMemoryCells[m_iLineIndex, m_iColumnIndex - 1]);
            }
            if (m_iColumnIndex + 1 < MainWindow.ForestSize)
            {
                lmcResult.Add(Hero.MatrixOfMemoryCells[m_iLineIndex, m_iColumnIndex + 1]);
            }

            return lmcResult;
        }

        public MemoryOfCell()
        {
        }
    }
}
