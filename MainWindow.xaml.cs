using NodeGraphTest.Controls;
using NodeGraphTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NodeGraphTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<GraphNode> nodes = new();
        public static GraphNode? _activeObject;
        public static Point offset;
        public static Canvas MainCanvas;

        public static MouseState mouseState = MouseState.None;

        public static bool IsConnecting = false;
        public static bool longPress = false;

        public static Connection To = new();
        public static Connection From = new();

        System.Timers.Timer time = new System.Timers.Timer();

        Point startSelect;

        List<GraphNode> selectedItems = new();

        


        public MainWindow()
        {
            InitializeComponent();
            MainCanvas = MyCanvas;
        }

        Path p = new();
        LineGeometry lg = new();
        private void MyCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //If not draging node around
            if (_activeObject == null)
            {
                if (IsConnecting && e.LeftButton == MouseButtonState.Pressed)
                {
                    lg.StartPoint = (Point)From.pos;

                    lg.EndPoint = e.GetPosition(sender as IInputElement);
                    if (!MyCanvas.Children.Contains(p))
                    {
                        p.Data = lg;
                        p.Stroke = Brushes.Red;
                        p.StrokeThickness = 3;
                        MyCanvas.Children.Add(p);
                    }
                }
                else if (IsConnecting && e.LeftButton == MouseButtonState.Released)
                {
                    MyCanvas.Children.Remove(p);
                    IsConnecting = false;
                }
                return;
            }


            //time.Interval = 30;
            //time.Enabled = false;
            //time.AutoReset = true;
            //time.Elapsed += Time_Elapsed;

            var pos = e.GetPosition(sender as IInputElement);
            if (pos.X < MyCanvas.ActualWidth && pos.X > 0 && pos.Y < MyCanvas.ActualHeight && pos.Y > 0)
            {
                Canvas.SetTop(_activeObject, pos.Y - offset.Y);
                Canvas.SetLeft(_activeObject, pos.X - offset.X);
                _activeObject.SetPosition(new Point(Canvas.GetLeft(_activeObject) + _activeObject.ActualWidth / 2, Canvas.GetTop(_activeObject) + _activeObject.ActualHeight / 2));
                //_activeObject.Effect = new DropShadowEffect();
                if (To.direction.HasValue)
                {
                    To.pos = To.node.GetUpdatedPosition((ConnectionDirection)To.direction);
                    From.pos = From.node.GetUpdatedPosition((ConnectionDirection)From.direction);
                    UpdateLine(From.line, (Point)To.pos);
                }
            }
        }

        private void UpdateLine(Path p, Point end)
        {
            LineGeometry geo = (LineGeometry)p.Data;
            Path line = new();
            geo.EndPoint = end;
            geo.StartPoint = ((LineGeometry)To.line.Data).StartPoint;
            line.Data = geo;
        }

        public static void DrawLine()
        {
            Path p = new();
            LineGeometry lg = new();

            if (To.pos.HasValue && From.pos.HasValue)
            {
                lg.StartPoint = (Point)From.pos;
                lg.EndPoint = (Point)To.pos;
                p.Data = lg;
                p.Stroke = Brushes.Red;
                p.StrokeThickness = 3;
                p.Tag = $"n{nodes.IndexOf(From.node)}_{nodes.IndexOf(To.node)}";
                MainCanvas.Children.Add(p);
                To.line = p;
                From.line = p;
                To.pos = null;
                From.pos = null;
            }
        }


        private void MyCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //_activeObject.ClearValue(EffectProperty);
            _activeObject = null;
            MyCanvas.ReleaseMouseCapture();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GraphNode node = new(new Point(MyCanvas.ActualWidth/ 2, MyCanvas.ActualHeight / 2),150,85);
            MyCanvas.Children.Add(node);
            Canvas.SetZIndex(node, 1);
            Canvas.SetLeft(node, MyCanvas.ActualWidth / 2 - node.MinWidth/2);
            Canvas.SetTop(node, MyCanvas.ActualHeight / 2 - node.MinHeight/2);
            nodes.Add(node);
            node.IsEditing = false;
        }

        private void ClearNotes_CLick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"Cleared {MyCanvas.Children.Count} items.");
            MyCanvas.Children.RemoveRange(0,MyCanvas.Children.Count);
        }

        public static void DeleteNode(GraphNode gn)
        {
            MainCanvas.Children.Remove(gn);
            nodes.Remove(gn);
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (selectedItems.Count > 0)
            {
                foreach (GraphNode gn in selectedItems)
                {
                    gn.Highlight(false);
                }
                selectedItems.Clear();
                r1 = null;
                MyCanvas.ReleaseMouseCapture();
                return;
            }

            if (_activeObject == null)
            {
                startSelect = e.GetPosition(MainCanvas);
                DrawRectangle(new Point(startSelect.X + 1, startSelect.Y+1));
            }
    
        }
        
        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            time.Enabled = false;
            //longpress = false;
            //Console.WriteLine(startSelect + " | " + e.GetPosition(MainCanvas));
            if (_activeObject == null)
            {
                MyCanvas.Children.Remove(r);
                MyCanvas.ReleaseMouseCapture();

                if (r1.HasValue)
                {
                    foreach (GraphNode ui in MyCanvas.Children.OfType<GraphNode>())
                    {
                        Rect r = new(new Point(Canvas.GetLeft(ui), Canvas.GetTop(ui)), new Size(ui.ActualWidth, ui.ActualHeight));
                        if (r.IntersectsWith((Rect)r1))
                        {
                            ui.Highlight(true);
                            selectedItems.Add(ui);

                        }

                    }
                    MyCanvas.CaptureMouse();
                }
            }
        }

        Rectangle r = new();
        Rect? r1;
        private void DrawRectangle(Point end)
        {
            //200 - 300 = -100
            double width = startSelect.X - end.X;
            double height = startSelect.Y - end.Y;
            
            r.Width = Math.Abs(width);
            r.Height = Math.Abs(height);
            r.Fill = (Brush)App.Current.Resources["selectFillBrush"];
            r.Stroke = (Brush)App.Current.Resources["selectStrokeBrush"];
            r.StrokeThickness = 1.5;
            if (!MyCanvas.Children.Contains(r))
                MyCanvas.Children.Add(r);


            double w = (width < 0) ? startSelect.X : end.X;
            double h = (height < 0) ? startSelect.Y : end.Y;
            Canvas.SetLeft(r, w);
            Canvas.SetTop(r, h);
            Canvas.SetZIndex(r, 1);
            r1 = new(Canvas.GetLeft(r),Canvas.GetTop(r), r.Width, r.Height);
        }        

       
        
        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawRectangle(e.GetPosition(MainCanvas));
            }
        }
    }
}
