using System;
using System.Collections.Generic;
using System.Text;

namespace VdsEngine
{
    public sealed class PlotEventLoadingWidget : PlotEventBehaviour
    {
        public FilePathProperty BackgroundImage
        {
            get;
            set;
        }

        public FilePathProperty ProgressBackgroundImage
        {
            get;
            set;
        }

        public StringProperty ProgressText
        {
            get;
            set;
        }

        private VdsView _currentView = null;
        private bool _haveBeenSet = false;
        private VdsLoadingWidget _loadingWidget = null;

        public PlotEventLoadingWidget()
        {
            BackgroundImage = new FilePathProperty();
            BackgroundImage.Value = "loading/background.tga";
            ProgressBackgroundImage = new FilePathProperty();
            ProgressBackgroundImage.Value = "loading/progressbackground.tga";
            ProgressText.Value = "加载中...";
        }

        public override void Start()
        {
            string viewID = ParentActor.ObjectViewID.ToString();
            _currentView = VdsEngineSystem.Instance.GetVdsViewByID(Convert.ToInt32(viewID));
            _behaviourIsWorking = true;
            _haveBeenSet = false;
            _loadingWidget = new VdsLoadingWidget();
            _loadingWidget.BackgroundImage = BackgroundImage.Value;
            _loadingWidget.ProgressBackgroundImage = ProgressBackgroundImage.Value;
            _loadingWidget.ProgressText = ProgressText.Value;
        }

        public override void End()
        {
            _currentView = null;
            _behaviourIsWorking = false;
            _haveBeenSet = false;
            base.End();
        }

        public override void UpdateStep(object param)
        {
            if (param == null)
                return;
            double? t = param as double?;
            if (t == null)
                return;
            _curTime = (double)t;
            if (_curTime < 0)
                return;
            VdsPlotEvent pEvent = ParentActor as VdsPlotEvent;
            if (_curTime > pEvent.EventStartTime && _curTime < pEvent.EventStartTime + pEvent.EventDurationTime)
            {
                if (!_haveBeenSet)
                {
                    ((IVdsGroupInterface)_currentView.GameLayer).AddChild(_loadingWidget);
                    _loadingWidget.ProgressValue = 0;
                    _haveBeenSet = true;
                }
                else
                {
                    double v = (_curTime - pEvent.EventStartTime) / pEvent.EventDurationTime * 100;
                    _loadingWidget.ProgressValue = (int)v;
                }
            }
            else if (_curTime >= pEvent.EventStartTime + pEvent.EventDurationTime && _behaviourIsWorking)
            {
                if (_haveBeenSet)
                {
                    ((IVdsGroupInterface)_currentView.GameLayer).RemoveChild(_loadingWidget);
                    _haveBeenSet = false;
                }
            }
        }

        public override void AsynchronousOperationStep(object param)
        { }

        public override void OnDestroy()
        { }
    }
}