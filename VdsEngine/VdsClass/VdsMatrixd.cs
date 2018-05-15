using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace VdsEngine
{
    /// <summary>
    /// 矩阵变换
    /// </summary>
    public class VdsMatrixd
    {
        private double[,] _mat = new double[4, 4];
        public double[,] Mat
        {
            get
            {
                return _mat;
            }
            private set
            {
                _mat = value;
            }
        }

        public VdsMatrixd()
        {
            MakeIdentity();
        }

        public VdsMatrixd(VdsMatrixd mat)
        {
            for (int r = 0; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    _mat[r, c] = mat._mat[r, c];
                }
            }
        }

        public VdsMatrixd(double a00, double a01, double a02, double a03,
                double a10, double a11, double a12, double a13,
                double a20, double a21, double a22, double a23,
                double a30, double a31, double a32, double a33)
        {
            SetMatrixd(a00, a01, a02, a03,
                a10, a11, a12, a13,
                a20, a21, a22, a23,
                a30, a31, a32, a33);
        }

        public void SetMatrixd(double a00, double a01, double a02, double a03,
            double a10, double a11, double a12, double a13,
            double a20, double a21, double a22, double a23,
            double a30, double a31, double a32, double a33)
        {
            SetRow(0, a00, a01, a02, a03);
            SetRow(1, a10, a11, a12, a13);
            SetRow(2, a20, a21, a22, a23);
            SetRow(3, a30, a31, a32, a33);
        }

        public void SetMatrixd(double[] array)
        {
            if (array.Length != 16)
                return;
            for (int i = 0; i < 4; ++i)
            {
                SetRow(i, array[i * 4], array[i * 4 + 1], array[i * 4 + 2], array[i * 4 + 3]);
            }
        }

        public void MakeIdentity()
        {
            SetRow(0, 1, 0, 0, 0);
            SetRow(1, 0, 1, 0, 0);
            SetRow(2, 0, 0, 1, 0);
            SetRow(3, 0, 0, 0, 1);
        }

        public bool IsNaN()
        {
            return Double.IsNaN(_mat[0, 0]) || Double.IsNaN(_mat[0, 1]) || Double.IsNaN(_mat[0, 2]) || Double.IsNaN(_mat[0, 3]) ||
                   Double.IsNaN(_mat[1, 0]) || Double.IsNaN(_mat[1, 1]) || Double.IsNaN(_mat[1, 2]) || Double.IsNaN(_mat[1, 3]) ||
                   Double.IsNaN(_mat[2, 0]) || Double.IsNaN(_mat[2, 1]) || Double.IsNaN(_mat[2, 2]) || Double.IsNaN(_mat[2, 3]) ||
                   Double.IsNaN(_mat[3, 0]) || Double.IsNaN(_mat[3, 1]) || Double.IsNaN(_mat[3, 2]) || Double.IsNaN(_mat[3, 3]);
        }

        public void MakeScale(VdsVec3d s)
        {
            SetRow(0, s.X, 0, 0, 0);
            SetRow(1, 0, s.Y, 0, 0);
            SetRow(2, 0, 0, s.Z, 0);
            SetRow(3, 0, 0, 0, 1);
        }

        public void MakeScale(double x, double y, double z)
        {
            SetRow(0, x, 0, 0, 0);
            SetRow(1, 0, y, 0, 0);
            SetRow(2, 0, 0, z, 0);
            SetRow(3, 0, 0, 0, 1);
        }

        public VdsVec3d getScale()
        {
            VdsVec3d x_vec = new VdsVec3d(_mat[0, 0], _mat[1, 0], _mat[2, 0]);
            VdsVec3d y_vec = new VdsVec3d(_mat[0, 1], _mat[1, 1], _mat[2, 1]);
            VdsVec3d z_vec = new VdsVec3d(_mat[0, 2], _mat[1, 2], _mat[2, 2]);
            return new VdsVec3d(x_vec.Length(), y_vec.Length(), z_vec.Length());
        }

        public void MakeTranslate(VdsVec3d t)
        {
            SetRow(0, 1, 0, 0, 0);
            SetRow(1, 0, 1, 0, 0);
            SetRow(2, 0, 0, 1, 0);
            SetRow(3, t.X, t.Y, t.Z, 1);
        }

        public VdsVec3d GetTrans()
        {
            return new VdsVec3d(_mat[3, 0], _mat[3, 1], _mat[3, 2]);
        }

        public void MakeTranslate(double x, double y, double z)
        {
            SetRow(0, 1, 0, 0, 0);
            SetRow(1, 0, 1, 0, 0);
            SetRow(2, 0, 0, 1, 0);
            SetRow(3, x, y, z, 1);
        }

        public void MakeRotate(VdsVec3d from, VdsVec3d to)
        {
            MakeIdentity();
            VdsQuat quat = new VdsQuat();
            quat.MakeRotate(from, to);
            MakeRotate(quat);
        }

        public void MakeRotate(double angle, VdsVec3d axis)
        {
            MakeIdentity();
            VdsQuat quat = new VdsQuat();
            quat.MakeRotate(angle, axis);
            MakeRotate(quat);
        }

        public void MakeRotate(double angle, double x, double y, double z)
        {
            MakeIdentity();
            VdsQuat quat = new VdsQuat();
            quat.MakeRotate(angle, x, y, z);
            MakeRotate(quat);
        }

        public void MakeRotate(VdsQuat q)
        {
            MakeIdentity();
            double length2 = q.Length2();
            if (Math.Abs(length2) <= Double.MinValue)
            {
                _mat[0, 0] = 0.0; _mat[1, 0] = 0.0; _mat[2, 0] = 0.0;
                _mat[0, 1] = 0.0; _mat[1, 1] = 0.0; _mat[2, 1] = 0.0;
                _mat[0, 2] = 0.0; _mat[1, 2] = 0.0; _mat[2, 2] = 0.0;
            }
            else
            {
                double rlength2;
                if (length2 != 1.0)
                {
                    rlength2 = 2.0 / length2;
                }
                else
                {
                    rlength2 = 2.0;
                }
                double wx, wy, wz, xx, yy, yz, xy, xz, zz, x2, y2, z2;
                x2 = rlength2 * q.QuatValue[0];
                y2 = rlength2 * q.QuatValue[1];
                z2 = rlength2 * q.QuatValue[2];

                xx = q.QuatValue[0] * x2;
                xy = q.QuatValue[0] * y2;
                xz = q.QuatValue[0] * z2;

                yy = q.QuatValue[1] * y2;
                yz = q.QuatValue[1] * z2;
                zz = q.QuatValue[2] * z2;

                wx = q.QuatValue[3] * x2;
                wy = q.QuatValue[3] * y2;
                wz = q.QuatValue[3] * z2;

                _mat[0, 0] = 1.0 - (yy + zz);
                _mat[1, 0] = xy - wz;
                _mat[2, 0] = xz + wy;

                _mat[0, 1] = xy + wz;
                _mat[1, 1] = 1.0 - (xx + zz);
                _mat[2, 1] = yz - wx;

                _mat[0, 2] = xz - wy;
                _mat[1, 2] = yz + wx;
                _mat[2, 2] = 1.0 - (xx + yy);
            }
        }

        public void MakeRotate(double angle1, VdsVec3d axis1,
                         double angle2, VdsVec3d axis2,
                         double angle3, VdsVec3d axis3)
        {
            MakeIdentity();
            VdsQuat quat = new VdsQuat();
            quat.MakeRotate(angle1, axis1,
                            angle2, axis2,
                            angle3, axis3);
            MakeRotate(quat);
        }

        public VdsQuat GetRotate()
        {
            VdsQuat q = new VdsQuat();
            double s;
            double[] tq = new double[4];
            int i, j;
            tq[0] = 1 + _mat[0, 0] + _mat[1, 1] + _mat[2, 2];
            tq[1] = 1 + _mat[0, 0] - _mat[1, 1] - _mat[2, 2];
            tq[2] = 1 - _mat[0, 0] + _mat[1, 1] - _mat[2, 2];
            tq[3] = 1 - _mat[0, 0] - _mat[1, 1] + _mat[2, 2];
            j = 0;
            for (i = 1; i < 4; i++) j = (tq[i] > tq[j]) ? i : j;
            if (j == 0)
            {
                q.QuatValue[3] = tq[0];
                q.QuatValue[0] = _mat[1, 2] - _mat[2, 1];
                q.QuatValue[1] = _mat[2, 0] - _mat[0, 2];
                q.QuatValue[2] = _mat[0, 1] - _mat[1, 0];
            }
            else if (j == 1)
            {
                q.QuatValue[3] = _mat[1, 2] - _mat[2, 1];
                q.QuatValue[0] = tq[1];
                q.QuatValue[1] = _mat[0, 1] + _mat[1, 0];
                q.QuatValue[2] = _mat[2, 0] + _mat[0, 2];
            }
            else if (j == 2)
            {
                q.QuatValue[3] = _mat[2, 0] - _mat[0, 2];
                q.QuatValue[0] = _mat[0, 1] + _mat[1, 0];
                q.QuatValue[1] = tq[2];
                q.QuatValue[2] = _mat[1, 2] + _mat[2, 1];
            }
            else
            {
                q.QuatValue[3] = _mat[0, 1] - _mat[1, 0];
                q.QuatValue[0] = _mat[2, 0] + _mat[0, 2];
                q.QuatValue[1] = _mat[1, 2] + _mat[2, 1];
                q.QuatValue[2] = tq[3];
            }
            s = Math.Sqrt(0.25 / tq[j]);
            q.QuatValue[3] *= s;
            q.QuatValue[0] *= s;
            q.QuatValue[1] *= s;
            q.QuatValue[2] *= s;
            return q;
        }

        public VdsMatrixd Inverse(VdsMatrixd matrix)
        {
            VdsMatrixd m = new VdsMatrixd();
            m.Invert(matrix);
            return m;
        }

        public bool Invert(VdsMatrixd rhs)
        {
            bool is_4x3 = (rhs._mat[0, 3] == 0.0 && rhs._mat[1, 3] == 0.0 && rhs._mat[2, 3] == 0.0 && rhs._mat[3, 3] == 1.0);
            return is_4x3 ? Invert_4x3(rhs) : Invert_4x4(rhs);
        }

        public VdsVec3d PreMult(VdsVec3d v)
        {
            double d = 1.0f / (_mat[0, 3] * v.X + _mat[1, 3] * v.Y + _mat[2, 3] * v.Z + _mat[3, 3]);
            return new VdsVec3d((_mat[0, 0] * v.X + _mat[1, 0] * v.Y + _mat[2, 0] * v.Z + _mat[3, 0]) * d,
                                (_mat[0, 1] * v.X + _mat[1, 1] * v.Y + _mat[2, 1] * v.Z + _mat[3, 1]) * d,
                                (_mat[0, 2] * v.X + _mat[1, 2] * v.Y + _mat[2, 2] * v.Z + _mat[3, 2]) * d);
        }

        public VdsVec4d PreMult(VdsVec4d v)
        {
            return new VdsVec4d((_mat[0, 0] * v.R + _mat[0, 1] * v.G + _mat[0, 2] * v.B + _mat[0, 3] * v.A),
                                (_mat[1, 0] * v.R + _mat[1, 1] * v.G + _mat[1, 2] * v.B + _mat[1, 3] * v.A),
                                (_mat[2, 0] * v.R + _mat[2, 1] * v.G + _mat[2, 2] * v.B + _mat[2, 3] * v.A),
                                (_mat[3, 0] * v.R + _mat[3, 1] * v.G + _mat[3, 2] * v.B + _mat[3, 3] * v.A));
        }

        public void PreMult(VdsMatrixd other)
        {
            double[] t = new double[4];
            for (int col = 0; col < 4; ++col)
            {
                t[0] = InnerProduct(other, this, 0, col);
                t[1] = InnerProduct(other, this, 1, col);
                t[2] = InnerProduct(other, this, 2, col);
                t[3] = InnerProduct(other, this, 3, col);
                _mat[0, col] = t[0];
                _mat[1, col] = t[1];
                _mat[2, col] = t[2];
                _mat[3, col] = t[3];
            }
        }

        public VdsVec3d PostMult(VdsVec3d v)
        {
            double d = 1.0f / (_mat[3, 0] * v.X + _mat[3, 1] * v.Y + _mat[3, 2] * v.Z + _mat[3, 3]);
            return new VdsVec3d((_mat[0, 0] * v.X + _mat[0, 1] * v.Y + _mat[0, 2] * v.Z + _mat[0, 3]) * d,
                                (_mat[1, 0] * v.X + _mat[1, 1] * v.Y + _mat[1, 2] * v.Z + _mat[1, 3]) * d,
                                (_mat[2, 0] * v.X + _mat[2, 1] * v.Y + _mat[2, 2] * v.Z + _mat[2, 3]) * d);
        }

        public VdsVec4d PostMult(VdsVec4d v)
        {
            return new VdsVec4d((_mat[0, 0] * v.R + _mat[0, 1] * v.G + _mat[0, 2] * v.B + _mat[0, 3] * v.A),
                                (_mat[1, 0] * v.R + _mat[1, 1] * v.G + _mat[1, 2] * v.B + _mat[1, 3] * v.A),
                                (_mat[2, 0] * v.R + _mat[2, 1] * v.G + _mat[2, 2] * v.B + _mat[2, 3] * v.A),
                                (_mat[3, 0] * v.R + _mat[3, 1] * v.G + _mat[3, 2] * v.B + _mat[3, 3] * v.A));
        }

        public void PostMult(VdsMatrixd other)
        {
            double[] t = new double[4];
            for (int row = 0; row < 4; ++row)
            {
                t[0] = InnerProduct(this, other, row, 0);
                t[1] = InnerProduct(this, other, row, 1);
                t[2] = InnerProduct(this, other, row, 2);
                t[3] = InnerProduct(this, other, row, 3);
                SetRow(row, t[0], t[1], t[2], t[3]);
            }
        }

        public void Mult(VdsMatrixd lhs, VdsMatrixd rhs)
        {
            if (lhs == this)
            {
                PostMult(rhs);
                return;
            }
            if (rhs == this)
            {
                PreMult(lhs);
                return;
            }
            _mat[0, 0] = InnerProduct(lhs, rhs, 0, 0);
            _mat[0, 1] = InnerProduct(lhs, rhs, 0, 1);
            _mat[0, 2] = InnerProduct(lhs, rhs, 0, 2);
            _mat[0, 3] = InnerProduct(lhs, rhs, 0, 3);
            _mat[1, 0] = InnerProduct(lhs, rhs, 1, 0);
            _mat[1, 1] = InnerProduct(lhs, rhs, 1, 1);
            _mat[1, 2] = InnerProduct(lhs, rhs, 1, 2);
            _mat[1, 3] = InnerProduct(lhs, rhs, 1, 3);
            _mat[2, 0] = InnerProduct(lhs, rhs, 2, 0);
            _mat[2, 1] = InnerProduct(lhs, rhs, 2, 1);
            _mat[2, 2] = InnerProduct(lhs, rhs, 2, 2);
            _mat[2, 3] = InnerProduct(lhs, rhs, 2, 3);
            _mat[3, 0] = InnerProduct(lhs, rhs, 3, 0);
            _mat[3, 1] = InnerProduct(lhs, rhs, 3, 1);
            _mat[3, 2] = InnerProduct(lhs, rhs, 3, 2);
            _mat[3, 3] = InnerProduct(lhs, rhs, 3, 3);
        }

        private void SetRow(int row, double v1, double v2, double v3, double v4)
        {
            _mat[row, 0] = (v1);
            _mat[row, 1] = (v2);
            _mat[row, 2] = (v3);
            _mat[row, 3] = (v4);
        }

        private double InnerProduct(VdsMatrixd a, VdsMatrixd b, int r, int c)
        {
            return (a._mat[r, 0] * (b)._mat[0, c])
                 + (a._mat[r, 1] * (b)._mat[1, c])
                 + (a._mat[r, 2] * (b)._mat[2, c])
                 + (a._mat[r, 3] * (b)._mat[3, c]);
        }

        private bool Invert_4x4(VdsMatrixd mat)
        {
            if (mat == this)
            {
                VdsMatrixd tm = new VdsMatrixd(mat);
                return Invert_4x4(tm);
            }
            int[] indxc = new int[4];
            int[] indxr = new int[4];
            int[] ipiv = new int[4];
            int i, j, k, l, ll;
            int icol = 0;
            int irow = 0;
            double pivinv, dum, big;
            Mat = mat.Mat;
            for (j = 0; j < 4; j++)
            {
                ipiv[j] = 0;
            }
            for (i = 0; i < 4; i++)
            {
                big = 0.0;
                for (j = 0; j < 4; j++)
                {
                    if (ipiv[j] != 1)
                    {
                        for (k = 0; k < 4; k++)
                        {
                            if (ipiv[k] == 0)
                            {
                                if (Math.Abs(Mat[j, k]) >= big)
                                {
                                    big = Math.Abs(Mat[j, k]);
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (ipiv[k] > 1)
                                return false;
                        }
                    }
                }
                ++(ipiv[icol]);
                if (irow != icol)
                {
                    for (l = 0; l < 4; l++)
                    {
                        StaticMethod.Swap(ref Mat[irow, l], ref Mat[icol, l]);
                    }
                }
                indxr[i] = irow;
                indxc[i] = icol;
                if (Mat[icol, icol] == 0)
                    return false;
                pivinv = 1.0 / Mat[icol, icol];
                Mat[icol, icol] = 1;
                for (l = 0; l < 4; l++)
                {
                    Mat[icol, l] *= pivinv;
                }
                for (ll = 0; ll < 4; ll++)
                {
                    if (ll != icol)
                    {
                        dum = Mat[ll, icol];
                        Mat[ll, icol] = 0;
                        for (l = 0; l < 4; l++) Mat[ll, l] -= Mat[icol, l] * dum;
                    }
                }
            }
            for (int lx = 4; lx > 0; --lx)
            {
                if (indxr[lx - 1] != indxc[lx - 1])
                {
                    for (k = 0; k < 4; k++)
                    {
                        StaticMethod.Swap(ref Mat[k, indxr[lx - 1]], ref Mat[k, indxc[lx - 1]]);
                    }
                }
            }
            return true;
        }

        private bool Invert_4x3(VdsMatrixd mat)
        {
            if (mat == this)
            {
                VdsMatrixd tm = new VdsMatrixd(mat);
                return Invert_4x3(tm);
            }
            double r00, r01, r02, r10, r11, r12, r20, r21, r22;
            r00 = mat._mat[0, 0];
            r01 = mat._mat[0, 1];
            r02 = mat._mat[0, 2];
            r10 = mat._mat[1, 0];
            r11 = mat._mat[1, 1];
            r12 = mat._mat[1, 2];
            r20 = mat._mat[2, 0];
            r21 = mat._mat[2, 1];
            r22 = mat._mat[2, 2];
            _mat[0, 0] = r11 * r22 - r12 * r21;
            _mat[0, 1] = r02 * r21 - r01 * r22;
            _mat[0, 2] = r01 * r12 - r02 * r11;
            double oneOverDet = 1.0 / (r00 * _mat[0, 0] + r10 * _mat[0, 1] + r20 * _mat[0, 2]);
            r00 *= oneOverDet;
            r10 *= oneOverDet;
            r20 *= oneOverDet;
            _mat[0, 0] *= oneOverDet;
            _mat[0, 1] *= oneOverDet;
            _mat[0, 2] *= oneOverDet;
            _mat[0, 3] = 0.0;
            _mat[1, 0] = r12 * r20 - r10 * r22;
            _mat[1, 1] = r00 * r22 - r02 * r20;
            _mat[1, 2] = r02 * r10 - r00 * r12;
            _mat[1, 3] = 0.0;
            _mat[2, 0] = r10 * r21 - r11 * r20;
            _mat[2, 1] = r01 * r20 - r00 * r21;
            _mat[2, 2] = r00 * r11 - r01 * r10;
            _mat[2, 3] = 0.0;
            _mat[3, 3] = 1.0;
            r22 = mat._mat[3, 3];
            if ((r22 - 1.0) * (r22 - 1.0) > 0.0000001)
            {
                VdsMatrixd tpInv = new VdsMatrixd();
                _mat[3, 0] = _mat[3, 1] = _mat[3, 2] = 0.0;
                r10 = mat._mat[0, 3];
                r11 = mat._mat[1, 3];
                r12 = mat._mat[2, 3];
                r00 = _mat[0, 0] * r10 + _mat[0, 1] * r11 + _mat[0, 2] * r12;
                r01 = _mat[1, 0] * r10 + _mat[1, 1] * r11 + _mat[1, 2] * r12;
                r02 = _mat[2, 0] * r10 + _mat[2, 1] * r11 + _mat[2, 2] * r12;

                r10 = mat._mat[3, 0];
                r11 = mat._mat[3, 1];
                r12 = mat._mat[3, 2];
                oneOverDet = 1.0 / (r22 - (r10 * r00 + r11 * r01 + r12 * r02));
                r10 *= oneOverDet;
                r11 *= oneOverDet;
                r12 *= oneOverDet;
                tpInv._mat[0, 0] = r10 * r00 + 1.0;
                tpInv._mat[0, 1] = r11 * r00;
                tpInv._mat[0, 2] = r12 * r00;
                tpInv._mat[0, 3] = -r00 * oneOverDet;
                tpInv._mat[1, 0] = r10 * r01;
                tpInv._mat[1, 1] = r11 * r01 + 1.0;
                tpInv._mat[1, 2] = r12 * r01;
                tpInv._mat[1, 3] = -r01 * oneOverDet;
                tpInv._mat[2, 0] = r10 * r02;
                tpInv._mat[2, 1] = r11 * r02;
                tpInv._mat[2, 2] = r12 * r02 + 1.0;
                tpInv._mat[2, 3] = -r02 * oneOverDet;
                tpInv._mat[3, 0] = -r10;
                tpInv._mat[3, 1] = -r11;
                tpInv._mat[3, 2] = -r12;
                tpInv._mat[3, 3] = oneOverDet;
                PreMult(tpInv);
            }
            else
            {
                r10 = mat._mat[3, 0];
                r11 = mat._mat[3, 1];
                r12 = mat._mat[3, 2];
                _mat[3, 0] = -(r10 * _mat[0, 0] + r11 * _mat[1, 0] + r12 * _mat[2, 0]);
                _mat[3, 1] = -(r10 * _mat[0, 1] + r11 * _mat[1, 1] + r12 * _mat[2, 1]);
                _mat[3, 2] = -(r10 * _mat[0, 2] + r11 * _mat[1, 2] + r12 * _mat[2, 2]);
            }
            return true;
        }

        public static void HprToMatrix(ref VdsMatrixd rotation, VdsVec3d hpr)
        {
            double tmp = hpr.X;
            hpr.X = hpr.Z;
            hpr.Z = hpr.Y;
            hpr.Y = tmp;
            double ch, sh, cp, sp, cr, sr, srsp, crsp, srcp;
            double magicEpsilon = 0.00001;
            if (StaticMethod.Equivalent(hpr.X, 0.0, magicEpsilon))
            {
                ch = 1.0;
                sh = 0.0;
            }
            else
            {
                sh = Math.Sin(StaticMethod.DegreesToRadians(hpr.X));
                ch = Math.Cos(StaticMethod.DegreesToRadians(hpr.X));
            }
            if (StaticMethod.Equivalent(hpr.Y, 0.0, magicEpsilon))
            {
                cp = 1.0;
                sp = 0.0;
            }
            else
            {
                sp = Math.Sin(StaticMethod.DegreesToRadians(hpr.Y));
                cp = Math.Cos(StaticMethod.DegreesToRadians(hpr.Y));
            }
            if (StaticMethod.Equivalent(hpr.Z, 0.0, magicEpsilon))
            {
                cr = 1.0;
                sr = 0.0;
                srsp = 0.0;
                srcp = 0.0;
                crsp = sp;
            }
            else
            {
                sr = Math.Sin(StaticMethod.DegreesToRadians(hpr.Z));
                cr = Math.Cos(StaticMethod.DegreesToRadians(hpr.Z));
                srsp = sr * sp;
                crsp = cr * sp;
                srcp = sr * cp;
            }
            rotation.SetMatrixd(ch * cr - sh * srsp, cr * sh + srsp * ch, -srcp, 0.0,
                                -sh * cp, ch * cp, sp, 0.0,
                                sr * ch + sh * crsp, sr * sh - crsp * ch, cr * cp, 0.0,
                                0.0, 0.0, 0.0, 1.0);
        }

        public static void MatrixToHpr(ref VdsVec3d hpr, VdsMatrixd rotation)
        {
            VdsMatrixd mat = new VdsMatrixd();
            VdsVec3d col1 = new VdsVec3d(rotation.Mat[0, 0], rotation.Mat[0, 1], rotation.Mat[0, 2]);
            double s = col1.Length();
            const double magicEpsilon = 0.00001;
            if (s <= magicEpsilon)
            {
                hpr.X = 0.0;
                hpr.Y = 0.0;
                hpr.Z = 0.0;
                return;
            }
            double oneOverS = 1.0f / s;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    mat.Mat[i, j] = rotation.Mat[i, j] * oneOverS;
                }
            }
            double sinPitch = StaticMethod.ClampUnity(mat.Mat[1, 2]);
            double pitch = Math.Asin(sinPitch);
            hpr.Y = StaticMethod.RadiansToDegrees(pitch);
            double cp = Math.Cos(pitch);
            if (cp > -magicEpsilon && cp < magicEpsilon)
            {
                double cr = StaticMethod.ClampUnity(-mat.Mat[2, 1]);
                double sr = StaticMethod.ClampUnity(mat.Mat[0, 1]);
                hpr.X = 0.0f;
                hpr.Z = StaticMethod.RadiansToDegrees(Math.Atan2(sr, cr));
            }
            else
            {
                double oneOverCp = 1.0 / cp;
                double sr = StaticMethod.ClampUnity(-mat.Mat[0, 2] * oneOverCp);
                double cr = StaticMethod.ClampUnity(mat.Mat[2, 2] * oneOverCp);
                double sh = StaticMethod.ClampUnity(-mat.Mat[1, 0] * oneOverCp);
                double ch = StaticMethod.ClampUnity(mat.Mat[1, 1] * oneOverCp);
                if ((StaticMethod.Equivalent(sh, 0.0, magicEpsilon) && StaticMethod.Equivalent(ch, 0.0, magicEpsilon)) ||
                    (StaticMethod.Equivalent(sr, 0.0, magicEpsilon) && StaticMethod.Equivalent(cr, 0.0, magicEpsilon)))
                {
                    cr = StaticMethod.ClampUnity(-mat.Mat[2, 1]);
                    sr = StaticMethod.ClampUnity(mat.Mat[0, 1]); ;
                    hpr.X = 0.0f;
                }
                else
                {
                    hpr.X = StaticMethod.RadiansToDegrees(Math.Atan2(sh, ch));
                }
                hpr.Z = StaticMethod.RadiansToDegrees(Math.Atan2(sr, cr));
            }
            double tmp = hpr.X;
            hpr.X = hpr.Y;
            hpr.Y = hpr.Z;
            hpr.Z = tmp;
        }
    }
}