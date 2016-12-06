using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public class Hero
    {
        private string m_sDirectionFacing;
        public readonly string[] m_asPossibleDirections = new string[4] { "top", "right", "bottom", "left" };
        private int m_iScore;
        private ForestCell m_fcCurrentCell;
        private bool m_bStillAlive = true;
        private bool m_bSmellDetected = false;
        private bool m_bWindDetected = false;
        private bool m_bLightDetected = false;

        public string DirectionFacing
        {
            get
            {
                return m_sDirectionFacing;
            }
            set
            {
                m_sDirectionFacing = value;
            }
        }

        public int Score
        {
            get
            {
                return m_iScore;
            }
            set
            {
                m_iScore = value;
            }
        }

        public ForestCell CurrentCell
        {
            get
            {
                return m_fcCurrentCell;
            }
            set
            {
                m_fcCurrentCell = value;
                // UPDATE SPRITE EMPLACEMENT
            }
        }

        public bool StillAlive
        {
            set
            {
                MainWindow.StopExecution();
            }
        }

        public Hero()
        {
            m_sDirectionFacing = m_asPossibleDirections[1];
            m_iScore = 0;
            //m_fcCurrentCell = MainWindow.Forest[0, 0];
        }

        public ForestCell getFrontCell()
        {
            ForestCell fcResult = null;
            switch (m_sDirectionFacing)
            {
                case "top":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex - 1, m_fcCurrentCell.LineIndex];
                    break;
                case "right":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex + 1];
                    break;
                case "bottom":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex + 1, m_fcCurrentCell.LineIndex];
                    break;
                case "left":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex - 1];
                    break;
                default:
                    break;
            }
            return fcResult;
        }

        public ForestCell getBackCell()
        {
            ForestCell fcResult = null;
            switch (m_sDirectionFacing)
            {
                case "top":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex + 1, m_fcCurrentCell.LineIndex];
                    break;
                case "right":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex - 1];
                    break;
                case "bottom":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex - 1, m_fcCurrentCell.LineIndex];
                    break;
                case "left":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex + 1];
                    break;
                default:
                    break;
            }
            return fcResult;
        }

        public bool AmIAlive()
        {
            return m_bStillAlive;
        }

        public void GetEnvironmentState()
        {
            m_bLightDetected = Sensor.HasLight(m_fcCurrentCell);
            m_bSmellDetected = Sensor.HasSmell(m_fcCurrentCell);
            m_bWindDetected = Sensor.HasWind(m_fcCurrentCell);
        }

        
    }
}