using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public static class StaticMethod
    {
        public static double DegreesToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }

        public static double RadiansToDegrees(double angle)
        {
            return angle * 180.0 / Math.PI;
        }

        public static double ClampUnity(double x)
        {
            if (x > 1.0) { return 1.0; }
            if (x < -1.0) { return -1.0; }
            return x;
        }

        public static bool Equivalent(double lhs, double rhs, double epsilon)
        {
            double delta = rhs - lhs;
            return delta < 0.0f ? delta >= -epsilon : delta <= epsilon;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        public static void ComputeCoordinateFrame(VdsActor actor, ref VdsMatrixd localToWorld)
        {
            VdsActor curActor = actor;
            localToWorld.MakeIdentity();
            while (curActor != null)
            {
                VdsMatrixd transMat = new VdsMatrixd();
                transMat.MakeTranslate(curActor.ActorTranslation);
                VdsMatrixd rotateMat = new VdsMatrixd();
                VdsMatrixd.HprToMatrix(ref rotateMat, curActor.ActorRotation);
                rotateMat.PostMult(transMat);
                localToWorld.PreMult(rotateMat);
                if (curActor.ParentObject != null && curActor.ParentObject is VdsActor)
                    curActor = curActor.ParentObject as VdsActor;
                else
                    curActor = null;
            }
        }

        public static List<VdsVec3d> ConstructCatmullRomSplinePoint(List<VdsVec3d> controlPoints, int seg)
        {
            List<VdsVec3d> vList = new List<VdsVec3d>();
            VdsVec3d[] dList = new VdsVec3d[controlPoints.Count + 2];
            controlPoints.CopyTo(0, dList, 1, controlPoints.Count);
            VdsVec3d p0 = controlPoints[0] * 2 - controlPoints[1];
            dList[0] = p0;
            VdsVec3d pn = controlPoints[controlPoints.Count - 1] * 2 - controlPoints[controlPoints.Count - 2];
            dList[dList.Length - 1] = pn;
            List<VdsVec3d> subConctrl = new List<VdsVec3d>(4);
            for (int i = 0; i < dList.Length; ++i)
            {
                if (i < dList.Length - 3)
                {
                    subConctrl[0] = dList[i];
                    subConctrl[1] = dList[i + 1];
                    subConctrl[2] = dList[i + 2];
                    subConctrl[3] = dList[i + 3];
                    double deltaU = 1.0 / (double)seg;
                    for (int j = 0; j < seg; ++j)
                    {
                        double uu = j * deltaU;
                        VdsVec3d tempp = DoCatmullRom(uu, subConctrl);
                        vList.Add(tempp);
                    }
                }
            }
            return vList;
        }

        private static VdsVec3d DoCatmullRom(double u, List<VdsVec3d> subConctrl)
        {
            double f0, f1, f2, f3;
            VdsVec3d vert;
            f0 = (-0.5) * Math.Pow(u, 3) + Math.Pow(u, 2) + (-0.5 * u);
            f1 = 1.5 * Math.Pow(u, 3) - 2.5 * Math.Pow(u, 2) + 1;
            f2 = (-1.5) * Math.Pow(u, 3) + 2.0 * Math.Pow(u, 2) + 0.5 * u;
            f3 = 0.5 * Math.Pow(u, 3) - 0.5 * Math.Pow(u, 2);
            vert = subConctrl[0] * f0 + subConctrl[1] * f1 + subConctrl[2] * f2 + subConctrl[3] * f3;
            return vert;
        }
    }
}