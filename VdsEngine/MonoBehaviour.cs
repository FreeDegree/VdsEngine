using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VdsEngine
{
    public class MonoBehaviour : SerializableXML, IVdsRenderCallback
    {
        private int _viewID = -1;
        private long _lastUpdateTick = 0;
        private long _stepNumber = 0;
        private long _lastStepTick = 0;
        protected bool _behaviourIsStart = false;
        protected bool _behaviourIsSleep = false;
        protected bool _behaviourIsWorking = false;
        public int ViewID
        {
            get
            {
                return _viewID;
            }
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

        internal bool BehaviourIsStart
        {
            get
            {
                return _behaviourIsStart;
            }
            set
            {
                _behaviourIsStart = value;
            }
        }

        internal bool DealWithTrigger
        {
            get;
            set;
        }
        /// <summary>
        /// 脚本所有者
        /// </summary>
        public VdsActor ParentActor
        {
            get;
            internal set;
        }

        public MonoBehaviour()
        { }
        /// <summary>
        /// 初始化，第一次异步调用
        /// </summary>
        public virtual void Start()
        {
            ///自己判是否需要注册view事件
        }
        /// <summary>
        /// 结束
        /// </summary>
        public virtual void End()
        { }
        /// <summary>
        /// 同步调用
        /// </summary>
        public virtual void UpdateStep(object param)
        {
            _stepNumber++;
            _lastStepTick = DateTime.Now.Ticks;
        }
        /// <summary>
        /// 异步调用
        /// </summary>
        public virtual void AsynchronousOperationStep(object param)
        { }
        /// <summary>
        /// 触发器进入
        /// </summary>
        /// <param name="trigger"></param>
        public virtual bool OnTrigger(TriggerRecord trigger)
        {
            return false;
        }
        /// <summary>
        /// 析构调用
        /// </summary>
        public virtual void OnDestroy()
        { }
        /// <summary>
        /// 克隆，通过序列化实现
        /// </summary>
        /// <returns></returns>
        public virtual object OnClone()
        {
            return DeserializeXML(SerializeXML());
        }
        /// <summary>
        /// 脚本是否正在运行,初始化默认是false
        /// </summary>
        /// <returns></returns>
        public bool BehaviourIsWorking()
        {
            return _behaviourIsWorking;
        }
        /// <summary>
        /// 最后更新时刻
        /// </summary>
        /// <returns></returns>
        public long LastUpdateTick()
        {
            return _lastUpdateTick;
        }
        /// <summary>
        /// 设置最后更新时刻
        /// </summary>
        /// <param name="t"></param>
        internal void SetLastUpdateTick(long t)
        {
            _lastUpdateTick = t;
        }

        private void GetPropertyList(out string[] nameList, out string[] typeList, out string[] valueList)
        {
            PropertyInfo[] pInfoList = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            List<string> nList = new List<string>();
            List<string> tList = new List<string>();
            List<string> vList = new List<string>();
            foreach (PropertyInfo info in pInfoList)
            {
                if (info.CanRead && info.CanWrite)
                {
                    object propertyObject = info.GetValue(this, null);
                    if (propertyObject is IPropertyUIInterface)
                    {
                        IPropertyUIInterface obj = propertyObject as IPropertyUIInterface;
                        nList.Add(info.Name);
                        tList.Add(info.PropertyType.Name);
                        vList.Add(obj.PropertyToString());
                    }
                }
            }
            nameList = nList.ToArray();
            typeList = tList.ToArray();
            valueList = vList.ToArray();
        }

        private void SetPropertyList(string[] nameList, string[] typeList, string[] valueList)
        {
            PropertyInfo[] pInfoList = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in pInfoList)
            {
                if (info.CanRead && info.CanWrite)
                {
                    int index = -1;
                    for (int i = 0; i < nameList.Length; ++i)
                    {
                        if (nameList[i] == info.Name)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index < 0)
                        continue;
                    object obj = Activator.CreateInstance(info.PropertyType);
                    IPropertyUIInterface newProperty = obj as IPropertyUIInterface;
                    newProperty.PropertyFromString(valueList[index]);
                    info.SetValue(this, newProperty, null);
                }
            }
        }

        ~MonoBehaviour()
        {
            OnDestroy();
        }
    }
}