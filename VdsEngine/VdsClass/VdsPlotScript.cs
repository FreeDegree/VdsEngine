using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class VdsPlotScript : VdsLayer
    {
        public bool PlotIsPlaying
        {
            get;
            set;
        }

        public bool PlotIsPause
        {
            get;
            set;
        }

        public string CurrentPlotID
        {
            get;
            set;
        }

        public double ScriptDurationTime
        {
            get;
            set;
        }

        private VdsView _currentView = null;
        private long _startTick = 0;
        private long _pauseTick = 0;
        private double _totalLength = 0;

        public VdsPlotScript()
            : base()
        { }

        public VdsPlotScript(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public override void UpdateStep(object param)
        {
            if (_stepNumber == 0)
                BuildLayerInternal();
            _stepNumber++;
            if (!PlotIsPlaying)
                return;
            double curTime = -1;
            if (PlotIsPause)
                curTime = (_pauseTick - _startTick) / TimeSpan.TicksPerMillisecond / 1000.0;
            else
            {
                long currentTick = DateTime.Now.Ticks;
                curTime = (currentTick - _startTick) / TimeSpan.TicksPerMillisecond / 1000.0;
            }
            if (curTime > ScriptDurationTime)
                return;
            base.UpdateStep(curTime);
        }

        public override void AsynchronousOperationStep(object param)
        {
            if (!PlotIsPlaying)
                return;
            double curTime = -1;
            if (PlotIsPause)
                curTime = (_pauseTick - _startTick) / TimeSpan.TicksPerMillisecond / 1000.0;
            else
            {
                long currentTick = DateTime.Now.Ticks;
                curTime = (currentTick - _startTick) / TimeSpan.TicksPerMillisecond / 1000.0;
            }
            base.AsynchronousOperationStep(curTime);
        }

        protected override void BuildLayerInternal()
        {
            base.BuildLayerInternal();
            if (_currentView == null)
            {
                _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(ObjectViewID));
                _currentView.ViewEvent.VdsViewEventEvent_PlayPlotScriptEventEvent += ViewEvent_VdsViewEventEvent_PlayPlotScriptEventEvent;
                _currentView.ViewEvent.VdsViewEventEvent_StopPlotScriptEventEvent += ViewEvent_VdsViewEventEvent_StopPlotScriptEventEvent;
                _currentView.ViewEvent.VdsViewEventEvent_PausePlotScriptEventEvent += ViewEvent_VdsViewEventEvent_PausePlotScriptEventEvent;
            }
        }

        protected override void InitActorPropertyList(string[] propertyNameList, string[] propertyValueList)
        {
            base.InitActorPropertyList(propertyNameList, propertyValueList);
            int index = 0;
            foreach (string name in _propertyNameList)
            {
                switch (name)
                {
                    case "calibratedscalemaxtime":
                        ScriptDurationTime = Convert.ToDouble(propertyValueList[index]);
                        break;
                    default:
                        break;
                }
                index++;
            }
        }

        void ViewEvent_VdsViewEventEvent_PlayPlotScriptEventEvent(object sender, EventArgs e)
        {
            VdsViewEventEvent.VEventArgs vArgs = e as VdsViewEventEvent.VEventArgs;
            List<string> eventParameter = vArgs.EventParameter as List<string>;
            if (_totalLength == 0)
            {
                for (int i = _childrenList.Count - 1; i >= 0; i--)
                {
                    VdsPlotTimeLine t = (VdsPlotTimeLine)_childrenList[i];
                    t.InitPlotTimeLine();
                }
            }
            if (!PlotIsPlaying)
            {
                double offsetTime = Convert.ToDouble(eventParameter[1]);
                _startTick = DateTime.Now.Ticks - (long)(offsetTime * TimeSpan.TicksPerSecond);
                CurrentPlotID = eventParameter[0];
                PlotIsPlaying = true;
            }
            else if (PlotIsPause)
            {
                PlotIsPause = false;
                long nowTick = DateTime.Now.Ticks;
                _startTick = _startTick + (nowTick - _pauseTick);
            }
        }

        void ViewEvent_VdsViewEventEvent_PausePlotScriptEventEvent(object sender, EventArgs e)
        {
            if (!PlotIsPlaying)
                return;
            VdsViewEventEvent.VEventArgs vArgs = e as VdsViewEventEvent.VEventArgs;
            string plotID = vArgs.EventParameter.ToString();
            if (CurrentPlotID != plotID)
                return;
            if (_pauseTick != 0)
                return;
            _pauseTick = DateTime.Now.Ticks;
            PlotIsPause = true;
        }

        void ViewEvent_VdsViewEventEvent_StopPlotScriptEventEvent(object sender, EventArgs e)
        {
            if (!PlotIsPlaying)
                return;
            VdsViewEventEvent.VEventArgs vArgs = e as VdsViewEventEvent.VEventArgs;
            string plotID = vArgs.EventParameter.ToString();
            if (CurrentPlotID == plotID)
            {
                for (int i = _childrenList.Count - 1; i >= 0; i--)
                {
                    VdsPlotTimeLine t = (VdsPlotTimeLine)_childrenList[i];
                    t.StopScript();
                }
                PlotIsPlaying = false;
                PlotIsPause = false;
                _pauseTick = 0;
                _startTick = 0;

                (ParentObject as VdsLayer).RemoveChild(this);
                if (_currentView != null)
                {
                    _currentView.ViewEvent.VdsViewEventEvent_PlayPlotScriptEventEvent -= ViewEvent_VdsViewEventEvent_PlayPlotScriptEventEvent;
                    _currentView.ViewEvent.VdsViewEventEvent_StopPlotScriptEventEvent -= ViewEvent_VdsViewEventEvent_StopPlotScriptEventEvent;
                    _currentView.ViewEvent.VdsViewEventEvent_PausePlotScriptEventEvent -= ViewEvent_VdsViewEventEvent_PausePlotScriptEventEvent;
                }
            }
        }

        ~VdsPlotScript()
        { }
    }
}