﻿using AirfoilOptimizationTool.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirfoilOptimizationTool.General.Behavior {
    class CanvasSizeBehavior {
        public static readonly DependencyProperty EnableSizeProperty = DependencyProperty.RegisterAttached(
            "EnableSize",
            typeof(bool),
            typeof(CanvasSizeBehavior),
            new PropertyMetadata(EnableSizeCallback)
        );
        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
            "Height",
            typeof(double),
            typeof(CanvasSizeBehavior)
        );
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
            "Width",
            typeof(double),
            typeof(CanvasSizeBehavior)
        );
        public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached(
            "Size",
            typeof(Size),
            typeof(CanvasSizeBehavior)
        );

        public static bool GetEnableSize(DependencyObject target) => (bool)target.GetValue(EnableSizeProperty);
        public static double GetHeight(DependencyObject target) => (double)target.GetValue(HeightProperty);
        public static double GetWidth(DependencyObject target) => (double)target.GetValue(WidthProperty);
        public static double GetSize(DependencyObject target) => (double)target.GetValue(SizeProperty);

        public static void SetEnableSize(DependencyObject target, bool value) => target.SetValue(EnableSizeProperty, value);
        public static void SetHeight(DependencyObject target, double value) => target.SetValue(HeightProperty, value);
        public static void SetWidth(DependencyObject target, double value) => target.SetValue(WidthProperty, value);
        public static void SetSize(DependencyObject target, double value) => target.SetValue(SizeProperty, value);

        private static void EnableSizeCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if ((bool)e.NewValue) {
                ((FrameworkElement)sender).Unloaded += CanvasSizeBehavior_IsVisibleChanged;
                ((FrameworkElement)sender).Loaded += CanvasSizeBehavior_Loaded;
                ((FrameworkElement)sender).SizeChanged += CanvasSizeBehavior_SizeChanged;
                updatePropertySize((FrameworkElement)sender);
            } else {
                ((FrameworkElement)sender).Loaded -= CanvasSizeBehavior_Loaded;
                ((FrameworkElement)sender).SizeChanged -= CanvasSizeBehavior_SizeChanged;
            }
        }

        private static void CanvasSizeBehavior_IsVisibleChanged(object sender, EventArgs e) {
            updatePropertySize((FrameworkElement)sender);
        }

        private static void CanvasSizeBehavior_Loaded(object sender, EventArgs e) {
            updatePropertySize((FrameworkElement)sender);
        }

        private static void CanvasSizeBehavior_SizeChanged(object sender, SizeChangedEventArgs e) {
            updatePropertySize((FrameworkElement)sender);
        }
        private static void updatePropertySize(FrameworkElement frameworkElement) {
            frameworkElement.SetCurrentValue(WidthProperty, frameworkElement.ActualWidth);
            frameworkElement.SetCurrentValue(HeightProperty, frameworkElement.ActualHeight);
            frameworkElement.SetCurrentValue(SizeProperty, new Size(frameworkElement.ActualWidth, 
                                                             frameworkElement.ActualHeight));        
        }
    }
}
