using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class PlotEventActorTranslationMotion : PlotEventBehaviour
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

        public VdsVec3dList ActorMotionKeyPoints
        {
            get;
            set;
        }

        public BoolProperty MotionAutoAhead
        {
            get;
            set;
        }

        private VdsView _currentView = null;
        private VdsActor _motioObject = null;
        private VdsVec3d _motionObjectOriginPos = null;
        private VdsVec3d _motionObjectOriginDirection = null;
        private VdsActor _relevanceObject = null;
        private VdsMatrixd _relevanceMatrixd = new VdsMatrixd();
        private double _motionSpeed = 0;

        public PlotEventActorTranslationMotion()
        {
            ActorBindingID = new StringProperty();
            ActorRelevanceID = new StringProperty();
            ActorStatus = new StringProperty();
            ActorMotionKeyPoints = new VdsVec3dList();
            RelevancePosition = new BoolProperty();
            RelevancePosition.Value = true;
            RelevanceRotation = new BoolProperty();
            RelevanceRotation.Value = true;
            MotionAutoAhead = new BoolProperty();
            MotionAutoAhead.Value = true;
        }

        public override void Start()
        {
            if (ActorBindingID.Value == "" || ActorBindingID.Value == null)
                return;
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            InitMotioObject();
            double totalLength = 0;
            for (int i = 0; i < ActorMotionKeyPoints.ValueList.Count - 1; ++i)
            {
                totalLength += (ActorMotionKeyPoints.ValueList[i] - ActorMotionKeyPoints.ValueList[i + 1]).Length();
            }
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            _motionSpeed = totalLength / pEvent.EventDurationTime;
            _behaviourIsWorking = true;
        }

        public override void End()
        {
            if (_currentView != null && _motioObject != null)
                _motioObject = null;
            _relevanceObject = null;
            _relevanceMatrixd = null;
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
            InitMotioObject();
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (_curTime > pEvent.EventStartTime && _curTime < pEvent.EventStartTime + pEvent.EventDurationTime)
            {
                if (_motioObject != null)
                {
                    VdsVec3d pos = new VdsVec3d();
                    VdsVec3d direction = new VdsVec3d();
                    GetTranslationAndRotation(_motioObject, _curTime - pEvent.EventStartTime, out pos, out direction);
                    SetActorStatus(_motioObject, ActorStatus.Value, true);
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
                        }
                        else if (!RelevancePosition.Value && RelevanceRotation.Value)
                        {
                            nowMt.MakeRotate(nowMt.GetRotate());
                            direction = nowMt.PreMult(direction);
                            direction = worldToLocal.PreMult(direction);
                            VdsVec3d zPos = new VdsVec3d();
                            zPos = nowMt.PreMult(zPos);
                            zPos = worldToLocal.PreMult(zPos);
                            direction = direction - zPos;

                            pos = _relevanceMatrixd.PreMult(pos);
                            pos = worldToLocal.PreMult(pos);
                        }
                        else if (RelevancePosition.Value && RelevanceRotation.Value)
                        {
                            direction = nowMt.PreMult(direction);
                            direction = worldToLocal.PreMult(direction);
                            VdsVec3d zPos = new VdsVec3d();
                            zPos = nowMt.PreMult(zPos);
                            zPos = worldToLocal.PreMult(zPos);
                            direction = direction - zPos;

                            pos = nowMt.PreMult(pos);
                            pos = worldToLocal.PreMult(pos);
                        }
                    }
                    direction.Normalize();
                    VdsVec3d zr = new VdsVec3d();
                    VdsMatrixd rMt = new VdsMatrixd();
                    rMt.MakeRotate(new VdsVec3d(1, 0, 0), direction);
                    VdsMatrixd.MatrixToHpr(ref zr, rMt);
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
                _motionObjectOriginPos = _motioObject.ActorTranslation;
                VdsVec3d direction = new VdsVec3d(1, 0, 0);
                VdsMatrixd rMt = new VdsMatrixd();
                VdsMatrixd.HprToMatrix(ref rMt, _motioObject.ActorRotation);
                direction = rMt.PreMult(direction);
                _motionObjectOriginDirection = direction;
            }
            if (_relevanceObject == null && ActorRelevanceID.Value != "" && (RelevanceRotation.Value || RelevancePosition.Value))
            {
                PtrClass a = ((IVdsGroupInterface)_currentView.GameLayer).GetObjectByID(ActorRelevanceID.Value);
                if (a != null)
                {
                    _relevanceObject = a as VdsActor;
                    if (RelevancePosition.Value || RelevanceRotation.Value)
                    {
                        VdsMatrixd localToWorld = new VdsMatrixd();
                        StaticMethod.ComputeCoordinateFrame(_motioObject.ParentObject as VdsActor, ref localToWorld);
                        StaticMethod.ComputeCoordinateFrame(_relevanceObject, ref _relevanceMatrixd);
                        VdsMatrixd worldToRelevance = _relevanceMatrixd.Inverse(_relevanceMatrixd);

                        _motionObjectOriginDirection = localToWorld.PreMult(_motionObjectOriginDirection);
                        _motionObjectOriginDirection = worldToRelevance.PreMult(_motionObjectOriginDirection);
                        VdsVec3d zPos = new VdsVec3d();
                        zPos = localToWorld.PreMult(zPos);
                        zPos = worldToRelevance.PreMult(zPos);
                        _motionObjectOriginDirection = _motionObjectOriginDirection - zPos;
                        _motionObjectOriginDirection.Normalize();
                        _motionObjectOriginPos = localToWorld.PreMult(_motionObjectOriginPos);
                        _motionObjectOriginPos = worldToRelevance.PreMult(_motionObjectOriginPos);
                        List<VdsVec3d> newKeyPointsList = new List<VdsVec3d>(ActorMotionKeyPoints.ValueList.Count);
                        foreach (VdsVec3d v in ActorMotionKeyPoints.ValueList)
                        {
                            VdsVec3d newV = localToWorld.PreMult(v);
                            newV = worldToRelevance.PreMult(newV);
                            newKeyPointsList.Add(newV);
                        }
                        ActorMotionKeyPoints.ValueList = newKeyPointsList;
                    }
                }
            }
        }

        private void GetTranslationAndRotation(VdsActor actor, double timeOffset, out VdsVec3d translation, out VdsVec3d direction)
        {
            direction = _motionObjectOriginDirection;
            if (_motionSpeed == 0)
            {
                translation = _motionObjectOriginPos;
                return;
            }
            double timeTotal = 0;
            for (int i = 0; i < ActorMotionKeyPoints.ValueList.Count - 1; ++i)
            {
                double eLength = (ActorMotionKeyPoints.ValueList[i] - ActorMotionKeyPoints.ValueList[i + 1]).Length();
                timeTotal += eLength / _motionSpeed;
                if (timeTotal >= timeOffset)
                {
                    if (MotionAutoAhead.Value)
                    {
                        direction = ActorMotionKeyPoints.ValueList[i + 1] - ActorMotionKeyPoints.ValueList[i];
                        direction.Normalize();
                    }
                    translation = ActorMotionKeyPoints.ValueList[i + 1] - direction * (timeTotal - timeOffset) * _motionSpeed;
                    return;
                }
            }
            if (MotionAutoAhead.Value)
            {
                direction = ActorMotionKeyPoints.ValueList[ActorMotionKeyPoints.ValueList.Count - 1] - ActorMotionKeyPoints.ValueList[ActorMotionKeyPoints.ValueList.Count - 2];
                direction.Normalize();
            }
            translation = ActorMotionKeyPoints.ValueList[ActorMotionKeyPoints.ValueList.Count - 1];
        }
    }
}