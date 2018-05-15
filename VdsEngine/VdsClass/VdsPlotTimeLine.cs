using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class VdsPlotTimeLine : VdsLayer
    {
        public double StartTime
        {
            get;
            set;
        }

        public double EndTime
        {
            get;
            set;
        }

        public VdsPlotTimeLine()
            : base()
        { }

        public VdsPlotTimeLine(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public override void UpdateStep(object param)
        {
            for (int i = _childrenList.Count - 1; i >= 0; i--)
            {
                VdsPlotEvent x = (VdsPlotEvent)_childrenList[i];
                x.UpdateStep(param);
            }
        }

        public override void AsynchronousOperationStep(object param)
        {
            for (int i = _childrenList.Count - 1; i >= 0; i--)
            {
                VdsPlotEvent x = (VdsPlotEvent)_childrenList[i];
                x.AsynchronousOperationStep(param);
            }
        }

        internal void InitPlotTimeLine()
        {
            StartTime = Int32.MaxValue;
            EndTime = Int32.MinValue;
            for (int i = _childrenList.Count - 1; i >= 0; i--)
            {
                VdsPlotEvent t = (VdsPlotEvent)_childrenList[i];
                StartTime = Math.Min(StartTime, t.EventStartTime);
                EndTime = Math.Max(EndTime, t.EventStartTime + t.EventDurationTime);
            }
            _childrenList.Sort(delegate(PtrClass x, PtrClass y)
            {
                VdsPlotEvent px = (VdsPlotEvent)x;
                VdsPlotEvent py = (VdsPlotEvent)y;
                return py.EventStartTime.CompareTo(px.EventStartTime);
            });
        }

        internal void StopScript()
        {
            for (int i = _childrenList.Count - 1; i >= 0; i--)
            {
                VdsPlotEvent x = (VdsPlotEvent)_childrenList[i];
                x.StopScript();
            }
        }
    }
}