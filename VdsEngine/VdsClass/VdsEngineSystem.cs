using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace VdsEngine
{
    public static class SystemSingnal
    {
        private static void RefreshSystem()
        {
            VdsEngineSystem.Instance.RefreshSystem();
        }

        private static void UpdateStep()
        {
            VdsEngineSystem.Instance.UpdateStep();
        }

        private static void AsynchronousOperationStep()
        {
            VdsEngineSystem.Instance.AsynchronousOperationStep();
        }

        private static string GetAllBehaviourClass()
        {
            string resultString = null;
            Assembly[] asmList = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asmList)
            {
                if (asm.GetName().Name == "CSharpAssembly")
                {
                    foreach (var item in asm.GetTypes())
                    {
                        Type bType = item.BaseType;
                        if (bType != null && bType.FullName == "VdsEngine.MonoBehaviour")
                            resultString = resultString == null ? item.FullName : resultString + "," + item.FullName;
                    }
                    break;
                }
            }
            return resultString;
        }

        private static string GetAllTriggerClass()
        {
            string resultString = null;
            Assembly[] asmList = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asmList)
            {
                foreach (var item in asm.GetTypes())
                {
                    Type bType = item.BaseType;
                    if (bType != null && bType.FullName == "VdsEngine.TriggerRecord")
                        resultString = resultString == null ? item.FullName : resultString + "," + item.FullName;
                }
            }
            return resultString;
        }
    }

    public sealed class VdsEngineSystem
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static VdsView[] IGetVdsViews();
        #endregion
        private VdsEngineSystem()
        { }

        public static readonly VdsEngineSystem Instance = new VdsEngineSystem();
        public event EventHandler SystemViewsClearEvent;
        public event EventHandler SystemViewsBuildEvent;
        private List<VdsView> _viewsList;

        public List<VdsView> EarthViewsList
        {
            get
            {
                return _viewsList;
            }
        }

        public VdsView GetVdsViewByID(int viewID)
        {
            foreach (VdsView view in _viewsList)
            {
                if (view.ObjectViewID == viewID)
                    return view;
            }
            return null;
        }

        internal void RefreshSystem()
        {
            if (SystemViewsClearEvent != null && SystemViewsClearEvent.GetInvocationList().GetLength(0) > 0)
                SystemViewsClearEvent(null, null);
            VdsView[] vList = IGetVdsViews();
            int size = vList.GetLength(0);
            if (size < 1)
                return;
            _viewsList = new List<VdsView>(size);
            for (int i = 0; i < size; ++i)
            {
                _viewsList.Add(vList[i]);
                TriggerSystem ts = new TriggerSystem();
                vList[i].MainTriggerSystem = ts;
            }
            if (SystemViewsClearEvent != null && SystemViewsClearEvent.GetInvocationList().GetLength(0) > 0)
                SystemViewsBuildEvent(null, null);
        }

        internal void UpdateStep()
        {
            foreach (VdsView view in _viewsList)
            {
                view.UpdateStep();
            }
            if (GlobalEnvironment.UpdateThreadID < 0)
                GlobalEnvironment.UpdateThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        internal void AsynchronousOperationStep()
        {
            foreach (VdsView view in _viewsList)
            {
                view.AsynchronousOperationStep();
            }
        }
    }
}