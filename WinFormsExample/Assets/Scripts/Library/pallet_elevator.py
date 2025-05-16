import ERS
class PalletElevator(ERS.ScriptBehavior):

    def __init__(self, connected_entity):
        super().__init__(connected_entity)
        print("PalletElevator init")
        ERS.add_atom_behavior(connected_entity)
        ERS.set_num_inputs(connected_entity, 1)

        # for now just destroy the pallet
        ERS.set_num_outputs(connected_entity, 0)
        ERS.add_transform(connected_entity)
        self.pallet_counter = 0
        self.in_pallet_counter = 0
        self.can_spawn = True

    def on_entered(self, new_child):
        self.in_pallet_counter += 1
        ERS.destroy_entity(new_child)

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
        #ERS.channel_output_open(self.entity, 0)


