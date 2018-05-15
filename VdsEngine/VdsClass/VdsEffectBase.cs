using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VdsEngine
{
    public abstract class VdsEffectBase : VdsActorBase
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IApplyEffect(IntPtr thisPtr, IntPtr targetPtr, bool apply, bool asyn);
        #endregion

        public PtrClass TargetActor
        {
            get;
            protected set;
        }

        public bool IsIdle
        {
            get;
            protected set;
        }

        public VdsEffectBase()
            : base()
        {
            IsIdle = true;
        }

        public VdsEffectBase(bool newNativeHandle)
            : base(newNativeHandle)
        {
            IsIdle = true;
        }

        public abstract void Apply(PtrClass obj);

        public abstract void Unapply();

        internal void ApplyEffectInternal(IntPtr thisPtr, IntPtr targetPtr, bool apply)
        {
            int updateThreadID = GlobalEnvironment.UpdateThreadID;
            int curThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            IApplyEffect(thisPtr, targetPtr, apply, updateThreadID != curThreadID);
        }
    }
}