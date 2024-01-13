using NodeGraphTest.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace NodeGraphTest.Controls
{
    /// <summary>
    /// Interaction logic for GraphNode.xaml
    /// </summary>
    public partial class GraphNode : UserControl
    {
        public List<NodeConnection> _conections = new();

        public Point CurrentPos;

        public Point TopCon;
        public Point BottomCon;
        public Point LeftCon;
        public Point RightCon;

        bool IsOverTextBox = false;
        public GraphNode()
        {
            Focusable = true;
            InitializeComponent();
        }

        public GraphNode(Point startLoc, double startW, double startH)
        {
            CurrentPos = startLoc;
            Focusable = true;
            InitializeComponent();
            CalculateConnections(startW, startH);
        }

        public GraphNode(Point startLoc, double startW, double startH, string Title, string Body)
        {
            CurrentPos = startLoc;
            InitializeComponent();
            TitleText = Title;
            BodyText = Body;
            CalculateConnections(startW, startH);
        }

        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(GraphNode), new PropertyMetadata());
        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        public static readonly DependencyProperty TitleTextProperty = DependencyProperty.Register("TitleText", typeof(string), typeof(GraphNode), new PropertyMetadata());
        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        public static readonly DependencyProperty BodyTextProperty = DependencyProperty.Register("BodyText", typeof(string), typeof(GraphNode), new PropertyMetadata());
        public string BodyText
        {
            get { return (string)GetValue(BodyTextProperty); }
            set { SetValue(BodyTextProperty, value); }
        }


        public void SetPosition(Point pos)
        {
            CurrentPos = pos;
            CalculateConnections(ActualWidth, ActualHeight);
        }

        public void CalculateConnections(double width, double height)
        {
            TopCon = new Point(CurrentPos.X, CurrentPos.Y - height / 2);
            BottomCon = new Point(CurrentPos.X, CurrentPos.Y + height / 2);
            RightCon = new Point(CurrentPos.X + width / 2, CurrentPos.Y);
            LeftCon = new Point(CurrentPos.X - width / 2, CurrentPos.Y);
        }

        private void TopNC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.From.pos = TopCon;
            MainWindow.From.direction = ConnectionDirection.Top;
            MainWindow.From.node = this;
            MainWindow.IsConnecting = true;
            MainWindow.mouseState = MouseState.Connecting;

            MainWindow.MainCanvas.CaptureMouse();
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void BottomNC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.From.pos = BottomCon;
            MainWindow.From.direction = ConnectionDirection.Bottom;
            MainWindow.From.node = this;
            MainWindow.IsConnecting = true;
            MainWindow.mouseState = MouseState.Connecting;

            MainWindow.MainCanvas.CaptureMouse();
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void LeftNC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.From.pos = LeftCon;
            MainWindow.From.direction = ConnectionDirection.Left;
            MainWindow.From.node = this;
            MainWindow.IsConnecting = true;
            MainWindow.mouseState = MouseState.Connecting;

            MainWindow.MainCanvas.CaptureMouse();
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void RightNC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.From.pos = RightCon;
            MainWindow.From.direction = ConnectionDirection.Right;
            MainWindow.From.node = this;
            MainWindow.IsConnecting = true;
            MainWindow.mouseState = MouseState.Connecting;

            MainWindow.MainCanvas.CaptureMouse();
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void TCon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.To.pos != TopCon)
            {
                MainWindow.To.pos = TopCon;
                MainWindow.To.direction = ConnectionDirection.Top;
                MainWindow.To.node = this;
                MainWindow.DrawLine();
                MainWindow.mouseState = MouseState.None;

                MainWindow.MainCanvas.ReleaseMouseCapture();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void BCon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.To.pos != BottomCon)
            {
                MainWindow.To.pos = BottomCon;
                MainWindow.To.direction = ConnectionDirection.Bottom;
                MainWindow.To.node = this;
                MainWindow.DrawLine();
                MainWindow.mouseState = MouseState.None;

                MainWindow.MainCanvas.ReleaseMouseCapture();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void LCon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.To.pos != LeftCon)
            {
                MainWindow.To.pos = LeftCon;
                MainWindow.To.direction = ConnectionDirection.Left;
                MainWindow.To.node = this;
                MainWindow.DrawLine();
                MainWindow.mouseState = MouseState.None;

                MainWindow.MainCanvas.ReleaseMouseCapture();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void RCon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.To.pos != RightCon)
            {
                MainWindow.To.pos = RightCon;
                MainWindow.To.direction = ConnectionDirection.Right;
                MainWindow.To.node = this;
                MainWindow.DrawLine();
                MainWindow.mouseState = MouseState.None;

                MainWindow.MainCanvas.ReleaseMouseCapture();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            
        }

        public void AddConnection(NodeConnection nc)
        {
            _conections.Add(nc);
        }
        double width;
        double height;

        private void onDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            width = ActualWidth;
            height = ActualHeight;
        }

        private void onDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            CurrentPos = new Point(CurrentPos.X + ((width - ActualWidth)/2), CurrentPos.Y + ((height - ActualHeight)/2)); ;
            CalculateConnections(ActualWidth,ActualHeight);
        }

        private void onDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double yadjust = baseGrid.ActualHeight + e.VerticalChange;
            double xadjust = baseGrid.ActualWidth + e.HorizontalChange;
            if (xadjust >= MinWidth && xadjust <= MaxWidth)
            {
                baseGrid.Width = xadjust;
            }
            if (yadjust >= MinHeight && yadjust <= MaxWidth)
            {
                baseGrid.Height = yadjust;
            }
        }

        public Point GetUpdatedPosition(ConnectionDirection dir)
        {
            Point p = new(0, 0);
            switch (dir)
            {
                case ConnectionDirection.Top:
                    p = TopCon;
                    break;
                case ConnectionDirection.Left:
                    p = LeftCon;
                    break;
                case ConnectionDirection.Right:
                    p = RightCon;
                    break;
                case ConnectionDirection.Bottom:
                    p = BottomCon;
                    break;

            }
            return p;
        }

        private void toolBar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOver)
            {
                IsEditing = false;
            }
        }


        private void colorFillBtn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.DeleteNode(this);
        }

        public void Highlight(bool status)
        {
            if (status)
            {
                border.BorderThickness = new Thickness(2);
                border.BorderBrush = (Brush)App.Current.Resources["HoverBrush"];
            }
            else
            {
                border.BorderThickness = new Thickness(0);
                border.BorderBrush = Brushes.Transparent;
            }
        }

        private void userControl_MouseEnter(object sender, MouseEventArgs e)
        {
            IsEditing = true;
        }

        private void userControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!toolBar.IsMouseOver)
            {
                IsEditing = false;
            }
        }

        private void userControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu contextMenu = ContextMenu;
            contextMenu.IsOpen = true;
            e.Handled = true;
        }

       

        //MainWindow._activeObject = this;
        //    MainWindow.offset = e.GetPosition(MainWindow.MainCanvas);
        //    MainWindow.offset.Y -= Canvas.GetTop(MainWindow._activeObject);
        //    MainWindow.offset.X -= Canvas.GetLeft(MainWindow._activeObject);

        //    MainWindow.MainCanvas.CaptureMouse();
        //    Mouse.OverrideCursor = Cursors.Hand;


        Stopwatch sw = new();
        bool IsMouseMoving = false;
        private void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sw.Restart();
            if (IsMouseMoving)
            {
                MainWindow._activeObject = this;
                MainWindow.offset = e.GetPosition(MainWindow.MainCanvas);
                MainWindow.offset.Y -= Canvas.GetTop(MainWindow._activeObject);
                MainWindow.offset.X -= Canvas.GetLeft(MainWindow._activeObject);

                MainWindow.MainCanvas.CaptureMouse();
                Mouse.OverrideCursor = Cursors.Hand;
            }
        }

        private void border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            sw.Stop();
            if(sw.ElapsedMilliseconds < 120)
            {
                if (!MainWindow.selectedItems.Contains(this))
                {
                    Highlight(true);
                    MainWindow.selectedItems.Add(this);
                }
            }
            else
            {
                sw.Stop();
            }

        }

        private void border_MouseMove(object sender, MouseEventArgs e)
        {
            sw.Stop();
            IsMouseMoving = true;
        }

        private void pasteKB_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Node: Paste");

        }

        private void copyKB_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Node: Copy");
            MainWindow.CopyNode(this);
        }

        private void deleteBtn_Click(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Node: Delete");
            MainWindow.DeleteNode(this);
        }
    }
}

