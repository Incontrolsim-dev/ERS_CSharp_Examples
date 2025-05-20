import ERS

def build_structure_with_wokkel(x, y, z):
    # Example usage with a more complex conveyor setup
    global builder_counter
    global routing
    builder = ConveyorBuilder(start_x=x, start_y=y, start_z=z)
    # Create source
    s = ERS.create_entity(f"{builder_counter}_source")
    s.script = source.Source(s)

    # Build first conveyor segment
    conv1 = builder.create_straight(length=5, dz=3)

    # Connect source to first conveyor segment. 
    routing.connect(s, 0, conv1, 0)

    builder.create_straight(length=5, dz=-3)
    builder.rotate(-0.05)  # Rotate 
    builder.create_straight(length=5)
    builder.create_straight(length=5, dz=2)
    builder.create_straight(length=5, dz=-2)
    builder.create_helical(radius=2, angle=0.5, height=0.0)
    builder.create_straight(length=10)
    builder.create_helical(radius=4, angle=0.75, height=5)  # 270-degree helix
    builder.create_helical(radius=3, angle=0.5, height=-3)  # 180-degree downward helix
    builder.rotate(-0.25)  # Rotate -90 degrees
    builder.create_straight(length=7)
    builder.create_helical(radius=6, angle=1.0, height=6)  # Full 360-degree helix
    builder.create_straight(length=8)
    builder.create_helical(radius=10, angle = 0.1, height=0.0)
    builder.create_helical(radius=10, angle = -0.1, height=0.0)
    builder.create_helical(radius=10, angle = 0.2, height=0.0)
    builder.create_helical(radius=10, angle = -0.2, height=0.0)
    builder.create_helical(radius=10, angle = -0.2, height=1.0)

    # The actual wokkel:
    builder.create_helical(radius=5.0, angle = 5, height=-20)

    builder.create_straight(length=200)
    builder.create_helical(radius=5.0, angle = 10, height=40)

def complicated_wokkel_structure(width, height):
    for x in range(width):
        for y in range(height):
            build_structure_with_wokkel(x*25, y*25, 0)