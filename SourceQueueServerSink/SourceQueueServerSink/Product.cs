using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Ers;

namespace SourceQueueServerSink
{
    /// <summary>
    /// Relevant information for a product.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Product : IDataComponent
    {
        [Category("Product"), Description("Whether the product is currently filled with items.")]
        public bool Filled { get; set; } = false;

        public Product()
        {
        }
    }
}
