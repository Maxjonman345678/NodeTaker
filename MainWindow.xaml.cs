using NodeGraphTest.Classes;
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
        //Lists
        static List<GraphNode> nodes = new();
        public static List<GraphNode> selectedItems = new();

        //Mouse State
        public static MouseState mouseState = MouseState.None;

        //Draging Operation
        public static GraphNode? _activeObject;
        public static Point offset;
        public static Canvas MainCanvas;

       
        //Line Connection
        public static Connection To = new();
        public static Connection From = new();

        public static Stack<GraphNodeData> UndoNodesList = new(Settings.UndoAmount);
        public static Stack<GraphNodeData> RedoNodesList = new(Settings.RedoAmount);
        public static GraphNodeData copiedItem;

        System.Timers.Timer time = new System.Timers.Timer();

        Point startSelect;

        public static bool IsConnecting = false;

        public MainWindow()
        {
            InitializeComponent();
            MainCanvas = MyCanvas;
            Focusable = true;
        }

        #region KeyBind Executes

        public void delectKB_Execute(object sender, RoutedEventArgs e)
        {
            Console.Write("MainWindow: Delete");
            if (selectedItems.Count > 0)
            {
                Console.WriteLine($"d {selectedItems.Count} items");
                foreach (GraphNode n in selectedItems.ToArray())
                {
                    DeleteNode(n);
                }
                
            }

            //if (_activeObject != null)
            //{
            //    DeleteNode(_activeObject);
            //}
        }

        private void copyKB_Execute(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MainWindow: Paste");
            //if (_activeObject != null)
            //{
            //    copiedItem = new(new GraphNode(new Point(Canvas.GetLeft(_activeObject), Canvas.GetTop(_activeObject)), _activeObject.Width, _activeObject.Height, _activeObject.TitleText, _activeObject.BodyText), new Point(Canvas.GetLeft(_activeObject), Canvas.GetTop(_activeObject)));
            //    Console.WriteLine($"Coppied {copiedItem != null}");
            //}

        }

        private void cutKB_Execute(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MainWindow: Cut");
        }


        private void pasteKB_Execute(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MainWindow: Pasting");
            if (copiedItem != null)
            {
                PasteNode(copiedItem);
            }
        }

        private void undoKB_Execute(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MainWindow: Undo"); ;
            //if (UndoNodesList.Count > 0)
            //{
            //    GraphNodeData node = UndoNodesList.Pop();
            //    PasteNode(node);
            //    RedoNodesList.Push(node);
            //}
        }

        private void redoKB_Execute(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MainWindow: Redo");
            //if (RedoNodesList.Count > 0)
            //{
            //    GraphNodeData node = RedoNodesList.Pop();
            //    PasteNode(node);
            //    UndoNodesList.Push(node);
            //}
        }

        private void selectAllKB_Execute(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MainWindow: Select All");
            foreach (GraphNode node in MyCanvas.Children.OfType<GraphNode>())
            {
                node.Highlight(true);
                selectedItems.Add(node);
            }
        }
        #endregion

        Path p = new();
        LineGeometry lg = new();
        private void MyCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //If not draging node around
            if (_activeObject == null)
            {
                
                if (mouseState == MouseState.Connecting && e.LeftButton == MouseButtonState.Pressed)
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
                else if (mouseState == MouseState.Connecting && e.LeftButton == MouseButtonState.Released)
                {
                    
                    //MyCanvas.Children.Remove(p);
                    IsConnecting = false;
                    MainWindow.mouseState = MouseState.None;
                }
                return;
            }

            var pos = e.GetPosition(sender as IInputElement);

            if (pos.X < MyCanvas.ActualWidth && pos.X > 0 && pos.Y < MyCanvas.ActualHeight && pos.Y > 0)
            {
                mouseState = MouseState.Draging;
                Keyboard.ClearFocus();
                Canvas.SetTop(_activeObject, pos.Y - offset.Y);
                Canvas.SetLeft(_activeObject, pos.X - offset.X);
                _activeObject.SetPosition(new Point(Canvas.GetLeft(_activeObject) + _activeObject.ActualWidth / 2, Canvas.GetTop(_activeObject) + _activeObject.ActualHeight / 2));
                //_activeObject.Effect = new DropShadowEffect();
                if (To.direction.HasValue)
                {
                    To.pos = To.node.GetUpdatedPosition((ConnectionDirection)To.direction);
                    From.pos = From.node.GetUpdatedPosition((ConnectionDirection)From.direction);
                    //UpdateLine(From.line, (Point)To.pos);
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
            mouseState = MouseState.None;
            MyCanvas.ReleaseMouseCapture();
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GraphNode node = new(new Point(MyCanvas.ActualWidth/ 2, MyCanvas.ActualHeight / 2),150,85);
            node.TitleText = "";
            node.BodyText = "";
            MyCanvas.Children.Add(node);
            Canvas.SetZIndex(node, 1);
            Canvas.SetLeft(node, MyCanvas.ActualWidth / 2 - node.MinWidth/2);
            Canvas.SetTop(node, MyCanvas.ActualHeight / 2 - node.MinHeight/2);
            nodes.Add(node);
            node.TitleText = "Pluh University";
        }

        private void ClearNotes_CLick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"Cleared {MyCanvas.Children.Count} items.");
            MyCanvas.Children.RemoveRange(0,MyCanvas.Children.Count);
        }

        public static void DeleteNode(GraphNode gn)
        {
            if (selectedItems.Contains(gn))
                selectedItems.Remove(gn);

            Point pos = new Point(Canvas.GetLeft(gn), Canvas.GetTop(gn));
            GraphNodeData gnd = new(new GraphNode(pos, gn.Width,gn.Height,gn.TitleText,gn.BodyText), pos);
            UndoNodesList.Push(gnd);
            MainCanvas.Children.Remove(gn);
            nodes.Remove(gn);
        }

        public static void PasteNode(GraphNodeData node)
        {
            Console.WriteLine($"MainWindow: Pasting with data: Title:{node.Node.TitleText}");
            GraphNode gnd = new GraphNode(new Point(Canvas.GetLeft(node.Node), Canvas.GetTop(node.Node)), node.Node.Width, node.Node.Height, node.Node.TitleText, node.Node.BodyText);
            MainCanvas.Children.Add(gnd);
            Canvas.SetLeft(gnd, node.Position.X);
            Canvas.SetTop(gnd, node.Position.Y);
        }

        public static void CopyNode(GraphNode gn)
        {
            Console.WriteLine("MainWindow: Recieved Node. Copying.....");
            GraphNodeData gnd = new(new GraphNode(new Point(Canvas.GetLeft(gn), Canvas.GetTop(gn)), gn.Width, gn.Height, gn.TitleText, gn.BodyText), new Point(Canvas.GetLeft(gn), Canvas.GetTop(gn)));
            Console.WriteLine($"MainWindow: Node Copied with data: Pos:{gnd.Position} Node{gnd.Node.TitleText}");
            copiedItem = gnd;
        }


        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

           
        }
        
        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
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
                    
                }
            }

            var pos = e.GetPosition(MainCanvas);
            if (!(pos.X < MyCanvas.ActualWidth) && !(pos.X > 0) && !(pos.Y < MyCanvas.ActualHeight) && !(pos.Y > 0))
            {
                MyCanvas.Children.Remove(r);
            }
        }

        Rectangle r = new();
        Rect? r1;
        private void DrawRectangle(Point end)
        {
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
            if (e.LeftButton == MouseButtonState.Pressed && mouseState == MouseState.Selecting)
            {
                var pos = e.GetPosition(MainCanvas);
                if (pos.X < MyCanvas.ActualWidth && pos.X > 0 && pos.Y < MyCanvas.ActualHeight && pos.Y > 0)
                {
                    DrawRectangle(pos);
                } 
                
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MyCanvas.Children.Contains(r) && !MyCanvas.IsMouseOver)
            {
                MyCanvas.Children.Remove(r);
                mouseState = MouseState.None;
            }
            
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            state.Text = mouseState.ToString();
            activeObj.Text = _activeObject == null ? "Null" : _activeObject.ToString();
            focus.Text = Keyboard.FocusedElement == null ? "Null" : Keyboard.FocusedElement.ToString();

            if (MyCanvas.IsMouseOver)
            {
                switch (mouseState)
                {
                    case MouseState.None:
                        break;
                    case MouseState.Connecting:
                        break;
                    case MouseState.Draging:
                        break;
                    case MouseState.Selecting:
                        break;
                }
            }
            else if (itemBar.IsMouseOver)
            {
                switch (mouseState)
                {
                    case MouseState.None:
                        break;
                    case MouseState.Connecting:
                        break;
                    case MouseState.Draging:
                        break;
                    case MouseState.Selecting:
                        break;
                }
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MyCanvas.Children.Contains(r))
                MyCanvas.Children.Remove(r);
            mouseState = MouseState.None;
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MyCanvas.IsMouseOver)
            {
                var pos = e.GetPosition(MainCanvas);
                ContextMenu contextMenu = MyCanvas.ContextMenu;
                contextMenu.IsOpen = true;
                e.Handled = true;

            }
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MyCanvas.IsMouseOver)
            {
                FocusManager.SetFocusedElement(this, this);
            }
            foreach (GraphNode n in MyCanvas.Children.OfType<GraphNode>())
            {
                if (n.IsMouseOver)
                {
                    FocusManager.SetFocusedElement(this, n);
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
            if (MyCanvas.IsMouseOver)
            {
                if (selectedItems.Count > 0)
                {
                    foreach (GraphNode gn in selectedItems)
                    {
                        if (!gn.IsMouseOver)
                        {
                            gn.Highlight(false);
                        }
                    }
                }
                selectedItems.Clear();

                if (_activeObject == null && mouseState == MouseState.None)
                {
                    startSelect = e.GetPosition(MainCanvas);
                    DrawRectangle(new Point(startSelect.X + 1, startSelect.Y + 1));
                    mouseState = MouseState.Selecting;
                }
            }

           
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MyCanvas.IsMouseOver)
            {
                FocusManager.SetFocusedElement(this,this);
            }
            foreach (GraphNode n in MyCanvas.Children.OfType<GraphNode>())
            {
                if (n.IsMouseOver)
                {
                    FocusManager.SetFocusedElement(this, n);
                }
            }
            
        }

    }
}
