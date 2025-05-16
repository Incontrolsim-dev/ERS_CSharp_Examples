import ERS
import source
import simpleconveyor
import conveyor
import packingstation
import sink
import queue
import math
import pallet_elevator

from types import MethodType

def cos(angle):
    return math.cos(angle * math.pi * 2)

def sin(angle):
    return math.sin(angle * math.pi * 2)

def normalize_angle(angle):
    # Normalize to range (-0.5, 0.5]
    while angle <= -0.5:
        angle += 1.0
    while angle > 0.5:
        angle -= 1.0
    return angle

class GlobalRouting:
    def __init__(self):
        # Graph stores direct connections: key = entity, value = list of tuples (neighbor, output_channel)
        self.graph = {}
        # Reachability maps each entity to the set of entities it can reach.
        self.reachability = {}

        # TODO: This is for testing only. This should become a component.
        # self.targets = 

    def connect(self, from_entity, output_channel, to_entity, input_channel):
        # Establish connection via ERS.
        ERS.channel_connect(from_entity, output_channel, to_entity, input_channel)
        # Build the graph.
        if from_entity not in self.graph:
            self.graph[from_entity] = []
        if to_entity not in self.graph:
            self.graph[to_entity] = []
        # Record only the output channel since target input channel is not relevant.
        self.graph[from_entity].append((to_entity, output_channel))

    def _dfs(self, start_entity):
        # Helper: DFS to compute all nodes reachable from start_entity.
        reachable = set()
        stack = [start_entity]
        while stack:
            current = stack.pop()
            if current not in reachable:
                reachable.add(current)
                for neighbor, _ in self.graph.get(current, []):
                    if neighbor not in reachable:
                        stack.append(neighbor)
        reachable.discard(start_entity)  # Exclude self.
        return reachable

    def precalc_reachability(self):
        # Precalculate reachability for all nodes.
        self.reachability = {}
        for entity in self.graph:
            self.reachability[entity] = self._dfs(entity)

    def next_hops(self, current_entity, target_entity):
        """
        Returns a list of direct connection tuples (neighbor, output_channel) from the current entity
        that eventually lead to the global destination target_entity.
        """
        possible = []
        if current_entity not in self.graph:
            return possible
        for neighbor, out_channel in self.graph[current_entity]:
            # Include this hop if neighbor is the target or can reach the target.
            if neighbor == target_entity or (neighbor in self.reachability and target_entity in self.reachability[neighbor]):
                possible.append((neighbor, out_channel))
        return possible

routing = GlobalRouting()

merge_counter = 0
def create_merge(from_x, from_y, from_z, to_x, to_y, to_z, num_inputs, throughput_time=1_000_000):
    global merge_counter
    merge_counter+=1
    e = ERS.create_straight_conveyor(f"merge_{merge_counter}", from_x, from_y, from_z, to_x, to_y, to_z)
    e.script = conveyor.Merge(e, num_inputs=num_inputs, throughput_time=throughput_time)
    return e

split_counter = 0
def create_split(from_x, from_y, from_z, to_x, to_y, to_z, num_outputs):
    global split_counter
    global routing
    split_counter+=1
    e = ERS.create_straight_conveyor(f"split_{split_counter}", from_x, from_y, from_z, to_x, to_y, to_z)
    e.script = conveyor.Split(e, routing, num_outputs=num_outputs)
    return e

