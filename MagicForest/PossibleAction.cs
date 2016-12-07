using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public abstract class PossibleAction
    {
        private string m_sName;
        protected Hero m_hHero;

        public PossibleAction(Hero p_hHero)
        {
            m_hHero = p_hHero;
        }

        public abstract string Name();

        public abstract void Act();
    }

    /*
    public class TurnLeft : PossibleAction
    {
        private string m_sName = "TurnLeft";

        public override string Name()
        {
            return m_sName;
        }

        public TurnLeft(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.TurnLelf(m_hHero);
        }
    }

    public class TurnRight : PossibleAction
    {
        private string m_sName = "TurnRight";

        public override string Name()
        {
            return m_sName;
        }

        public TurnRight(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.TurnRight(m_hHero);
        }
    }
    */

    /*public class GoBackward : PossibleAction
    {
        private string m_sName = "GoBackward";

        public override string Name()
        {
            return m_sName;
        }

        public GoBackward(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.GoBackward(m_hHero);
        }
    }

    public class GoForward : PossibleAction
    {
        private string m_sName = "GoForward";

        public override string Name()
        {
            return m_sName;
        }

        public GoForward(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.GoForward(m_hHero);
        }
    }*/

    public class Move : PossibleAction
    {
        private string m_sName = "Move";

        private ForestCell m_fcDestination;

        public Move(Hero p_hHero, ForestCell p_fcDestinationCell) : base(p_hHero)
        {
            m_fcDestination = p_fcDestinationCell;
        }

        public override string Name()
        {
            return m_sName;
        }

        public override void Act()
        {
            Actuator.Move(m_hHero, m_fcDestination);
        }
    }

    public class ThrowRock : PossibleAction
    {
        private string m_sName = "ThrowRock";

        private string m_sDirection;

        public override string Name()
        {
            return m_sName;
        }

        public ThrowRock(Hero p_hHero, string p_sDirection) : base(p_hHero)
        {
            m_sDirection = p_sDirection;
        }

        public override void Act()
        {
            Actuator.ThrowRock(m_hHero, m_sDirection);
        }
    }

    public class Exit : PossibleAction
    {
        private string m_sName = "Exit";

        public override string Name()
        {
            return m_sName;
        }

        public Exit(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.Exit(m_hHero);
        }
    }
}
