using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class PlotEventSound : PlotEventBehaviour
    {
        public StringProperty ActorBindingID
        {
            get;
            set;
        }

        public FilePathProperty SoundFileName
        {
            get;
            set;
        }

        public BoolProperty SoundLoop
        {
            get;
            set;
        }

        public DoubleProperty SoundGain
        {
            get;
            set;
        }

        public DoubleProperty SoundPitch
        {
            get;
            set;
        }

        public BoolProperty ListenerRelative
        {
            get;
            set;
        }

        public DoubleProperty MaxDistance
        {
            get;
            set;
        }

        public DoubleProperty SoundRolloffFactor
        {
            get;
            set;
        }

        private VdsView _currentView = null;
        private VdsActor _motioObject = null;
        private VdsEffectSound _soundEffect = null;

        public PlotEventSound()
        {
            ActorBindingID = new StringProperty();
            SoundFileName = new FilePathProperty();
            SoundLoop = new BoolProperty();
            SoundLoop.Value = true;
            SoundGain = new DoubleProperty();
            SoundGain.Value = 1.0;
            SoundPitch = new DoubleProperty();
            SoundPitch.Value = 1.0;
            ListenerRelative = new BoolProperty();
            ListenerRelative.Value = true;
            MaxDistance = new DoubleProperty();
            MaxDistance.Value = 100.0;
            SoundRolloffFactor = new DoubleProperty();
            SoundRolloffFactor.Value = 1.0;
        }

        public override void Start()
        {
            if (ActorBindingID.Value == "" || ActorBindingID.Value == null)
                return;
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(ActorBindingID.Value);
            if (a != null)
                _motioObject = a as VdsActor;
            _soundEffect = new VdsEffectSound(true);
            _soundEffect.SoundFileName = SoundFileName.Value;
            _soundEffect.SoundLoop = SoundLoop.Value;
            _soundEffect.SoundGain = SoundGain.Value;
            _soundEffect.SoundPitch = SoundPitch.Value;
            _soundEffect.ListenerRelative = ListenerRelative.Value;
            _soundEffect.MaxDistance = MaxDistance.Value;
            _soundEffect.SoundRolloffFactor = SoundRolloffFactor.Value;
            _behaviourIsWorking = true;
        }

        public override void End()
        {
            if (_currentView != null && _motioObject != null)
            {
                _soundEffect.Unapply();
                _soundEffect = null;
            }
            _behaviourIsWorking = false;
            _currentView = null;
            base.End();
        }

        public override void UpdateStep(object param)
        {
            if (param == null)
                return;
            double? t = param as double?;
            if (t == null)
                return;
            _curTime = (double)t;
            if (_curTime < 0)
                return;
            if (_motioObject == null)
            {
                PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(ActorBindingID.Value);
                if (a != null)
                    _motioObject = a as VdsActor;
            }
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (_curTime > pEvent.EventStartTime && _curTime < pEvent.EventStartTime + pEvent.EventDurationTime)
            {
                if (_motioObject != null && _soundEffect.IsIdle)
                {
                    _soundEffect.Apply(_motioObject);
                }
            }
            else
            {
                if (_soundEffect != null)
                    _soundEffect.Unapply();
            }
        }

        public override void AsynchronousOperationStep(object param)
        { }

        public override void OnDestroy()
        { }
    }
}