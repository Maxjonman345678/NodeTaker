using NodeGraphTest.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphTest.Data
{
    public class Connection
    {
        public System.Windows.Point? pos { get; set; }
        public ConnectionDirection? direction { get; set; }
        public GraphNode? node { get; set; }
        public System.Windows.Shapes.Path line { get; set; }

        public Connection()
        {
            pos = null;
            direction = null;
            node = null;
        }

        public Connection(System.Windows.Point p, ConnectionDirection dir, GraphNode n)
        {
            pos = p;
            direction = dir;
            node = n;
        }
    }
}
