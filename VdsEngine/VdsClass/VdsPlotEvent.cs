using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public class VdsPlotEvent : VdsGameActor
    {
        public double EventStartTime
        {
            get;
            set;
        }

        public double EventDurationTime
        {
            get;
            set;
        }

        public VdsPlotEvent()
            : base()
        { }

        public VdsPlotEvent(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        protected override void InitActorPropertyList(string[] propertyNameList, string[] propertyValueList)
        {
            base.InitActorPropertyList(propertyNameList, propertyValueList);
            int index = 0;
            foreach (string name in _propertyNameList)
            {
                switch (name)
                {
                    case "ploteventstarttime":
                        EventStartTime = Convert.ToDouble(propertyValueList[index]);
                        break;
                    case "ploteventdurationtime":
                        EventDurationTime = Convert.ToDouble(propertyValueList[index]);
                        break;
                    default:
                        break;
                }
                index++;
            }
        }
    }
}