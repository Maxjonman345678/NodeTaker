using NodeGraphTest.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphTest.Data
{
    public class NodeConnection
    {
        public ConnectionDirection _ToDir;
        public ConnectionDirection _FromDir;

        public GraphNode _ToGN;
        public GraphNode _FromGN;
        
        public NodeConnection(ConnectionDirection todir, ConnectionDirection fromdir, GraphNode ToGn, GraphNode FromGn)
        {
            _ToDir = todir;
            _FromDir = fromdir;
            _ToGN = ToGn;
            _FromGN = FromGn;
        }

    }
}
