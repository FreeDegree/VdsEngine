using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public interface IVdsGroupInterface
    {
        List<PtrClass> ChildrenList
        {
            get;
        }

        int ChildrenSize
        {
            get;
        }

        void AddChild(PtrClass child);

        void RemoveChild(PtrClass child);

        PtrClass GetActorByNativeHandle(IntPtr ptr);

        PtrClass GetObjectByID(string idOrBindingID);
    }
}