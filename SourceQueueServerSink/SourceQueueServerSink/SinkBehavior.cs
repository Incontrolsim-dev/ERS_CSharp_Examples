using System;
using System.ComponentModel;
using System.Numerics;
using Ers;

namespace SourceQueueServerSink
{
    public class SinkBehavior : ScriptBehaviorComponent
    {
        [Category("Sink")]
        public ulong Received { get; set; } = 0;

        /// <summary>
        /// Helper function to easily create a sink entity.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static SinkBehavior Create(string name, Vector3 pos)
        {
            SubModel subModel = SubModel.GetSubModel();
            Entity entity = subModel.CreateEntity(name);
            var transform = entity.AddComponent<TransformComponent>();
            transform.Value.SetPosition(pos);
            transform.Value.SetScale(4, 2, 1);
            SinkBehavior sink = entity.AddComponent<SinkBehavior>();
            return sink;
        }

        public override void OnEntered(Entity newChild)
        {
            Received++;
            Logger.Debug($"Sink received {newChild.GetName()}");
            SubModel.GetSubModel().DestroyEntity(newChild);
        }
    }
}
