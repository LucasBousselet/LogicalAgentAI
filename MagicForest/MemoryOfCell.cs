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
        private int m_iIsSafe, m_iMayContainMonster, m_iMayContainHole, m_iContainMonster, m_iContainHole, m_iHasNoMonster, m_iHasNoHole = 0;

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

        public MemoryOfCell()
        {
        }
    }
}
