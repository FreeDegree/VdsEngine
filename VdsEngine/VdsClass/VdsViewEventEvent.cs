using System;
using System.Collections.Generic;

namespace VdsEngine
{
    public sealed class VdsViewEventEvent
    {
        public sealed class VEventArgs : EventArgs
        {
            /// <summary>
            /// 视图ID
            /// </summary>
            public int ViewID
            {
                get;
                set;
            }
            /// <summary>
            /// 按键
            /// </summary>
            public object EventParameter
            {
                get;
                set;
            }
            /// <summary>
            /// 光标位置
            /// </summary>
            public VdsVec2d EventPosition
            {
                get;
                set;
            }
        }
        /// <summary>
        /// 鼠标按下事件，leftbutton 1, middlebutton 2, rightbutton 3
        /// </summary>
        /// <param name="button"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public event EventHandler VdsViewEventEvent_MouseButtonPressEvent;
        /// <summary>
        /// 鼠标弹起事件，leftbutton 1, middlebutton 2, rightbutton 3
        /// </summary>
        /// <param name="button"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public event EventHandler VdsViewEventEvent_MouseButtonReleaseEvent;
        /// <summary>
        /// 鼠标双击事件，leftbutton 1, middlebutton 2, rightbutton 3
        /// </summary>
        /// <param name="button"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public event EventHandler VdsViewEventEvent_MouseDoubleClickEvent;
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="button"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public event EventHandler VdsViewEventEvent_MouseMoveEvent;
        /// <summary>
        /// 鼠标滚动事件
        /// </summary>
        /// <param name="z"></param>
        public event EventHandler VdsViewEventEvent_MouseZoomEvent;
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        /// <param name="key"></param>
        public event EventHandler VdsViewEventEvent_KeyboardDownEvent;
        /// <summary>
        /// 键盘弹起事件
        /// </summary>
        /// <param name="key"></param>
        public event EventHandler VdsViewEventEvent_KeyboardUpEvent;
        /// <summary>
        /// 场景加载图层改变事件
        /// </summary>
        /// <param name="viewID">View I.</param>
        public event EventHandler VdsViewEventEvent_LayerInSceneChangedEvent;
        /// <summary>
        /// Game图层添加角色事件
        /// </summary>
        public event EventHandler VdsViewEventEvent_GameLayerAddActorEvent;
        /// <summary>
        /// Game图层删除角色事件
        /// </summary>
        public event EventHandler VdsViewEventEvent_GameLayerRemoveActorEvent;
        /// <summary>
        /// 播放剧情脚本事件
        /// </summary>
        public event EventHandler VdsViewEventEvent_PlayPlotScriptEventEvent;
        /// <summary>
        /// 停止播放剧情脚本事件
        /// </summary>
        public event EventHandler VdsViewEventEvent_StopPlotScriptEventEvent;
        /// <summary>
        /// 暂停播放剧情脚本事件
        /// </summary>
        public event EventHandler VdsViewEventEvent_PausePlotScriptEventEvent;

        /// <summary>
        /// 二维UI事件
        /// EVENT_NONE          = 0x0000,
        /// EVENT_FOCUS         = 0x0001,
        /// EVENT_UNFOCUS       = 0x0002,
        /// EVENT_MOUSE_ENTER   = 0x0004,
        /// EVENT_MOUSE_OVER    = 0x0008,
        /// EVENT_MOUSE_LEAVE   = 0x0010,
        /// EVENT_MOUSE_DRAG    = 0x0020,
        /// EVENT_MOUSE_PUSH    = 0x0040,
        /// EVENT_MOUSE_RELEASE = 0x0080,
        /// EVENT_MOUSE_SCROLL  = 0x0100,
        /// EVENT_KEY_DOWN      = 0x0200,
        /// EVENT_KEY_UP        = 0x0400,
        /// EVENT_ALL           = 0xFFFF
        /// </summary>
        /// <param name="viewID"></param>
        /// <param name="uiName"></param>
        /// <param name="eventType"></param>
        public event EventHandler VdsViewEventEvent_UIEvent;

