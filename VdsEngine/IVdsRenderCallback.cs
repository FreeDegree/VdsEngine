using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public interface IVdsRenderCallback
    {
        long StepNumber
        {
            get;
        }

        long LastStepTick
        {
            get;
        }

        void UpdateStep(object param);

        void AsynchronousOperationStep(object param);
    }
}