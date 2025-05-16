using System;
using System.ComponentModel;
using System;
using System.Runtime.InteropServices;
using Ers;

namespace SourceQueueServerSink
{
    /// <summary>
    /// Meta-data for an entity to set its capacity.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Resource : IDataComponent
    {
        [Category("Resource")]
        public ulong Capacity { get; set; } = 0;

        public Resource()
        {
        }
    }
}
