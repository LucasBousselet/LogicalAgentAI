using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public class MemoryOfCell : ForestCell
    {
        /* for all integers :
         * -1 -> false
         * 1 -> true
         * 0 -> unknown */ 
        public int m_iIsSafe, m_iMayContainMonster, m_iMayContainHole, m_iContainMonster, m_iContainHole = 0;

        public MemoryOfCell()
        {
        }
    }
}
