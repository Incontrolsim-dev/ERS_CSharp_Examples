using System;
using Ers;
using Ers.Visualization;
using System.Numerics; // Ensure Vector3 is used

namespace WinFormsExample
{
    /// <summary>
    /// Represents a bay in a high-bay warehouse storage system.
    /// Configurable parameters define the rack structure and spacing.
    /// Z is up, X is along the aisle, Y is depth (left/right of aisle).
    /// </summary>
    public struct BayComponent : IDataComponent
    {
        // Storage configuration
        public int XPositions; // Number of pallet positions along the aisle
        public int YPositions; // Number of positions per level (left/right depth)
        public int ZPositions; // Number of vertical levels

        // Physical dimensions (configurable)
        public float PositionWidth;   // Width of each storage position (X)
        public float PositionHeight;  // Height of each storage position (Z)
        public float PositionDepth;   // Depth of each storage position (Y)

        public float SpacingX;  // Spacing between columns (along aisle)
        public float SpacingY;  // Depth spacing (left/right)
        public float SpacingZ;  // Vertical spacing between levels

        public float BeamWidth;   // Width of support beams
        public float BeamHeight;  // Height of support beams

        // Mirroring for opposite-side racks (mirrored in Y axis)
        public bool IsMirrored;
        
        // Color scheme
        public Vector3 VerticalBeamColor;  // Vertical beam color (blue)
        public Vector3 HorizontalBeamColor; // Horizontal beam color (orange)
        public Vector3 ShelfColor;          // Shelf color

        public BayComponent(int xPositions, int yPositions, int zPositions,
                            float positionWidth, float positionHeight, float positionDepth,
                            float spacingX, float spacingY, float spacingZ,
                            float beamWidth, float beamHeight, bool isMirrored)
        {
            XPositions = xPositions;
            YPositions = yPositions;
            ZPositions = zPositions;

            PositionWidth = positionWidth;
            PositionHeight = positionHeight;
            PositionDepth = positionDepth;

            SpacingX = spacingX;
            SpacingY = spacingY;
            SpacingZ = spacingZ;

            BeamWidth = beamWidth;
            BeamHeight = beamHeight;

            IsMirrored = isMirrored;
            
            // Colors - blue vertical beams, orange horizontal/cross beams
            VerticalBeamColor = new Vector3(0.0f, 0.3f, 0.8f);    // Blue vertical beams
            HorizontalBeamColor = new Vector3(1.0f, 0.5f, 0.0f);  // Orange horizontal beams
            ShelfColor = new Vector3(0.75f, 0.75f, 0.75f);        // Light gray shelves
        }

        public BayComponent()
        {
            XPositions = 34;  // Number of pallet positions along the aisle
            YPositions = 6;   // Number of positions per level (left/right depth)
            ZPositions = 10;  // Number of vertical levels

            PositionWidth = 1.3f;   // Standard pallet width in meters (EUR pallet + margin)
            PositionHeight = 1.5f;  // Standard pallet height in meters (incl. load)
            PositionDepth = 0.9f;   // Standard pallet depth in meters (EUR pallet + margin)

            SpacingX = 1.5f;  // Spacing between columns (enough for structural beams)
            SpacingY = 1.2f;  // Depth spacing (based on standard racking systems)
            SpacingZ = 1.8f;  // Vertical spacing between levels (enough for forklifts & automation)

            BeamWidth = 0.08f;   // Standard warehouse racking beam width
            BeamHeight = 0.12f;  // Standard warehouse racking beam height

            IsMirrored = false; // Default is left-side racks
            
            // Colors - blue vertical beams, orange horizontal/cross beams
            VerticalBeamColor = new Vector3(0.0f, 0.3f, 0.8f);    // Blue vertical beams
            HorizontalBeamColor = new Vector3(0.8f, 0.4f, 0.0f);  // Orange horizontal beams
            ShelfColor = new Vector3(0.5f, 0.5f, 0.5f);        // Light gray shelves
        }

