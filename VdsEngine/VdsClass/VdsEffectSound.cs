using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public class VdsEffectSound : VdsEffectBase
    {
        public string SoundFileName
        {
            get
            {
                return _soundFileName;
            }
            set
            {
                _soundFileName = value;
                SetActorProperty("soundfilename", _soundFileName);
            }
        }

        public bool SoundLoop
        {
            get
            {
                return _soundLoop;
            }
            set
            {
                _soundLoop = value;
                SetActorProperty("soundloop", _soundLoop ? "1" : "0");
            }
        }

        public double SoundGain
        {
            get
            {
                return _soundGain;
            }
            set
            {
                _soundGain = value;
                SetActorProperty("soundgain", Convert.ToString(_soundGain));
            }
        }

        public double SoundPitch
        {
            get
            {
                return _soundPitch;
            }
            set
            {
                _soundPitch = value;
                SetActorProperty("soundpitch", Convert.ToString(_soundPitch));
            }
        }

        public bool ListenerRelative
        {
            get
            {
                return _listenerRelative;
            }
            set
            {
                _listenerRelative = value;
                SetActorProperty("listenerrelative", _listenerRelative ? "1" : "0");
            }
        }

        public double MaxDistance
        {
            get
            {
                return _maxDistance;
            }
            set
            {
                _maxDistance = value;
                SetActorProperty("sounddistance", Convert.ToString(_maxDistance));
            }
        }

        public double SoundRolloffFactor
        {
            get
            {
                return _soundRolloffFactor;
            }
            set
            {
                _soundRolloffFactor = value;
                SetActorProperty("soundrollofffactor", Convert.ToString(_soundRolloffFactor));
            }
        }

        private string _soundFileName = null;
        private bool _soundLoop = true;
        private double _soundGain = 1.0;
        private double _soundPitch = 1.0;
        private bool _listenerRelative = true;
        private double _maxDistance = 100;
        private double _soundRolloffFactor = 1.0;

        public VdsEffectSound()
            : base()
        { }

        public VdsEffectSound(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public override void Apply(PtrClass obj)
        {
            if (obj != null && IsIdle)
            {
                ApplyEffectInternal(NativeHandle, obj.NativeHandle, true);
                IsIdle = false;
            }
        }

        public override void Unapply()
        {
            if (IsIdle)
                return;
            ApplyEffectInternal(NativeHandle, IntPtr.Zero, false);
            IsIdle = true;
        }

        protected override void InitActorPropertyList(string[] propertyNameList, string[] propertyValueList)
        {
            _propertyNameList = propertyNameList;
            _propertyValueList = propertyValueList;
            int index = 0;
            foreach (string name in _propertyNameList)
            {
                switch (name)
                {
                    case "soundloop":
                        _soundLoop = (propertyValueList[index] != "0");
                        break;
                    case "soundgain":
                        _soundGain = Convert.ToDouble(propertyValueList[index]);
                        break;
                    case "soundpitch":
                        _soundPitch = Convert.ToDouble(propertyValueList[index]);
                        break;
                    case "listenerrelative":
                        _listenerRelative = (propertyValueList[index] != "0");
                        break;
                    case "sounddistance":
                        _maxDistance = Convert.ToDouble(propertyValueList[index]);
                        break;
                    case "soundrollofffactor":
                        _soundRolloffFactor = Convert.ToDouble(propertyValueList[index]);
                        break;
                    case "soundfilename":
                        _soundFileName = propertyValueList[index];
                        break;
                    default:
                        break;
                }
                index++;
            }
        }
    }
}