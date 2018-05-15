using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace VdsEngine
{
    public class VdsGameActor : VdsActor, IVdsRenderCallback
    {
        public MonoBehaviour ScriptBehaviour
        {
            get
            {
                if (_scriptBehaviour == null && _scriptNamespaceAndClassName != null && _scriptSerializeXml != null
                    && _scriptNamespaceAndClassName != "" && _scriptSerializeXml != "")
                {
                    Assembly[] asmList = System.AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly asm in asmList)
                    {
                        object obj = asm.CreateInstance(_scriptNamespaceAndClassName);
                        if (obj != null)
                        {
                            MonoBehaviour mObj = obj as MonoBehaviour;
                            object copyObj = mObj.DeserializeXML(_scriptSerializeXml);
                            _scriptBehaviour = copyObj as MonoBehaviour;
                            _scriptBehaviour.ParentActor = this;
                            break;
                        }
                    }
                }
                return _scriptBehaviour;
            }
            set
            {
                if (value != null)
                {
                    _scriptBehaviour = value;
                    _scriptBehaviour.ParentActor = this;
                }
                else
                {
                    _scriptBehaviour = null;
                    _scriptNamespaceAndClassName = null;
                    _scriptSerializeXml = null;
                }
            }
        }

        public List<TriggerRecord> TriggerList
        {
            get;
            set;
        }

        public List<string> TriggerResponseList
        {
            get;
            set;
        }

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

        private MonoBehaviour _scriptBehaviour = null;
        private MonoBehaviour _activeScriptBehaviour = null;
        private string _scriptNamespaceAndClassName = null;
        private string _scriptSerializeXml = null;
        private long _stepNumber = 0;
        private long _lastStepTick = 0;
        private VdsView _currentView = null;

        public VdsGameActor()
            : base()
        { }

        public VdsGameActor(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public VdsGameActor(string resName)
            : base(resName)
        { }

        public VdsGameActor(VdsActor cloneFrom)
            : base(cloneFrom)
        { }

        public override void CloneActor(VdsActorBase cloneFrom)
        {
            base.CloneActor(cloneFrom);
        }

        public void AddTriggerRecord(TriggerRecord trigger)
        {
            if (TriggerList == null)
                TriggerList = new List<TriggerRecord>();
            trigger.ParentActor = this;
            TriggerList.Add(trigger);
            //add into TriggerSystem
            long nowTick = DateTime.Now.Ticks;
            double interval = (nowTick - _lastStepTick) / TimeSpan.TicksPerMillisecond;
            if (interval < 50)
                ((TriggerSystem)_currentView.MainTriggerSystem).RegisterTrigger(trigger);
        }

        public void RemoveTriggerRecord(TriggerRecord trigger)
        {
            if (TriggerList == null)
                return;
            TriggerList.Remove(trigger);
            //remove from TriggerSystem
            long nowTick = DateTime.Now.Ticks;
            double interval = (nowTick - _lastStepTick) / TimeSpan.TicksPerMillisecond;
            if (interval < 50)
                ((TriggerSystem)_currentView.MainTriggerSystem).UnregisterTrigger(trigger);
        }

        public void AddTriggerResponse(string triggerName)
        {
            if (TriggerResponseList == null)
                TriggerResponseList = new List<string>();
            if (TriggerResponseList.Contains(triggerName))
                return;
            TriggerResponseList.Add(triggerName);
            //add into TriggerSystem
            long nowTick = DateTime.Now.Ticks;
            double interval = (nowTick - _lastStepTick) / TimeSpan.TicksPerMillisecond;
            if (interval < 50)
                ((TriggerSystem)_currentView.MainTriggerSystem).RegisterResponder(_activeScriptBehaviour, triggerName);
        }

        public void RemoveTriggerResponse(string triggerName)
        {
            if (TriggerResponseList == null)
                return;
            TriggerResponseList.Remove(triggerName);
            //remove from TriggerSystem
            long nowTick = DateTime.Now.Ticks;
            double interval = (nowTick - _lastStepTick) / TimeSpan.TicksPerMillisecond;
            if (interval < 50)
                ((TriggerSystem)_currentView.MainTriggerSystem).UnregisterResponder(_activeScriptBehaviour, triggerName);
        }

        public virtual void UpdateStep(object param)
        {
            if (_stepNumber == 0)
                ActivateGameActor();
            _stepNumber++;
            if (_activeScriptBehaviour != null && _activeScriptBehaviour.BehaviourIsStart)
            {
                _activeScriptBehaviour.UpdateStep(param);
                if (_activeScriptBehaviour.BehaviourIsWorking())
                    _activeScriptBehaviour.SetLastUpdateTick(DateTime.Now.Ticks);
            }
        }

        public virtual void AsynchronousOperationStep(object param)
        {
            if (_currentView != null)
            {
                long nowTick = DateTime.Now.Ticks;
                double interval = (nowTick - _lastStepTick) / TimeSpan.TicksPerMillisecond;
                _lastStepTick = nowTick;
                if (_activeScriptBehaviour != ScriptBehaviour)
                {
                    if (_activeScriptBehaviour != null)
                    {
                        foreach (string typeName in TriggerResponseList)
                        {
                            ((TriggerSystem)_currentView.MainTriggerSystem).UnregisterResponder(_activeScriptBehaviour, typeName);
                        }
                        foreach (TriggerRecord trigger in TriggerList)
                        {
                            ((TriggerSystem)_currentView.MainTriggerSystem).UnregisterTrigger(trigger);
                        }
                        if (interval < 3000)
                            _lastStepTick = 0;
                        _scriptBehaviour.End();
                        _scriptBehaviour.OnDestroy();
                    }
                    _activeScriptBehaviour = ScriptBehaviour;
                }
                if (interval > 3000)
                {
                    if (TriggerResponseList != null && TriggerResponseList.Count > 0 && _activeScriptBehaviour != null)
                    {
                        foreach (string typeName in TriggerResponseList)
                        {
                            ((TriggerSystem)_currentView.MainTriggerSystem).RegisterResponder(_activeScriptBehaviour, typeName);
                        }
                    }
                    if (TriggerList != null)
                    {
                        foreach (TriggerRecord trigger in TriggerList)
                        {
                            ((TriggerSystem)_currentView.MainTriggerSystem).RegisterTrigger(trigger);
                        }
                    }
                }
                if (_activeScriptBehaviour != null)
                {
                    if (!_activeScriptBehaviour.BehaviourIsStart)
                    {
                        _activeScriptBehaviour.Start();
                        _activeScriptBehaviour.BehaviourIsStart = true;
                    }
                    else
                        _activeScriptBehaviour.AsynchronousOperationStep(param);
                }
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
                    case "NamespaceAndClassName":
                        {
                            _scriptNamespaceAndClassName = propertyValueList[index];
                            break;
                        }
                    case "SerializeXml":
                        {
                            _scriptSerializeXml = propertyValueList[index];
                            break;
                        }
                    case "TriggerResponseList":
                        {
                            string vstr = propertyValueList[index];
                            string[] tArray = vstr.Split(',');
                            if (tArray.Length > 0)
                            {
                                if (TriggerResponseList == null)
                                    TriggerResponseList = new List<string>();
                                TriggerResponseList.Clear();
                                TriggerResponseList.AddRange(tArray);
                            }
                            break;
                        }
                    default:
                        break;
                }
                index++;
            }
        }

        internal void StopScript()
        {
            if (_activeScriptBehaviour != null)
                _activeScriptBehaviour.End();
        }

        private void ActivateGameActor()
        {
            if (_currentView == null)
                _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(ObjectViewID));
            PtrClass[] baseActorList = GetAllChildrenInternal(typeof(VdsTrigger), true, false);
            if (baseActorList == null)
                return;
            foreach (PtrClass p in baseActorList)
            {
                VdsTrigger t = p as VdsTrigger;
                AddTriggerRecord(t.ProduceImmediately());
            }
        }
    }
}