        public void MouseButtonPressEvent(int viewID, int button, int x, int y)
        {
            if (VdsViewEventEvent_MouseButtonPressEvent == null || VdsViewEventEvent_MouseButtonPressEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            VdsVec2d pos = new VdsVec2d(x, y);
            eArgs.ViewID = viewID;
            eArgs.EventParameter = button;
            eArgs.EventPosition = pos;
            VdsViewEventEvent_MouseButtonPressEvent(null, eArgs);
        }

        public void MouseButtonReleaseEvent(int viewID, int button, int x, int y)
        {
            if (VdsViewEventEvent_MouseButtonReleaseEvent == null || VdsViewEventEvent_MouseButtonReleaseEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            VdsVec2d pos = new VdsVec2d(x, y);
            eArgs.ViewID = viewID;
            eArgs.EventParameter = button;
            eArgs.EventPosition = pos;
            VdsViewEventEvent_MouseButtonReleaseEvent(null, eArgs);
        }

        public void MouseDoubleClickEvent(int viewID, int button, int x, int y)
        {
            if (VdsViewEventEvent_MouseDoubleClickEvent == null || VdsViewEventEvent_MouseDoubleClickEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            VdsVec2d pos = new VdsVec2d(x, y);
            eArgs.ViewID = viewID;
            eArgs.EventParameter = button;
            eArgs.EventPosition = pos;
            VdsViewEventEvent_MouseDoubleClickEvent(null, eArgs);
        }

        public void MouseMoveEvent(int viewID, int x, int y)
        {
            if (VdsViewEventEvent_MouseMoveEvent == null || VdsViewEventEvent_MouseMoveEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            VdsVec2d pos = new VdsVec2d(x, y);
            eArgs.ViewID = viewID;
            eArgs.EventPosition = pos;
            VdsViewEventEvent_MouseMoveEvent(null, eArgs);
        }

        public void MouseZoomEvent(int viewID, int z)
        {
            if (VdsViewEventEvent_MouseZoomEvent == null || VdsViewEventEvent_MouseZoomEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            eArgs.EventParameter = z;
            VdsViewEventEvent_MouseZoomEvent(null, eArgs);
        }

        public void KeyboardDownEvent(int viewID, int key)
        {
            if (VdsViewEventEvent_KeyboardDownEvent == null || VdsViewEventEvent_KeyboardDownEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            eArgs.EventParameter = key;
            VdsViewEventEvent_KeyboardDownEvent(null, eArgs);
        }

        public void KeyboardUpEvent(int viewID, int key)
        {
            if (VdsViewEventEvent_KeyboardUpEvent == null || VdsViewEventEvent_KeyboardUpEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            eArgs.EventParameter = key;
            VdsViewEventEvent_KeyboardUpEvent(null, eArgs);
        }

        public void LayerInSceneChangedEvent(int viewID)
        {
            if (VdsViewEventEvent_LayerInSceneChangedEvent == null || VdsViewEventEvent_LayerInSceneChangedEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            VdsViewEventEvent_LayerInSceneChangedEvent(null, eArgs);
        }

        public void GameLayerAddActorEvent(int viewID, string actorID)
        {
            if (VdsViewEventEvent_GameLayerAddActorEvent == null || VdsViewEventEvent_GameLayerAddActorEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            eArgs.EventParameter = actorID;
            VdsViewEventEvent_GameLayerAddActorEvent(null, eArgs);
        }

        public void GameLayerRemoveActorEvent(int viewID, string actorID)
        {
            if (VdsViewEventEvent_GameLayerRemoveActorEvent == null || VdsViewEventEvent_GameLayerRemoveActorEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            eArgs.EventParameter = actorID;
            VdsViewEventEvent_GameLayerRemoveActorEvent(null, eArgs);
        }

        public void PlayPlotScriptEvent(int viewID, string plotScriptID, string startSecond)
        {
            if (VdsViewEventEvent_PlayPlotScriptEventEvent == null || VdsViewEventEvent_PlayPlotScriptEventEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            List<string> eventParameter = new List<string>(2);
            eventParameter.Add(plotScriptID);
            eventParameter.Add(startSecond);
            eArgs.EventParameter = eventParameter;
            VdsViewEventEvent_PlayPlotScriptEventEvent(null, eArgs);
        }

        public void StopPlotScriptEvent(int viewID, string plotScriptID)
        {
            if (VdsViewEventEvent_StopPlotScriptEventEvent == null || VdsViewEventEvent_StopPlotScriptEventEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            eArgs.EventParameter = plotScriptID;
            VdsViewEventEvent_StopPlotScriptEventEvent(null, eArgs);
        }

        public void PausePlotScriptEvent(int viewID, string plotScriptID)
        {
            if (VdsViewEventEvent_PausePlotScriptEventEvent == null || VdsViewEventEvent_PausePlotScriptEventEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            eArgs.EventParameter = plotScriptID;
            VdsViewEventEvent_PausePlotScriptEventEvent(null, eArgs);
        }

        public void UIEvent(int viewID, string uiName, string eventType)
        {
            if (VdsViewEventEvent_UIEvent == null || VdsViewEventEvent_UIEvent.GetInvocationList().GetLength(0) < 1)
                return;
            VEventArgs eArgs = new VEventArgs();
            eArgs.ViewID = viewID;
            List<string> uiEventParameter = new List<string>(2);
            uiEventParameter.Add(uiName);
            uiEventParameter.Add(eventType);
            eArgs.EventParameter = uiEventParameter;
            VdsViewEventEvent_UIEvent(null, eArgs);
        }
    }
}