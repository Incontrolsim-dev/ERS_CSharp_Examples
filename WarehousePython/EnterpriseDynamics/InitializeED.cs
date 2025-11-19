using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ers;
using Ers.Interpreter;

namespace ED
{
    public static class InitializeED
    {
        public static void RegisterFunctions()
        {
            Interpreter.RegisterInterpreterFunction("add_atom_behavior", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                entity.AddComponent<InterpreterAtomBehavior>();
                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("set_num_inputs", scriptArguments =>
            {
                // TODO: Reset and such, remove until 0 and then add. This is
                // quick and dirty.
                Entity entity = scriptArguments.GetEntityArgument(0);
                int num  = (int)scriptArguments.GetInt64Argument(1);
                for(int i = 0; i < num; i++)
                {
                    entity.GetComponent<InterpreterAtomBehavior>().InputSlots.Add(new());
                }
                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("set_num_outputs", scriptArguments =>
            {
                // TODO: Reset and such, remove until 0 and then add. This is
                // quick and dirty.
                Entity entity = scriptArguments.GetEntityArgument(0);
                int num  = (int)scriptArguments.GetInt64Argument(1);
                for(int i = 0; i < num; i++)
                {
                    entity.GetComponent<InterpreterAtomBehavior>().OutputSlots.Add(new());
                }
                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("get_num_inputs", scriptArguments =>
            {
                // TODO: Reset and such, remove until 0 and then add. This is
                // quick and dirty.
                Entity entity = scriptArguments.GetEntityArgument(0);
                int count = entity.GetComponent<InterpreterAtomBehavior>().InputSlots.Count();
                return InterpreterVariable.CreateUInt64((UInt64)count);
            });

            Interpreter.RegisterInterpreterFunction("get_num_outputs", scriptArguments =>
            {
                // TODO: Reset and such, remove until 0 and then add. This is
                // quick and dirty.
                Entity entity = scriptArguments.GetEntityArgument(0);
                int count = entity.GetComponent<InterpreterAtomBehavior>().OutputSlots.Count();
                return InterpreterVariable.CreateUInt64((UInt64)count);
            });

            Interpreter.RegisterInterpreterFunction("channel_connect", scriptArguments =>
            {
                Entity from = scriptArguments.GetEntityArgument(0);
                int fromIdx = (int)scriptArguments.GetInt64Argument(1);
                Entity to = scriptArguments.GetEntityArgument(2);
                int toIdx = (int)scriptArguments.GetInt64Argument(3);

                // TODO: Error handling? Bounds checking? Disconnecting existing slots??
                from.GetComponent<InterpreterAtomBehavior>().OutputSlots[fromIdx].Connect(to, toIdx);
                to.GetComponent<InterpreterAtomBehavior>().InputSlots[toIdx].Connect(from, fromIdx);

                var interpreterScript = SubModel.GetSubModel().GetInterpreterScriptComponent(from);

                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("channel_input_open", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                int inputIdx = (int)scriptArguments.GetInt64Argument(1);

                if(!entity.GetComponent<InterpreterAtomBehavior>().HasInput(inputIdx))
                {
                    // TODO: Good python error handling when this channel does not exist. 
                    Logger.Error($"[ED] Entity {entity.GetName()} has no valid input on slot {inputIdx}");
                    return InterpreterVariable.None();
                }
                entity.GetComponent<InterpreterAtomBehavior>().OpenInput(inputIdx);

                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("channel_input_close", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                int inputIdx = (int)scriptArguments.GetInt64Argument(1);

                // TODO: Error handling? Bounds checking? Disconnecting existing slots??
                entity.GetComponent<InterpreterAtomBehavior>().CloseInput(inputIdx);

                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("channel_output_send", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                int outputIdx = (int)scriptArguments.GetInt64Argument(1);
                Entity toSend = scriptArguments.GetEntityArgument(2);
                var atom = entity.GetComponent<InterpreterAtomBehavior>();
                Entity sendTo = atom.OutputSlots[outputIdx].GetConnectedTo();
                SubModel.GetSubModel().UpdateParentOnEntity(toSend, sendTo);
                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("channel_output_open", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                int outputIdx = (int)scriptArguments.GetInt64Argument(1);

                // TODO: Error handling? Bounds checking? Disconnecting existing slots??
                var atomBehavior = entity.GetComponent<InterpreterAtomBehavior>();
                atomBehavior.OpenOutput(outputIdx);

                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("channel_output_close", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                int outputIdx = (int)scriptArguments.GetInt64Argument(1);

                // TODO: Error handling? Bounds checking? Disconnecting existing slots??
                entity.GetComponent<InterpreterAtomBehavior>().CloseOutput(outputIdx);

                return InterpreterVariable.None();
            });

            Interpreter.RegisterInterpreterFunction("is_output_channel_ready", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                int outputIdx = (int)scriptArguments.GetInt64Argument(1);

                // TODO: Error handling? Bounds checking? Disconnecting existing slots??
                bool result = entity.GetComponent<InterpreterAtomBehavior>().IsOutputChannelReady(outputIdx);

                return InterpreterVariable.CreateBool(result);
            });

            Interpreter.RegisterInterpreterFunction("is_connected", scriptArguments =>
            {
                Entity entity = scriptArguments.GetEntityArgument(0);
                int outputIdx = (int)scriptArguments.GetInt64Argument(1);

                if(outputIdx >= entity.GetComponent<InterpreterAtomBehavior>().OutputSlots.Count)
                {
                    return InterpreterVariable.CreateBool(false);
                }

                if (!entity.GetComponent<InterpreterAtomBehavior>().OutputSlots[outputIdx].IsConnected())
                {
                    return InterpreterVariable.CreateBool(false);
                }

                return InterpreterVariable.CreateBool(true);
            });

            Interpreter.RegisterInterpreterFunction("draw_rect_2d", scriptArguments => { 
                   
                return InterpreterVariable.None();
            });
        }
    }
}
