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

        private bool m_bNothing = false;
        private bool m_bSmellDetected = false;
        private bool m_bWindDetected = false;
        private bool m_bLightDetected = false;

        private string m_sGoal = "GETOUTOMG!";

        private MemoryCell m_mcCurrentMemoryCell;

        private List<ForestCell> m_lfcCellsOK;
        private List<ForestCell> m_lfcCellsSuspicous;
        private List<ForestCell> m_lfcCellsWithSmell;
        private int m_iMemorySize = 3;

        /// <summary>
        /// 2-dimensional array of the knowledge our hero has of the current environment
        /// </summary>
        private static MemoryCell[,] m_lmcMemory = null;

        public static MemoryCell[,] Memory
        {
            get
            {
                return m_lmcMemory;
            }
        }

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

        /// <summary>
        /// Puts MemoryCells inside the matrix of MemoryCell the hero has of his environment
        /// </summary>
        public void PopulateMemoryCellMatrix()
        {
            for (int i = 0; i < m_iMemorySize; i++)
            {
                for (int j = 0; j < m_iMemorySize; j++)
                {
                    Memory[i, j] = new MemoryCell();
                    Memory[i, j].LineIndex = i;
                    Memory[i, j].ColumnIndex = j;
                }
            }
        }

     /*   public ForestCell getFrontCell()
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
        }*/

        public bool AmIAlive()
        {
            return m_bStillAlive;
        }

        public void GetEnvironmentState()
        {
            m_bNothing = Sensor.HasNothing(m_fcCurrentCell);
            m_bLightDetected = Sensor.HasLight(m_fcCurrentCell);
            m_bSmellDetected = Sensor.HasSmell(m_fcCurrentCell);
            m_bWindDetected = Sensor.HasWind(m_fcCurrentCell);
        }

        public int UpdateMyState()
        {
            int iResultState = -1;

            if (m_bNothing)
            {
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                foreach (MemoryCell mcItem in MemoryCells)
                {
                    m_lfcCellsOK.AddRange(m_fcCurrentCell.getAdjacentCells());
                    // Remove duplicates
                    m_lfcCellsOK = m_lfcCellsOK.Distinct().ToList();

                    mcItem.HasNoMonster = 1;
                    mcItem.ContainMonster = -1;
                    mcItem.MayContainMonster = -1;

                    mcItem.HasNoHole = 1;
                    mcItem.ContainHole = -1;
                    mcItem.MayContainHole = -1;
                }

                iResultState = 0;
            }

            if (m_bSmellDetected == true && m_bWindDetected == false)
            {
                // Add cell to radList
                m_lfcCellsWithSmell.Add(m_fcCurrentCell);

                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                foreach (MemoryCell mcItem in MemoryCells)
                {
                    // Check in neighboor cell isn't already ok
                    foreach (ForestCell fcItem in m_lfcCellsOK)
                    {
                        if (mcItem.LineIndex == fcItem.LineIndex && mcItem.ColumnIndex == fcItem.ColumnIndex)
                        {
                            MemoryCells.Remove(mcItem);
                        }
                    }

                    // Add ramaining cells to suspicious cells
                    m_lfcCellsSuspicous.AddRange(m_fcCurrentCell.getAdjacentCells());
                    m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                    // Update memory cells
                    mcItem.MayContainMonster = 1;
                    mcItem.HasNoMonster = -1;

                    mcItem.HasNoHole = 1;
                    mcItem.ContainHole = -1;
                    mcItem.MayContainHole = -1;
                }

                iResultState = 1;
            }
            if (m_bWindDetected == true && m_bSmellDetected == false)
            {
                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                foreach (MemoryCell mcItem in MemoryCells)
                {
                    // Check in neighboor cell isn't already ok
                    foreach (ForestCell fcItem in m_lfcCellsOK)
                    {
                        if (mcItem.LineIndex == fcItem.LineIndex && mcItem.ColumnIndex == fcItem.ColumnIndex)
                        {
                            MemoryCells.Remove(mcItem);
                        }
                    }

                    // Add ramaining cells to suspicious cells
                    m_lfcCellsSuspicous.AddRange(m_fcCurrentCell.getAdjacentCells());
                    m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                    // Update memory cells
                    mcItem.MayContainHole = 1;
                    mcItem.HasNoHole = -1;

                    mcItem.HasNoMonster = 1;
                    mcItem.ContainMonster = -1;
                    mcItem.MayContainMonster = -1;
                }

                iResultState = 2;
            }
            if (m_bWindDetected == true && m_bSmellDetected == true)
            {
                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                foreach (MemoryCell mcItem in MemoryCells)
                {
                    // Check in neighboor cell isn't already ok
                    foreach (ForestCell fcItem in m_lfcCellsOK)
                    {
                        if (mcItem.LineIndex == fcItem.LineIndex && mcItem.ColumnIndex == fcItem.ColumnIndex)
                        {
                            MemoryCells.Remove(mcItem);
                        }
                    }

                    // Add ramaining cells to suspicious cells
                    m_lfcCellsSuspicous.AddRange(m_fcCurrentCell.getAdjacentCells());
                    m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                    // Update memory cells
                    mcItem.MayContainHole = 1;
                    mcItem.HasNoHole = -1;

                    mcItem.MayContainMonster = 1;
                    mcItem.HasNoMonster = -1;
                }

                iResultState = 3;
            }
            if (m_bLightDetected)
            {
                iResultState = 4;
            }
            if (m_fcCurrentCell.HasMonster)
            {
                iResultState = 5;
            }
            if (m_fcCurrentCell.HasHole)
            {
                iResultState = 6;
            }

            return iResultState;
        }

        private List<PossibleAction> ActionDeclenchable(int p_iStateEnv)
        {
            //TurnLeft apActToTurnLeft = new TurnLeft(this);
            //TurnRight apActToTurnRIght = new TurnRight(this);
            GoBackward apActToGoBackward = new GoBackward(this);
            GoForward apActToGoForward = new GoForward(this);
            ThrowRock apActToThrowRock = new ThrowRock(this);
            Exit apActToExit = new Exit(this);

            List<PossibleAction> aListActionPossible = new List<PossibleAction>();

            // Empty cell
            if (p_iStateEnv == 0)
            {

            }
            // Smell
            if (p_iStateEnv == 1)
            {

            }
            // Wind
            if (p_iStateEnv == 2)
            {

            }
            // Smell + Wind
            if (p_iStateEnv == 3)
            {

            }
            // Light
            if (p_iStateEnv == 4)
            {
                // GET OUT !!!
            }
            // Monster
            if (p_iStateEnv == 5)
            {
                m_bStillAlive = false;
            }
            // Hole
            if (p_iStateEnv == 5)
            {
                m_bStillAlive = false;
            }

            return aListActionPossible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_iStateEnv"></param>
        /// <returns></returns>
        public PossibleAction DetermineActionUponMyGoal(int p_iStateEnv)
        {
            List<PossibleAction> lapListPossibleAction = ActionDeclenchable(p_iStateEnv);

            // Initialises the index used to keep track of the best action to perform.
            int iIndexActionToDo = -1;

            // Each iteration, initialises the worthiness of the action currently evaluated.
            int iWorthiness = -1;

            for (int i = 0; i < lapListPossibleAction.Count; i++)
            {
                if (iWorthiness < CalculateWorthiness(lapListPossibleAction[i], m_sGoal, p_iStateEnv))
                {
                    iWorthiness = CalculateWorthiness(lapListPossibleAction[i], m_sGoal, p_iStateEnv);
                    // If the current action is the most relevant, we keep its index in the list.
                    iIndexActionToDo = i;
                }
            }
            // When we went through the whole list, the index returned is the one of the most relevant action.
            return lapListPossibleAction[iIndexActionToDo];
        }

        public void DoAction(PossibleAction p_paAction)
        {
            p_paAction.Act();
        }

        private int CalculateWorthiness(PossibleAction p_paAction, string p_sMyGoal, int p_iStateEnv)
        {
            int iWorthiness = -1;

            /* Depending on the state of the environment and the goal, we logically attibute a number of point to each counter
             * This way, a particularly interesting action to perform will be set a high value of worthiness */
            switch (p_iStateEnv)
            {
                // Empty
                case 0:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Smell
                case 1:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Wind
                case 2:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Wind + smell
                case 3:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Light
                case 4:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 1;
                    }
                    break;

                default:
                    break;
            }

            // Then, we return the counter associated with the current goal of the agent.
            int iResult = -1;
            if (p_sMyGoal == "GETOUTOMG!")
            {
                iResult = iWorthiness;
            }
            return iResult;
        }

    }
}