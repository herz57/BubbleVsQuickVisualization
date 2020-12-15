using System;
using System.Collections.Generic;
using System.Text;

namespace BubbleVsQuickVisualization
{
    public class CustomEventsArgs : EventArgs
    {
        public int[] Indexes { get; set; }
        public CustomEventsArgs(int[] indexes)
        {
            Indexes = indexes;
    }
    }
}
