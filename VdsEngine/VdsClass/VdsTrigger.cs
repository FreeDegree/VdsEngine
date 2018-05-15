using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace VdsEngine
{
    public class VdsTrigger : VdsActorBase
    {
        private string _scriptNamespaceAndClassName = null;
        private string _scriptSerializeXml = null;
        private TriggerRecord _triggerTemplate = null;

        public VdsTrigger()
            : base()
        { }

        public VdsTrigger(TriggerRecord triggerTemplate)
            : base()
        {
            _triggerTemplate = triggerTemplate;
        }

        public TriggerRecord ProduceImmediately()
        {
            if (_triggerTemplate != null)
            {
                object copyObj = _triggerTemplate.DeserializeXML(_triggerTemplate.SerializeXML());
                TriggerRecord resultObj = copyObj as TriggerRecord;
                return resultObj;
            }
            if (_scriptNamespaceAndClassName == null)
                return null;
            Assembly[] asmList = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asmList)
            {
                object obj = asm.CreateInstance(_scriptNamespaceAndClassName);
                if (obj != null)
                {
                    TriggerRecord tObj = obj as TriggerRecord;
                    object copyObj = tObj.DeserializeXML(_scriptSerializeXml);
                    TriggerRecord resultObj = copyObj as TriggerRecord;
                    return resultObj;
                }
            }
            return null;
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
                    default:
                        break;
                }
                index++;
            }
        }
    }
}