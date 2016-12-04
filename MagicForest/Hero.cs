using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    class Hero
    {
        private bool _lightSensor = new bool();

        private bool _stenchSensor = new bool();

        private bool _windSensor = new bool();

        public int _score = new int();

        private int[] _position;

        public Hero()
        {
            this._lightSensor = false;
            this._stenchSensor = false;
            this._windSensor = false;
            this._score = 0;
            this._position = new int[2] { 0, 0 };
        }

        public bool HasLight
        {
            get
            {
                return _lightSensor;
            }
            set
            {
                _lightSensor = value;
            }
        }

        public bool HasStench
        {
            get
            {
                return _stenchSensor;
            }
            set
            {
                _stenchSensor = value;
            }
        }

        public bool HasWind
        {
            get
            {
                return _windSensor;
            }
            set
            {
                _windSensor = value;
            }
        }

        public void ThrowRock()
        {

        }

        public void ActivatePortal()
        {

        }
    }
}