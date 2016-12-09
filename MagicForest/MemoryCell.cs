using System.Collections.Generic;

namespace MagicForest
{
    /// <summary>
    /// Cell used to create a memory cell.
    /// </summary>
    public class MemoryCell
    {
        /// <summary>
        /// Memory cell line index.
        /// </summary>
        private int m_iLineIndex;
        /// <summary>
        /// Memory cell column index.
        /// </summary>
        private int m_iColumnIndex;

        /* for all integers :
         * -1 -> false
         * 1 -> true
         * 0 -> unknown 
         */
        private int m_iMayContainAlien, m_iMayContainHole, m_iHasNoAlien, m_iHasNoHole, m_iIsSafe = 0;

        /// <summary>
        /// Get / set the m_iLineIndex.
        /// </summary>
        public int LineIndex
        {
            get
            {
                return m_iLineIndex;
            }
            set
            {
                m_iLineIndex = value;
            }
        }

        /// <summary>
        /// Get / set the m_iColumnIndex.
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return m_iColumnIndex;
            }
            set
            {
                m_iColumnIndex = value;
            }
        }

        /// <summary>
        /// Get / set the m_iIsSafe.
        /// </summary>
        public int IsSafe
        {
            get
            {
                return m_iIsSafe;
            }
            set
            {
                m_iIsSafe = value;
            }
        }

        /// <summary>
        /// Get / set the m_iMayContainAlien.
        /// </summary>
        public int MayContainAlien
        {
            get
            {
                return m_iMayContainAlien;
            }
            set
            {
                m_iMayContainAlien = value;
            }
        }

        /// <summary>
        /// Get / set the m_iMayContainHole.
        /// </summary>
        public int MayContainHole
        {
            get
            {
                return m_iMayContainHole;
            }
            set
            {
                m_iMayContainHole = value;
            }
        }

        /// <summary>
        /// Get / set the m_iHasNoAlien.
        /// </summary>
        public int HasNoAlien
        {
            get
            {
                return m_iHasNoAlien;
            }
            set
            {
                m_iHasNoAlien = value;
            }
        }

        /// <summary>
        /// Get / set the m_iHasNoHole.
        /// </summary>
        public int HasNoHole
        {
            get
            {
                return m_iHasNoHole;
            }
            set
            {
                m_iHasNoHole = value;
            }
        }

        /// <summary>
        /// Find the MemoryCells directly on the left/right/up/down of the current cell.
        /// </summary>
        /// <returns> A list of the cells (up to 4) neighbors of the current cell. </returns>
        public List<MemoryCell> getAdjacentMemoryCells()
        {
            List<MemoryCell> lmcResult = new List<MemoryCell>();

            if (m_iLineIndex - 1 >= 0)
            {
                lmcResult.Add(Hero.Memory[m_iLineIndex - 1, m_iColumnIndex]);
            }
            if (m_iLineIndex + 1 < MainWindow.ForestSize)
            {
                lmcResult.Add(Hero.Memory[m_iLineIndex + 1, m_iColumnIndex]);
            }
            if (m_iColumnIndex - 1 >= 0)
            {
                lmcResult.Add(Hero.Memory[m_iLineIndex, m_iColumnIndex - 1]);
            }
            if (m_iColumnIndex + 1 < MainWindow.ForestSize)
            {
                lmcResult.Add(Hero.Memory[m_iLineIndex, m_iColumnIndex + 1]);
            }

            return lmcResult;
        }

        /// <summary>
        /// Create a new memory cell.
        /// </summary>
        public MemoryCell()
        {
        }
    }
}
