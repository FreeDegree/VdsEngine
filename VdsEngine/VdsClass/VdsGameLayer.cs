using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VdsEngine
{
    public sealed class VdsGameLayer : VdsLayer
    {
        private VdsView _currentView = null;

        public VdsGameLayer()
            : base()
        { }

        public VdsGameLayer(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        internal void AddActorInternal(string actorID)
        {
            PtrClass actor = GetObjectByID(actorID);
            if (actor != null)
                return;
            PtrClass findActor = GetInternalChildByID(actorID, false);
            AddChild(findActor);
        }

        internal void RemoveActorInternal(string actorID)
        {
            PtrClass vActor = GetObjectByID(actorID);
            if (actorID != null)
                RemoveChild(vActor);
        }

        protected override void BuildLayerInternal()
        {
            base.BuildLayerInternal();
            if (_currentView == null)
            {
                _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(ObjectViewID));
                _currentView.ViewEvent.VdsViewEventEvent_GameLayerAddActorEvent += _viewEvent_VdsViewEventEvent_GameLayerAddActorEvent;
                _currentView.ViewEvent.VdsViewEventEvent_GameLayerRemoveActorEvent += _viewEvent_VdsViewEventEvent_GameLayerRemoveActorEvent;
            }
        }

        void _viewEvent_VdsViewEventEvent_GameLayerRemoveActorEvent(object sender, EventArgs e)
        {
            VdsEngine.VdsViewEventEvent.VEventArgs args = e as VdsEngine.VdsViewEventEvent.VEventArgs;
            string id = args.EventParameter as string;
            RemoveActorInternal(id);
        }

        void _viewEvent_VdsViewEventEvent_GameLayerAddActorEvent(object sender, EventArgs e)
        {
            VdsEngine.VdsViewEventEvent.VEventArgs args = e as VdsEngine.VdsViewEventEvent.VEventArgs;
            string id = args.EventParameter as string;
            AddActorInternal(id);
        }
    }
}