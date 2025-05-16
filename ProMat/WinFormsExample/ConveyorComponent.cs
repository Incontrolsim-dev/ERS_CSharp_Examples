using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ers;

namespace WinFormsExample
{
    public class ConveyorComponent : ScriptBehaviorComponent
    {

        // TODO: figure out if this is the way to  model now with scripting.
        // Python calls this function but it feels not right.
        public static Entity CreateConveyor(string name)
        {
            Entity entity = CEntity.Create(name);
            entity.AddComponent<ConveyorComponent>();
            entity.AddComponent<PathComponent>();
            entity.AddComponent<TransformComponent>();
            return entity;
        }
    }
}
