using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VdsEngine
{
    /// <summary>
    /// 封装图形
    /// </summary>
    public class VdsGeometry : PtrClass
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static void IUpdateGeometry(IntPtr thisPtr, VdsVec3d[] vArray, VdsVec3d[] nArray, VdsVec4d[] cArray, VdsVec2d[] tArray,
                                                int drawModel, int cullFaceMode, string textureFileName, VdsVec4d[] maturalArray);
        #endregion
        public List<VdsVec3d> VertexArray
        {
            get
            {
                return _vertexArray;
            }
            set
            {
                if (_vertexArray != null && _vertexArray.Count != value.Count)
                    return;
                _vertexArray = value;
            }
        }

        public List<VdsVec3d> NormalArray
        {
            get
            {
                return _normalArray;
            }
            set
            {
                if (_normalArray != null && _normalArray.Count != value.Count)
                    return;
                _normalArray = value;
            }
        }

        public List<VdsVec4d> ColorArray
        {
            get
            {
                return _colorArray;
            }
            set
            {
                if (_colorArray != null && _colorArray.Count != value.Count)
                    return;
                _colorArray = value;
            }
        }

        public List<VdsVec2d> TexCoordArray
        {
            get
            {
                return _texCoordArray;
            }
            set
            {
                if (_texCoordArray != null && _texCoordArray.Count != value.Count)
                    return;
                _texCoordArray = value;
            }
        }

        public enum GeometryDrawModel
        {
            POINTS,
            LINES,
            LINE_STRIP,
            LINE_LOOP,
            TRIANGLES,
            TRIANGLE_STRIP,
            TRIANGLE_FAN,
            QUADS,
            QUAD_STRIP,
        }

        public enum GeometryCullFaceMode
        {
            FRONT,
            BACK,
            FRONT_AND_BACK,
            NOCULLFACE,
            NOTSET,
        };

        public GeometryDrawModel DrawMode
        {
            get;
            private set;
        }

        public GeometryCullFaceMode CullFaceMode
        {
            get;
            set;
        }

        public string TextureFileName
        {
            get;
            set;
        }

        public VdsVec4d AmbientMatural
        {
            get;
            set;
        }

        public VdsVec4d DiffuseMatural
        {
            get;
            set;
        }

        public VdsVec4d SpecularMatural
        {
            get;
            set;
        }

        public VdsVec4d EmissionMatural
        {
            get;
            set;
        }

        private List<VdsVec3d> _vertexArray = null;
        private List<VdsVec3d> _normalArray = null;
        private List<VdsVec4d> _colorArray = null;
        private List<VdsVec2d> _texCoordArray = null;

        public VdsGeometry()
            : base(true)
        {
            DrawMode = GeometryDrawModel.TRIANGLES;
            CullFaceMode = GeometryCullFaceMode.NOTSET;
        }

        public VdsGeometry(GeometryDrawModel drawModel)
            : base(true)
        {
            DrawMode = drawModel;
            CullFaceMode = GeometryCullFaceMode.NOTSET;
        }

        public void UpdateGeometry()
        {
            if (VertexArray == null)
                return;
            int updateThreadID = GlobalEnvironment.UpdateThreadID;
            int curThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (updateThreadID != curThreadID)
                return;
            VdsVec3d[] vArray = VertexArray.ToArray();
            VdsVec3d[] nArray = null;
            if (NormalArray != null)
                nArray = NormalArray.ToArray();
            else
                nArray = new VdsVec3d[0];
            VdsVec4d[] cArray = null;
            if (ColorArray != null)
                cArray = ColorArray.ToArray();
            else
                cArray = new VdsVec4d[0];
            VdsVec2d[] tArray = null;
            if (TexCoordArray != null)
                tArray = TexCoordArray.ToArray();
            else
                tArray = new VdsVec2d[0];

            VdsVec4d[] maturalArray = null;
            if (AmbientMatural == null && DiffuseMatural == null && SpecularMatural == null && EmissionMatural == null)
                maturalArray = new VdsVec4d[0];
            else
            {
                maturalArray = new VdsVec4d[4];
                if (AmbientMatural == null)
                    maturalArray[0] = new VdsVec4d(0.8, 0.8, 0.8, 1.0);
                else
                    maturalArray[0] = AmbientMatural;
                if (DiffuseMatural == null)
                    maturalArray[1] = new VdsVec4d(0.9, 0.9, 0.9, 1.0);
                else
                    maturalArray[1] = DiffuseMatural;
                if (SpecularMatural == null)
                    maturalArray[2] = new VdsVec4d(0.95, 0.95, 0.95, 1.0);
                else
                    maturalArray[2] = SpecularMatural;
                if (EmissionMatural == null)
                    maturalArray[3] = new VdsVec4d(0.1, 0.1, 0.1, 1.0);
                else
                    maturalArray[3] = EmissionMatural;
            }
            string tFileName = "";
            if (TextureFileName != null)
                tFileName = TextureFileName;
            int drawMode = 0;
            switch (DrawMode)
            {
                case GeometryDrawModel.POINTS:
                    drawMode = 0;
                    break;
                case GeometryDrawModel.LINES:
                    drawMode = 1;
                    break;
                case GeometryDrawModel.LINE_STRIP:
                    drawMode = 3;
                    break;
                case GeometryDrawModel.LINE_LOOP:
                    drawMode = 2;
                    break;
                case GeometryDrawModel.TRIANGLES:
                    drawMode = 4;
                    break;
                case GeometryDrawModel.TRIANGLE_STRIP:
                    drawMode = 5;
                    break;
                case GeometryDrawModel.TRIANGLE_FAN:
                    drawMode = 6;
                    break;
                case GeometryDrawModel.QUADS:
                    drawMode = 7;
                    break;
                case GeometryDrawModel.QUAD_STRIP:
                    drawMode = 8;
                    break;
            }
            IUpdateGeometry(this.NativeHandle, vArray, nArray, cArray, tArray, drawMode, (int)CullFaceMode, tFileName, maturalArray);
        }
    }
}