import ERS

class Sink(ERS.ScriptBehavior):

    def __init__(self, connected_entity):
        super().__init__(connected_entity)
        self.sink_time = 4_000_000
        ERS.add_atom_behavior(connected_entity)
        ERS.set_num_inputs(connected_entity, 1)
        ERS.set_num_outputs(connected_entity, 0)
        ERS.add_transform(connected_entity)
        #print("Sink is initialized")

    def on_entered(self, new_child):
        # DESTROY THE NEW CHILD
        #print(f"Destroying {new_child.name}")
        ERS.destroy_entity(new_child)

    def draw_2d(self):
        ERS.draw_rect(self.entity.pos_x, 0, 1, 1, 0, 1, 1, 0, 1),

    def on_input_channel_ready(self, index):
        #print(f"Sink input channel {index} is ready.")
        pass
    
    def on_start(self):
        #print("Hello i am sink, now starting")
        def do_sink():
            ERS.channel_input_open(self.entity, 0)
            ERS.schedule_local_event(self.sink_time, do_sink)
        do_sink()


