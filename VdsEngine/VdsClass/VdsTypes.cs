using System;
using System.Collections.Generic;

namespace VdsEngine
{
    /// <summary>
    /// int
    /// </summary>
    public sealed class IntProperty : IPropertyUIInterface
    {
        public int Value
        {
            get;
            set;
        }

        public IntProperty()
        {
            Value = 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void FromString(string str)
        {
            int v = 0;
            bool result = int.TryParse(str, out v);
            Value = v;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// int list
    /// </summary>
    public sealed class IntListProperty : IPropertyUIInterface
    {
        public List<int> ValueList
        {
            get;
            set;
        }

        public IntListProperty()
        {
            ValueList = new List<int>();
        }

        public override string ToString()
        {
            if (ValueList == null || ValueList.Count < 1)
                return "";
            string resultStr = "";
            for (int i = 0; i < ValueList.Count; ++i)
            {
                string vs = ValueList[i].ToString();
                if (i == 0)
                    resultStr += vs;
                else
                {
                    resultStr += ",";
                    resultStr += vs;
                }
            }
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            List<int> resultList = new List<int>(vList.Length);
            int v = 0;
            for (int i = 0; i < vList.Length; ++i)
            {
                bool result = int.TryParse(vList[i], out v);
                resultList.Add(v);
            }
            ValueList = resultList;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// double
    /// </summary>
    public sealed class DoubleProperty : IPropertyUIInterface
    {
        public double Value
        {
            get;
            set;
        }

        public DoubleProperty()
        {
            Value = 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void FromString(string str)
        {
            double v = 0;
            bool result = double.TryParse(str, out v);
            Value = v;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// double list
    /// </summary>
    public sealed class DoubleListProperty : IPropertyUIInterface
    {
        public List<double> ValueList
        {
            get;
            set;
        }

        public DoubleListProperty()
        {
            ValueList = new List<double>();
        }

        public override string ToString()
        {
            if (ValueList == null || ValueList.Count < 1)
                return "";
            string resultStr = "";
            for (int i = 0; i < ValueList.Count; ++i)
            {
                string vs = ValueList[i].ToString();
                if (i == 0)
                    resultStr += vs;
                else
                {
                    resultStr += ",";
                    resultStr += vs;
                }
            }
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            List<double> resultList = new List<double>(vList.Length);
            double v = 0;
            for (int i = 0; i < vList.Length; ++i)
            {
                bool result = double.TryParse(vList[i], out v);
                resultList.Add(v);
            }
            ValueList = resultList;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// bool
    /// </summary>
    public sealed class BoolProperty : IPropertyUIInterface
    {
        public bool Value
        {
            get;
            set;
        }

        public BoolProperty()
        {
            Value = false;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void FromString(string str)
        {
            bool v = false;
            bool result = bool.TryParse(str, out v);
            Value = v;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// string
    /// </summary>
    public sealed class StringProperty : IPropertyUIInterface
    {
        public string Value
        {
            get;
            set;
        }

        public StringProperty()
        {
            Value = "";
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void FromString(string str)
        {
            Value = str;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// file path
    /// </summary>
    public sealed class FilePathProperty : IPropertyUIInterface
    {
        public string Value
        {
            get;
            set;
        }

        public FilePathProperty()
        {
            Value = "";
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void FromString(string str)
        {
            Value = str;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// string list
    /// </summary>
    public sealed class StringListProperty : IPropertyUIInterface
    {
        public List<string> ValueList
        {
            get;
            set;
        }

        public StringListProperty()
        {
            ValueList = new List<string>();
        }

        public override string ToString()
        {
            if (ValueList == null || ValueList.Count < 1)
                return "";
            string resultStr = "";
            for (int i = 0; i < ValueList.Count; ++i)
            {
                string vs = ValueList[i].ToString();
                if (i == 0)
                    resultStr += vs;
                else
                {
                    resultStr += ",";
                    resultStr += vs;
                }
            }
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultList = new List<string>(vList.Length);
            for (int i = 0; i < vList.Length; ++i)
            {
                resultList.Add(vList[i]);
            }
            ValueList = resultList;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// enum
    /// </summary>
    public sealed class EnumProperty : IPropertyUIInterface
    {
        public List<string> ValueList
        {
            get;
            set;
        }

        public EnumProperty()
        {
            ValueList = new List<string>();
        }

        public override string ToString()
        {
            if (ValueList == null || ValueList.Count < 1)
                return "";
            string resultStr = "";
            for (int i = 0; i < ValueList.Count; ++i)
            {
                string vs = ValueList[i].ToString();
                if (i == 0)
                    resultStr += vs;
                else
                {
                    resultStr += ",";
                    resultStr += vs;
                }
            }
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultList = new List<string>(vList.Length);
            for (int i = 0; i < vList.Length; ++i)
            {
                resultList.Add(vList[i]);
            }
            ValueList = resultList;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// 二元向量
    /// </summary>
    public sealed class VdsVec2d : IPropertyUIInterface
    {
        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }

        public VdsVec2d()
        { }

        public VdsVec2d(VdsVec2d v)
        {
            X = v.X;
            Y = v.Y;
        }

        public VdsVec2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(VdsVec2d v0, VdsVec2d v1)
        {
            if (ReferenceEquals(v0, null) && ReferenceEquals(v1, null))
                return true;
            else if (ReferenceEquals(v0, null) || ReferenceEquals(v1, null))
                return false;
            return v0.X == v1.X && v0.Y == v1.Y;
        }

        public static bool operator !=(VdsVec2d v0, VdsVec2d v1)
        {
            if (ReferenceEquals(v0, null) && ReferenceEquals(v1, null))
                return false;
            else if (ReferenceEquals(v0, null) || ReferenceEquals(v1, null))
                return true;
            return v0.X != v1.X || v0.Y != v1.Y;
        }

        public bool IsNaN()
        {
            return Double.IsNaN(X) || Double.IsNaN(Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this == (VdsVec2d)obj;
        }

        public override int GetHashCode()
        {
            return (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" + Convert.ToString(X)).GetHashCode();
        }

        public override string ToString()
        {
            return Convert.ToString(X) + " " + Convert.ToString(Y);
        }

        public void FromString(string str)
        {
            try
            {
                char[] c = new char[1] { ' ' };
                string[] sList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
                if (sList.Length > 0)
                    X = Convert.ToDouble(sList[0]);
                if (sList.Length > 1)
                    Y = Convert.ToDouble(sList[1]);
            }
            catch
            { }
        }

        public static VdsVec2d operator +(VdsVec2d v0, VdsVec2d v1)
        {
            return new VdsVec2d(v0.X + v1.X, v0.Y + v1.Y);
        }

        public static VdsVec2d operator -(VdsVec2d v)
        {
            return new VdsVec2d(-v.X, -v.Y);
        }

        public static VdsVec2d operator -(VdsVec2d v0, VdsVec2d v1)
        {
            return new VdsVec2d(v0.X - v1.X, v0.Y - v1.Y);
        }

        public static double operator *(VdsVec2d v0, VdsVec2d v1)
        {
            return v0.X * v1.X + v0.Y * v1.Y;
        }

        public static VdsVec2d operator *(VdsVec2d v0, double v)
        {
            return new VdsVec2d(v0.X * v, v0.Y * v);
        }

        public static VdsVec2d operator /(VdsVec2d v0, double v)
        {
            double x = v0.X / v;
            double y = v0.Y / v;
            return new VdsVec2d(x, y);
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public double Length2()
        {
            return X * X + Y * Y;
        }

        public double Normalize()
        {
            double norm = Length();
            if (norm > 0.0)
            {
                double inv = 1.0 / norm;
                X *= inv;
                Y *= inv;
            }
            return norm;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// 二元向量数组
    /// </summary>
    public sealed class VdsVec2dList : IPropertyUIInterface
    {
        public List<VdsVec2d> ValueList
        {
            get;
            set;
        }

        public VdsVec2dList()
        {
            ValueList = new List<VdsVec2d>();
        }

        public override string ToString()
        {
            if (ValueList == null || ValueList.Count < 1)
                return "";
            string resultStr = "";
            for (int i = 0; i < ValueList.Count; ++i)
            {
                string vs = ValueList[i].ToString();
                if (i == 0)
                    resultStr += vs;
                else
                {
                    resultStr += ",";
                    resultStr += vs;
                }
            }
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            List<VdsVec2d> resultList = new List<VdsVec2d>(vList.Length);
            for (int i = 0; i < vList.Length; ++i)
            {
                VdsVec2d v = new VdsVec2d();
                v.FromString(vList[i]);
                resultList.Add(v);
            }
            ValueList = resultList;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// 三元向量
    /// </summary>
    public sealed class VdsVec3d : IPropertyUIInterface
    {
        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }

        public double Z
        {
            get;
            set;
        }

        public VdsVec3d()
        { }

        public VdsVec3d(VdsVec3d v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public VdsVec3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static bool operator ==(VdsVec3d v0, VdsVec3d v1)
        {
            if (ReferenceEquals(v0, null) && ReferenceEquals(v1, null))
                return true;
            else if (ReferenceEquals(v0, null) || ReferenceEquals(v1, null))
                return false;
            return v0.X == v1.X && v0.Y == v1.Y && v0.Z == v1.Y;
        }

        public static bool operator !=(VdsVec3d v0, VdsVec3d v1)
        {
            if (ReferenceEquals(v0, null) && ReferenceEquals(v1, null))
                return false;
            else if (ReferenceEquals(v0, null) || ReferenceEquals(v1, null))
                return true;
            return v0.X != v1.X || v0.Y != v1.Y || v0.Z != v1.Z;
        }

        public bool IsNaN()
        {
            return Double.IsNaN(X) || Double.IsNaN(Y) || Double.IsNaN(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this == (VdsVec3d)obj;
        }

        public override int GetHashCode()
        {
            return (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" + Convert.ToString(X)).GetHashCode();
        }

        public override string ToString()
        {
            return Convert.ToString(X) + " " + Convert.ToString(Y) + " " + Convert.ToString(Z);
        }

        public void FromString(string str)
        {
            try
            {
                char[] c = new char[1] { ' ' };
                string[] sList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
                if (sList.Length > 0)
                    X = Convert.ToDouble(sList[0]);
                if (sList.Length > 1)
                    Y = Convert.ToDouble(sList[1]);
                if (sList.Length > 2)
                    Z = Convert.ToDouble(sList[2]);
            }
            catch
            { }
        }

        public static VdsVec3d operator +(VdsVec3d v0, VdsVec3d v1)
        {
            return new VdsVec3d(v0.X + v1.X, v0.Y + v1.Y, v0.Z + v1.Z);
        }

        public static VdsVec3d operator -(VdsVec3d v)
        {
            return new VdsVec3d(-v.X, -v.Y, -v.Z);
        }

        public static VdsVec3d operator -(VdsVec3d v0, VdsVec3d v1)
        {
            return new VdsVec3d(v0.X - v1.X, v0.Y - v1.Y, v0.Z - v1.Z);
        }

        public static double operator *(VdsVec3d v0, VdsVec3d v1)
        {
            return v0.X * v1.X + v0.Y * v1.Y + v0.Z * v1.Z;
        }

        public static VdsVec3d operator *(VdsVec3d v0, double v1)
        {
            return new VdsVec3d(v0.X * v1, v0.Y * v1, v0.Z * v1);
        }

        public static VdsVec3d operator /(VdsVec3d v0, double v1)
        {
            double x = v0.X / v1;
            double y = v0.Y / v1;
            double z = v0.Z / v1;
            return new VdsVec3d(x, y, z);
        }

        public static VdsVec3d operator ^(VdsVec3d v0, VdsVec3d v1)
        {
            return new VdsVec3d(v0.Y * v1.Z - v0.Z * v1.Y,
                                v0.Z * v1.X - v0.X * v1.Z,
                                v0.X * v1.Y - v0.Y * v1.X);
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public double Length2()
        {
            return X * X + Y * Y + Z * Z;
        }

        public double Normalize()
        {
            double norm = Length();
            if (norm > 0.0)
            {
                double inv = 1.0 / norm;
                X *= inv;
                Y *= inv;
                Z *= inv;
            }
            return norm;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// 三元向量数组
    /// </summary>
    public sealed class VdsVec3dList : IPropertyUIInterface
    {
        public List<VdsVec3d> ValueList
        {
            get;
            set;
        }

        public VdsVec3dList()
        {
            ValueList = new List<VdsVec3d>();
        }

        public override string ToString()
        {
            if (ValueList == null || ValueList.Count < 1)
                return "";
            string resultStr = "";
            for (int i = 0; i < ValueList.Count; ++i)
            {
                string vs = ValueList[i].ToString();
                if (i == 0)
                    resultStr += vs;
                else
                {
                    resultStr += ",";
                    resultStr += vs;
                }
            }
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            List<VdsVec3d> resultList = new List<VdsVec3d>(vList.Length);
            for (int i = 0; i < vList.Length; ++i)
            {
                VdsVec3d v = new VdsVec3d();
                v.FromString(vList[i]);
                resultList.Add(v);
            }
            ValueList = resultList;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// 相机姿态
    /// </summary>
    public sealed class CameraPoseProperty : IPropertyUIInterface
    {
        public VdsVec3d Eye
        {
            get;
            set;
        }

        public VdsVec3d Center
        {
            get;
            set;
        }

        public VdsVec3d Up
        {
            get;
            set;
        }

        public CameraPoseProperty()
        {
            Eye = new VdsVec3d(50, 50, 50);
            Center = new VdsVec3d();
            Up = new VdsVec3d(0, 0, 1);
        }

        public override string ToString()
        {
            string resultStr = Eye.ToString();
            resultStr += ",";
            resultStr += Center.ToString();
            resultStr += ",";
            resultStr += Up.ToString();
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            if (vList.Length > 0)
            {
                VdsVec3d v = new VdsVec3d();
                v.FromString(vList[0]);
                Eye = v;
            }
            if (vList.Length > 1)
            {
                VdsVec3d v = new VdsVec3d();
                v.FromString(vList[1]);
                Center = v;
            }
            if (vList.Length > 2)
            {
                VdsVec3d v = new VdsVec3d();
                v.FromString(vList[2]);
                Up = v;
            }
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// 四元向量
    /// </summary>
    public sealed class VdsVec4d : IPropertyUIInterface
    {
        public double R
        {
            get;
            set;
        }

        public double G
        {
            get;
            set;
        }

        public double B
        {
            get;
            set;
        }

        public double A
        {
            get;
            set;
        }

        public VdsVec4d()
        { }

        public VdsVec4d(VdsVec4d v)
        {
            R = v.R;
            G = v.G;
            B = v.B;
            A = v.A;
        }

        public VdsVec4d(double x, double y, double z, double w)
        {
            R = x;
            G = y;
            B = z;
            A = w;
        }

        public static bool operator ==(VdsVec4d v0, VdsVec4d v1)
        {
            if (ReferenceEquals(v0, null) && ReferenceEquals(v1, null))
                return true;
            else if (ReferenceEquals(v0, null) || ReferenceEquals(v1, null))
                return false;
            return v0.R == v1.R && v0.G == v1.G && v0.B == v1.G && v0.A == v1.A;
        }

        public static bool operator !=(VdsVec4d v0, VdsVec4d v1)
        {
            if (ReferenceEquals(v0, null) && ReferenceEquals(v1, null))
                return false;
            else if (ReferenceEquals(v0, null) || ReferenceEquals(v1, null))
                return true;
            return v0.R != v1.R || v0.G != v1.G || v0.B != v1.B || v0.A != v1.A;
        }

        public bool IsNaN()
        {
            return Double.IsNaN(R) || Double.IsNaN(G) || Double.IsNaN(B) || Double.IsNaN(A);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this == (VdsVec4d)obj;
        }

        public override int GetHashCode()
        {
            return (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" + Convert.ToString(R)).GetHashCode();
        }

        public override string ToString()
        {
            return Convert.ToString(R) + " " + Convert.ToString(G) + " " + Convert.ToString(B) + " " + Convert.ToString(A);
        }

        public void FromString(string str)
        {
            try
            {
                char[] c = new char[1] { ' ' };
                string[] sList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
                if (sList.Length > 0)
                    R = Convert.ToDouble(sList[0]);
                if (sList.Length > 1)
                    G = Convert.ToDouble(sList[1]);
                if (sList.Length > 2)
                    B = Convert.ToDouble(sList[2]);
                if (sList.Length > 3)
                    A = Convert.ToDouble(sList[3]);
            }
            catch
            { }
        }

        public static VdsVec4d operator +(VdsVec4d v0, VdsVec4d v1)
        {
            return new VdsVec4d(v0.R + v1.R, v0.G + v1.G, v0.B + v1.B, v0.A + v1.A);
        }

        public static VdsVec4d operator -(VdsVec4d v)
        {
            return new VdsVec4d(-v.R, -v.G, -v.B, -v.A);
        }

        public static VdsVec4d operator -(VdsVec4d v0, VdsVec4d v1)
        {
            return new VdsVec4d(v0.R - v1.R, v0.G - v1.G, v0.B - v1.B, v0.A - v1.A);
        }

        public static double operator *(VdsVec4d v0, VdsVec4d v1)
        {
            return v0.R * v1.R + v0.G * v1.G + v0.B * v1.B + v0.A * v1.A;
        }

        public static VdsVec4d operator *(VdsVec4d v0, double v)
        {
            return new VdsVec4d(v0.R * v, v0.G * v, v0.B * v, v0.A * v);
        }

        public static VdsVec4d operator /(VdsVec4d v0, double v)
        {
            double x = v0.R / v;
            double y = v0.G / v;
            double z = v0.B / v;
            double w = v0.A / v;
            return new VdsVec4d(x, y, z, w);
        }

        public double Length()
        {
            return Math.Sqrt(R * R + G * G + B * B + A * A);
        }

        public double Length2()
        {
            return R * R + G * G + B * B + A * A;
        }

        public double Normalize()
        {
            double norm = Length();
            if (norm > 0.0)
            {
                double inv = 1.0 / norm;
                R *= inv;
                G *= inv;
                B *= inv;
                A *= inv;
            }
            return norm;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
    /// <summary>
    /// 四元向量数组
    /// </summary>
    public sealed class VdsVec4dList : IPropertyUIInterface
    {
        public List<VdsVec4d> ValueList
        {
            get;
            set;
        }

        public VdsVec4dList()
        {
            ValueList = new List<VdsVec4d>();
        }

        public override string ToString()
        {
            if (ValueList == null || ValueList.Count < 1)
                return "";
            string resultStr = "";
            for (int i = 0; i < ValueList.Count; ++i)
            {
                string vs = ValueList[i].ToString();
                if (i == 0)
                    resultStr += vs;
                else
                {
                    resultStr += ",";
                    resultStr += vs;
                }
            }
            return resultStr;
        }

        public void FromString(string str)
        {
            char[] c = new char[1] { ',' };
            string[] vList = str.Split(c, StringSplitOptions.RemoveEmptyEntries);
            List<VdsVec4d> resultList = new List<VdsVec4d>(vList.Length);
            for (int i = 0; i < vList.Length; ++i)
            {
                VdsVec4d v = new VdsVec4d();
                v.FromString(vList[i]);
                resultList.Add(v);
            }
            ValueList = resultList;
        }

        public string PropertyToString()
        {
            return ToString();
        }

        public void PropertyFromString(string s)
        {
            FromString(s);
        }
    }
}