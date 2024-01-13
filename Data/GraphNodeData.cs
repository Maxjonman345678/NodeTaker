using NodeGraphTest.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NodeGraphTest.Data
{
    public class GraphNodeData
    {
        public GraphNode Node { get; set; }
        public Point Position { get; set; }

        public GraphNodeData(GraphNode node, Point position)
        {
            Node = node;
            Position = position;
        }
    }
}
