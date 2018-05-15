using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class PlotEventStaticCamera : PlotEventBehaviour
    {
        public CameraPoseProperty CameraStartPose
        {
            get;
            set;
        }

        public BoolProperty WithAnimation
        {
            get;
            set;
        }

        private VdsView _currentView = null;
        private bool _haveBeenSet = false;

        public PlotEventStaticCamera()
        {
            CameraStartPose = new CameraPoseProperty();
            WithAnimation = new BoolProperty();
        }

        public override void Start()
        {
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            _behaviourIsWorking = true;
            _haveBeenSet = false;
        }

        public override void End()
        {
            _behaviourIsWorking = false;
            _currentView = null;
            CameraStartPose = null;
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
            if (CameraStartPose == null)
                return;
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (!_haveBeenSet && _curTime > pEvent.EventStartTime)
            {
                VdsCamera newCamera = new VdsCamera();
                newCamera.CameraPose = CameraStartPose;
                if (_curTime > pEvent.EventStartTime + pEvent.EventDurationTime)
                    newCamera.WithAnimation = false;
                else
                    newCamera.WithAnimation = WithAnimation.Value;
                _currentView.MainCamera = newCamera;
                _haveBeenSet = true;
            }
        }

        public override void AsynchronousOperationStep(object param)
        { }

        public override void OnDestroy()
        { }
    }
}