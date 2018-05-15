using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VdsEngine
{
    /// <summary>
    /// 三维视图
    /// </summary>
    public class VdsView : PtrClass
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void ISetMainCamera(int viewID, int mode, IntPtr targetActor, double ex, double ey, double ez,
            double cx, double cy, double cz, double ux, double uy, double uz, bool withAnimation);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static VdsCamera IGetMainCamera(int viewID);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static object ILoadSceneLayer(int viewID, string layerName);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static object IGetCurrentSceneLayer(int viewID);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static IntPtr IDoPickActor(int viewID, int x, int y, int effectType);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static IntPtr IGetPickActor(int viewID);
        #endregion

        public enum PickObjectEffect
        {
            NoEffect,
            BoundingBox_Animation,
            Scale_Effect,
            Material_Effect,
            Halo_Effect,
            Outline_Effect,
            Default_Effect,
        };
        private bool _renderPause = false;
        private object _gameLayer = null;
        private object _rootLayer = null;
        private object _currentSceneLayer = null;
        private VdsViewEventEvent _viewEvent = null;
        private List<UIWidget> _uiWidgetList = null;

        public VdsView()
            : base(false)
        { }

        public object GameLayer
        {
            get
            {
                return _gameLayer;
            }
        }

        public object CurrentSceneLayer
        {
            get
            {
                if (_currentSceneLayer == null)
                {
                    _currentSceneLayer = IGetCurrentSceneLayer(ObjectViewID);
                    _viewEvent.VdsViewEventEvent_LayerInSceneChangedEvent += _viewEvent_VdsViewEventEvent_LayerInSceneChangedEvent;
                }
                return _currentSceneLayer;
            }
            private set
            {
                if (_currentSceneLayer != null)
                    _currentSceneLayer = value;
            }
        }

        public VdsViewEventEvent ViewEvent
        {
            get
            {
                return _viewEvent;
            }
        }

        public VdsCamera MainCamera
        {
            get
            {
                VdsCamera camera = IGetMainCamera(ObjectViewID);
                return camera;
            }
            set
            {
                VdsCamera camera = value;
                ISetMainCamera(ObjectViewID, (int)camera.CurrentCameraMode, camera.TargetActorNativeHandle, camera.CameraPose.Eye.X, camera.CameraPose.Eye.Y, camera.CameraPose.Eye.Z,
                    camera.CameraPose.Center.X, camera.CameraPose.Center.Y, camera.CameraPose.Center.Z, camera.CameraPose.Up.X, camera.CameraPose.Up.Y, camera.CameraPose.Up.Z, camera.WithAnimation);
            }
        }

        public object MainTriggerSystem
        {
            get;
            internal set;
        }

        public void LoadSceneLayer(string layerName)
        {
            IVdsRenderCallback l = (IVdsRenderCallback)ILoadSceneLayer(ObjectViewID, layerName);
            if (l != null)
                CurrentSceneLayer = l;
        }

        public PtrClass DoPickActor(int x, int y, PickObjectEffect effectType)
        {
            IntPtr ptr = IDoPickActor(ObjectViewID, x, y, (int)effectType);
            return GetActorByNativeHandle(ptr);
        }

        public PtrClass GetPickActor()
        {
            IntPtr ptr = IGetPickActor(ObjectViewID);
            return GetActorByNativeHandle(ptr);
        }

        public PtrClass GetActorByNativeHandle(IntPtr ptr)
        {
            PtrClass actor = ((IVdsGroupInterface)_rootLayer).GetActorByNativeHandle(ptr);
            if (actor != null)
                return actor;
            actor = ((IVdsGroupInterface)GameLayer).GetActorByNativeHandle(ptr);
            if (actor != null)
                return actor;
            if (CurrentSceneLayer != null)
            {
                actor = ((IVdsGroupInterface)CurrentSceneLayer).GetActorByNativeHandle(ptr);
                if (actor != null)
                    return actor;
            }
            return null;
        }

        public PtrClass GetObjectByID(string idOrBindingID)
        {
            PtrClass actor = ((IVdsGroupInterface)_rootLayer).GetObjectByID(idOrBindingID);
            if (actor != null)
                return actor;
            actor = ((IVdsGroupInterface)GameLayer).GetObjectByID(idOrBindingID);
            if (actor != null)
                return actor;
            if (CurrentSceneLayer != null)
            {
                actor = ((IVdsGroupInterface)CurrentSceneLayer).GetObjectByID(idOrBindingID);
                if (actor != null)
                    return actor;
            }
            return null;
        }

        public void AddUIWidget(UIWidget widget)
        {
            if (_uiWidgetList == null)
                _uiWidgetList = new List<UIWidget>();
            if (_uiWidgetList.Contains(widget))
                return;
            _uiWidgetList.Add(widget);
            widget.ParentView = this;
        }

        public void RemoveUIWidget(UIWidget widget)
        {
            if (_uiWidgetList == null || !_uiWidgetList.Contains(widget))
                return;
            widget.Hide();
            _uiWidgetList.Remove(widget);
        }

        internal void UpdateStep()
        {
            if (_renderPause)
                return;
            ((IVdsRenderCallback)_rootLayer).UpdateStep(null);
            ((IVdsRenderCallback)GameLayer).UpdateStep(null);
            if (CurrentSceneLayer != null)
                ((IVdsRenderCallback)CurrentSceneLayer).UpdateStep(null);
            ((IVdsRenderCallback)MainTriggerSystem).UpdateStep(null);
        }

        internal void AsynchronousOperationStep()
        {
            if (_renderPause)
                return;
            ((IVdsRenderCallback)_rootLayer).AsynchronousOperationStep(null);
            ((IVdsRenderCallback)GameLayer).AsynchronousOperationStep(null);
            if (CurrentSceneLayer != null)
                ((IVdsRenderCallback)CurrentSceneLayer).AsynchronousOperationStep(null);
            ((IVdsRenderCallback)MainTriggerSystem).AsynchronousOperationStep(null);
        }

        void _viewEvent_VdsViewEventEvent_LayerInSceneChangedEvent(object sender, EventArgs e)
        {
            CurrentSceneLayer = IGetCurrentSceneLayer(ObjectViewID);
        }
    }
}