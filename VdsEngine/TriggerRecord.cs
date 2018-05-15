using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public abstract class TriggerRecord : SerializableXML
    {
        public double LifeTimeSecond
        {
            get;
            set;
        }

        public int TriggerMaxTimes
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public bool IsDynamic
        {
            get;
            set;
        }

        public VdsActorBase ParentActor
        {
            get;
            set;
        }

        public VdsVec3d TriggerPosition
        {
            get;
            set;
        }

        private string _triggerID = null;
        private int _triggerTimes = 0;
        private long _startTicks = 0;
        private long _timeTicks = 0;
        private bool _timeOut = false;

        public TriggerRecord()
        {
            _triggerID = System.Guid.NewGuid().ToString();
            LifeTimeSecond = 0;
            TriggerMaxTimes = Int32.MaxValue;
            IsDynamic = false;
            Priority = 3;
        }

        public string TriggerID()
        {
            return _triggerID;
        }

        public void DealWithTrigger()
        {
            _triggerTimes++;
        }

        public virtual void UpdateTrigger()
        {
            if (IsTimeOut())
                return;
            if (IsDynamic)
                TriggerPosition = (ParentActor as VdsGameActor).ActorTranslation;
            if (_triggerTimes >= TriggerMaxTimes)
            {
                SetTimeOut(true);
                return;
            }
            long curTicks = DateTime.Now.Ticks;
            if (_startTicks == 0)
                _startTicks = curTicks;
            SetTimeTicks(curTicks);
            double t = (GetTimeTicks() - _startTicks) / TimeSpan.TicksPerMillisecond / 1000.0;
            if (LifeTimeSecond > 0 && t >= LifeTimeSecond)
                SetTimeOut(true);
        }

        public long GetTimeTicks()
        {
            return _timeTicks;
        }

        public void SetTimeTicks(long ticks)
        {
            _timeTicks = ticks;
        }

        public bool IsTimeOut()
        {
            return _timeOut;
        }

        public void SetTimeOut(bool t)
        {
            _timeOut = t;
        }
    }

    public class RangeTrigger : TriggerRecord
    {
        public double TriggerRadius
        {
            get;
            set;
        }

        public RangeTrigger()
        {
            TriggerRadius = 1;
            IsDynamic = true;
        }

        public override void UpdateTrigger()
        {
            base.UpdateTrigger();
            if (ParentActor != null && ParentActor is VdsGameActor)
                TriggerPosition = ((VdsGameActor)ParentActor).ActorTranslation;
        }
    }

    public class StatusTrigger : TriggerRecord
    {
        public string StatusName
        {
            get
            {
                if (ParentActor != null && ParentActor is VdsGameActor)
                {
                    TriggerPosition = ((VdsGameActor)ParentActor).ActorTranslation;
                    return ((VdsGameActor)ParentActor).ActorStatus;
                }
                return "";
            }
        }

        public string StatusIcon
        {
            get;
            set;
        }

        public StatusTrigger()
        {
            StatusIcon = "";
        }
    }

    public class CollisionTrigger : TriggerRecord
    {
        public double CollisionRangeOffset
        {
            get;
            set;
        }

        public CollisionTrigger()
        {
            CollisionRangeOffset = 0;
            IsDynamic = true;
        }

        public override void UpdateTrigger()
        {
            base.UpdateTrigger();
            if (ParentActor != null && ParentActor is VdsGameActor)
                TriggerPosition = ((VdsGameActor)ParentActor).ActorTranslation;
        }
    }
}