builder_counter = 0
class ConveyorBuilder:
    def __init__(self, name_prefix="", start_x=0.0, start_y=0.0, start_z=0.0, orientation=0.0):
        if len(name_prefix) == 0:
            global builder_counter
            self.name_prefix = f"b{builder_counter}"
            builder_counter+=1
        else:
            self.name_prefix = name_prefix
        self.x = start_x
        self.y = start_y
        self.z = start_z
        self.orientation = orientation  # Angle in turns, 0 = +X direction
        self.previous_conveyor = None
        self.straight_count = 0
        self.helical_count = 0
        # In m/s
        self.conveyor_speed = 1.5

    def create_straight(self, name=None, length=5, dz=0.0, throughput_time=None):
        if name is None:
            name = f"{self.name_prefix}_straight_{self.straight_count}"
            self.straight_count += 1
        
        x_to = self.x + length * cos(self.orientation)
        y_to = self.y + length * sin(self.orientation)
        z_to = self.z + dz

        if throughput_time == None:
            dx = x_to - self.x
            dy = y_to - self.y
            dz = z_to - self.z
            total_len = math.sqrt(dx*dx + dy*dy + dz*dz)
            throughput_time = total_len / self.conveyor_speed
            throughput_time *= 1_000_000
            throughput_time = int(throughput_time)

        conv = ERS.create_straight_conveyor(name, self.x, self.y, self.z, x_to, y_to, z_to)
        print(f"throughput time = {throughput_time}")
        conv.script = simpleconveyor.SimpleConveyor(conv, throughput_time=throughput_time)
        
        if self.previous_conveyor:
            global routing
            routing.connect(self.previous_conveyor, 0, conv, 0)
        
        self.x, self.y, self.z = x_to, y_to, z_to
        self.previous_conveyor = conv
        return conv

    def create_helical(self, name=None, radius=5, angle=0.5, height=0.0, throughput_time=None):
        if name is None:
            name = f"{self.name_prefix}_helical_{self.helical_count}"
            self.helical_count += 1

        # Determine turning direction and compute the starting angle on the circle.
        if angle >= 0:
            # Left turn: tangent = phi + 0.25, so phi = orientation - 0.25.
            phi = self.orientation - 0.25
            new_tangent = phi + angle + 0.25
        else:
            # Right turn: tangent = phi - 0.25, so phi = orientation + 0.25.
            phi = self.orientation + 0.25
            new_tangent = phi + angle - 0.25
        
        if throughput_time==None:
            total_angle = abs(angle)
            w = radius*3.14*2*total_angle
            l = math.sqrt(w*w+height*height)
            throughput_time = int(1_000_000*l)

        # Compute the circle center so that the starting point is self.x, self.y.
        center_x = self.x - radius * cos(phi)
        center_y = self.y - radius * sin(phi)
        z_center = self.z
        end_z = self.z + height

        conv = ERS.create_helical_conveyor(name, center_x, center_y, z_center, radius, phi, phi + angle, end_z)
        conv.script = simpleconveyor.SimpleConveyor(conv, throughput_time=throughput_time)

        if self.previous_conveyor:
            global routing
            routing.connect(self.previous_conveyor, 0, conv, 0)

        # Calculate the helix end point on the circle.
        end_x = center_x + radius * cos(phi + angle)
        end_y = center_y + radius * sin(phi + angle)

        # Update builder state.
        self.x, self.y, self.z = end_x, end_y, end_z
        self.orientation = normalize_angle(new_tangent)
        self.previous_conveyor = conv
        return conv

    def rotate(self, angle):
        """Rotates the orientation by a given angle in turns."""
        self.orientation += angle
        self.orientation = normalize_angle(self.orientation)
    
    def set_position(self, x, y, z):
        """Manually sets the current position."""
        self.x, self.y, self.z = x, y, z
    
    def set_orientation(self, angle):
        """Manually sets the current orientation."""
        self.orientation = normalize_angle(angle)

start_y = 80
start_x = -80   
bay_x = start_x
bay_center_y = start_y
lane_width = 1.5
left_bay = ERS.create_highbay("inbound0", bay_x-20, bay_center_y+lane_width/2.0, 0, 0)
right_bay = ERS.create_highbay("inbound1", bay_x-20, bay_center_y-lane_width/2.0, 0, 1)

bld = ConveyorBuilder(start_x=start_x, start_y=start_y, start_z=0, orientation=0.0)
first_conv = bld.create_straight(length=40)
bld.rotate(-0.25)
before_queue = bld.create_straight(length=start_y)

# test queue
q = ERS.create_straight_conveyor(f"queue", bld.x, bld.y, bld.z, 0, 0, 0)
q.script = queue.Queue(q)
q.script.throughput_time_per_position = 1_500_000
def on_exited(self, old_child):
    if len(self.children) < 4:
        ERS.channel_output_close(self.entity, 0)

pe = ERS.create_entity("pallet_elevator")
pe.script = pallet_elevator.PalletElevator(pe)

routing.connect(before_queue, 0, q, 0)
routing.connect(q, 0, pe, 0)

inbound_source = ERS.create_entity("inbound_source")
inbound_source.script = source.Source(inbound_source)   
inbound_source.script.spawn_time=20_000_000
routing.connect(inbound_source, 0, first_conv, 0)
def spawn(self):
    pallet = ERS.create_load_with_type(f"inbound_pallet_{self.spawn_counter}", 8+self.spawn_counter%8)
    return pallet
inbound_source.script.spawn = MethodType(spawn, inbound_source.script)

