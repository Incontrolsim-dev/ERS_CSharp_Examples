using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ers;

namespace ED
{
    /// <summary>
    /// Manages a connection from the perspective of this entity. Can be an input or output slot. 
    /// </summary>
    public class ConnectionSlot
    {
        /// <summary>
        /// Another entity also containing a connectionslot that this slot is connected to.
        /// </summary>
        private Entity connectedTo = CEntity.InvalidEntity();

        /// <summary>
        /// The index of the slot in the connectedTo entity.
        /// </summary>
        private int toIdx = 0;
        private bool isOpen = false;

        public Entity GetConnectedTo()
        {
            return connectedTo;
        }

        public int GetToIdx()
        {
            return toIdx;
        }

        public void Connect(Entity to, int toIdx)
        {
            connectedTo = to;
            this.toIdx = toIdx;
        }

        public void Disconnect()
        {
            connectedTo = 0;
            toIdx = 0;
        }

        public void Open()
        {
            isOpen = true;
        }

        public void Close()
        {
            isOpen = false;
        }
        public bool IsOpen()
        {
            return isOpen;
        }

        public bool IsConnected()
        {
            return connectedTo.IsValid();
        }
    }
}
