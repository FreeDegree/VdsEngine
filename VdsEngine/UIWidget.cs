using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public class UIWidget : PtrClass
    {
        public VdsVec2d WidgetPosition
        {
            get;
            set;
        }

        public bool IsShow
        {
            get;
            protected set;
        }

        public PtrClass ParentView
        {
            get;
            set;
        }

        protected enum OperationType
        {
            UpdateUI,
            ShowUI,
            HideUI,
        }

        public UIWidget(int viewID)
            : base(false)
        {
            InitUI(viewID);
        }

        public virtual void InitUI(int viewID)
        {
            SetNativeHandle(CreateInternal(this.GetType().FullName, viewID));
            WidgetPosition = new VdsVec2d(0, 0);
            IsShow = false;
        }

        public virtual void Show()
        {
            IsShow = true;
        }

        public virtual void Hide()
        {
            IsShow = false;
        }
    }
}