def cool_lanes(source_idx, num_splits=2):
    s0 = ERS.create_entity(f"source{source_idx}")
    s0.script = source.Source(s0)
    def spawn(self, i=source_idx):
        tote = ERS.create_load_with_type(f"load {self.spawn_counter}", self.spawn_counter%4+i*4)
        return tote
    s0.script.spawn = MethodType(spawn, s0.script)
    builder0 = ConveyorBuilder(start_x=0, start_y=source_idx-0.5, start_z=12)
    conv0 = builder0.create_straight(length=3)
    routing.connect(s0, 0, conv0, 0)

    builder0.create_straight(length=10, dz=source_idx*2)
    builder0.create_helical(radius=3+source_idx, angle=0.25)

    if source_idx > 0:
        builder0.create_straight(length=source_idx*20)

    pos_x = builder0.x
    pos_y = builder0.y
    pos_z = builder0.z
    split_len = 10

    outputs = []
    split = None
    for i in range(num_splits):
        straight_builder = ConveyorBuilder(orientation=0.25, start_x=pos_x, start_y=pos_y, start_z=pos_z)
        straight = straight_builder.create_straight(length=split_len-1)
        if split != None:
            routing.connect(split, 1, straight, 0)
        else:
            routing.connect(builder0.previous_conveyor, 0, straight, 0)
        bld = ConveyorBuilder(orientation=0.25, start_x=pos_x, start_y=pos_y+split_len, start_z=pos_z)
        split = create_split(pos_x, pos_y+split_len-1, pos_z, pos_x, pos_y+split_len, pos_z, num_outputs=(2 if i<num_splits-1 else 1))
        routing.connect(straight, 0, split, 0)
        if i != (num_splits-1):
            def route(self, incoming, s_id=source_idx):
                type = ERS.get_load_type(incoming)
                w = type-s_id*4
                output_idx = 0 if w < 2 else 1
                #print(f"{self.entity.name} routing type {type} to {output_idx} (index = {index})")
                return output_idx
            split.script.route = MethodType(route, split.script)
        offramp = bld.create_helical(radius=5, angle=0.125)
        routing.connect(split, 0, offramp, 0)
        bld.create_helical(radius=3, angle=0.125)
        bld.create_straight(length=10)
        bld.create_helical(radius=2, angle=-0.2)
        bld.create_helical(radius=2, angle=0.2)
        bld.create_straight(length=10+source_idx)

        # The wokkel
        bld.create_helical(radius=2, angle=5+source_idx, height=-12-2*source_idx, throughput_time=15_000_000)

        #bld.create_straight(length=10)
        outputs.append(bld)
        pos_y += split_len

    return outputs

lane_outputs = cool_lanes(0)
lane_outputs += cool_lanes(1)

sort_outputs = []
for index, bld in enumerate(lane_outputs):
    conv0 = bld.create_straight(length=5+len(lane_outputs)*2-index*2)
    bld.create_helical(radius=2, angle=0.25)
    bld.create_straight(length=3)
    bld.create_straight(length=3, dz=1)

    # move until y=10+index
    bld.create_straight(length=bld.y-10-index)
    bld.create_helical(radius=len(lane_outputs)*2-index*2, angle=0.25)

    # Highway
    last = bld.create_straight(length=60, throughput_time=10_000_000)

    sort_split = create_split(bld.x, bld.y, bld.z, bld.x+1, bld.y, bld.z, num_outputs=2)
    def route(self, incoming):
        type = ERS.get_load_type(incoming)
        output_idx = type%2
        #print(f"{self.entity.name} routing type {type} to {output_idx} (index = {index})")
        return output_idx
    sort_split.script.route = MethodType(route, sort_split.script)
    routing.connect(last, 0, sort_split, 0)

    angle = 0.1
    r = 2

    bld1 = ConveyorBuilder(start_x=bld.x+1, start_y=bld.y, start_z=bld.z)
    down = bld1.create_helical(radius=r, angle=-angle)
    bld1.create_helical(radius=r, angle=angle)
    bld1.create_straight(length=3)
    routing.connect(sort_split, 1, down, 0)
    sort_outputs.append(bld1)

    bld0 = ConveyorBuilder(start_x=bld.x+1, start_y=bld.y, start_z=bld.z)
    up = bld0.create_helical(radius=r, angle=angle)
    bld0.create_helical(radius=r, angle=-angle)
    bld0.create_straight(length=3)
    routing.connect(sort_split, 0, up, 0)
    sort_outputs.append(bld0)

output_merges = []
last_x = 0
last_y = 0
last_z = 0
first_x = 0
first_y = 0
first_z = 0
for index, bld in enumerate(sort_outputs):
    bld.create_straight(length=10+index*10)
    bld.create_helical(radius=1, angle=-0.25)
    bld.create_straight(length=4)
    bld.create_helical(radius=1, angle=-0.125)

    # test queue
    q = ERS.create_straight_conveyor(f"queue", bld.x, bld.y, bld.z, bld.x-10, bld.y-10, bld.z)
    q.script = queue.Queue(q)

    packing_station = ERS.create_entity(f"packing station {index}")
    packing_station.script = packingstation.PackingStation(packing_station)
    # A pallet just for show :)
    pallet = ERS.create_load_with_type("test pallet", 16)
    pallet.local_x = bld.x - 12
    pallet.local_y = bld.y - 10

    routing.connect(bld.previous_conveyor, 0, q, 0)

    bld = ConveyorBuilder(start_x=bld.x-14, start_y=bld.y-10, start_z=bld.z, orientation=-0.25)
    y_diff = bld.y+30
    conv_out = bld.create_straight(length=y_diff)

    routing.connect(q, 0, packing_station, 0)
    routing.connect(packing_station, 0, conv_out, 0)
    merge_inputs = 2
    length = 10
    last_x = bld.x-length
    last_y = bld.y-1
    last_z = bld.z
    if index == 0:
        first_x = bld.x-length
        first_y = bld.y-1
        first_z = bld.z
    merge = create_merge(bld.x, bld.y-1, bld.z, last_x, last_y, last_z, merge_inputs, throughput_time=10_000_000)
    routing.connect(conv_out, 0, merge, 0)
    output_merges.append(merge)

