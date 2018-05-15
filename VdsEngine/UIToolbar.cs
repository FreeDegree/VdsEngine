using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VdsEngine
{
    public class UIToolbarData
    {
        public List<string> ButtonNameList
        {
            get;
            private set;

        }

        public List<string> ButtonImageList
        {
            get;
            private set;
        }

        public List<string> ButtonToolTipList
        {
            get;
            private set;
        }

        public UIToolbarData()
        { }

        public void AddButton(string buttonName, string imageFileName, string tooltipString)
        {
            if (ButtonNameList == null)
            {
                ButtonNameList = new List<string>();
                ButtonImageList = new List<string>();
                ButtonToolTipList = new List<string>();
            }
            ButtonNameList.Add(buttonName);
            ButtonImageList.Add(imageFileName);
            ButtonToolTipList.Add(tooltipString);
        }
    }

    public class UIToolbar : UIWidget
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IUpdateToolbar(IntPtr thisPtr, string[] buttonNameList, string[] butonImageList, string[] butonTooltipList, int tAnchor, int bAnchor, int show);
        #endregion
        public event EventHandler UIToolbar_MouseButtonClickEvent;

        private UIToolbarData _toolbarData = null;

        public UIToolbarData ToolbarData
        {
            get
            {
                return _toolbarData;
            }
            set
            {
                _toolbarData = value;
            }
        }

        public enum ToolbarDockAnchor
        {
            DA_Left,
            DA_Down,
            DA_Right,
            DA_Up,
        }

        public enum ButtonDockAnchor
        {
            CENTER,
            HA_LEFT,
            HA_RIGHT,
            VA_TOP,
            VA_BOTTOM
        }

        public ToolbarDockAnchor TDockAnchor
        {
            get;
            set;
        }

        public ButtonDockAnchor BDockAnchor
        {
            get;
            set;
        }

        public UIToolbar(int viewID)
            : base(viewID)
        {
            TDockAnchor = ToolbarDockAnchor.DA_Right;
            BDockAnchor = ButtonDockAnchor.CENTER;
        }

        public void MouseButtonClickEvent(int viewID, string buttonKey)
        {
            if (UIToolbar_MouseButtonClickEvent != null)
            {
                UIToolbar_MouseButtonClickEvent(buttonKey, null);
            }
        }

        public void UpdateToolbar()
        {
            if (_toolbarData == null)
                return;
            if (_toolbarData.ButtonNameList.Count != _toolbarData.ButtonImageList.Count)
                return;
            int updateThreadID = GlobalEnvironment.UpdateThreadID;
            int curThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (updateThreadID != curThreadID)
                return;
            IUpdateToolbar(this.NativeHandle, _toolbarData.ButtonNameList.ToArray(),
                _toolbarData.ButtonImageList.ToArray(), _toolbarData.ButtonToolTipList.ToArray(), (int)TDockAnchor, (int)BDockAnchor, (int)OperationType.UpdateUI);
        }

        public override void Show()
        {
            if (_toolbarData == null || IsShow || ParentView == null)
                return;
            string[] buttonNameList = { };
            string[] butonImageList = { };
            string[] butonTooltipList = { };
            IUpdateToolbar(this.NativeHandle, buttonNameList, butonImageList, butonTooltipList, 0, 0, (int)OperationType.ShowUI);
            base.Show();
        }

        public override void Hide()
        {
            if (_toolbarData == null || !IsShow || ParentView == null)
                return;
            string[] buttonNameList = { };
            string[] butonImageList = { };
            string[] butonTooltipList = { };
            IUpdateToolbar(this.NativeHandle, buttonNameList, butonImageList, butonTooltipList, 0, 0, (int)OperationType.HideUI);
            base.Hide();
        }
    }
}