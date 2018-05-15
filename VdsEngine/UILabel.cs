using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VdsEngine
{
    public class UILabel : UIWidget
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IUpdateLabel(IntPtr thisPtr, double[] pos, string text, double[] textColor, string textFont
            , int textSize, int backdrop, double[] backdropColor, int alignment, double[] labelMargin, int show);
        #endregion

        public enum BackdropType
        {
            DROP_SHADOW_BOTTOM_RIGHT,
            DROP_SHADOW_CENTER_RIGHT,
            DROP_SHADOW_TOP_RIGHT,
            DROP_SHADOW_BOTTOM_CENTER,
            DROP_SHADOW_TOP_CENTER,
            DROP_SHADOW_BOTTOM_LEFT,
            DROP_SHADOW_CENTER_LEFT,
            DROP_SHADOW_TOP_LEFT,
            OUTLINE,
            NONE,
        }

        public enum AlignmentType
        {
            LEFT_TOP,
            LEFT_CENTER,
            LEFT_BOTTOM,
            CENTER_TOP,
            CENTER_CENTER,
            CENTER_BOTTOM,
            RIGHT_TOP,
            RIGHT_CENTER,
            RIGHT_BOTTOM,
            LEFT_BASE_LINE,
            CENTER_BASE_LINE,
            RIGHT_BASE_LINE,
            LEFT_BOTTOM_BASE_LINE,
            CENTER_BOTTOM_BASE_LINE,
            RIGHT_BOTTOM_BASE_LINE,
            BASE_LINE = LEFT_BASE_LINE,
        }

        public string TextString
        {
            get;
            set;
        }

        public VdsVec4d TextColor
        {
            get;
            set;
        }

        public string TextFont
        {
            get;
            set;
        }

        public int TextSize
        {
            get;
            set;
        }

        public BackdropType Backdrop
        {
            get;
            set;
        }

        public VdsVec4d BackdropColor
        {
            get;
            set;
        }

        public AlignmentType Alignment
        {
            get;
            set;
        }

        public VdsVec4d LabelMargin
        {
            get;
            set;
        }

        public UILabel(int viewID)
            : base(viewID)
        {
            TextColor = new VdsVec4d(0, 0, 0, 1);
            TextFont = "msyhl.ttc";
            TextSize = 12;
            Backdrop = BackdropType.OUTLINE;
            BackdropColor = new VdsVec4d(1, 1, 1, 1);
            Alignment = AlignmentType.LEFT_CENTER;
            LabelMargin = new VdsVec4d(10, 5, 10, 5);
        }

        public void UpdateText()
        {
            if (TextString == null)
                return;
            int updateThreadID = GlobalEnvironment.UpdateThreadID;
            int curThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (updateThreadID != curThreadID)
                return;
            double[] pos = { WidgetPosition.X, WidgetPosition.Y };
            double[] textColor = { TextColor.R, TextColor.G, TextColor.B, TextColor.A };
            double[] backdropColor = { BackdropColor.R, BackdropColor.G, BackdropColor.B, BackdropColor.A };
            double[] labelMargin = { LabelMargin.R, LabelMargin.G, LabelMargin.B, LabelMargin.A };
            IUpdateLabel(NativeHandle, pos, TextString, textColor, TextFont
            , TextSize, (int)Backdrop, backdropColor, (int)Alignment, labelMargin, (int)OperationType.UpdateUI);
        }

        public override void Show()
        {
            if (TextString == null || IsShow || ParentView == null)
                return;
            double[] pos = { };
            double[] textColor = { };
            double[] backdropColor = { };
            double[] labelMargin = { };
            IUpdateLabel(NativeHandle, pos, TextString, textColor, TextFont
            , TextSize, (int)Backdrop, backdropColor, (int)Alignment, labelMargin, (int)OperationType.ShowUI);
            base.Show();
        }

        public override void Hide()
        {
            if (TextString == null || !IsShow || ParentView == null)
                return;
            double[] pos = { };
            double[] textColor = { };
            double[] backdropColor = { };
            double[] labelMargin = { };
            IUpdateLabel(NativeHandle, pos, TextString, textColor, TextFont
            , TextSize, (int)Backdrop, backdropColor, (int)Alignment, labelMargin, (int)OperationType.HideUI);
            base.Hide();
        }
    }
}