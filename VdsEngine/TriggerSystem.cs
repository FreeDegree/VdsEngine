using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public class TriggerSystem : IVdsRenderCallback
    {
        public long StepNumber
        {
            get
            {
                return _stepNumber;
            }
        }

        public long LastStepTick
        {
            get
            {
                return _lastStepTick;
            }
        }

        private Dictionary<string, List<MonoBehaviour>> _responderBehaviourMap = null;
        private List<TriggerRecord> _triggerList = null;
        private long _stepNumber = 0;
        private long _lastStepTick = 0;
        protected object _locker = new object();

        public TriggerSystem()
        { }

        public void RegisterResponder(object responder, string triggerTypeName)
        {
            if (_responderBehaviourMap == null)
                _responderBehaviourMap = new Dictionary<string, List<MonoBehaviour>>();
            if (responder is MonoBehaviour)
            {
                MonoBehaviour b = responder as MonoBehaviour;
                if (!_responderBehaviourMap.ContainsKey(triggerTypeName))
                    _responderBehaviourMap.Add(triggerTypeName, new List<MonoBehaviour>());
                if (!_responderBehaviourMap[triggerTypeName].Contains(b))
                {
                    lock (_locker)
                    {
                        _responderBehaviourMap[triggerTypeName].Add(b);
                    }
                }
            }
        }

        public void UnregisterResponder(object responder, string triggerTypeName)
        {
            if (_responderBehaviourMap == null)
                return;
            if (responder is MonoBehaviour)
            {
                if (!_responderBehaviourMap.ContainsKey(triggerTypeName))
                    return;
                MonoBehaviour b = responder as MonoBehaviour;
                lock (_locker)
                {
                    _responderBehaviourMap[triggerTypeName].Remove(b);
                }
            }
        }

        public void RegisterTrigger(object obj)
        {
            if (_triggerList == null)
                _triggerList = new List<TriggerRecord>();
            if (obj is TriggerRecord)
            {
                TriggerRecord trigger = obj as TriggerRecord;
                int i = 0;
                for (; i < _triggerList.Count; ++i)
                {
                    if (_triggerList[i].Priority > trigger.Priority)
                        break;
                }
                lock (_locker)
                {
                    _triggerList.Insert(i, trigger);
                }
            }
        }

        public void UnregisterTrigger(object obj)
        {
            if (_triggerList == null)
                return;
            if (obj is TriggerRecord)
            {
                TriggerRecord trigger = obj as TriggerRecord;
                lock (_locker)
                {
                    _triggerList.Remove(trigger);
                }
            }
        }

        public void UpdateStep(object param)
        {
            _stepNumber++;
            if (_triggerList == null)
                return;
            if (_responderBehaviourMap == null)
                return;
            lock (_locker)
            {
                foreach (TriggerRecord trigger in _triggerList)
                {
                    trigger.UpdateTrigger();
                    bool dealWithTrigger = false;
                    if (!trigger.IsTimeOut())
                    {
                        string triggerName = trigger.GetType().FullName;
                        if (_responderBehaviourMap.ContainsKey(triggerName))
                        {
                            foreach (MonoBehaviour script in _responderBehaviourMap[triggerName])
                            {
                                //gameactor deal with trigger only one time every step
                                if (!script.DealWithTrigger && script.ParentActor != trigger.ParentActor)
                                {
                                    if (script.OnTrigger(trigger))
                                    {
                                        script.DealWithTrigger = true;
                                        dealWithTrigger = true;
                                    }
                                }
                            }
                        }
                    }
                    if (dealWithTrigger)
                        trigger.DealWithTrigger();
                }
                //reduction script Trigger state
                foreach (var item in _responderBehaviourMap)
                {
                    foreach (MonoBehaviour script in item.Value)
                    {
                        script.DealWithTrigger = false;
                    }
                }
            }
        }

        public void AsynchronousOperationStep(object param)
        {
            if (_triggerList == null)
                return;
            if (_responderBehaviourMap == null)
                return;
            _lastStepTick = DateTime.Now.Ticks;
            ///remove trigger out of date
            for (int i = _triggerList.Count - 1; i >= 0; i--)
            {
                long lastTick = ((IVdsRenderCallback)_triggerList[i].ParentActor).LastStepTick;
                if (_triggerList[i].IsTimeOut() || (_lastStepTick - lastTick) / TimeSpan.TicksPerMillisecond > 2900)
                {
                    lock (_locker)
                    {
                        _triggerList.RemoveAt(i);
                    }
                }
            }
            ///remove Responder out of date
            foreach (var item in _responderBehaviourMap)
            {
                for (int i = item.Value.Count - 1; i >= 0; i--)
                {
                    long lastTick = ((IVdsRenderCallback)item.Value[i].ParentActor).LastStepTick;
                    if ((_lastStepTick - lastTick) / TimeSpan.TicksPerMillisecond > 2900)
                    {
                        lock (_locker)
                        {
                            item.Value.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}