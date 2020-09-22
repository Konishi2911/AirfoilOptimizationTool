using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace AirfoilOptimizationTool.GAConfigurationWindow.AirfoilRepresentation {
    public class BasisAirfoilItem : INotifyPropertyChanged {
        private string _canvas;
        private Size _canvasSize;
        private PointCollection _airfoilCurve1;
        private string airfoilName1;

        public delegate void BasisAirfoilCanvasSizeDidChangeEventHandler(in BasisAirfoilItem sender, EventArgs e);
        public event PropertyChangedEventHandler PropertyChanged;

        public BasisAirfoilItem() {

        }
        public BasisAirfoilItem(Airfoil.Airfoil airfoil) {
            this.airfoil = airfoil;
            airfoilName = airfoil.name;
        }

        #region Binding Propreties
        //
        // Binding Prorpertis =============================================== //
        //
        public string canvasName {
            get => _canvas;
            set => _canvas = value;
        }
        public Size canvasSize {
            get => _canvasSize;
            set {
                _canvasSize = value;

                Logs.Logger.getLogger("GAStandardLogger").trace(airfoilName + " Canvas Width: " + (value.Width.ToString()));


                // Update Airfoil Preview Curve
                var converter = new AirfoilPreview.AirfoilPreviewCurveConverter(this.airfoil, this.canvasSize);
                var newCurve = converter.getDrawingOutlineCurve();

                this.airfoilCurve = newCurve;
                //


                // notyfy Property did change
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(canvasSize)));
            }
        }

        public Airfoil.Airfoil airfoil { get; private set; }
        public string airfoilName { 
            get => airfoilName1; 
            set => airfoilName1 = value; 
        }
        public PointCollection airfoilCurve {
            get => _airfoilCurve1;
            set {
                _airfoilCurve1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(airfoilCurve)));
            }
        }
        #endregion
    }
}
