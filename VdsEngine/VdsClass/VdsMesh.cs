using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VdsEngine
{
    /// <summary>
    /// Mesh绘制节点
    /// </summary>
    public class VdsMesh : VdsTransform
    {
        #region 内部调用
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static IntPtr ICreateByNodeFullName(string nodeFileName);
        #endregion

        public struct NodeStruct
        {
            public IntPtr _nodePtr;
            public string _nodeFileName;
        }
        protected List<VdsGeometry> _childGeometryList = new List<VdsGeometry>();
        protected List<NodeStruct> _childNodeList = new List<NodeStruct>();

        public VdsMesh()
            : base(true)
        { }

        public void AddChild(VdsGeometry shape)
        {
            AddChildInternal(this.NativeHandle, shape.NativeHandle);
            _childGeometryList.Add(shape);
        }

        public void RemoveChild(VdsGeometry shape)
        {
            RemoveChildInternal(this.NativeHandle, shape.NativeHandle);
            _childGeometryList.Remove(shape);
        }

        public bool ContainsChild(VdsGeometry child)
        {
            return _childGeometryList.Contains(child);
        }

        public void AddChild(string meshFileName)
        {
            IntPtr nodePtr = ICreateByNodeFullName(meshFileName);
            AddChildInternal(this.NativeHandle, nodePtr);
            NodeStruct newStruct = new NodeStruct();
            newStruct._nodeFileName = meshFileName;
            newStruct._nodePtr = nodePtr;
            _childNodeList.Add(newStruct);
        }

        public void RemoveChild(string meshFileName)
        {
            for (int i = 0; i < _childNodeList.Count; ++i)
            {
                if (_childNodeList[i]._nodeFileName == meshFileName)
                {
                    RemoveChildInternal(this.NativeHandle, _childNodeList[i]._nodePtr);
                    _childNodeList.RemoveAt(i);
                    break;
                }
            }
        }

        public bool ContainsChild(string meshFileName)
        {
            for (int i = 0; i < _childNodeList.Count; ++i)
            {
                if (_childNodeList[i]._nodeFileName == meshFileName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

