using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirfoilOptimizationTool.CustomControlls {
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AirfoilOptimizationTool.CustomControlls"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AirfoilOptimizationTool.CustomControlls;assembly=AirfoilOptimizationTool.CustomControlls"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:ObservableCanvas/>
    ///
    /// </summary>
    public class ObservableCanvas : Canvas {
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size),
            typeof(Size),
            typeof(ObservableCanvas)
        );
        public static readonly DependencyProperty ObservableWidthProperty = DependencyProperty.Register(
            nameof(ObservableWidth),
            typeof(double),
            typeof(ObservableCanvas)
        );

        public Size Size {
            get => (Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        public double ObservableWidth {
            get => (double)GetValue(ObservableWidthProperty);
            set => SetValue(ObservableWidthProperty, value);
        }

        static ObservableCanvas() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ObservableCanvas), new FrameworkPropertyMetadata(typeof(ObservableCanvas)));
        }
        public ObservableCanvas() : base() {
            SizeChanged += ObservableCanvas_SizeChanged;
        }

        private void ObservableCanvas_SizeChanged(object sender, SizeChangedEventArgs e) {
            ObservableWidth = ActualWidth;
            Size = new Size(this.ActualWidth, this.ActualHeight);
        }
    }
}
