using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphTest.Data
{
    public enum ConnectionDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public enum MouseState
    {
        None,
        Connecting,
        Draging,
        Selecting
    }
}
