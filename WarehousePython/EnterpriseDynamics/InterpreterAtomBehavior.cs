using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ers;
using Ers.Interpreter;

namespace ED
{
    public class InterpreterAtomBehavior : ScriptBehaviorComponent
    {
        public List<ConnectionSlot> InputSlots = new();
        public List<ConnectionSlot> OutputSlots = new();

        public void Receive(Entity entity, int inputSlotIdx)
        {
        }

        public void Send(Entity toSend, int outputSlotIdx)
        {
            // TODO: Error handling checking etc. Check if channels were really all open.
            ConnectionSlot slot = ConnectedEntity.GetComponent<InterpreterAtomBehavior>().OutputSlots[outputSlotIdx];
            Entity to = slot.GetConnectedTo();
            SubModel.GetSubModel().UpdateParentOnEntity(toSend, to);
            to.GetComponent<InterpreterAtomBehavior>().Receive(toSend, slot.GetToIdx());
        }

        public void OnInputChannelReady(int inputChannelIdx)
        {
            var interpreterScript = SubModel.GetSubModel().GetInterpreterScriptComponent(ConnectedEntity);
            var call = InterpreterArgs.Create(1);
            call.SetArgument(0, InterpreterVariable.CreateUInt64((ulong)inputChannelIdx));
            call.DynamicInvoke(interpreterScript, "on_input_channel_ready");
            call.Destruct();
        }

        public void OnOutputChannelReady(int outputChannelIdx)
        {
            var interpreterScript = SubModel.GetSubModel().GetInterpreterScriptComponent(ConnectedEntity);
            var call = InterpreterArgs.Create(1);
            call.SetArgument(0, InterpreterVariable.CreateUInt64((ulong)outputChannelIdx));
            call.DynamicInvoke(interpreterScript, "on_output_channel_ready");
            call.Destruct();
        }

        public bool IsOutputChannelReady(int index)
        {
            var outputSlot = OutputSlots[index];
            if(!outputSlot.IsOpen()) return false;
            Entity to = outputSlot.GetConnectedTo();
            bool isConnectedOpen = to.GetComponent<InterpreterAtomBehavior>().InputSlots[outputSlot.GetToIdx()].IsOpen();
            return isConnectedOpen;
        }

        public bool HasInput(int index)
        {
            if (index >= InputSlots.Count) return false;
            return InputSlots[index].IsConnected();
        }

        public void OpenInput(int index)
        {
            ConnectionSlot inputSlot = InputSlots[index];
            inputSlot.Open();

            // Call ic and oc ready if all open.
            // TODO: Lots and lots of error handling and bounds checking.
            Entity from = inputSlot.GetConnectedTo();
            bool isConnectedOpen = from.GetComponent<InterpreterAtomBehavior>().OutputSlots[inputSlot.GetToIdx()].IsOpen();
            if(isConnectedOpen)
            {
                from.GetComponent<InterpreterAtomBehavior>().OnOutputChannelReady(inputSlot.GetToIdx());
                OnInputChannelReady(index);
            }
        }

        public void CloseInput(int index)
        {
            ConnectionSlot inputSlot = InputSlots[index];
            inputSlot.Close();

            // Call callbacks?
            // TODO: Lots and lots of error handling and bounds checking.
            // Entity from = inputSlot.GetConnectedTo();
            // bool isConnectedOpen = from.GetComponent<InterpreterAtomBehavior>().OutputSlots[inputSlot.GetToIdx()].IsOpen();
            // if(isConnectedOpen)
            // {
            //     from.GetComponent<InterpreterAtomBehavior>().OnOutputChannelReady(inputSlot.GetToIdx());
            //     OnInputChannelReady(index);
            // }
        }

        public void OpenOutput(int index)
        {
            ConnectionSlot outputSlot = OutputSlots[index];
            outputSlot.Open();

            // Call ic and oc ready if all open.
            // TODO: Lots and lots of error handling and bounds checking.
            Entity from = outputSlot.GetConnectedTo();
            bool isConnectedOpen = from.GetComponent<InterpreterAtomBehavior>().InputSlots[outputSlot.GetToIdx()].IsOpen();
            if(isConnectedOpen)
            {
                from.GetComponent<InterpreterAtomBehavior>().OnInputChannelReady(outputSlot.GetToIdx());
                OnOutputChannelReady(index);
            }
        }
        public void CloseOutput(int index)
        {
            ConnectionSlot outputSlot = OutputSlots[index];
            outputSlot.Close();
        }
    }
}
