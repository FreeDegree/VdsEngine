using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public interface IVdsNodeInterface
    {
        PtrClass ParentObject
        {
            get;
        }

        void SetParent(PtrClass parent);
    }
}