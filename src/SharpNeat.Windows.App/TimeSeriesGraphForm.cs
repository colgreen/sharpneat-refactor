﻿using System.Windows.Forms;
using ZedGraph;

namespace SharpNeat.Windows.App
{
    /// <summary>
    /// Form for displaying a graph plot of time series data (e.g. best genome fitness per generation).
    /// </summary>
    public partial class TimeSeriesGraphForm : Form
    {
        readonly TimeSeriesDataSource[] _dataSourceArray;
        RollingPointPairList[] _pointPlotArray;
        GraphPane _graphPane;

        #region Constructor

        /// <summary>
        /// Construct the form with the provided details and data sources.
        /// </summary>
        public TimeSeriesGraphForm(
            string title, string xAxisTitle, string y1AxisTitle, string y2AxisTitle,
            TimeSeriesDataSource[] dataSourceArray)
        {
            InitializeComponent();

            this.Text = $"SharpNEAT Graph - {title}";
            _dataSourceArray = dataSourceArray;
            InitGraph(title, xAxisTitle, y1AxisTitle, y2AxisTitle, dataSourceArray);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh view.
        /// </summary>
        public void RefreshView()
        {
            if(this.InvokeRequired)
            {
                // Note. Must use Invoke(). BeginInvoke() will execute asynchronously and the evolution algorithm therefore 
                // may have moved on and will be in an intermediate and indeterminate (between generations) state.
                this.Invoke(new MethodInvoker(delegate()
                {
                    RefreshView();
                }));
                return;
            }

            if(this.IsDisposed) {
                return;
            }
                
            // For each series, generate a point and add it to that series' point-pair list.
            int sourceCount = _dataSourceArray.Length;
            for(int i=0; i < sourceCount; i++)
            {
                TimeSeriesDataSource ds = _dataSourceArray[i];
                Point2DDouble point  = ds.GetPoint();
                _pointPlotArray[i].Add(point.X, point.Y);
            }

            // Trigger graph to redraw.
            zed.AxisChange();
            Refresh();
        }      

        #endregion

        #region Private Methods

        private void InitGraph(
            string title, string xAxisTitle,
            string y1AxisTitle, string y2AxisTitle,
            TimeSeriesDataSource[] dataSourceArray)
        {
            _graphPane = zed.GraphPane;
            _graphPane.Title.Text = title;

			_graphPane.XAxis.Title.Text = xAxisTitle;
			_graphPane.XAxis.MajorGrid.IsVisible = true;

			_graphPane.YAxis.Title.Text = y1AxisTitle;
			_graphPane.YAxis.MajorGrid.IsVisible = true;

			_graphPane.Y2Axis.Title.Text = y2AxisTitle;
			_graphPane.Y2Axis.MajorGrid.IsVisible = false;

            // Create point-pair lists and bind them to the graph control.
            int sourceCount = dataSourceArray.Length;
            _pointPlotArray = new RollingPointPairList[sourceCount];
            for(int i=0; i < sourceCount; i++)
            {
                TimeSeriesDataSource ds = dataSourceArray[i];
                _pointPlotArray[i] = new RollingPointPairList(ds.HistoryLength);
                LineItem lineItem = _graphPane.AddCurve(ds.Name,  _pointPlotArray[i], ds.Color, SymbolType.None);
                lineItem.IsY2Axis = (ds.YAxis == 1);
            }
        }

        #endregion
    }
}