# Connect all merges together to form a single lane
for i in range(len(output_merges) - 1):
    current_merge = output_merges[i+1]
    next_merge = output_merges[i]
    
    # Connect the merges through the straight connector
    print(f"Connecting {current_merge.name} to {next_merge.name}")
    routing.connect(current_merge, 0, next_merge, 1)

# generate test pallet
pallet = ERS.create_load_with_type("test pallet", 9)
pallet2 = ERS.create_load_with_type("test pallet2", 16)
pallet.local_x = 30
pallet2.local_x = 32

bay_x = -20
bay_center_y = -31
lane_width = 1.5
left_bay = ERS.create_highbay("bay0", bay_x, bay_center_y+lane_width/2.0, 0, 0)
right_bay = ERS.create_highbay("bay1", bay_x, bay_center_y-lane_width/2.0, 0, 1)

bay_x = -20
bay_center_y = -51
left_bay = ERS.create_highbay("bay2", bay_x, bay_center_y+lane_width/2.0, 0, 0)
right_bay = ERS.create_highbay("bay3", bay_x, bay_center_y-lane_width/2.0, 0, 1)

bay_x = -20
bay_center_y = -71
left_bay = ERS.create_highbay("bay4", bay_x, bay_center_y+lane_width/2.0, 0, 0)
right_bay = ERS.create_highbay("bay5", bay_x, bay_center_y-lane_width/2.0, 0, 1)

last_merge = output_merges[0]
bld = ConveyorBuilder(start_x=first_x, start_y=first_y, start_z=first_z, orientation=-0.25)

# Create splits and connections for each highbay
bay_positions = [-31, -51, -71]  # Y positions of highbay centers
split_length = 1  # 1 meter long splits

# First straight segment to the first bay position
first_straight = bld.create_straight(length=abs(bld.y - bay_positions[0]))

# Create the splits and connection-
previous_conveyor = first_straight
for bay_idx, bay_y in enumerate(bay_positions):
    # Create split going down 1m in -Y direction

    split = create_split(
        bld.x, bay_y + split_length/2, bld.z,  # From position (at bay Y)
        bld.x, bay_y, bld.z,  # To position (1m down)
        num_outputs=(1 if bay_idx == len(bay_positions) - 1 else 2)  # Last split has only 1 output
    )
    if bay_idx == 0:
        routing.connect(last_merge, 0, split, 0)
    else:
        routing.connect(previous_conveyor, 0, split, 0)
    
    # Create lane into bay from bottom of split
    bay_builder = ConveyorBuilder(
        start_x=bld.x,
        start_y=bay_y, 
        start_z=bld.z,
        orientation=0.0
    )
    bay_builder.rotate(0.5)  # Rotate to face -X direction
    into_bay = bay_builder.create_straight(length=30)
    routing.connect(split, 0, into_bay, 0)
    
    # Create and connect sink
    s = ERS.create_entity(f"sink_bay_{bay_idx}")
    s.script = s.script = sink.Sink(s)
    routing.connect(into_bay, 0, s, 0)
    
    # If not the last split, create straight segment to next split position
    if bay_idx < len(bay_positions) - 1:
        # Create straight segment from this bay Y to next bay Y
        straight = ERS.create_straight_conveyor(
            f"bay_connector_{bay_idx}",
            bld.x, bay_y, bld.z,  # From current bay Y
            bld.x, bay_positions[bay_idx + 1], bld.z  # To next bay Y
        )
        straight.script = simpleconveyor.SimpleConveyor(straight)
        routing.connect(split, 1, straight, 0)
        previous_conveyor = straight
        
        # Add routing logic
        def route(self, incoming, bay_index=bay_idx):
            pallet_type = ERS.get_load_type(incoming)
            target_bay = (pallet_type - 8) // 3
            output_idx = 0 if target_bay == bay_index else 1
            return output_idx
        split.script.route = MethodType(route, split.script)

routing.precalc_reachability()

print("HOT RELOADING!!!!")

