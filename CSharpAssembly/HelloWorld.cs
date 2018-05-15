using System;
using System.Collections.Generic;
using System.ComponentModel;
using VdsEngine;

namespace CSharpAssembly
{
    public class HelloWorld : MonoBehaviour
    {
        public VdsEngine.IntProperty TestInt
        {
            get;
            set;
        }
        public VdsEngine.IntListProperty TestIntList
        {
            get;
            set;
        }
        public VdsEngine.DoubleProperty TestDouble
        {
            get;
            set;
        }
        public VdsEngine.DoubleListProperty TestDoubleList
        {
            get;
            set;
        }
        public VdsEngine.StringProperty TestString
        {
            get;
            set;
        }
        public VdsEngine.StringListProperty TestStringList
        {
            get;
            set;
        }
        public VdsEngine.BoolProperty TestBool
        {
            get;
            set;
        }
        //public VdsEngine.BoolListProperty TestBoolList
        //{
        //    get;
        //    set;
        //}
        public VdsEngine.VdsVec2d TestVec2
        {
            get;
            set;
        }
        public VdsEngine.VdsVec2dList TestVec2List
        {
            get;
            set;
        }
        public VdsEngine.VdsVec3d TestVec3
        {
            get;
            set;
        }
        public VdsEngine.VdsVec3dList TestVec3List
        {
            get;
            set;
        }
        public VdsEngine.VdsVec4d TestVec4
        {
            get;
            set;
        }
        public VdsEngine.VdsVec4dList TestVec4List
        {
            get;
            set;
        }
        public VdsEngine.FilePathProperty TestFilePath
        {
            get;
            set;
        }
        public VdsEngine.EnumProperty TestEnum
        {
            get;
            set;
        }
        //private VdsMatrixd _mt = null;
        private VdsView _currentView = null;
        //private UILabel _uiLabel = null;
        //private UIToolbar _uiToolbar = null;
        //private long _currentTick = 0;
        private VdsGameActor _userDefineGameActor = null;
        //private VdsGeometry _testGeometry = null;
        //private double _length = 20.0;
        private VdsGameActor _cloneGameActor = null;
        //private VdsGameActor _loadGameActor = null;
        public HelloWorld()
        {
            TestInt = new IntProperty();
            TestInt.Value = 1;
            TestIntList = new IntListProperty();
            TestIntList.ValueList.Add(1);
            TestIntList.ValueList.Add(2);
            TestDouble = new DoubleProperty();
            TestDouble.Value = 2.0;
            TestDoubleList = new DoubleListProperty();
            TestDoubleList.ValueList.Add(3.0);
            TestDoubleList.ValueList.Add(4.0);
            TestString = new StringProperty();
            TestString.Value = "5";
            TestStringList = new StringListProperty();
            TestStringList.ValueList.Add("6");
            TestStringList.ValueList.Add("7");
            TestBool = new BoolProperty();
            TestBool.Value = true;
            //TestBoolList = new BoolListProperty();
            //TestBoolList.ValueList.Add(false);
            //TestBoolList.ValueList.Add(true);
            TestVec2 = new VdsVec2d(8, 9);
            TestVec2List = new VdsVec2dList();
            TestVec2List.ValueList.Add(new VdsVec2d(10, 11));
            TestVec2List.ValueList.Add(new VdsVec2d(12, 13));
            TestVec3 = new VdsVec3d(14, 15, 16);
            TestVec3List = new VdsVec3dList();
            TestVec3List.ValueList.Add(new VdsVec3d(17, 18, 19));
            TestVec3List.ValueList.Add(new VdsVec3d(20, 21, 22));
            TestVec4 = new VdsVec4d(23, 24, 25, 26);
            TestVec4List = new VdsVec4dList();
            TestVec4List.ValueList.Add(new VdsVec4d(27, 28, 29, 30));
            TestVec4List.ValueList.Add(new VdsVec4d(31, 32, 33, 34));
            TestFilePath = new FilePathProperty();
            TestFilePath.Value = "35";
            TestEnum = new EnumProperty();
            TestEnum.ValueList.Add("36");
            TestEnum.ValueList.Add("37");
        }
        public override void Start()
        {
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            _currentView.ViewEvent.VdsViewEventEvent_MouseButtonReleaseEvent += ViewEvent_VdsViewEventEvent_MouseButtonReleaseEvent;
            //-----------------------------------------------------------1
            //			_uiLabel = new UILabel(Convert.ToInt32(viewID));
            //            _uiLabel.TextString = HelloWorldString;
            //            _uiLabel.WidgetPosition = new VdsVec2d(100, 100);
            //			_uiLabel.UpdateText ();
            //            _currentView.AddUIWidget(_uiLabel);
            //--------------------------------------------------------2
            //_currentView.ViewEvent.VdsViewEventEvent_UIEvent += ViewEvent_VdsViewEventEvent_UIEvent;
            //UIToolbarData barData = new UIToolbarData();
            //barData.AddButton("test0", "E:/VirtualDataScene/NSIS/scenetextures/line/0.tga", "test0");
            //barData.AddButton("test1", "E:/VirtualDataScene/NSIS/scenetextures/line/1.tga", "汉字");
            //barData.AddButton("test2", "E:/VirtualDataScene/NSIS/scenetextures/line/0.tga", "test2");
            //_uiToolbar = new UIToolbar(Convert.ToInt32(viewID));
            //_uiToolbar.ToolbarData = barData;
            //_uiToolbar.UpdateToolbar();
            //_currentView.AddUIWidget(_uiToolbar);
            //-------------------------------------------------------3
            //_currentTick = System.DateTime.Now.Ticks;
            //-------------------------------------------------------4
            //_userDefineGameActor = new VdsGameActor(true);
            //VdsMesh mesh = new VdsMesh();
            //_testGeometry = new VdsGeometry();
            //List<VdsVec3d> vertexArray = new List<VdsVec3d>();
            //vertexArray.Add(new VdsVec3d(0, 0, 0));
            //vertexArray.Add(new VdsVec3d(_length, 0, 0));
            //vertexArray.Add(new VdsVec3d(_length, _length, 0));
            //vertexArray.Add(new VdsVec3d(0, 0, 0));
            //vertexArray.Add(new VdsVec3d(_length, _length, 0));
            //vertexArray.Add(new VdsVec3d(0, _length, 0));

            //List<VdsVec4d> colorArray = new List<VdsVec4d>();
            //colorArray.Add(new VdsVec4d(0, 0, 0, 1));
            //colorArray.Add(new VdsVec4d(1, 0, 0, 1));
            //colorArray.Add(new VdsVec4d(1, 1, 0, 1));
            //colorArray.Add(new VdsVec4d(0, 0, 0, 1));
            //colorArray.Add(new VdsVec4d(1, 1, 0, 1));
            //colorArray.Add(new VdsVec4d(0, 1, 0, 1));

            //_testGeometry.VertexArray = vertexArray;
            //_testGeometry.ColorArray = colorArray;
            //_testGeometry.UpdateGeometry();
            //mesh.AddChild(_testGeometry);
            //_userDefineGameActor.AddChild(mesh);
            //VdsMatrixd trans = new VdsMatrixd();
            //trans.MakeTranslate(0.0, 0.0, 60.0);
            //_userDefineGameActor.Transform = trans;
            //-------------------------------------------------------5
            //VdsMatrixd trans = new VdsMatrixd();
            //trans.MakeTranslate(0.0, 0.0, 60.0);
            //-------------------------------------------------------5
            //_cloneGameActor = new VdsGameActor(ParentActor);
            //VdsVec3d pos = new VdsVec3d(0.0, 0.0, 60.0);
            //_cloneGameActor.ActorTranslation = pos;
            //_currentView.CurrentSceneLayer.AddChild(_cloneGameActor);
        }
        void ViewEvent_VdsViewEventEvent_UIEvent(object sender, EventArgs e)
        {
            //VdsEngine.VdsViewEventEvent.VEventArgs args = e as VdsEngine.VdsViewEventEvent.VEventArgs;
            //List<string> paramList = args.EventParameter as List<string>;
            //string p0 = paramList[0];
            //string p1 = paramList[1];
        }
        void ViewEvent_VdsViewEventEvent_MouseButtonReleaseEvent(object sender, EventArgs e)
        {
            #region 测试
#if DEBUG
            //----------------------------------------------------------1
            //VdsEngine.VdsViewEventEvent.VEventArgs args = e as VdsEngine.VdsViewEventEvent.VEventArgs;
            //int key = Convert.ToInt32(args.EventParameter);
            //if (key == 1)
            //{
            //    PtrClass pObj = _currentView.DoPickActor((int)args.EventPosition._x, (int)args.EventPosition._y, VdsView.PickObjectEffect.Halo_Effect);
            //    if (pObj != null)
            //    {
            //        VdsActor pickActor = pObj as VdsActor;
            //        VdsVec3d curPos = pickActor.ActorTranslation;
            //        pickActor.ActorTranslation = curPos + new VdsVec3d(0, 0, 0.3);
            //    }
            //    else
            //    {
            //        VdsGameActor gameActor = new VdsGameActor("oldman.vdsa");
            //        gameActor.ActorStatus = null;
            //        gameActor.ActorTranslation = new VdsVec3d(-107.6560668945313, 36.1014480590820, 0.2000058293343);
            //        VdsEngine.IVdsGroupInterface group = _currentView.GameLayer as VdsEngine.IVdsGroupInterface;
            //        group.AddChild(gameActor);
            //    }
            //}
            //------------------------------------------------------------2
#endif
            #endregion
        }
        public override void UpdateStep(object param)
        {
            #region 测试
#if DEBUG
            //------------------------------------1
            //_mt = ParentActor.Transform;
            //VdsMatrixd trans = new VdsMatrixd();
            //trans.MakeTranslate(0.31, 0.0, 0.0);
            //_mt.PostMult(trans);
            //ParentActor.Transform = _mt;
            //-----------------------------2
            //            if (!_uiLabel.IsShow)
            //                _uiLabel.Show();
            //---------------------------3
            //if (!_uiToolbar.IsShow)
            //    _uiToolbar.Show();
            //---------------------------4
            //if (_userDefineGameActor.ParentList == null || !_userDefineGameActor.ParentList.Contains(_currentView.RootLayer))
            //{
            //    _currentView.RootLayer.AddChild(_userDefineGameActor);
            //}
            //else
            //{
            //    if (_length < 40)
            //        _length += 0.1;
            //    else
            //        _length = 20;
            //    List<VdsVec3d> vertexArray = new List<VdsVec3d>();
            //    vertexArray.Add(new VdsVec3d(0, 0, 0));
            //    vertexArray.Add(new VdsVec3d(_length, 0, 0));
            //    vertexArray.Add(new VdsVec3d(_length, _length, 0));
            //    vertexArray.Add(new VdsVec3d(0, 0, 0));
            //    vertexArray.Add(new VdsVec3d(20, 20, 0));
            //    vertexArray.Add(new VdsVec3d(0, 20, 0));

            //    List<VdsVec4d> colorArray = new List<VdsVec4d>();
            //    colorArray.Add(new VdsVec4d(0, 0, 0, 0.6));
            //    colorArray.Add(new VdsVec4d(1, 0, 0, 0.6));
            //    colorArray.Add(new VdsVec4d(1, 1, 0, 0.6));
            //    colorArray.Add(new VdsVec4d(0, 0, 0, 0.6));
            //    colorArray.Add(new VdsVec4d(1, 1, 0, 0.6));
            //    colorArray.Add(new VdsVec4d(0, 1, 0, 0.6));

            //    _testGeometry.VertexArray = vertexArray;
            //    _testGeometry.ColorArray = colorArray;
            //    _testGeometry.CullFaceMode = VdsGeometry.GeometryCullFaceMode.NOCULLFACE;
            //    _testGeometry.UpdateGeometry();
            //}
            //---------------------------5
#endif
            #endregion
        }
        public override void AsynchronousOperationStep(object param)
        {
            //------------------------------------1
            //long curTicks = System.DateTime.Now.Ticks;
            //long d = curTicks - _currentTick;
            //long v = d / System.TimeSpan.TicksPerSecond;
            //if (v > 5)
            //{
            //    Random rand = new Random();
            //    VdsCamera newCamera = new VdsCamera();
            //    newCamera.Eye = new VdsVec3d(rand.Next(-400, 400), rand.Next(-400, 400), rand.Next(300, 400));
            //    newCamera.Center = new VdsVec3d(rand.Next(-100, 100), rand.Next(-100, 100), 0);
            //    newCamera.Up = new VdsVec3d(0, 0, 1);
            //    newCamera.WithAnimation = true;
            //    _currentView.MainCamera = newCamera;

            //    _currentTick = curTicks;
            //}
            //-----------------------------2
        }
        public override void OnDestroy()
        {
            //------------------------------------1
            //            _currentView.RemoveUIWidget(_uiLabel);
            //            _uiLabel = null;
            //------------------------------------2
            //_currentView.RemoveUIWidget(_uiToolbar);
            //_uiToolbar = null;
            //------------------------------------3
            //if (_userDefineGameActor.ParentList != null || _userDefineGameActor.ParentList.Contains(_currentView.RootLayer))
            //{
            //    _currentView.RootLayer.RemoveChild(_userDefineGameActor);
            //}
        }
    }
}