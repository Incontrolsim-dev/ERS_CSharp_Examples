import ERS

class Queue(ERS.ScriptBehavior):
    def __init__(self, entity, capacity=10):
        super().__init__(entity)
        ERS.add_atom_behavior(entity)
        ERS.set_num_inputs(entity, 1)
        ERS.set_num_outputs(entity, 1)
        
        self.capacity = capacity
        self.throughput_time_per_position = 500_000
        self.queue = []  # Store incoming transportables

    def on_entered(self, new_child):
        #print(f"Queue received entity {new_child.name}.")

        self.queue.append(new_child)
        queue_position = self.capacity - len(self.queue)  # Determine position in queue
        throughput_time = abs(queue_position * self.throughput_time_per_position)
        ERS.path_animate(new_child, throughput_time, 0.0, queue_position / self.capacity, self.entity, 0)

        # Close input if queue reaches max capacity
        if len(self.queue) >= self.capacity:
            ERS.channel_output_open(self.entity, 0)
    
    def on_input_channel_ready(self, index):
        pass

    def on_output_channel_ready(self, index):
        #print("Trying to send from queue to output.")

        if not self.queue:
            #print("Queue is empty, reopening input channel.")
            ERS.channel_input_open(self.entity, 0)
            return

        # **Animate and send all queued transportables sequentially**
        for i, child in enumerate(self.queue):
            #print(f"Animating and sending {child.name}.")

            # **Animate entity from queue position to output**
            queue_position = i + 1
            throughput_time = abs(queue_position * self.throughput_time_per_position)
            ERS.path_animate(child, throughput_time, (self.capacity-queue_position)/self.capacity, 1.0, self.entity, 0)

            # **schedule_local_event sending after animation**
            ERS.schedule_local_event(throughput_time, lambda c=child: ERS.channel_output_send(self.entity, 0, c))
        self.queue.clear()

        # **Reopen input after sending all transportables**
        ERS.channel_input_open(self.entity, 0)

    def on_start(self):
        #print("Queue initialized.")
        ERS.channel_input_open(self.entity, 0)
    
    def draw_2d(self):
        ERS.draw_rect(self.entity.pos_x, 0, 1, 1, 0, 1, 1, 0, 1)
