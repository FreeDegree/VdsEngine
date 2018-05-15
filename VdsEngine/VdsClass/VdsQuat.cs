using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    /// <summary>
    /// 四元数
    /// </summary>
    public class VdsQuat
    {
        private double[] _v = new double[4];

        public double[] QuatValue
        {
            get
            {
                return _v;
            }
            private set
            {
                _v = value;
            }
        }

        public VdsQuat()
        {
            _v[0] = 0.0;
            _v[1] = 0.0;
            _v[2] = 0.0;
            _v[3] = 1.0;
        }

        public VdsQuat(double x, double y, double z, double w)
        {
            _v[0] = x;
            _v[1] = y;
            _v[2] = z;
            _v[3] = w;
        }

        public VdsQuat(VdsVec4d v)
        {
            _v[0] = v.R;
            _v[1] = v.G;
            _v[2] = v.B;
            _v[3] = v.A;
        }

        public void SetQuat(double x, double y, double z, double w)
        {
            _v[0] = x;
            _v[1] = y;
            _v[2] = z;
            _v[3] = w;
        }

        public void SetQuat(VdsQuat q)
        {
            _v[0] = q._v[0];
            _v[1] = q._v[1];
            _v[2] = q._v[2];
            _v[3] = q._v[3];
        }

        public bool ZeroRotation()
        {
            return _v[0] == 0.0 && _v[1] == 0.0 && _v[2] == 0.0 && _v[3] == 1.0;
        }

        public static VdsQuat operator *(VdsQuat q, double rhs)
        {
            return new VdsQuat(q._v[0] * rhs, q._v[1] * rhs, q._v[2] * rhs, q._v[3] * rhs);
        }

        public static VdsQuat operator +(VdsQuat ql, VdsQuat qr)
        {
            return new VdsQuat(ql._v[0] + qr._v[0], ql._v[1] + qr._v[1], ql._v[2] + qr._v[2], ql._v[3] + qr._v[3]);
        }

        public static VdsQuat operator -(VdsQuat ql, VdsQuat qr)
        {
            return new VdsQuat(ql._v[0] - qr._v[0], ql._v[1] - qr._v[1], ql._v[2] - qr._v[2], ql._v[3] - qr._v[3]);
        }

        public static VdsQuat operator -(VdsQuat qr)
        {
            return new VdsQuat(-qr._v[0], -qr._v[1], -qr._v[2], -qr._v[3]);
        }

        public static VdsQuat operator *(VdsQuat q, VdsQuat rhs)
        {
            return new VdsQuat(rhs._v[3] * q._v[0] + rhs._v[0] * q._v[3] + rhs._v[1] * q._v[2] - rhs._v[2] * q._v[1],
                 rhs._v[3] * q._v[1] - rhs._v[0] * q._v[2] + rhs._v[1] * q._v[3] + rhs._v[2] * q._v[0],
                 rhs._v[3] * q._v[2] + rhs._v[0] * q._v[1] - rhs._v[1] * q._v[0] + rhs._v[2] * q._v[3],
                 rhs._v[3] * q._v[3] - rhs._v[0] * q._v[0] - rhs._v[1] * q._v[1] - rhs._v[2] * q._v[2]);
        }

        public static VdsQuat operator /(VdsQuat q, double rhs)
        {
            double div = 1.0 / rhs;
            return new VdsQuat(q._v[0] * div, q._v[1] * div, q._v[2] * div, q._v[3] * div);
        }

        public static VdsVec3d operator *(VdsQuat q, VdsVec3d v)
        {
            VdsVec3d uv, uuv;
            VdsVec3d qvec = new VdsVec3d(q._v[0], q._v[1], q._v[2]);
            uv = qvec ^ v;
            uuv = qvec ^ uv;
            uv *= (2.0f * q._v[3]);
            uuv *= 2.0f;
            return v + uv + uuv;
        }

        public double Length()
        {
            return Math.Sqrt(_v[0] * _v[0] + _v[1] * _v[1] + _v[2] * _v[2] + _v[3] * _v[3]);
        }

        public double Length2()
        {
            return _v[0] * _v[0] + _v[1] * _v[1] + _v[2] * _v[2] + _v[3] * _v[3];
        }

        public VdsQuat Conj()
        {
            return new VdsQuat(-_v[0], -_v[1], -_v[2], _v[3]);
        }

        public VdsQuat Inverse()
        {
            return Conj() / Length2();
        }

        public void MakeRotate(double angle, double x, double y, double z)
        {
            double epsilon = 0.0000001;
            double length = Math.Sqrt(x * x + y * y + z * z);
            if (length < epsilon)
            {
                _v[0] = 0.0;
                _v[1] = 0.0;
                _v[2] = 0.0;
                _v[3] = 1.0;
                return;
            }

            double inversenorm = 1.0 / length;
            double coshalfangle = Math.Cos(0.5 * angle);
            double sinhalfangle = Math.Sin(0.5 * angle);

            _v[0] = x * sinhalfangle * inversenorm;
            _v[1] = y * sinhalfangle * inversenorm;
            _v[2] = z * sinhalfangle * inversenorm;
            _v[3] = coshalfangle;
        }

        public void MakeRotate(double angle, VdsVec3d vec)
        {
            MakeRotate(angle, vec.X, vec.Y, vec.Z);
        }

        public void MakeRotate(double angle1, VdsVec3d axis1,
                                 double angle2, VdsVec3d axis2,
                                 double angle3, VdsVec3d axis3)
        {
            VdsQuat q1 = new VdsQuat();
            q1.MakeRotate(angle1, axis1);
            VdsQuat q2 = new VdsQuat();
            q2.MakeRotate(angle2, axis2);
            VdsQuat q3 = new VdsQuat();
            q3.MakeRotate(angle3, axis3);
            VdsQuat q = q1 * q2 * q3;
            SetQuat(q);
        }

        public void MakeRotate(VdsVec3d from, VdsVec3d to)
        {
            VdsVec3d sourceVector = new VdsVec3d(from);
            VdsVec3d targetVector = new VdsVec3d(to);
            double fromLen2 = from.Length2();
            double fromLen;
            if ((fromLen2 < 1.0 - 1e-7) || (fromLen2 > 1.0 + 1e-7))
            {
                fromLen = Math.Sqrt(fromLen2);
                sourceVector /= fromLen;
            }
            else fromLen = 1.0;
            double toLen2 = to.Length2();
            if ((toLen2 < 1.0 - 1e-7) || (toLen2 > 1.0 + 1e-7))
            {
                double toLen;
                if ((toLen2 > fromLen2 - 1e-7) && (toLen2 < fromLen2 + 1e-7))
                {
                    toLen = fromLen;
                }
                else toLen = Math.Sqrt(toLen2);
                targetVector /= toLen;
            }
            double dotProdPlus1 = 1.0 + sourceVector * targetVector;
            if (dotProdPlus1 < 1e-7)
            {
                if (Math.Abs(sourceVector.X) < 0.6)
                {
                    double norm = Math.Sqrt(1.0 - sourceVector.X * sourceVector.X);
                    _v[0] = 0.0;
                    _v[1] = sourceVector.Z / norm;
                    _v[2] = -sourceVector.Y / norm;
                    _v[3] = 0.0;
                }
                else if (Math.Abs(sourceVector.Y) < 0.6)
                {
                    double norm = Math.Sqrt(1.0 - sourceVector.Y * sourceVector.Y);
                    _v[0] = -sourceVector.Z / norm;
                    _v[1] = 0.0;
                    _v[2] = sourceVector.X / norm;
                    _v[3] = 0.0;
                }
                else
                {
                    double norm = Math.Sqrt(1.0 - sourceVector.Z * sourceVector.Z);
                    _v[0] = sourceVector.Y / norm;
                    _v[1] = -sourceVector.X / norm;
                    _v[2] = 0.0;
                    _v[3] = 0.0;
                }
            }

            else
            {
                double s = Math.Sqrt(0.5 * dotProdPlus1);
                VdsVec3d tmp = sourceVector ^ targetVector / (2.0 * s);
                _v[0] = tmp.X;
                _v[1] = tmp.Y;
                _v[2] = tmp.Z;
                _v[3] = s;
            }
        }

        public static VdsQuat Slerp(double interpolationValue, VdsQuat from, VdsQuat to)
        {
            double epsilon = 0.00001;
            double omega, cosomega, sinomega, scaleFrom, scaleTo;
            VdsQuat quatTo = new VdsQuat();
            quatTo.SetQuat(to);
            VdsVec4d f = new VdsVec4d(from._v[0], from._v[1], from._v[2], from._v[3]);
            VdsVec4d t = new VdsVec4d(to._v[0], to._v[1], to._v[2], to._v[3]);
            cosomega = f * t;
            if (cosomega < 0.0)
            {
                cosomega = -cosomega;
                quatTo = -to;
            }
            if ((1.0 - cosomega) > epsilon)
            {
                omega = Math.Acos(cosomega);
                sinomega = Math.Sin(omega);
                scaleFrom = Math.Sin((1.0 - interpolationValue) * omega) / sinomega;
                scaleTo = Math.Sin(interpolationValue * omega) / sinomega;
            }
            else
            {
                scaleFrom = 1.0 - interpolationValue;
                scaleTo = interpolationValue;
            }
            VdsQuat resultQuat = new VdsQuat();
            resultQuat = (from * scaleFrom) + (quatTo * scaleTo);
            return resultQuat;
        }
    }
}