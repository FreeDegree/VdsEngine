using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VdsEngine
{
    public sealed class PlotEventActorCreate : PlotEventBehaviour
    {
        public StringProperty ActorBindingID
        {
            get;
            set;
        }

        public FilePathProperty ActorResource
        {
            get;
            set;
        }

        public StringProperty ActorStatus
        {
            get;
            set;
        }

        public VdsVec3d ActorPosition
        {
            get;
            set;
        }

        public VdsVec3d ActorRotation
        {
            get;
            set;
        }

        private VdsView _currentView = null;
        private VdsActor _createObject = null;
        private bool _haveBeenSet = false;

        public PlotEventActorCreate()
        {
            ActorBindingID = new StringProperty();
            ActorResource = new FilePathProperty();
            ActorStatus = new StringProperty();
            ActorPosition = new VdsVec3d();
            ActorRotation = new VdsVec3d();
        }

        public override void Start()
        {
            if (ActorBindingID.Value == "" || ActorBindingID.Value == null)
                return;
            _haveBeenSet = false;
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(ActorBindingID.Value);
            if (a == null)
            {
                if (ActorResource.Value == "")
                    return;
                else
                {
                    string sourceStr = Path.GetFileName(ActorResource.Value);
                    if (sourceStr.EndsWith(".vds"))
                        _createObject = new VdsActor(sourceStr);
                    else if (sourceStr.EndsWith(".vdsa"))
                        _createObject = new VdsGameActor(sourceStr);
                    else
                        return;
                }
            }
            else
                _createObject = a as VdsActor;
            if (_createObject.NativeHandle == IntPtr.Zero)
            {
                _createObject = null;
                return;
            }
            if (a == null)
            {
                _createObject.ObjectBindID = ActorBindingID.Value;
                _createObject.ActorStatus = "DefaultStatus";
            }
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            _behaviourIsWorking = true;
        }

        public override void End()
        {
            if (_currentView != null && _createObject != null && _haveBeenSet)
            {
                ((IVdsGroupInterface)_currentView.GameLayer).RemoveChild(_createObject);
                _createObject = null;
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
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (!_haveBeenSet && _curTime > pEvent.EventStartTime)
            {
                if (_createObject != null)
                {
                    _createObject.ActorStatus = ActorStatus.Value;
                    _createObject.ActorTranslation = ActorPosition;
                    _createObject.ActorRotation = ActorRotation;
                    ((IVdsGroupInterface)_currentView.GameLayer).AddChild(_createObject);
                    _haveBeenSet = true;
                }
            }
        }

        public override void AsynchronousOperationStep(object param)
        { }

        public override void OnDestroy()
        { }
    }
}