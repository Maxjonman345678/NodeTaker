using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NodeGraphTest.Classes
{
    public static class KeyExecutes
    {
        #region Statics

        static RoutedUICommand delete = new RoutedUICommand("deletes the selection", "Delete", typeof(KeyExecutes));
        static RoutedUICommand paste = new RoutedUICommand("pastes clipboard item", "Paste", typeof(KeyExecutes));
        static RoutedUICommand copy = new RoutedUICommand("copies an item to the clipboard", "Copy", typeof(KeyExecutes));
        static RoutedUICommand cut = new RoutedUICommand("removes and copies an item to the clipboard", "Cut", typeof(KeyExecutes));
        static RoutedUICommand redo = new RoutedUICommand("redoes an action", "Redo", typeof(KeyExecutes));
        static RoutedUICommand undo = new RoutedUICommand("undoes an action", "Undo", typeof(KeyExecutes));
        static RoutedUICommand selectAll = new RoutedUICommand("selects all nodes on the graph", "SelectAll", typeof(KeyExecutes));



        #endregion

        public static RoutedUICommand Delete { get { return delete; } }
        public static RoutedUICommand Paste { get { return paste; } }
        public static RoutedUICommand Copy { get { return copy; } }
        public static RoutedUICommand Cut { get { return cut; } }
        public static RoutedUICommand Undo { get { return redo; } }
        public static RoutedUICommand Redo { get { return undo; } }
        public static RoutedUICommand SelectAll { get { return selectAll; } }

    }
}
