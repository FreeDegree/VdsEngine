using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VdsEngine
{
    public class VdsActor : VdsActorBase
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static PtrClass[] IGetAllChildren(IntPtr parent, string actorClassName, bool isInstanceOf, bool findChildren);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static IntPtr ICreateActorByResFullName(string className);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void ISetActorStatus(IntPtr actorPtr, string statusName, bool asyn);
        #endregion

        public string ActorStatus
        {
            get
            {
                if (_actorStatus == null || _actorStatus == "")
                    _actorStatus = "DefaultStatus";
                return _actorStatus;
            }
            set
            {
                if (value == null || value == "")
                    _actorStatus = "DefaultStatus";
                else
                    _actorStatus = value;
                int updateThreadID = GlobalEnvironment.UpdateThreadID;
                int curThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
                ISetActorStatus(NativeHandle, _actorStatus, updateThreadID != curThreadID);
            }
        }

        public VdsVec3d ActorTranslation
        {
            get
            {
                string pStr = GetActorProperty("translation");
                VdsVec3d v = new VdsVec3d();
                v.FromString(pStr);
                return v;
            }
            set
            {
                VdsVec3d pos = value;
                SetActorProperty("translation", pos.ToString());
            }
        }

        public VdsVec3d ActorRotation
        {
            get
            {
                string pStr = GetActorProperty("rotation");
                VdsVec3d v = new VdsVec3d();
                v.FromString(pStr);
                return v;
            }
            set
            {
                VdsVec3d pos = value;
                SetActorProperty("rotation", pos.ToString());
            }
        }

        public bool ActorActive
        {
            get
            {
                return _actorActive;
            }
            set
            {
                _actorActive = value;
                SetActorProperty("defaultactive", _actorActive ? "1" : "0");
            }
        }

        private string _actorStatus = null;
        private bool _actorActive = true;

        public VdsActor()
            : base()
        { }

        public VdsActor(bool newNativeHandle)
            : base(newNativeHandle)
        { }

        public VdsActor(string resName)
            : base(false)
        {
            SetNativeHandle(ICreateActorByResFullName(resName));
        }

        public VdsActor(VdsActor cloneFrom)
            : base(false)
        {
            CloneActor(cloneFrom);
        }

        public override void CloneActor(VdsActorBase cloneFrom)
        {
            base.CloneActor(cloneFrom);
            ActorStatus = (cloneFrom as VdsActor).ActorStatus;
        }

        protected override void InitActorPropertyList(string[] propertyNameList, string[] propertyValueList)
        {
            base.InitActorPropertyList(propertyNameList, propertyValueList);
            int index = 0;
            foreach (string name in _propertyNameList)
            {
                switch (name)
                {
                    case "validstatus":
                        {
                            string vstr = propertyValueList[index];
                            string[] vList = vstr.Split(',');
                            if (vList.Length > 0)
                                _actorStatus = vList[0];
                            break;
                        }
                    case "defaultactive":
                        {
                            string vstr = propertyValueList[index];
                            _actorActive = (Convert.ToInt32(vstr) != 0);
                            break;
                        }
                    default:
                        break;
                }
                index++;
            }
        }

        protected PtrClass[] GetAllChildrenInternal(Type type, bool isInstanceOf, bool findChildren)
        {
            PtrClass[] allActors = IGetAllChildren(NativeHandle, type.Name, isInstanceOf, findChildren);
            return allActors;
        }
    }
}