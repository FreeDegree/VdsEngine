using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public class PlotEventBehaviour : MonoBehaviour
    {
        protected double _curTime = -1;
        private List<VdsActor> _actorStatusCacheWait = null;
        private List<VdsActor> _actorStatusCacheUnderway = null;
        private List<VdsActor> _actorStatusCacheFinish = null;

        public PlotEventBehaviour()
        { }

        public override void End()
        {
            _actorStatusCacheWait = null;
            _actorStatusCacheUnderway = null;
            _actorStatusCacheFinish = null;
        }

        protected void SetActorStatus(VdsActor actor, string statusName, bool repeat)
        {
            if (repeat || _curTime < 0)
            {
                actor.ActorStatus = statusName;
                return;
            }
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (_curTime < pEvent.EventStartTime)
            {
                if (_actorStatusCacheWait != null && _actorStatusCacheWait.Contains(actor))
                    return;
                actor.ActorStatus = statusName;
                if (_actorStatusCacheWait == null)
                    _actorStatusCacheWait = new List<VdsActor>();
                _actorStatusCacheWait.Add(actor);
                return;
            }
            if (_curTime > pEvent.EventStartTime && _curTime < pEvent.EventStartTime + pEvent.EventDurationTime)
            {
                if (_actorStatusCacheUnderway != null && _actorStatusCacheUnderway.Contains(actor))
                    return;
                actor.ActorStatus = statusName;
                if (_actorStatusCacheUnderway == null)
                    _actorStatusCacheUnderway = new List<VdsActor>();
                _actorStatusCacheUnderway.Add(actor);
                return;
            }
            if (_actorStatusCacheFinish != null && _actorStatusCacheFinish.Contains(actor))
                return;
            actor.ActorStatus = statusName;
            if (_actorStatusCacheFinish == null)
                _actorStatusCacheFinish = new List<VdsActor>();
            _actorStatusCacheFinish.Add(actor);
        }
    }
}