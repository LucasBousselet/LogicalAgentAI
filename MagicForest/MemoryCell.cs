using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public class MemoryCell
    {
        private int m_iLineIndex;
        private int m_iColumnIndex;

        /* for all integers :
         * -1 -> false
         * 1 -> true
         * 0 -> unknown */
        private int m_iIsSafe, m_iMayContainMonster, m_iMayContainHole, m_iContainMonster, m_iContainHole, m_iHasNoMonster, m_iHasNoHole = 0;

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

        public int MayContainMonster
        {
            get
            {
                return m_iMayContainMonster;
            }
            set
            {
                m_iMayContainMonster = value;
            }
        }

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

        public int ContainMonster
        {
            get
            {
                return m_iContainMonster;
            }
            set
            {
                m_iContainMonster = value;
            }
        }

        public int ContainHole
        {
            get
            {
                return m_iContainHole;
            }
            set
            {
                m_iContainHole = value;
            }
        }

        public int HasNoMonster
        {
            get
            {
                return m_iHasNoMonster;
            }
            set
            {
                m_iHasNoMonster = value;
            }
        }

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
        /// Find the MemoryCells directly on the left/right/up/down of the current cell
        /// </summary>
        /// <returns>A list of the cells (up to 4) neighbors of the current cell</returns>
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

        public MemoryCell()
        {
        }
    }
}
