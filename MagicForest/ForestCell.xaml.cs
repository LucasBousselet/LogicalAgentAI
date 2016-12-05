using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicForest
{
    /// <summary>
    /// Logique d'interaction pour ForestCell.xaml
    /// </summary>
    public partial class ForestCell : UserControl
    {
        private int m_iLineIndex;
        private int m_iColumnIndex;

        private bool m_bHasPortal = false;
        private bool m_bHasMonster = false;
        private bool m_bHasHole = false;
        private bool m_bHasWind = false;
        private bool m_bHasPoop = false;
        private bool m_bHasNothing = false;

        private bool m_bAlreadyVisited = false;

        public int LineIndex
        {
            get
            {
                return m_iLineIndex;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return m_iColumnIndex;
            }
        }

        public bool HasPortal
        {
            get
            {
                return m_bHasPortal;
            }
            set
            {
                m_bHasPortal = value;
            }
        }

        public bool HasMonster
        {
            get
            {
                return m_bHasMonster;
            }
            set
            {
                m_bHasMonster = value;
            }
        }

        public bool HasHole
        {
            get
            {
                return m_bHasHole;
            }
            set
            {
                m_bHasHole = value;
            }
        }

        public bool HasWind
        {
            get
            {
                return m_bHasWind;
            }
            set
            {
                m_bHasWind = value;
            }
        }

        public bool HasPoop
        {
            get
            {
                return m_bHasPoop;
            }
            set
            {
                m_bHasPoop = value;
                // UPDATE SPRITE ! (REMOVE IF MONSTER IS KILLED)
            }
        }

        public bool HasNothing
        {
            get
            {
                return m_bHasNothing;
            }
            set
            {
                m_bHasNothing = value;
            }
        }

        public bool AlreadyVisited
        {
            get
            {
                return m_bAlreadyVisited;
            }
            set
            {
                m_bAlreadyVisited = value;
            }
        }

        public List<ForestCell> getAdjacentCells()
        {
            List<ForestCell> lfcResult = null;

            if (m_iLineIndex - 1 > 0)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex - 1, m_iColumnIndex]);
            }
            if (m_iLineIndex + 1 < MainWindow.ForestSize)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex + 1, m_iColumnIndex]);
            }
            if (m_iColumnIndex - 1 > 0)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex, m_iColumnIndex - 1]);
            }
            if (m_iColumnIndex + 1 < MainWindow.ForestSize)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex, m_iColumnIndex + 1]);
            }

            return lfcResult;
        }

        public ForestCell()
        {
            InitializeComponent();
        }
    }
}
