using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class PlotEventFollowCamera : PlotEventBehaviour
    {
        public StringProperty FollowActorID
        {
            get;
            set;
        }

        public DoubleProperty EyeHeightOffset
        {
            get;
            set;
        }

        public DoubleProperty EyeHorizontalOffset
        {
            get;
            set;
        }

        public BoolProperty WithAnimation
        {
            get;
            set;
        }

        public BoolProperty StayAtStop
        {
            get;
            set;
        }

        private VdsView _currentView = null;
        private bool _haveBeenSet = false;
        private VdsActor _targetObject = null;
        private VdsCamera _preCamera = null;

        public PlotEventFollowCamera()
        {
            FollowActorID = new StringProperty();
            EyeHeightOffset = new DoubleProperty();
            EyeHeightOffset.Value = 4;
            EyeHorizontalOffset = new DoubleProperty();
            EyeHorizontalOffset.Value = 8;
            WithAnimation = new BoolProperty();
            WithAnimation.Value = true;
            StayAtStop = new BoolProperty();
        }

        public override void Start()
        {
            if (FollowActorID.Value == null || FollowActorID.Value == "")
                return;
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(FollowActorID.Value);
            if (a != null)
                _targetObject = a as VdsActor;
            _behaviourIsWorking = true;
            _haveBeenSet = false;
        }

        public override void End()
        {
            _currentView = null;
            _targetObject = null;
            _behaviourIsWorking = false;
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
            if (_targetObject == null)
            {
                PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(FollowActorID.Value);
                if (a != null)
                    _targetObject = a as VdsActor;
                else
                    return;
            }
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (_curTime > pEvent.EventStartTime && _curTime < pEvent.EventStartTime + pEvent.EventDurationTime)
            {
                if (_targetObject != null && !_haveBeenSet)
                {
                    _preCamera = _currentView.MainCamera;
                    VdsCamera newCamera = new VdsCamera();
                    newCamera.TargetActorNativeHandle = _targetObject.NativeHandle;
                    newCamera.CurrentCameraMode = CameraMode.FollowCamera;
                    newCamera.CameraPose.Eye = new VdsVec3d(-EyeHorizontalOffset.Value, 0, EyeHeightOffset.Value);
                    if (_curTime > pEvent.EventStartTime + pEvent.EventDurationTime)
                        newCamera.WithAnimation = false;
                    else
                        newCamera.WithAnimation = WithAnimation.Value;
                    _currentView.MainCamera = newCamera;
                    _haveBeenSet = true;
                }
            }
            else if (_curTime >= pEvent.EventStartTime + pEvent.EventDurationTime && _haveBeenSet && _behaviourIsWorking)
            {
                if (StayAtStop.Value)
                {
                    VdsCamera camera = _currentView.MainCamera;
                    camera.CurrentCameraMode = _preCamera.CurrentCameraMode;
                    _currentView.MainCamera = camera;
                }
                else
                    _currentView.MainCamera = _preCamera;
                _haveBeenSet = false;
            }
        }

        public override void AsynchronousOperationStep(object param)
        { }

        public override void OnDestroy()
        { }
    }
}