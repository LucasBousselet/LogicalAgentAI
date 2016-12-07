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
    }
    */

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

    public class ThrowRockLeft : PossibleAction
    {
        private string m_sName = "ThrowRockLeft";

        public override string Name()
        {
            return m_sName;
        }

        public ThrowRockLeft(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.ThrowRockLeft(m_hHero);
        }
    }

    public class ThrowRockRight : PossibleAction
    {
        private string m_sName = "ThrowRockRight";

        public override string Name()
        {
            return m_sName;
        }

        public ThrowRockRight(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.ThrowRockRight(m_hHero);
        }
    }

    public class ThrowRockTop : PossibleAction
    {
        private string m_sName = "ThrowRockTop";

        public override string Name()
        {
            return m_sName;
        }

        public ThrowRockTop(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.ThrowRockTop(m_hHero);
        }
    }

    public class ThrowRockBottom : PossibleAction
    {
        private string m_sName = "ThrowRockBottom";

        public override string Name()
        {
            return m_sName;
        }

        public ThrowRockBottom(Hero p_hHero) : base(p_hHero)
        {
        }

        public override void Act()
        {
            Actuator.ThrowRockBottom(m_hHero);
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
