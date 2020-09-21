using AirfoilOptimizationTool.Airfoil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.AirfoilPreview {
    class AirfoilPreviewCurveConverter {
        Point[] outlineCurve;
        Point[] leadingEdgeCircle;
        Point[] camberCurve;
        Size drawingRegionSize;

        public AirfoilPreviewCurveConverter(Size previewRegionSize) {
            drawingRegionSize = previewRegionSize;
        }
        public AirfoilPreviewCurveConverter(Airfoil.Airfoil airfoil) {
            outlineCurve = PairedPoint.convertToPointArray(airfoil.airfoilCurve, PairedPoint.Direction.FromUpperTrailing);
            camberCurve = airfoil.camberLine;

        }
        public AirfoilPreviewCurveConverter(Airfoil.Airfoil airfoil, Size previewRegionSize) {
            outlineCurve = PairedPoint.convertToPointArray(airfoil.airfoilCurve, PairedPoint.Direction.FromUpperTrailing);
            camberCurve = airfoil.camberLine;

            drawingRegionSize = previewRegionSize;
        }

        public double margine { get; set; }
        public void setAirfoil(Airfoil.Airfoil airfoil) {
            outlineCurve = PairedPoint.convertToPointArray(airfoil.airfoilCurve, PairedPoint.Direction.FromUpperTrailing);
            camberCurve = airfoil.camberLine;
        }
        public void setPreviewRegionSize(Size previewRegionSize) {
            drawingRegionSize = previewRegionSize;
        }

        //
        // Make airfoil Curve Points on the Display Coordinate
        public System.Windows.Media.PointCollection? getDrawingOutlineCurve() {
            if (outlineCurve == null) return null;
            if (drawingRegionSize == null) return null;

            AirfoilPreview.Scaler pointScaler = new Scaler(drawingRegionSize, outlineCurve, leadingEdgeCircle, camberCurve);
            return new System.Windows.Media.PointCollection(pointScaler.adjustScale(outlineCurve, true));
        }
        public System.Windows.Media.PointCollection? getDrawingLeadingEdgeCircle() {
            if (leadingEdgeCircle == null) return null;
            if (drawingRegionSize == null) return null;

            AirfoilPreview.Scaler pointScaler = new Scaler(drawingRegionSize, outlineCurve, leadingEdgeCircle, camberCurve);
            return new System.Windows.Media.PointCollection(pointScaler.adjustScale(leadingEdgeCircle, true));
        }
        public System.Windows.Media.PointCollection? getDrawingCamberLineCurve() {
            if (camberCurve == null) return null;
            if (drawingRegionSize == null) return null;

            AirfoilPreview.Scaler pointScaler = new Scaler(drawingRegionSize, outlineCurve, leadingEdgeCircle, camberCurve);
            return new System.Windows.Media.PointCollection(pointScaler.adjustScale(camberCurve, true));
        }
    }
}