        /// <summary>
        /// Loops over all entities with a BayComponent and adds the storage bay structure to the mesh.
        /// </summary>
        public static void AddToMesh(Mesh mesh)
        {
            var bayView = SubModel.GetSubModel().GetView<TransformComponent, BayComponent>([]);
            
            // Create pallet model once
            Mesh palletMesh = CreateEurPallet(new Vector3(0.82f, 0.71f, 0.55f));
            
            while (bayView.Next())
            {
                var transform = bayView.GetComponent<TransformComponent>();
                var bayComponent = bayView.GetComponent<BayComponent>();

                Vector3 basePosition = transform.Value.GetGlobalPosition3D();
                bool isMirrored = bayComponent.Value.IsMirrored;

                // Adjust Y position for mirroring (X axis is along the aisle, Z is up, Y is depth)
                float mirrorFactor = isMirrored ? -1f : 1f;

                Vector3 verticalBeamColor = bayComponent.Value.VerticalBeamColor;    // Blue vertical structure
                Vector3 horizontalBeamColor = bayComponent.Value.HorizontalBeamColor; // Orange horizontal beams
                Vector3 shelfColor = bayComponent.Value.ShelfColor;                  // Shelf color

                float bayWidth = bayComponent.Value.XPositions * bayComponent.Value.SpacingX;
                float bayHeight = bayComponent.Value.ZPositions * bayComponent.Value.SpacingZ;
                float bayDepth = bayComponent.Value.YPositions * bayComponent.Value.SpacingY;

                // First, build the main frame structure
                BuildMainFramework(mesh, basePosition, bayComponent.Value, mirrorFactor, verticalBeamColor, horizontalBeamColor);
                
                // Add cross bracing for stability
                AddCrossBracing(mesh, basePosition, bayComponent.Value, mirrorFactor, horizontalBeamColor);
                
                // Add shelving and storage positions
                AddStoragePositions(mesh, basePosition, bayComponent.Value, mirrorFactor, shelfColor, palletMesh);
            }
        }
        
        /// <summary>
        /// Builds the main framework of vertical and horizontal beams
        /// </summary>
        private static void BuildMainFramework(Mesh mesh, Vector3 basePosition, BayComponent bay, float mirrorFactor, 
                                              Vector3 verticalColor, Vector3 horizontalColor)
        {
            // Calculate total dimensions
            float totalWidth = bay.SpacingX * bay.XPositions;
            float totalHeight = bay.SpacingZ * bay.ZPositions;
            float totalDepth = bay.SpacingY * bay.YPositions;
            
            // Vertical support columns - stronger at corners and intervals
            int columnInterval = 3; // Place major columns every X positions
            
            for (int x = 0; x <= bay.XPositions; x++)
            {
                bool isMajorColumn = (x % columnInterval == 0 || x == bay.XPositions);
                float columnWidth = isMajorColumn ? bay.BeamWidth * 1.5f : bay.BeamWidth;
                
                for (int y = 0; y <= bay.YPositions; y++)
                {
                    // Only place columns at edges and intervals in Y direction
                    if (y == 0 || y == bay.YPositions || y % 3 == 0)
                    {
                        float posX = basePosition.X + x * bay.SpacingX;
                        float posY = basePosition.Y + y * bay.SpacingY * mirrorFactor;
                        
                        // Ground to top vertical column - blue color
                        Vector3 columnStart = new Vector3(posX, posY, basePosition.Z);
                        Vector3 columnEnd = new Vector3(posX, posY, basePosition.Z + totalHeight);
                        
                        mesh.PushBeam(
                            columnStart,
                            columnEnd,
                            new Vector3(0, 1, 0), // Right direction (for the column)
                            columnWidth,
                            columnWidth,
                            verticalColor // Blue for vertical beams
                        );
                    }
                }
            }
            
            // Horizontal beams - X direction (along the aisle) - orange color
            for (int z = 0; z <= bay.ZPositions; z++)
            {
                float zPos = basePosition.Z + z * bay.SpacingZ;
                
                for (int y = 0; y <= bay.YPositions; y += bay.YPositions) // Only at front and back
                {
                    float yPos = basePosition.Y + y * bay.SpacingY * mirrorFactor;
                    
                    Vector3 beamStart = new Vector3(basePosition.X, yPos, zPos);
                    Vector3 beamEnd = new Vector3(basePosition.X + totalWidth, yPos, zPos);
                    
                    mesh.PushBeam(
                        beamStart,
                        beamEnd,
                        new Vector3(0, 0, 1), // Up direction (Z is up)
                        bay.BeamWidth,
                        bay.BeamHeight,
                        horizontalColor // Orange for horizontal beams
                    );
                }
            }
            
            // Horizontal beams - Y direction (depth) - orange color
            for (int z = 0; z <= bay.ZPositions; z++)
            {
                float zPos = basePosition.Z + z * bay.SpacingZ;
                
                for (int x = 0; x <= bay.XPositions; x += bay.XPositions) // Only at ends
                {
                    float xPos = basePosition.X + x * bay.SpacingX;
                    
                    Vector3 beamStart = new Vector3(xPos, basePosition.Y, zPos);
                    Vector3 beamEnd = new Vector3(xPos, basePosition.Y + totalDepth * mirrorFactor, zPos);
                    
                    mesh.PushBeam(
                        beamStart,
                        beamEnd,
                        new Vector3(0, 0, 1), // Up direction (Z is up)
                        bay.BeamWidth,
                        bay.BeamHeight,
                        horizontalColor // Orange for horizontal beams
                    );
                }
            }
            
            // Add horizontal support beams at each level for better pallet support
            for (int z = 0; z < bay.ZPositions; z++)
            {
                float zPos = basePosition.Z + z * bay.SpacingZ;
                
                for (int x = 0; x < bay.XPositions; x++)
                {
                    // Add horizontal X-direction beams for pallet support
                    float xPos = basePosition.X + x * bay.SpacingX;
                    
                    for (int y = 0; y <= bay.YPositions; y += bay.YPositions) // Only at front and back edges
                    {
                        float yPos = basePosition.Y + y * bay.SpacingY * mirrorFactor;
                        
                        Vector3 beamStart = new Vector3(xPos, yPos, zPos);
                        Vector3 beamEnd = new Vector3(xPos + bay.SpacingX, yPos, zPos);
                        
                        mesh.PushBeam(
                            beamStart,
                            beamEnd,
                            new Vector3(0, 0, 1), // Up direction (Z is up)
                            bay.BeamWidth,
                            bay.BeamHeight,
                            horizontalColor // Orange for horizontal beams
                        );
                    }
                }
            }
        }
        
