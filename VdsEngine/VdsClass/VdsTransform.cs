using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VdsEngine
{
    public class VdsTransform : PtrClass, IVdsGroupInterface, IVdsNodeInterface
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void ISetMatrixd(IntPtr thisPtr,
                                            double a00, double a01, double a02, double a03,
                                            double a10, double a11, double a12, double a13,
                                            double a20, double a21, double a22, double a23,
                                            double a30, double a31, double a32, double a33);
        #endregion

        private PtrClass _parentObject = null;
        public delegate void CallAddChild(VdsTransform child);
        public delegate void CallRemoveChild(VdsTransform child);
        private CallAddChild _callAddChild = null;
        private CallRemoveChild _callRemoveChild = null;
        protected VdsMatrixd _mt = null;
        protected List<VdsTransform> _childrenList = null;
        protected object _locker = new object();

        public PtrClass ParentObject
        {
            get
            {
                return _parentObject;
            }
        }

        public List<PtrClass> ChildrenList
        {
            get
            {
                if (_childrenList == null)
                    return null;
                VdsTransform[] vArray = new VdsTransform[_childrenList.Count];
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

        public VdsMatrixd Transform
        {
            get
            {
                return _mt;
            }
            set
            {
                _mt = value;
                ISetMatrixd(NativeHandle, _mt.Mat[0, 0], _mt.Mat[0, 1], _mt.Mat[0, 2], _mt.Mat[0, 3],
                                           _mt.Mat[1, 0], _mt.Mat[1, 1], _mt.Mat[1, 2], _mt.Mat[1, 3],
                                           _mt.Mat[2, 0], _mt.Mat[2, 1], _mt.Mat[2, 2], _mt.Mat[2, 3],
                                           _mt.Mat[3, 0], _mt.Mat[3, 1], _mt.Mat[3, 2], _mt.Mat[3, 3]);
            }
        }

        public VdsTransform()
            : base()
        { }

        public VdsTransform(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public virtual void AddChild(PtrClass child)
        {
            if (child is VdsTransform)
            {
                if (_callAddChild == null)
                    _callAddChild = DoAddChild;
                IAsyncResult result = _callAddChild.BeginInvoke(
                        child as VdsTransform,
                        delegate(IAsyncResult ar)
                        {
                            _callAddChild.EndInvoke(ar);
                        },
                        null);
                result.AsyncWaitHandle.WaitOne(50, true);
            }
        }

        public virtual void RemoveChild(PtrClass child)
        {
            if (child is VdsTransform)
            {
                if (_callRemoveChild == null)
                    _callRemoveChild = DoRemoveChild;
                IAsyncResult result = _callRemoveChild.BeginInvoke(
                        child as VdsTransform,
                        delegate(IAsyncResult ar)
                        {
                            _callRemoveChild.EndInvoke(ar);
                        },
                        null);
                result.AsyncWaitHandle.WaitOne(50, true);
            }
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

        public PtrClass GetObjectByID(string idOrBinding)
        {
            if (idOrBinding == "" || idOrBinding == null)
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

        protected virtual bool ContainsChildInternal(VdsTransform child, bool findChildren)
        {
            return ContainsChildInternal(NativeHandle, child.NativeHandle, findChildren);
        }

        public virtual void SetParent(PtrClass parent)
        {
            _parentObject = parent;
            if (parent != null)
                ObjectViewID = parent.ObjectViewID;
            else
                ObjectViewID = -1;
        }

        private void DoAddChild(VdsTransform child)
        {
            AddChildInternal(this.NativeHandle, child.NativeHandle);
            if (_childrenList == null)
                _childrenList = new List<VdsTransform>();
            if (_childrenList.Contains(child))
                return;
            lock (_locker)
            {
                _childrenList.Add(child);
            }
            child.SetParent(this);
        }

        private void DoRemoveChild(VdsTransform child)
        {
            if (_childrenList != null && _childrenList.Contains(child))
            {
                RemoveChildInternal(this.NativeHandle, child.NativeHandle);
                lock (_locker)
                {
                    _childrenList.Remove(child);
                }
                child.SetParent(null);
            }
        }

        ~VdsTransform()
        {
            if (_childrenList != null)
            {
                VdsTransform[] dList = new VdsTransform[_childrenList.Count];
                lock (_locker)
                {
                    _childrenList.CopyTo(dList);
                }
                foreach (VdsTransform child in dList)
                {
                    RemoveChildInternal(this.NativeHandle, child.NativeHandle);
                    child.SetParent(null);
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