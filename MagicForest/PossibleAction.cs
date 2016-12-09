namespace MagicForest
{
    /// <summary>
    /// Abstrat class used to create the possible actions.
    /// </summary>
    public abstract class PossibleAction
    {
        /// <summary>
        /// Class name.
        /// </summary>
        private string m_sName;
        /// <summary>
        /// Hero.
        /// </summary>
        protected Hero m_hHero;

        /// <summary>
        /// Create new possible action for hero.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public PossibleAction(Hero p_hHero)
        {
            m_hHero = p_hHero;
        }

        /// <summary>
        /// Return possible action name.
        /// </summary>
        /// <returns> possible action name. </returns>
        public abstract string Name();
        /// <summary>
        /// Take an action.
        /// </summary>
        public abstract void Act();
    }

    /// <summary>
    /// Move classe
    /// </summary>
    public class Move : PossibleAction
    {
        /// <summary>
        /// Name.
        /// </summary>
        private string m_sName = "Move";
        /// <summary>
        /// Destination cell.
        /// </summary>
        private ForestCell m_fcDestination;
        /// <summary>
        /// Move cost.
        /// </summary>
        private int m_iCost;

        /// <summary>
        /// New move action.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        /// <param name="p_fcDestinationCell"> Destination cell. </param>
        /// <param name="p_iCost"> Cost </param>
        public Move(Hero p_hHero, ForestCell p_fcDestinationCell, int p_iCost) : base(p_hHero)
        {
            m_fcDestination = p_fcDestinationCell;
            m_iCost = p_iCost;
        }

        /// <summary>
        /// class Name.
        /// </summary>
        /// <returns> Return name. </returns>
        public override string Name()
        {
            return m_sName;
        }

        /// <summary>
        /// Move to destination cell.
        /// </summary>
        public override void Act()
        {
            Actuator.Move(m_hHero, m_fcDestination, m_iCost);
        }
    }

    /// <summary>
    /// ThrowRockLeft class.
    /// </summary>
    public class ThrowRockLeft : PossibleAction
    {
        /// <summary>
        /// Class name.
        /// </summary>
        private string m_sName = "ThrowRockLeft";

        /// <summary>
        /// Get class name.
        /// </summary>
        /// <returns> Class name. </returns>
        public override string Name()
        {
            return m_sName;
        }

        /// <summary>
        /// Create new ThrowLeft action.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public ThrowRockLeft(Hero p_hHero) : base(p_hHero)
        {
        }

        /// <summary>
        /// Take action.
        /// </summary>
        public override void Act()
        {
            Actuator.ThrowRockLeft(m_hHero);
        }
    }

    /// <summary>
    /// ThrowRockRight class.
    /// </summary>
    public class ThrowRockRight : PossibleAction
    {
        /// <summary>
        /// Class name.
        /// </summary>
        private string m_sName = "ThrowRockRight";

        /// <summary>
        /// Get class name.
        /// </summary>
        /// <returns> Class name. </returns>
        public override string Name()
        {
            return m_sName;
        }

        /// <summary>
        /// Create new ThrowRockRight action.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public ThrowRockRight(Hero p_hHero) : base(p_hHero)
        {
        }

        /// <summary>
        /// Take action.
        /// </summary>
        public override void Act()
        {
            Actuator.ThrowRockRight(m_hHero);
        }
    }

    /// <summary>
    /// ThrowRockTop class.
    /// </summary>
    public class ThrowRockTop : PossibleAction
    {
        /// <summary>
        /// Class name.
        /// </summary>
        private string m_sName = "ThrowRockTop";

        /// <summary>
        /// Get class name.
        /// </summary>
        /// <returns> Class name. </returns>
        public override string Name()
        {
            return m_sName;
        }
        /// <summary>
        /// Create new ThrowRockTop action.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public ThrowRockTop(Hero p_hHero) : base(p_hHero)
        {
        }

        /// <summary>
        /// Take action.
        /// </summary>
        public override void Act()
        {
            Actuator.ThrowRockTop(m_hHero);
        }
    }

    /// <summary>
    /// ThrowRockBottom class.
    /// </summary>
    public class ThrowRockBottom : PossibleAction
    {
        /// <summary>
        /// Class name.
        /// </summary>
        private string m_sName = "ThrowRockBottom";

        /// <summary>
        /// Get class name.
        /// </summary>
        /// <returns> Class name. </returns>
        public override string Name()
        {
            return m_sName;
        }

        /// <summary>
        /// Create new ThrowRockBottom action.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public ThrowRockBottom(Hero p_hHero) : base(p_hHero)
        {
        }

        /// <summary>
        /// Take action.
        /// </summary>
        public override void Act()
        {
            Actuator.ThrowRockBottom(m_hHero);
        }
    }

    /// <summary>
    /// Exit class.
    /// </summary>
    public class Exit : PossibleAction
    {
        /// <summary>
        /// Class name.
        /// </summary>
        private string m_sName = "Exit";

        /// <summary>
        /// Get class name.
        /// </summary>
        /// <returns> Class name. </returns>
        public override string Name()
        {
            return m_sName;
        }

        /// <summary>
        /// Create new Exit action.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public Exit(Hero p_hHero) : base(p_hHero)
        {
        }

        /// <summary>
        /// Take action.
        /// </summary>
        public override void Act()
        {
            Actuator.Exit(m_hHero);
        }
    }
}
