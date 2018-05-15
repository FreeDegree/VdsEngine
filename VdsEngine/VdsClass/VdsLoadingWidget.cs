using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public class VdsLoadingWidget : VdsActorBase
    {
        public string BackgroundImage
        {
            get
            {
                return GetActorProperty("backgroundimage");
            }
            set
            {
                SetActorProperty("backgroundimage", value);
            }
        }

        public string ProgressBackgroundImage
        {
            get
            {
                return GetActorProperty("progressbackgroundimage");
            }
            set
            {
                SetActorProperty("progressbackgroundimage", value);
            }
        }

        public VdsVec4d ProgressContentsMargin
        {
            get
            {
                string pStr = GetActorProperty("progresscontentsmargin");
                VdsVec4d v = new VdsVec4d();
                v.FromString(pStr);
                return v;
            }
            set
            {
                VdsVec4d pos = value;
                SetActorProperty("progresscontentsmargin", pos.ToString());
            }
        }

        public VdsVec4d ProgressTextContentsMargin
        {
            get
            {
                string pStr = GetActorProperty("progresstextcontentsmargin");
                VdsVec4d v = new VdsVec4d();
                v.FromString(pStr);
                return v;
            }
            set
            {
                VdsVec4d pos = value;
                SetActorProperty("progresstextcontentsmargin", pos.ToString());
            }
        }

        public string ProgressText
        {
            get
            {
                return GetActorProperty("progresstext");
            }
            set
            {
                SetActorProperty("progresstext", value);
            }
        }

        public int ProgressValue
        {
            get
            {
                return Convert.ToInt32(GetActorProperty("progressvalue"));
            }
            set
            {
                SetActorProperty("progressvalue", value.ToString());
            }
        }

        public VdsLoadingWidget()
            : base(true)
        { }
    }
}