        /// <summary>
        /// Adds cross bracing for structural stability
        /// </summary>
        private static void AddCrossBracing(Mesh mesh, Vector3 basePosition, BayComponent bay, float mirrorFactor, Vector3 color)
        {
            // X-Z plane cross bracing (sides)
            for (int y = 0; y <= bay.YPositions; y += bay.YPositions) // Only front and back planes
            {
                for (int x = 0; x < bay.XPositions; x += 3) // Every few sections
                {
                    for (int z = 0; z < bay.ZPositions; z += 2) // Every other level
                    {
                        if (x + 3 <= bay.XPositions && z + 2 <= bay.ZPositions)
                        {
                            float yPos = basePosition.Y + y * bay.SpacingY * mirrorFactor;
                            
                            // Calculate the four corners of the section to brace
                            Vector3 bottomLeft = new Vector3(
                                basePosition.X + x * bay.SpacingX,
                                yPos,
                                basePosition.Z + z * bay.SpacingZ);
                                
                            Vector3 bottomRight = new Vector3(
                                basePosition.X + (x + 3) * bay.SpacingX,
                                yPos,
                                basePosition.Z + z * bay.SpacingZ);
                                
                            Vector3 topLeft = new Vector3(
                                basePosition.X + x * bay.SpacingX,
                                yPos,
                                basePosition.Z + (z + 2) * bay.SpacingZ);
                                
                            Vector3 topRight = new Vector3(
                                basePosition.X + (x + 3) * bay.SpacingX,
                                yPos,
                                basePosition.Z + (z + 2) * bay.SpacingZ);
                            
                            // Draw diagonal cross braces - use orange color (horizontal beam color)
                            mesh.PushBeam(bottomLeft, topRight, new Vector3(0, 1, 0), bay.BeamWidth * 0.6f, bay.BeamHeight * 0.6f, color);
                            mesh.PushBeam(bottomRight, topLeft, new Vector3(0, 1, 0), bay.BeamWidth * 0.6f, bay.BeamHeight * 0.6f, color);
                        }
                    }
                }
            }
            
            // Y-Z plane cross bracing (back)
            for (int x = 0; x <= bay.XPositions; x += bay.XPositions) // Only on the edges
            {
                for (int z = 0; z < bay.ZPositions; z += 2) // Every other level
                {
                    if (z + 2 <= bay.ZPositions)
                    {
                        float xPos = basePosition.X + x * bay.SpacingX;
                        
                        // Calculate the four corners of the section to brace
                        Vector3 bottomNear = new Vector3(
                            xPos,
                            basePosition.Y,
                            basePosition.Z + z * bay.SpacingZ);
                            
                        Vector3 bottomFar = new Vector3(
                            xPos,
                            basePosition.Y + bay.YPositions * bay.SpacingY * mirrorFactor,
                            basePosition.Z + z * bay.SpacingZ);
                            
                        Vector3 topNear = new Vector3(
                            xPos,
                            basePosition.Y,
                            basePosition.Z + (z + 2) * bay.SpacingZ);
                            
                        Vector3 topFar = new Vector3(
                            xPos,
                            basePosition.Y + bay.YPositions * bay.SpacingY * mirrorFactor,
                            basePosition.Z + (z + 2) * bay.SpacingZ);
                        
                        // Draw diagonal cross braces - use orange color (horizontal beam color)
                        mesh.PushBeam(bottomNear, topFar, new Vector3(1, 0, 0), bay.BeamWidth * 0.6f, bay.BeamHeight * 0.6f, color);
                        mesh.PushBeam(bottomFar, topNear, new Vector3(1, 0, 0), bay.BeamWidth * 0.6f, bay.BeamHeight * 0.6f, color);
                    }
                }
            }
            
            // X-Y plane cross bracing (bottom and top)
            for (int z = 0; z < bay.ZPositions; z += bay.ZPositions - 1) // Only on the bottom and top
            {
                for (int x = 0; x < bay.XPositions; x += 3) // Every few sections
                {
                    if (x + 3 <= bay.XPositions)
                    {
                        float zPos = basePosition.Z + z * bay.SpacingZ;
                        
                        // Calculate the four corners of the section to brace
                        Vector3 nearLeft = new Vector3(
                            basePosition.X + x * bay.SpacingX,
                            basePosition.Y,
                            zPos);
                            
                        Vector3 nearRight = new Vector3(
                            basePosition.X + (x + 3) * bay.SpacingX,
                            basePosition.Y,
                            zPos);
                            
                        Vector3 farLeft = new Vector3(
                            basePosition.X + x * bay.SpacingX,
                            basePosition.Y + bay.YPositions * bay.SpacingY * mirrorFactor,
                            zPos);
                            
                        Vector3 farRight = new Vector3(
                            basePosition.X + (x + 3) * bay.SpacingX,
                            basePosition.Y + bay.YPositions * bay.SpacingY * mirrorFactor,
                            zPos);
                        
                        // Draw diagonal cross braces - use orange color (horizontal beam color)
                        mesh.PushBeam(nearLeft, farRight, new Vector3(0, 0, 1), bay.BeamWidth * 0.6f, bay.BeamHeight * 0.6f, color);
                        mesh.PushBeam(nearRight, farLeft, new Vector3(0, 0, 1), bay.BeamWidth * 0.6f, bay.BeamHeight * 0.6f, color);
                    }
                }
            }
        }
        
