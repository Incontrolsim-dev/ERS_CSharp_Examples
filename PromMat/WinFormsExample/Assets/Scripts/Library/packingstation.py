import ERS

class PackingStation(ERS.ScriptBehavior):

    def __init__(self, connected_entity):
        super().__init__(connected_entity)
        ERS.add_atom_behavior(connected_entity)
        ERS.set_num_inputs(connected_entity, 1)
        ERS.set_num_outputs(connected_entity, 1)
        ERS.add_transform(connected_entity)
        self.pallet_counter = 0
        self.in_pallet_counter = 0
        self.can_spawn = True
        self.type = 0

    def on_entered(self, new_child):
        self.in_pallet_counter += 1
        self.type = ERS.get_load_type(new_child)
        ERS.destroy_entity(new_child)
        if self.in_pallet_counter == 10:
            self.in_pallet_counter = 0
            pallet = ERS.create_load_with_type(f"pallet {self.pallet_counter}", 8+self.type)
            self.pallet_counter += 1
            ERS.channel_output_send(self.entity, 0, pallet)

    def on_exited(self, old_child):
        pass

    def draw_2d(self):
        ERS.draw_rect(self.entity.pos_x, 0, 1, 1, 0, 1, 1, 0, 1),

    def on_input_channel_ready(self, index):
        pass

    def on_output_channel_ready(self, index):
        pass
    
    def on_start(self):
        ERS.channel_input_open(self.entity, 0)
        ERS.channel_output_open(self.entity, 0)

