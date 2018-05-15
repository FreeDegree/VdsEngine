using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VdsEngine
{
    /// <summary>
    /// 图层
    /// </summary>
    public class VdsLayer : VdsActor, IVdsGroupInterface, IVdsRenderCallback
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static PtrClass IGetChildByID(IntPtr parent, string actorID, bool findChildren);
        #endregion
        public List<PtrClass> ChildrenList
        {
            get
            {
                if (_childrenList == null)
                    return null;
                PtrClass[] vArray = new PtrClass[_childrenList.Count];
                _childrenList.CopyTo(vArray);
                List<PtrClass> resultList = new List<PtrClass>(_childrenList.Count);
                resultList.AddRange(vArray);
                return resultList;
            }
        }

        public int ChildrenSize
        {
            get
            {
                if (_childrenList == null)
                    return 0;
                return _childrenList.Count;
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

        public delegate void CallAddChild(PtrClass child);
        public delegate void CallRemoveChild(PtrClass child);
        protected long _stepNumber = 0;
        protected List<PtrClass> _childrenList = null;
        private CallAddChild _callAddChild = null;
        private CallRemoveChild _callRemoveChild = null;
        private long _lastStepTick = 0;

        public VdsLayer()
            : base()
        { }

        public VdsLayer(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public virtual void AddChild(PtrClass child)
        {
            if (_callAddChild == null)
                _callAddChild = DoAddChild;
            IAsyncResult result = _callAddChild.BeginInvoke(
                    child,
                    delegate(IAsyncResult ar)
                    {
                        _callAddChild.EndInvoke(ar);
                    },
                    null);
            result.AsyncWaitHandle.WaitOne(50, true);
        }

        public virtual void RemoveChild(PtrClass child)
        {
            if (_callRemoveChild == null)
                _callRemoveChild = DoRemoveChild;
            IAsyncResult result = _callRemoveChild.BeginInvoke(
                    child,
                    delegate(IAsyncResult ar)
                    {
                        _callRemoveChild.EndInvoke(ar);
                    },
                    null);
            result.AsyncWaitHandle.WaitOne(50, true);
        }

        public PtrClass GetObjectByID(string idOrBinding)
        {
            if (idOrBinding == "" || idOrBinding == null || _childrenList == null)
                return null;
            for (int i = _childrenList.Count - 1; i >= 0; i--)
            {
                if (_childrenList[i] is PtrClass)
                {
                    PtrClass actor = _childrenList[i] as PtrClass;
                    if (actor.ObjectID == idOrBinding || actor.ObjectBindID == idOrBinding)
                        return actor;
                }
            }
            return null;
        }

        public PtrClass GetActorByNativeHandle(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero || _childrenList == null)
                return null;
            for (int i = _childrenList.Count - 1; i >= 0; i--)
            {
                if (_childrenList[i].NativeHandle == ptr)
                    return _childrenList[i];
            }
            return null;
        }

        public virtual void UpdateStep(object param)
        {
            if (_stepNumber == 0)
                BuildLayerInternal();
            _stepNumber++;
            _lastStepTick = DateTime.Now.Ticks;
            if (ChildrenSize < 1)
                return;
            lock (_locker)
            {
                for (int i = _childrenList.Count - 1; i >= 0; i--)
                {
                    if (_childrenList[i] is VdsGameActor)
                    {
                        VdsGameActor g = (VdsGameActor)_childrenList[i];
                        g.UpdateStep(param);
                    }
                    else if (_childrenList[i] is VdsLayer)
                    {
                        VdsLayer l = (VdsLayer)_childrenList[i];
                        l.UpdateStep(param);
                    }
                }
            }
        }

        public virtual void AsynchronousOperationStep(object param)
        {
            if (ChildrenSize < 1)
                return;
            lock (_locker)
            {
                for (int i = _childrenList.Count - 1; i >= 0; i--)
                {
                    if (_childrenList[i] is VdsGameActor)
                    {
                        VdsGameActor g = (VdsGameActor)_childrenList[i];
                        g.AsynchronousOperationStep(param);
                    }
                    else if (_childrenList[i] is VdsLayer)
                    {
                        VdsLayer l = (VdsLayer)_childrenList[i];
                        l.AsynchronousOperationStep(param);
                    }
                }
            }
        }

        internal PtrClass GetInternalChildByID(string actorID, bool findChildren)
        {
            return IGetChildByID(NativeHandle, actorID, findChildren);
        }

        protected virtual void BuildLayerInternal()
        {
            SetNativeHandleAfter();
            PtrClass[] baseActorList = GetAllChildrenInternal(typeof(VdsActor), true, false);
            if (baseActorList == null)
                return;
            foreach (PtrClass p in baseActorList)
            {
                AddChild(p);
                if (p is VdsLayer)
                {
                    ((VdsLayer)p).BuildLayerInternal();
                }
            }
        }

        private void DoAddChild(PtrClass child)
        {
            AddChildInternal(this.NativeHandle, child.NativeHandle);
            if (_childrenList == null)
                _childrenList = new List<PtrClass>();
            if (_childrenList.Contains(child))
                return;
            lock (_locker)
            {
                _childrenList.Add(child);
            }
            (child as IVdsNodeInterface).SetParent(this);
        }

        private void DoRemoveChild(PtrClass child)
        {
            if (_childrenList != null && _childrenList.Contains(child))
            {
                if (child.IsCreateInScript)
                {
                    RemoveChildInternal(this.NativeHandle, child.NativeHandle);
                }
                lock (_locker)
                {
                    _childrenList.Remove(child);
                }
                (child as IVdsNodeInterface).SetParent(null);
            }
        }

        ~VdsLayer()
        {
            if (_childrenList != null)
            {
                PtrClass[] dList = new PtrClass[_childrenList.Count];
                lock (_locker)
                {
                    _childrenList.CopyTo(dList);
                }
                foreach (PtrClass child in dList)
                {
                    if (child.IsCreateInScript)
                    {
                        RemoveChildInternal(this.NativeHandle, child.NativeHandle);
                    }
                    IVdsNodeInterface node = child as IVdsNodeInterface;
                    node.SetParent(null);
                    child.Dispose();
                }
                lock (_locker)
                {
                    _childrenList.Clear();
                    _childrenList = null;
                }
            }
        }
    }
}