using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagicForest
{
    public class Hero
    {
        private int m_iScore;
        private ForestCell m_fcCurrentForestCell;
        private ForestCell m_fcPreviousForestCell;

        private bool m_bStillAlive = true;

        private bool m_bNothing = false;
        private bool m_bSmellDetected = false;
        private bool m_bWindDetected = false;
        private bool m_bLightDetected = false;

        private string m_sGoal = "GETOUTOMG!";

        private MemoryCell m_mcCurrentMemoryCell;

        private List<ForestCell> m_lfcCellsOK = new List<ForestCell>();
        private List<ForestCell> m_lfcCellsSuspicous = new List<ForestCell>();
        private List<ForestCell> m_lfcCellsWithSmell = new List<ForestCell>();
        private int m_iMemorySize;

        // New delegate for player death
        public delegate void dlgDeath();
        public dlgDeath OnDeath;

        /// <summary>
        /// 2-dimensional array of the knowledge our hero has of the current environment
        /// </summary>
        private static MemoryCell[,] m_lmcMemory;

        public MemoryCell CurrentMemoryCell
        {
            get
            {
                return m_mcCurrentMemoryCell;
            }
            set
            {
                m_mcCurrentMemoryCell = value;
            }
        }

        public ForestCell PreviousForestCell
        {
            get
            {
                return m_fcPreviousForestCell;
            }
            set
            {
                m_fcPreviousForestCell = value;
            }
        }

        public List<ForestCell> CellsOK
        {
            get
            {
                return m_lfcCellsOK;
            }
        }

        public List<ForestCell> CellsSuspicous
        {
            get
            {
                return m_lfcCellsSuspicous;
            }
        }

        public static MemoryCell[,] Memory
        {
            get
            {
                return m_lmcMemory;
            }
            set
            {
                m_lmcMemory = value;
            }
        }

        public int MemorySize
        {
            get
            {
                return m_iMemorySize;
            }
            set
            {
                m_iMemorySize = value;
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

        public ForestCell CurrentForestCell
        {
            get
            {
                return m_fcCurrentForestCell;
            }
            set
            {
                m_fcCurrentForestCell = value;
            }
        }

        public Hero(int memSize, int Score)
        {
            //m_sDirectionFacing = m_asPossibleDirections[1];
            m_iScore = 0;
            m_iMemorySize = memSize;
            m_iScore = Score;
            //m_fcCurrentForestCell = MainWindow.Forest[0, 0];
            PopulateMemoryCellMatrix();
            m_mcCurrentMemoryCell = Memory[0, 0];
        }

        /// <summary>
        /// Puts MemoryCells inside the matrix of MemoryCell the hero has of his environment
        /// </summary>
        public void PopulateMemoryCellMatrix()
        {
            m_lmcMemory = new MemoryCell[m_iMemorySize, m_iMemorySize];

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

        public bool AmIAlive()
        {
            return m_bStillAlive;
        }

        public void GetEnvironmentState()
        {
            m_bNothing = Sensor.HasNothing(m_fcCurrentForestCell);
            m_bLightDetected = Sensor.HasLight(m_fcCurrentForestCell);
            m_bSmellDetected = Sensor.HasSmell(m_fcCurrentForestCell);
            m_bWindDetected = Sensor.HasWind(m_fcCurrentForestCell);
        }

        public int UpdateMyState()
        {
            int iResultState = -1;

            if (m_bNothing)
            {
                // Remove from list of cells with smell
                if (m_lfcCellsWithSmell.Contains(CurrentForestCell))
                {
                    m_lfcCellsWithSmell.Remove(CurrentForestCell);
                }

                // Update Cell with knowledge that it's empty
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                foreach (MemoryCell mcItem in MemoryCells)
                {
                    m_lfcCellsOK.AddRange(m_fcCurrentForestCell.getAdjacentCells());
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

            if (m_bSmellDetected && !m_bWindDetected && !m_bLightDetected)
            {
                // Add cell to radList
                m_lfcCellsWithSmell.Add(m_fcCurrentForestCell);

                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                for (int i = MemoryCells.Count - 1; i >= 0; i--)
                {
                    // Check in neighboor cell isn't already ok
                    List<ForestCell> fcNeighboorForestCells = m_fcCurrentForestCell.getAdjacentCells();

                    foreach (ForestCell fcItem in fcNeighboorForestCells)
                    {
                        if (!m_lfcCellsOK.Contains(fcItem))
                        {
                            m_lfcCellsSuspicous.Add(fcItem);
                            m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                            // Update memory cells
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainMonster = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoMonster = -1;

                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoHole = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].ContainHole = -1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainHole = -1;
                        }
                    }
                }

                iResultState = 1;
            }
            if (m_bWindDetected && !m_bSmellDetected && !m_bLightDetected)
            {
                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                for (int i = MemoryCells.Count - 1; i >= 0; i--)
                {
                    // Check in neighboor cell isn't already ok
                    List<ForestCell> fcNeighboorForestCells = m_fcCurrentForestCell.getAdjacentCells();

                    foreach (ForestCell fcItem in fcNeighboorForestCells)
                    {
                        if (!m_lfcCellsOK.Contains(fcItem))
                        {
                            m_lfcCellsSuspicous.Add(fcItem);
                            m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                            //Memory[fcItem.LineIndex, fcItem.ColumnIndex]

                            // Update memory cells
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainHole = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoHole = -1;

                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoMonster = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].ContainMonster = -1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainMonster = -1;
                        }
                    }
                }

                iResultState = 2;
            }
            if (m_bWindDetected && m_bSmellDetected && !m_bLightDetected)
            {
                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                for (int i = MemoryCells.Count - 1; i >= 0; i--)
                {
                    // Check in neighboor cell isn't already ok
                    List<ForestCell> fcNeighboorForestCells = m_fcCurrentForestCell.getAdjacentCells();

                    foreach (ForestCell fcItem in fcNeighboorForestCells)
                    {
                        if (!m_lfcCellsOK.Contains(fcItem))
                        {
                            m_lfcCellsSuspicous.Add(fcItem);
                            m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                            //Memory[fcItem.LineIndex, fcItem.ColumnIndex]

                            // Update memory cells
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainHole = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoHole = -1;

                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainMonster = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoMonster = -1;

                        }
                    }
                }

                /*
                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                for (int i = MemoryCells.Count - 1; i >= 0; i--)
                {
                    // Check in neighboor cell isn't already ok
                    foreach (ForestCell fcItem in m_lfcCellsOK)
                    {
                        if (MemoryCells[i].LineIndex == fcItem.LineIndex && MemoryCells[i].ColumnIndex == fcItem.ColumnIndex)
                        {
                            MemoryCells.Remove(MemoryCells[i]);
                        }
                    }

                    // Add ramaining cells to suspicious cells
                    m_lfcCellsSuspicous.AddRange(m_fcCurrentForestCell.getAdjacentCells());
                    m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                    // Update memory cells
                    MemoryCells[i].MayContainHole = 1;
                    MemoryCells[i].HasNoHole = -1;

                    MemoryCells[i].MayContainMonster = 1;
                    MemoryCells[i].HasNoMonster = -1;
                }
                */

                iResultState = 3;
            }
            if (m_bLightDetected)
            {
                iResultState = 4;
            }
            if (m_fcCurrentForestCell.HasMonster)
            {
                iResultState = 5;
            }
            if (m_fcCurrentForestCell.HasHole)
            {
                iResultState = 6;
            }

            return iResultState;
        }

        private List<PossibleAction> ActionDeclenchable(int p_iStateEnv)
        {
            /*
            TurnLeft apActToTurnLeft = new TurnLeft(this);
            TurnRight apActToTurnRIght = new TurnRight(this);
            GoBackward apActToGoBackward = new GoBackward(this);
            GoForward apActToGoForward = new GoForward(this);
            */
            //Move paActToMove = new Move(this);
            ThrowRockLeft paActToThrowRockLeft = new ThrowRockLeft(this);
            ThrowRockRight paActToThrowRockRight = new ThrowRockRight(this);
            ThrowRockTop paActToThrowRockTop = new ThrowRockTop(this);
            ThrowRockBottom paActToThrowRockBottom = new ThrowRockBottom(this);
            Exit paActToExit = new Exit(this);

            List<PossibleAction> aListActionPossible = new List<PossibleAction>();

            // Empty cell or wind
            if (p_iStateEnv == 0 || p_iStateEnv == 2)
            {
                bool flag = false;
                // If there are some remaining safe cells, move there
                if (m_lfcCellsOK.Any())
                {
                    //ForestCell fcNextCell;
                    foreach (ForestCell fcItem in m_lfcCellsOK)
                    {
                        if (!fcItem.AlreadyVisited)
                        {
                            Move paActToMove = new Move(this, fcItem);
                            aListActionPossible.Add(paActToMove);
                            flag = true;
                            break;
                        }
                    }
                }

                bool flagship = false;
                // Else go to a cell with smell to kill monster
                if (m_lfcCellsWithSmell.Any() && !flag)
                {
                    Random r = new Random();
                    int index = r.Next(0, m_lfcCellsWithSmell.Count);
                    ForestCell fcNextCell = m_lfcCellsWithSmell[index];
                    Move paActToMove = new Move(this, fcNextCell);
                    aListActionPossible.Add(paActToMove);
                    flagship = true;
                }
                if (!flagship && !flag)
                {
                    // Else try a random remaining cell
                    if (m_lfcCellsSuspicous.Any())
                    {
                        Random r = new Random();
                        int index = r.Next(0, m_lfcCellsSuspicous.Count);
                        ForestCell fcNextCell = m_lfcCellsSuspicous[index];
                        Move paActToMove = new Move(this, fcNextCell);
                        aListActionPossible.Add(paActToMove);
                    }
                }
            }
            // Smell
            if (p_iStateEnv == 1)
            {
                bool flag = false;
                // If there are some remaining safe cells, move there
                if (m_lfcCellsOK.Any())
                {
                    //ForestCell fcNextCell;
                    foreach (ForestCell fcItem in m_lfcCellsOK)
                    {
                        if (!fcItem.AlreadyVisited)
                        {
                            Move paActToMove = new Move(this, fcItem);
                            aListActionPossible.Add(paActToMove);
                            flag = true;
                            break;
                        }
                    }
                }
                // Else try to kill monster
                if (!flag)
                {
                    List<ForestCell> lfcTargets = CurrentForestCell.getAdjacentCells();
                    // Remove cell visited just before from targets
                    for (int i = lfcTargets.Count - 1; i >= 0; i--)
                    {
                        if (Memory[lfcTargets[i].LineIndex, lfcTargets[i].ColumnIndex].HasNoMonster == 1)
                        {
                            lfcTargets.Remove(lfcTargets[i]);
                        }
                    }

                    // Randomly choose target from remaining cells
                    Random r = new Random();
                    int iTargetIndex = r.Next(0, lfcTargets.Count);

                    if (lfcTargets[iTargetIndex].ColumnIndex < m_fcCurrentForestCell.ColumnIndex)
                    {
                        aListActionPossible.Add(paActToThrowRockLeft);
                        Debug.WriteLine("Throw rock left");
                    }
                    else
                    {
                        if (lfcTargets[iTargetIndex].ColumnIndex > m_fcCurrentForestCell.ColumnIndex)
                        {
                            aListActionPossible.Add(paActToThrowRockRight);
                            Debug.WriteLine("Throw rock right");
                        }
                        else
                        {
                            if (lfcTargets[iTargetIndex].LineIndex < m_fcCurrentForestCell.LineIndex)
                            {
                                aListActionPossible.Add(paActToThrowRockTop);
                                Debug.WriteLine("Throw rock top");
                            }
                            else
                            {
                                if (lfcTargets[iTargetIndex].LineIndex > m_fcCurrentForestCell.LineIndex)
                                {
                                    aListActionPossible.Add(paActToThrowRockBottom);
                                    Debug.WriteLine("Throw rock bottom");
                                }
                            }
                        }
                    }
                    Move paActToMove = new Move(this, lfcTargets[iTargetIndex]);
                    //aListActionPossible.Add(paActToMove);
                }
            }

            // Smell + Wind
            if (p_iStateEnv == 3)
            {
                bool flag = false;
                // If there are some remaining safe cells, move there
                if (m_lfcCellsOK.Any())
                {
                    //ForestCell fcNextCell;
                    foreach (ForestCell fcItem in m_lfcCellsOK)
                    {
                        if (!fcItem.AlreadyVisited)
                        {
                            Move paActToMove = new Move(this, fcItem);
                            aListActionPossible.Add(paActToMove);
                            flag = true;
                            break;
                        }
                    }
                }
                // Else try to kill monster and wait for result
                if (!flag)
                {
                    List<ForestCell> lfcTargets = CurrentForestCell.getAdjacentCells();
                    // Remove cell visited just before from targets
                    for (int i = lfcTargets.Count - 1; i >= 0; i--)
                    {
                        if (Memory[lfcTargets[i].LineIndex, lfcTargets[i].ColumnIndex].HasNoMonster == 1)
                        {
                            lfcTargets.Remove(lfcTargets[i]);
                        }
                    }
                    // if no more target, try random cell
                    if (!lfcTargets.Any())
                    {
                        Random r = new Random();
                        int index = r.Next(0, m_lfcCellsSuspicous.Count);
                        ForestCell fcNextCell = m_lfcCellsSuspicous[index];
                        Move paActToMove = new Move(this, fcNextCell);
                        aListActionPossible.Add(paActToMove);
                    }
                    else
                    {
                        // Randomly choose target from remaining cells
                        Random r = new Random();
                        int iTargetIndex = r.Next(0, lfcTargets.Count);

                        if (lfcTargets[iTargetIndex].ColumnIndex < m_fcCurrentForestCell.ColumnIndex)
                        {
                            aListActionPossible.Add(paActToThrowRockLeft);
                            Debug.WriteLine("Throw rock left");
                        }
                        else
                        {
                            if (lfcTargets[iTargetIndex].ColumnIndex > m_fcCurrentForestCell.ColumnIndex)
                            {
                                aListActionPossible.Add(paActToThrowRockRight);
                                Debug.WriteLine("Throw rock right");

                            }
                            else
                            {
                                if (lfcTargets[iTargetIndex].LineIndex < m_fcCurrentForestCell.LineIndex)
                                {
                                    aListActionPossible.Add(paActToThrowRockTop);
                                    Debug.WriteLine("Throw rock top");
                                }
                                else
                                {
                                    if (lfcTargets[iTargetIndex].LineIndex > m_fcCurrentForestCell.LineIndex)
                                    {
                                        aListActionPossible.Add(paActToThrowRockBottom);
                                        Debug.WriteLine("Throw rock bottom");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Light
            if (p_iStateEnv == 4)
            {
                // GET OUT !
                aListActionPossible.Add(paActToExit);
            }
            // Monster
            if (p_iStateEnv == 5)
            {
                m_bStillAlive = false;
                OnDeath();
                return null;
            }
            // Hole
            if (p_iStateEnv == 6)
            {
                m_bStillAlive = false;
                OnDeath();
                return null;
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

            if (lapListPossibleAction != null)
            {
                // Initialises the index used to keep track of the best action to perform.
                int iIndexActionToDo = -1;

                // Each iteration, initialises the worthiness of the action currently evaluated.
                int iWorthiness = -1;

                for (int i = 0; i < lapListPossibleAction.Count; i++)
                {
                    int temp = CalculateWorthiness(lapListPossibleAction[i], m_sGoal, p_iStateEnv);
                    if (iWorthiness < temp)
                    {
                        iWorthiness = temp;
                        // If the current action is the most relevant, we keep its index in the list.
                        iIndexActionToDo = i;
                    }
                }
                // When we went through the whole list, the index returned is the one of the most relevant action.
                return lapListPossibleAction[iIndexActionToDo];
            }
            return null;
        }

        public void DoAction(PossibleAction p_paAction)
        {
            if (p_paAction != null)
            {
                p_paAction.Act();
            }
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
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Smell
                case 1:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 1;
                    }
                    break;
                // Wind
                case 2:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Wind + smell
                case 3:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 1;
                    }
                    break;
                // Light
                case 4:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
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

        /// <summary>
        /// Logical Stuff
        /// </summary>
        public void Inference()
        {
            if (AmIAlive())
            {
                /* The function call execute the BDI model.
                 * - First we call GetEnvironmentState() which return the state of the environment.
                 * - The we call DetermineActionUponMyGoal() which determines which action will bring 
                 * the robot to its goal.
                 * Finally we call DoAction() which executes the action which has been chose,.
                 */
                GetEnvironmentState();
                DoAction(DetermineActionUponMyGoal(UpdateMyState()));
            }
        }
    }
}