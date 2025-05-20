import ERS

class SimpleConveyor(ERS.ScriptBehavior):

    def __init__(self, entity, throughput_time=10_000_000):
        super().__init__(entity)
        self.throughput_time = throughput_time
        ERS.add_atom_behavior(entity)
        ERS.set_num_inputs(entity, 1)
        ERS.set_num_outputs(entity, 1)

    def on_entered(self, new_child):
        #print(f"{self.entity.name} received entity {new_child.name}.")
        ERS.path_animate(new_child, self.throughput_time, 0.0, 1.0, self.entity, 0)
        # Just always send
        #print(f"{self.entity.name} is connected to output: {ERS.is_connected(self.entity, 0)}")
        if ERS.is_connected(self.entity, 0):
            #print(f"{self.entity.name} is sending {new_child.name}")
            ERS.schedule_local_event(self.throughput_time, lambda: ERS.channel_output_send(self.entity, 0, new_child))
    
    def on_start(self):
        ERS.channel_input_open(self.entity, 0)
        #print(f"{self.entity.name} intput open")
        if ERS.is_connected(self.entity, 0):
            ERS.channel_output_open(self.entity, 0)
            #print(f"{self.entity.name} output open")

    def on_input_channel_ready(self, index):
        #print(f"{self.entity.name} input channel {index} is ready.")
        pass

    def on_output_channel_ready(self, index):
        #print("Output channel is ready on conveyor")
        pass