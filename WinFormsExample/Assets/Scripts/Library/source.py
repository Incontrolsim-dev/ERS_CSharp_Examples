import ERS

class Source(ERS.ScriptBehavior):

    def __init__(self, connected_entity):
        super().__init__(connected_entity)
        ERS.add_atom_behavior(connected_entity)
        ERS.set_num_inputs(connected_entity, 0)
        ERS.set_num_outputs(connected_entity, 1)
        ERS.add_transform(connected_entity)
        self.spawn_counter = 0
        self.spawn_time = 1_000_000
        self.throughput_time = 1_000_000
        self.can_spawn = True
        self.time = 0.0

    def on_entered(self, new_child):
        pass

    def on_exited(self, old_child):
        pass

    def draw_2d(self):
        ERS.draw_rect(self.entity.pos_x, 0, 1, 1, 0, 1, 1, 0, 1),
    
    def spawn(self):
        tote = ERS.create_load_with_type(f"load {self.spawn_counter}", self.spawn_counter%17)
        return tote

    def try_spawn(self):
        if not self.can_spawn:
            return

        # TODO: Write an n_children attribute on entity. This is super slow 
        n_children = len(self.entity.children)
        if n_children > 0:
            return

        #tote = ERS.create_load_with_type(f"load {self.spawn_counter}", self.spawn_counter%10)
        self.spawn_counter += 1
        tote = self.spawn()
        tote.parent = self.entity

        self.can_spawn = False
        def set_can_spawn():
            self.can_spawn = True
            self.try_spawn()
        ERS.schedule_local_event(self.spawn_time, set_can_spawn)

        self.try_send()

    def on_output_channel_ready(self, index):
        #print(f"Source output channel {index} is ready.")
        self.try_send()
    
    def try_send(self):
        #print("Trying to send")
        n_children = len(self.entity.children)
        if n_children == 0:
            #print("But we have no totes")
            return

        if not ERS.is_output_channel_ready(self.entity, 0):
            #print("But output is not ready")
            return 

        child = self.entity.children[0]
        #print(f"{self.entity.name} is sending {child.name}")
        ERS.channel_output_send(self.entity, 0, child)
        ERS.channel_output_close(self.entity, 0)
        ERS.schedule_local_event(self.throughput_time, lambda: ERS.channel_output_open(self.entity, 0), {})
        self.try_spawn()

    def on_start(self):
        self.try_spawn()
        ERS.channel_output_open(self.entity, 0)

#print("Source has been loaded")