        /// <summary>
        /// Adds storage shelves and visualizes storage positions
        /// </summary>
        private static void AddStoragePositions(Mesh mesh, Vector3 basePosition, BayComponent bay, float mirrorFactor, Vector3 shelfColor, Mesh palletMesh)
        {
            // Add storage shelves for each position
            Random random = new Random(42); // Seed for consistent randomness
            
            for (int z = 0; z < bay.ZPositions; z++)
            {
                float shelfZ = basePosition.Z + z * bay.SpacingZ + bay.BeamHeight; // Just above the beam
                
                // Add support beams at each level first - thicker for better visibility and pallet support
                for (int x = 0; x < bay.XPositions; x++)
                {
                    // Add thicker support beams in Y direction for each pallet position
                    for (int y = 0; y < bay.YPositions; y++)
                    {
                        float posX = basePosition.X + x * bay.SpacingX;
                        float leftY = basePosition.Y + y * bay.SpacingY * mirrorFactor;
                        float rightY = basePosition.Y + (y + 1) * bay.SpacingY * mirrorFactor;
                        
                        // Support beam in Y direction
                        mesh.PushBeam(
                            new Vector3(posX, leftY, shelfZ),
                            new Vector3(posX, rightY, shelfZ),
                            new Vector3(0, 0, 1), // Up direction
                            bay.BeamWidth * 1.5f, // Slightly wider support
                            bay.BeamHeight,
                            bay.HorizontalBeamColor // Orange for horizontal beams
                        );
                    }
                }
                
                // Now add shelves on top of beams for each position
                for (int x = 0; x < bay.XPositions; x++)
                {
                    for (int y = 0; y < bay.YPositions; y++)
                    {
                        float posX = basePosition.X + x * bay.SpacingX;
                        float posY = basePosition.Y + y * bay.SpacingY * mirrorFactor;
                        float nextY = basePosition.Y + (y + 1) * bay.SpacingY * mirrorFactor;
                        float posZ = shelfZ + bay.BeamHeight; // Position on top of support beams
                        
                        // Determine shelf center and size
                        Vector3 shelfCenter = new Vector3(
                            posX + bay.PositionWidth / 2,
                            (posY + nextY) / 2,
                            posZ + 0.025f // Half thickness of shelf
                        );
                        
                        // Ensure shelf isn't larger than the position spacing
                        Vector3 shelfSize = new Vector3(
                            bay.PositionWidth,
                            Math.Abs(posY - nextY) * 0.95f, // Slightly smaller than the full cell
                            0.05f // Shelf thickness
                        );
                        
                        // Add shelf platform
                        mesh.PushCube(shelfCenter, shelfSize, shelfColor);
                        
                        // Randomly decide if this position has a pallet (70% chance)
                        if (random.NextDouble() < 0.7)
                        {
                            // Calculate exact pallet position to sit on shelf
                            // Position pallet to sit directly on the shelf
                            float palletPosZ = posZ + 0.05f; // Top of shelf
                            
                            // Create a pallet with a load (for visualization)
                            // Make sure it's properly aligned with framework (not sticking out)
                            Vector3 palletPos = new Vector3(
                                posX + bay.PositionWidth / 2, // Center of position
                                (posY + nextY) / 2, // Center of position
                                palletPosZ // On top of shelf
                            );
                            
                            CreateLoadedPalletVisualization(mesh, palletPos, random);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Creates a EUR pallet for visualization
        /// </summary>
        private static Mesh CreateEurPallet(Vector3 woodColor)
        {
            Mesh palletMesh = new Mesh();
            
            // Standard EUR pallet dimensions
            float palletLength = 1.2f;
            float palletWidth = 0.8f;
            float palletHeight = 0.144f;
            
            // Simplified pallet for visualization
            // Just a single block with pallet dimensions and color
            Vector3 palletCenter = new Vector3(0, 0, palletHeight / 2f); // Z is up
            Vector3 palletDims = new Vector3(palletLength, palletWidth, palletHeight); // X, Y, Z
            palletMesh.PushCube(palletCenter, palletDims, woodColor);
            
            return palletMesh;
        }
        
        /// <summary>
        /// Creates a pallet with random load for visualization
        /// </summary>
        private static void CreateLoadedPalletVisualization(Mesh mesh, Vector3 position, Random random)
        {
            // Pallet dimensions
            float palletLength = 1.2f;
            float palletWidth = 0.8f;
            float palletHeight = 0.144f;
            
            // Wood color for pallet
            Vector3 palletWoodColor = new Vector3(0.82f, 0.71f, 0.55f);
            
            // Create the pallet
            Vector3 palletPos = new Vector3(
                position.X,
                position.Y,
                position.Z + palletHeight / 2f
            ); // Z is up
            
            Vector3 palletDims = new Vector3(palletLength, palletWidth, palletHeight);
            mesh.PushCube(palletPos, palletDims, palletWoodColor);
            
            // Random load color
            Vector3 loadColor = new Vector3(
                (float)random.NextDouble() * 0.5f + 0.2f,
                (float)random.NextDouble() * 0.5f + 0.2f,
                (float)random.NextDouble() * 0.5f + 0.2f
            );
            
            // Random load height (0.5m to 1.0m to ensure it doesn't collide with level above)
            float loadHeight = 0.5f + (float)random.NextDouble() * 0.5f;
            
            // Load dimensions with slight random variation but ensuring it fits within pallet
            float loadLength = palletLength * (0.85f + (float)random.NextDouble() * 0.15f);
            float loadWidth = palletWidth * (0.85f + (float)random.NextDouble() * 0.15f);
            
            // Create the load - placement when Z is up
            Vector3 loadPos = new Vector3(
                position.X,
                position.Y,
                position.Z + palletHeight + loadHeight / 2f
            );
            
            Vector3 loadDims = new Vector3(loadLength, loadWidth, loadHeight);
            mesh.PushCube(loadPos, loadDims, loadColor);
        }
    }
}