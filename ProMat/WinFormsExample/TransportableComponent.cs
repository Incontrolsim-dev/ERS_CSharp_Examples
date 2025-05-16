using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ers;

namespace WinFormsExample
{
    public struct TransportableComponent : IDataComponent
    {
        public int Type = 0;
        public Entity Target;
        public TransportableComponent() { }

        public static Entity Create(int type, string name)
        {
            Entity entity = CEntity.Create(name);
            var transportable = entity.AddComponent<TransportableComponent>();
            transportable.Value.Type = type;
            entity.AddComponent<TransformComponent>();
            return entity;
        }
    }
}
