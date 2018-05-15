using System;
using System.Runtime.CompilerServices;

namespace VdsEngine
{
    public class PtrClass : IDisposable
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static IntPtr ICreate(string className, int viewID);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IAddChild(IntPtr parent, IntPtr child, bool asyn);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IRemoveChild(IntPtr parent, IntPtr child, bool asyn);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static bool IContainsChild(IntPtr parent, IntPtr child, bool findChildren);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IDispose(IntPtr thisPtr);
        #endregion

        public IntPtr NativeHandle
        {
            get
            {
                return _nativeHandle;
            }
            private set
            {
                if (_nativeHandle == value)
                    return;
                if (_nativeHandle != IntPtr.Zero)
                {
                    if (IsCreateInScript)
                        IDispose(_nativeHandle);
                }
                _nativeHandle = value;
                SetNativeHandleAfter();
            }
        }

        public bool IsCreateInScript
        {
            get;
            private set;
        }

        public int ObjectViewID
        {
            get;
            protected set;
        }

        public string ObjectID
        {
            get;
            protected set;
        }

        public string ObjectBindID
        {
            get;
            set;
        }

        public string ObjectName
        {
            get;
            set;
        }

        private IntPtr _nativeHandle = IntPtr.Zero;

        public PtrClass()
            : this(false)
        {
            IsCreateInScript = false;
        }

        public PtrClass(bool initNativeHandle)
        {
            IsCreateInScript = false;
            if (initNativeHandle)
                SetNativeHandle(CreateInternal(this.GetType().FullName, 0));
        }

        protected void SetNativeHandle(IntPtr ptr)
        {
            NativeHandle = ptr;
        }

        public void Dispose()
        {
            if (_nativeHandle != IntPtr.Zero)
            {
                if (IsCreateInScript)
                    IDispose(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }
        }

        internal IntPtr CreateInternal(string className, int viewID)
        {
            IsCreateInScript = true;
            return ICreate(className, viewID);
        }

        internal void AddChildInternal(IntPtr parent, IntPtr child)
        {
            int updateThreadID = GlobalEnvironment.UpdateThreadID;
            int curThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            IAddChild(parent, child, updateThreadID != curThreadID);
        }

        internal void RemoveChildInternal(IntPtr parent, IntPtr child)
        {
            int updateThreadID = GlobalEnvironment.UpdateThreadID;
            int curThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            IRemoveChild(parent, child, updateThreadID != curThreadID);
        }

        internal bool ContainsChildInternal(IntPtr parent, IntPtr child, bool findChildren)
        {
            return IContainsChild(parent, child, findChildren);
        }

        protected virtual void SetNativeHandleAfter()
        { }

        ~PtrClass()
        {
            Dispose();
        }
    }
}