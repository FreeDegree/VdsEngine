using System;

namespace VdsEngine
{
    public enum CameraMode
    {
        NoCamera,
        FreeCamera,
        EarthCamera,
        RoamCamera,
        FollowCamera,
        FirstPersonCamera,
        CurrentMode,
    }

    public class VdsCamera
    {
        public CameraPoseProperty CameraPose
        {
            get;
            set;
        }

        public bool WithAnimation
        {
            get;
            set;
        }

        public CameraMode CurrentCameraMode
        {
            get;
            set;
        }

        public IntPtr TargetActorNativeHandle
        {
            get;
            set;
        }

        public VdsCamera()
        {
            CameraPose = new CameraPoseProperty();
            CurrentCameraMode = CameraMode.CurrentMode;
            WithAnimation = true;
        }
    }
}