using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VdsEngine
{
    public class VdsActorBase : PtrClass, IVdsNodeInterface
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static IntPtr ICreateActorByCloneFrom(IntPtr thisPtr);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IGetActorPropertyList(IntPtr actorPtr, out string[] propertyNameList, out string[] propertyValueList);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static string IGetActorProperty(IntPtr actorPtr, string propertyName);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void ISetActorProperty(IntPtr actorPtr, string propertyName, string propertyValue);
        #endregion

        public PtrClass ParentObject
        {
            get
            {
                return _parentObject;
            }
        }

        private VdsActorBase _parentObject = null;
        protected string[] _propertyNameList = null;
        protected string[] _propertyValueList = null;
        protected object _locker = new object();

        public VdsActorBase()
            : base()
        { }

        public VdsActorBase(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public virtual void CloneActor(VdsActorBase cloneFrom)
        {
            SetNativeHandle(ICreateActorByCloneFrom(cloneFrom.NativeHandle));
        }

        public virtual void SetParent(PtrClass parent)
        {
            if (parent is VdsActorBase)
            {
                _parentObject = parent as VdsActorBase;
                ObjectViewID = _parentObject.ObjectViewID;
            }
            else
            {
                _parentObject = null;
                ObjectViewID = -1;
            }
        }

        protected void SetActorProperty(string propertyName, string propertyValue)
        {
            for (int i = 0; i < _propertyNameList.Length; ++i)
            {
                if (_propertyNameList[i] == propertyName)
                {
                    _propertyValueList[i] = propertyValue;
                    ISetActorProperty(NativeHandle, propertyName, propertyValue);
                    break;
                }
            }
        }

        protected string GetActorProperty(string propertyName)
        {
            for (int i = 0; i < _propertyNameList.Length; ++i)
            {
                if (_propertyNameList[i] == propertyName)
                {
                    _propertyValueList[i] = IGetActorProperty(NativeHandle, propertyName);
                    return _propertyValueList[i];
                }
            }
            return null;
        }

        protected override void SetNativeHandleAfter()
        {
            if (_propertyNameList == null)
            {
                string[] propertyNameList, propertyValueList;
                IGetActorPropertyList(NativeHandle, out propertyNameList, out propertyValueList);
                InitActorPropertyList(propertyNameList, propertyValueList);
            }
        }

        protected virtual void InitActorPropertyList(string[] propertyNameList, string[] propertyValueList)
        {
            _propertyNameList = propertyNameList;
            _propertyValueList = propertyValueList;
            int index = 0;
            foreach (string name in _propertyNameList)
            {
                switch (name)
                {
                    case "id":
                        ObjectID = propertyValueList[index];
                        break;
                    case "bindid":
                        ObjectBindID = propertyValueList[index];
                        break;
                    case "viewid":
                        ObjectViewID = Convert.ToInt32(propertyValueList[index]);
                        break;
                    default:
                        break;
                }
                index++;
            }
        }
    }
}