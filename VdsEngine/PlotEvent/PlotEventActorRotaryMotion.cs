using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class PlotEventActorRotaryMotion : PlotEventBehaviour
    {
        public StringProperty ActorBindingID
        {
            get;
            set;
        }

        public StringProperty ActorRelevanceID
        {
            get;
            set;
        }

        public BoolProperty RelevancePosition
        {
            get;
            set;
        }

        public BoolProperty RelevanceRotation
        {
            get;
            set;
        }

        public StringProperty ActorStatus
        {
            get;
            set;
        }

        public VdsVec3d TargetPose
        {
            get;
            set;
        }

        private VdsQuat _originPose = null;
        private VdsVec3d _originPos = null;
        private VdsQuat _targetPose = null;
        private VdsView _currentView = null;
        private VdsActor _motioObject = null;
        private VdsActor _relevanceObject = null;
        private VdsMatrixd _relevanceMatrixd = new VdsMatrixd();
        private VdsMatrixd _parentLocalToWorld = new VdsMatrixd();

        public PlotEventActorRotaryMotion()
        {
            ActorBindingID = new StringProperty();
            ActorRelevanceID = new StringProperty();
            ActorStatus = new StringProperty();
            TargetPose = new VdsVec3d();
            RelevancePosition = new BoolProperty();
            RelevancePosition.Value = true;
            RelevanceRotation = new BoolProperty();
            RelevanceRotation.Value = true;
        }

        public override void Start()
        {
            if (ActorBindingID.Value == "" || ActorBindingID.Value == null)
                return;
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            InitMotioObject();
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            _behaviourIsWorking = true;
        }

        public override void End()
        {
            if (_currentView != null && _motioObject != null)
                _motioObject = null;
            _relevanceObject = null;
            _relevanceMatrixd = null;
            _parentLocalToWorld = null;
            _behaviourIsWorking = false;
            _originPose = null;
            _originPos = null;
            _targetPose = null;
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
            InitMotioObject();
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (_curTime > pEvent.EventStartTime && _curTime < pEvent.EventStartTime + pEvent.EventDurationTime)
            {
                if (_motioObject != null)
                {
                    VdsVec3d pos = _originPos;
                    double interpolationValue = (_curTime - pEvent.EventStartTime) / pEvent.EventDurationTime;
                    VdsQuat quat = VdsQuat.Slerp(interpolationValue, _originPose, _targetPose);
                    SetActorStatus(_motioObject, ActorStatus.Value, true);
                    VdsMatrixd rotateMt = new VdsMatrixd();
                    rotateMt.MakeRotate(quat);
                    if (_relevanceObject != null)
                    {
                        VdsMatrixd nowMt = new VdsMatrixd();
                        VdsMatrixd localToWorld = new VdsMatrixd();
                        StaticMethod.ComputeCoordinateFrame(_relevanceObject, ref nowMt);
                        StaticMethod.ComputeCoordinateFrame(_motioObject.ParentObject as VdsActor, ref localToWorld);
                        VdsMatrixd worldToLocal = localToWorld.Inverse(localToWorld);
                        if (RelevancePosition.Value && !RelevanceRotation.Value)
                        {
                            nowMt.MakeTranslate(nowMt.GetTrans());
                            pos = nowMt.PreMult(pos);
                            pos = worldToLocal.PreMult(pos);
                            rotateMt.PreMult(nowMt);
                            rotateMt.PreMult(worldToLocal);
                        }
                        else if (!RelevancePosition.Value && RelevanceRotation.Value)
                        {
                            nowMt.MakeRotate(nowMt.GetRotate());
                            pos = _relevanceMatrixd.PreMult(pos);
                            pos = worldToLocal.PreMult(pos);
                            rotateMt.PreMult(nowMt);
                            rotateMt.PreMult(worldToLocal);
                        }
                        else if (RelevancePosition.Value && RelevanceRotation.Value)
                        {
                            rotateMt.PreMult(nowMt);
                            rotateMt.PreMult(worldToLocal);
                            pos = nowMt.PreMult(pos);
                            pos = worldToLocal.PreMult(pos);
                        }
                    }
                    VdsVec3d zr = new VdsVec3d();
                    VdsMatrixd.MatrixToHpr(ref zr, rotateMt);
                    VdsVec3d rotation = new VdsVec3d(0, 0, zr.Z);
                    _motioObject.ActorRotation = rotation;
                    _motioObject.ActorTranslation = pos;
                }
            }
            else if (_motioObject != null)
            {
                SetActorStatus(_motioObject, "DefaultStatus", false);
            }
        }

        public override void AsynchronousOperationStep(object param)
        { }

        public override void OnDestroy()
        { }

        private void InitMotioObject()
        {
            if (_motioObject == null)
            {
                PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(ActorBindingID.Value);
                if (a == null)
                    return;
                _motioObject = a as VdsActor;
                VdsVec3d tRotate = TargetPose;
                VdsVec3d oRotate = _motioObject.ActorRotation;
                VdsMatrixd oMt = new VdsMatrixd();
                VdsMatrixd.HprToMatrix(ref oMt, tRotate);
                _targetPose = oMt.GetRotate();
                VdsMatrixd tMt = new VdsMatrixd();
                VdsMatrixd.HprToMatrix(ref tMt, oRotate);
                _originPose = tMt.GetRotate();
                _originPos = _motioObject.ActorTranslation;
            }
            if (_relevanceObject == null && ActorRelevanceID.Value != "" && (RelevanceRotation.Value || RelevancePosition.Value))
            {
                PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(ActorRelevanceID.Value);
                if (a != null)
                {
                    _relevanceObject = a as VdsActor;
                    if (RelevancePosition.Value || RelevanceRotation.Value)
                    {
                        StaticMethod.ComputeCoordinateFrame(_relevanceObject, ref _relevanceMatrixd);
                        StaticMethod.ComputeCoordinateFrame(_motioObject.ParentObject as VdsActor, ref _parentLocalToWorld);
                        VdsMatrixd worldToRelevance = _relevanceMatrixd.Inverse(_relevanceMatrixd);
                        VdsMatrixd originPoseMt = new VdsMatrixd();
                        originPoseMt.MakeRotate(_originPose);
                        originPoseMt.PreMult(_parentLocalToWorld);
                        originPoseMt.PreMult(worldToRelevance);
                        _originPose = originPoseMt.GetRotate();

                        VdsMatrixd targetPoseMt = new VdsMatrixd();
                        targetPoseMt.MakeRotate(_targetPose);
                        targetPoseMt.PreMult(_parentLocalToWorld);
                        targetPoseMt.PreMult(worldToRelevance);
                        _originPose = targetPoseMt.GetRotate();

                        _originPos = _parentLocalToWorld.PreMult(_originPos);
                        _originPos = worldToRelevance.PreMult(_originPos);
                    }
                }
            }
        }
    }
}