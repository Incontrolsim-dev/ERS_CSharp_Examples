using System.Numerics;
using Ers;
using Ers.Interpreter;
using Ers.Model;
using Ers.Visualization;
using Ers.WinForms;
using Ers.Math;
using ED;
using Ers.Engine;

namespace WinFormsExample
{
    public partial class FormMain : Form
    {
        private ModelContainer modelContainer;
        private int timeCounter = 0;

        private Mesh convMesh;
        private List<InstancedModel> loadInstanceModels = new();

        private List<Model3D> textModels = new();
        public void AddText(string text, Vector3 pos)
        {
            var model = new Model3D();
            model.AddText(text, pos, 1.5f, new Vector3(0, -1, 0), new Vector3(0, 0, 1));
            textModels.Add(model);
            model.SendToRenderDevice();
        }

        // Define 10 different loads with varying sizes and colors
        Vector3[] centers = { new Vector3(0, 0, 0.25f), new Vector3(0, 0, 0.3f), new Vector3(0, 0, 0.2f),  new Vector3(0, 0, 0.35f),
                              new Vector3(0, 0, 0.15f), new Vector3(0, 0, 0.4f), new Vector3(0, 0, 0.22f), new Vector3(0, 0, 0.28f),
                              new Vector3(0, 0, 0.18f), new Vector3(0, 0, 0.33f) };

        Vector3[] dimensions = { new Vector3(0.5f, 0.4f, 0.5f),    new Vector3(0.6f, 0.45f, 0.6f),  new Vector3(0.4f, 0.35f, 0.4f),
                                 new Vector3(0.55f, 0.42f, 0.7f),  new Vector3(0.3f, 0.3f, 0.3f),   new Vector3(0.7f, 0.5f, 0.8f),
                                 new Vector3(0.45f, 0.38f, 0.55f), new Vector3(0.48f, 0.4f, 0.65f), new Vector3(0.35f, 0.28f, 0.4f),
                                 new Vector3(0.6f, 0.5f, 0.75f) };

        Vector3[] colors = { new Vector3(1, 0, 0),          new Vector3(0, 1, 0),          new Vector3(0, 0, 1),
                             new Vector3(1, 1, 0),          new Vector3(1, 0, 1),          new Vector3(0, 1, 1),
                             new Vector3(0.5f, 0.2f, 0.8f), new Vector3(0.3f, 0.7f, 0.4f), new Vector3(0.9f, 0.5f, 0.1f),
                             new Vector3(0.2f, 0.8f, 0.6f) };

        private void PushEmptyPallet(Mesh palletMesh)
        {
            // Lighter wood color (lighter tan/beige)
            Vector3 woodColor = new Vector3(0.82f, 0.71f, 0.55f);

            // Standard EUR pallet dimensions (in meters)
            // Overall dimensions: 1.2m x 0.8m x 0.144m
            float palletLength = 1.2f;
            float palletWidth  = 0.8f;
            float palletHeight = 0.144f; // Standard EUR pallet height

            float palletHalfLength = palletLength / 2f; // 0.6m
            float palletHalfWidth  = palletWidth / 2f;  // 0.4m

            // Board dimensions
            float deckBoardLength    = 1.2f;   // Full length
            float deckBoardWidth     = 0.1f;   // Slightly adjusted for visual accuracy
            float deckBoardThickness = 0.022f; // Standard thickness

            // Stringer (beam) dimensions
            float stringerLength = 1.2f;
            float stringerWidth  = 0.08f;
            float stringerHeight = 0.1f; // Height of the central blocks

            // Bottom board dimensions
            float bottomBoardLength    = 0.8f; // Width of the pallet
            float bottomBoardWidth     = 0.1f;
            float bottomBoardThickness = 0.022f;

            // Z-positions (heights)
            float bottomZ     = 0f;                                                            // Bottom of pallet
            float bottomDeckZ = bottomZ + bottomBoardThickness / 2f;                           // Center of bottom boards
            float stringerZ   = bottomDeckZ + bottomBoardThickness / 2f + stringerHeight / 2f; // Center of stringers
            float topDeckZ    = stringerZ + stringerHeight / 2f + deckBoardThickness / 2f;     // Center of top deck boards

            // ===== TOP DECK BOARDS (5 boards running lengthwise) =====
            float topBoardSpacing = (palletWidth - 5 * deckBoardWidth) / 4f;

            for (int i = 0; i < 5; i++)
            {
                float yPos          = -palletHalfWidth + deckBoardWidth / 2f + i * (deckBoardWidth + topBoardSpacing);
                Vector3 boardCenter = new Vector3(0, yPos, topDeckZ + bottomBoardThickness);
                Vector3 boardDims   = new Vector3(deckBoardLength, deckBoardWidth, deckBoardThickness);
                palletMesh.PushCube(boardCenter, boardDims, woodColor);
            }

            // ===== STRINGERS (3 beams running lengthwise) =====
            // Positions for the three stringers: left, center, right
            float[] stringerYPositions = {
                -palletHalfWidth + stringerWidth / 2f, // Left
                0,                                     // Center
                palletHalfWidth - stringerWidth / 2f   // Right
            };

            foreach (float yPos in stringerYPositions)
            {
                Vector3 stringerCenter = new Vector3(0, yPos, stringerZ);
                Vector3 stringerDims   = new Vector3(stringerLength, stringerWidth, stringerHeight);
                palletMesh.PushCube(stringerCenter, stringerDims, woodColor);
            }

            // ===== BOTTOM DECK BOARDS (3 boards running widthwise) =====
            // The bottom deck consists of three boards running across the width of the pallet
            float bottomBoardSpacing = (palletLength - 3 * bottomBoardWidth) / 2f;

            for (int i = 0; i < 3; i++)
            {
                float xPos          = -palletHalfLength + bottomBoardWidth / 2f + i * (bottomBoardWidth + bottomBoardSpacing);
                Vector3 boardCenter = new Vector3(xPos, 0, bottomDeckZ);
                Vector3 boardDims   = new Vector3(bottomBoardWidth, bottomBoardLength, bottomBoardThickness);
                palletMesh.PushCube(boardCenter, boardDims, woodColor);
            }

            for (int i = 0; i < 3; i++)
            {
                float xPos          = -palletHalfLength + bottomBoardWidth / 2f + i * (bottomBoardWidth + bottomBoardSpacing);
                Vector3 boardCenter = new Vector3(xPos, 0, topDeckZ);
                Vector3 boardDims   = new Vector3(bottomBoardWidth, bottomBoardLength, bottomBoardThickness);
                palletMesh.PushCube(boardCenter, boardDims, woodColor);
            }
        }

        public FormMain()
        {
            InitializeComponent();

            this.comboBox2D3D.SelectedIndex         = 0;
            this.comboBox3DCameraMode.Enabled       = false;
            this.comboBox3DCameraMode.SelectedIndex = 0;
            this.KeyPreview                         = true;
            this.KeyDown += this.ersVisualization1.OnKeyDown;
            this.KeyUp += this.ersVisualization1.OnKeyUp;
            labelSpeedVsRealtime.Text = "0x real-time";

            ersVisualization1.TargetFrameTime = 1000.0f / 144.0f;
            ersVisualization1.RenderEvent2D += Render2D;
            ersVisualization1.RenderEvent3D += Render3D;
            ersVisualization1.SelectedEntityChanged += Visualization_SelectedEntityChanged;
            ersVisualization1.Init();
            ersVisualization1.Camera2D.Position = new Vector2(5, 0);
            ersVisualization1.Camera2D.Zoom     = 50.0f;
            ersVisualization1.Camera3D.LookAt   = new Vector3(5, 0, 0);
            ersVisualization1.Camera3D.ZFar     = 1000.0f;

            ersRunControl1.Stop += StopTick;
            ersRunControl1.AttachObjects(Tick, ersClock1);

            ersTreeView1.AfterSelect += ErsTreeView_AfterSelect;

            // convMesh = new Mesh();

            //// Parameters for the central conveyor belt (rollers)
            // Vector3 centralCenter = new Vector3(0, 0, 0);
            // float radius = 5.0f;
            // float beginAngle = 0.0f;
            // float endAngle = (float)(4 * Math.PI); // Four full turns.
            // float endZ = 10.0f;

            // ConveyorBuilder.AddHelicalConveyor(convMesh, centralCenter, radius, beginAngle, endAngle, beginZ, endZ, 50);
            // ConveyorBuilder.AddHelicalConveyor(convMesh, new Vector3(0, 12, 0), radius, beginAngle, endAngle, beginZ, 15.0f, 50);
            // ConveyorBuilder.AddHelicalConveyor(convMesh, new Vector3(12, 12, 0), radius, beginAngle, endAngle, beginZ, 25.0f, 50);
            // ConveyorBuilder.AddHelicalConveyor(convMesh, new Vector3(12, 0, 0), radius, beginAngle, endAngle, beginZ, 45.0f, 50);
            // ConveyorBuilder.AddHelicalConveyor(convMesh, new Vector3(-12, 0, 0), radius, beginAngle, endAngle * 5, beginZ, 15.0f, 50);
            // ConveyorBuilder.AddHelicalConveyor(convMesh, new Vector3(-12, 12, 0), radius, beginAngle, endAngle * 10, beginZ, 45.0f, 500);
            // ConveyorBuilder.AddHelicalConveyor(convMesh, new Vector3(-24, 24, 0), radius, beginAngle, -endAngle * 10, beginZ, 45.0f,
            // 500); ConveyorBuilder.AddStraightConveyor(convMesh, new Vector3(0, 24, 24), new Vector3(10, 34, 34)); ConveyorBuilder builder
            // = new ConveyorBuilder(new Vector3(0, 0, 0), new Vector3(1, 0, 0)); // Start at origin, facing X
            // builder.Forward(convMesh, 5.0f);           // Straight 5 units along X
            // builder.Turn(convMesh, (float)Math.PI / 2, 0, 3); // Turn 90 degrees left around Z (now facing Y)
            // builder.Forward(convMesh, 3.0f);           // Straight 3 units along Y
            // builder.Turn(convMesh, -(float)Math.PI / 2, 0, 3); // Turn 90 degrees left around Z (now facing Y)

            // convMesh.SendToRenderDevice();

            // Mount asset directories
            // VirtualFileSystem.MountDirectory("./Assets/Textures", "Textures");
            // VirtualFileSystem.MountDirectory("./Assets/Meshes", "Meshes");

            // 3D conveyors and action points
            // AssetManager.AddMesh("Meshes/conveyor.glb", "conveyorPieceMesh");
            // Mesh conveyorPieceMesh = AssetManager.RetrieveMesh("conveyorPieceMesh").Copy();
            // conveyorPieceMesh.TranslateToFloor();

            InitializeED.RegisterFunctions();

            // Functions for this model.
            Interpreter.RegisterInterpreterFunction(
                "create_load", args =>
                               {
                                   string name = args.GetStringArgument(0);
                                   Entity load = TransportableComponent.Create(0, name);
                                   return InterpreterVariable.CreateEntity(load);
                               });
            Interpreter.RegisterInterpreterFunction(
                "create_load_with_type", args =>
                                         {
                                             string name = args.GetStringArgument(0);
                                             int type    = (int)args.GetUInt64Argument(1);
                                             Entity load = TransportableComponent.Create(type, name);
                                             return InterpreterVariable.CreateEntity(load);
                                         });
            Interpreter.RegisterInterpreterFunction(
                "get_load_target", args =>
                                   {
                                       Entity load   = args.GetEntityArgument(0);
                                       Entity target = load.GetComponent<TransportableComponent>().Value.Target;
                                       return InterpreterVariable.CreateEntity(target);
                                   });
            Interpreter.RegisterInterpreterFunction(
                "set_load_target", args =>
                                   {
                                       Entity load                                              = args.GetEntityArgument(0);
                                       Entity target                                            = args.GetEntityArgument(1);
                                       load.GetComponent<TransportableComponent>().Value.Target = target;
                                       return InterpreterVariable.None();
                                   });
            Interpreter.RegisterInterpreterFunction(
                "get_load_type", args =>
                                 {
                                     Entity load = args.GetEntityArgument(0);
                                     int type    = load.GetComponent<TransportableComponent>().Value.Type;
                                     return InterpreterVariable.CreateUInt64((UInt64)type);
                                 });
            Interpreter.RegisterInterpreterFunction(
                "set_load_type", args =>
                                 {
                                     Entity load                                            = args.GetEntityArgument(0);
                                     int type                                               = (int)args.GetUInt64Argument(1);
                                     load.GetComponent<TransportableComponent>().Value.Type = type;
                                     return InterpreterVariable.CreateUInt64((UInt64)type);
                                 });
            Interpreter.RegisterInterpreterFunction(
                "create_highbay", args =>
                                  {
                                      string name = args.GetStringArgument(0);
                                      float x     = (float)args.GetDoubleArgument(1);
                                      float y     = (float)args.GetDoubleArgument(2);
                                      float z     = (float)args.GetDoubleArgument(3);
                                      // TODO: Make bool argument
                                      bool mirrored = args.GetUInt64Argument(4) > 0;

                                      Entity highbay = CEntity.Create(name);
                                      var transform  = highbay.AddComponent<TransformComponent>();
                                      transform.Value.SetPosition(new Vector3(x, y, z));
                                      var bay0Component              = highbay.AddComponent<BayComponent>();
                                      bay0Component.Value.IsMirrored = mirrored;

                                      return InterpreterVariable.CreateEntity(highbay);
                                  });
            Interpreter.RegisterInterpreterFunction(
                "create_straight_conveyor", args =>
                                            {
                                                string name = args.GetStringArgument(0);
                                                float xFrom = (float)args.GetDoubleArgument(1);
                                                float yFrom = (float)args.GetDoubleArgument(2);
                                                float zFrom = (float)args.GetDoubleArgument(3);
                                                float xTo   = (float)args.GetDoubleArgument(4);
                                                float yTo   = (float)args.GetDoubleArgument(5);
                                                float zTo   = (float)args.GetDoubleArgument(6);

                                                Entity conveyor = ConveyorComponent.CreateConveyor(name);
                                                Vector3 from    = new(xFrom, yFrom, zFrom);
                                                Vector3 to      = new(xTo, yTo, zTo);
                                                conveyor.GetComponent<PathComponent>().Value.AddStraight(from, to);
                                                return InterpreterVariable.CreateEntity(conveyor);
                                            });
            Interpreter.RegisterInterpreterFunction(
                "create_helical_conveyor",
                args =>
                {
                    string name      = args.GetStringArgument(0);
                    float xCenter    = (float)args.GetDoubleArgument(1);
                    float yCenter    = (float)args.GetDoubleArgument(2);
                    float zCenter    = (float)args.GetDoubleArgument(3);
                    float radius     = (float)args.GetDoubleArgument(4);
                    float beginAngle = (float)args.GetDoubleArgument(5);
                    float endAngle   = (float)args.GetDoubleArgument(6);
                    float endZ       = (float)args.GetDoubleArgument(7);

                    Entity conveyor = ConveyorComponent.CreateConveyor(name);
                    Vector3 from    = new(xCenter, yCenter, zCenter);
                    conveyor.GetComponent<PathComponent>().Value.AddHelical(from, radius, beginAngle, endAngle, endZ);
                    return InterpreterVariable.CreateEntity(conveyor);
                });
            Interpreter.RegisterInterpreterFunction(
                "path_animate", args =>
                                {
                                    Entity toAnimate  = args.GetEntityArgument(0);
                                    UInt64 duration   = args.GetUInt64Argument(1);
                                    float from        = (float)args.GetDoubleArgument(2);
                                    float to          = (float)args.GetDoubleArgument(3);
                                    Entity pathEntity = args.GetEntityArgument(4);
                                    int index         = (int)args.GetUInt64Argument(5);

                                    PathAnimationSystem.Animate(toAnimate, duration, from, to, pathEntity, index);
                                    return InterpreterVariable.None();
                                });

            // Build model
            // Entity conveyor0 = ConveyorComponent.CreateConveyor("conveyor0");
            // var from = new Vector3(0, 0, 0);
            // var to = new Vector3(10, 10, 10);
            // conveyor0.GetComponent<PathComponent>().Value.AddStraight(from, to);
            ////conveyor0.GetComponent<PathComponent>().Value.AddHelical(from, 10, 0, 3.0f, 10.0f);
            //// Spawn tote
            // Entity tote = TransportableComponent.Create(0, "test animation");
            // PathAnimationSystem.Animate(tote, TimeUtil.SecondsToUnits(10), 0, 1, conveyor0, 0);

            VirtualFileSystem.MountDirectory("Assets", "Assets");
            AssetManager.AddModel3D("Assets/totewhite.dae", "tote_white");

            // Create 8 unique totes
            for (int i = 0; i < 8; i++)
            {
                // Mesh loadMesh = new Mesh();
                //  loadMesh.PushCube(centers[i], dimensions[i], colors[i]);

                Model3D toteWhiteModel = AssetManager.RetrieveModel3D("tote_white").Copy();
                // toteWhiteModel.GetMesh(0).TransformVertices(new Vector3(0,0,0.05f));
                toteWhiteModel.GetMesh(0).CenterAtOrigin();
                toteWhiteModel.GetMesh(0).Normalize();
                Vector3 dims = dimensions[i];
                dims.Y *= 1.3f;
                toteWhiteModel.GetMesh(0).TransformVertices(Vector3.Zero, default, 0, dimensions[i]);
                toteWhiteModel.GetMesh(0).TranslateToFloor();
                toteWhiteModel.GetMesh(0).TransformVertices(new Vector3(0, 0, 0.05f));
                toteWhiteModel.AddModel3D(toteWhiteModel);
                toteWhiteModel.GetMaterial(0).Color = colors[i];
                // var transform = toteModel.GetTransform(0);
                //  toteModel.AddMesh(loadMesh);

                var loadInstanceModel = new InstancedModel(toteWhiteModel);
                loadInstanceModels.Add(loadInstanceModel);
            }

            // For each color and each box, create a filled pallet
            for (int i = 0; i < 8; i++)
            {
                Mesh pMesh = new Mesh();
                PushEmptyPallet(pMesh);

                // Standard eur pallet height
                float zStart = 0.144f;
                float length = 1.2f - 0.1f;
                float width  = 0.8f - 0.08f;
                float height = 2.0f;
                pMesh.PushCube(new(0, 0, zStart + height / 2.0f), new(length, width, height), colors[i]);

                var pModel = new Model3D();
                pModel.AddMesh(pMesh);
                var pInstanceModel = new InstancedModel(pModel);
                loadInstanceModels.Add(pInstanceModel);
            }

            // Empty EUR Pallet Model

            var emptyPalletMesh = new Mesh();
            PushEmptyPallet(emptyPalletMesh);
            var palletModel = new Model3D();
            palletModel.AddMesh(emptyPalletMesh);
            var palletInstanceModel = new InstancedModel(palletModel);
            loadInstanceModels.Add(palletInstanceModel);

            AddText("Depalletizer", new Vector3(0, 0, 13));
            AddText("Sorter", new Vector3(40, 8, 2));
            AddText("Picking Stations", new Vector3(40, -10, 2));
            AddText("Highbay Storage", new Vector3(5, -51, 30));
            AddText("Inbound", new Vector3(-80, 80, 30));

            RestartModel();
        }

        protected override void OnFormClosed(FormClosedEventArgs e) { base.OnFormClosed(e); }

        private void Tick(object? sender, EventArgs e)
        {
            modelContainer.Update(TimeUtil.SecondsToUnits(ersRunControl1.StepSize / 60.0f));
            timeCounter++;
            if (timeCounter % 15 == 0)
            {
                timeCounter               = 0;
                labelSpeedVsRealtime.Text = string.Format("{0:F2}x real-time", modelContainer.GetSpeedUp());
            }
        }

        private void StopTick(object? sender, EventArgs e)
        {
            ersTreeView1.ClearEntityTree();
            ersTreeView1.RebuildEntityTree();
            ersTreeView1.ExpandAll();
        }

        private void Render2D(object? sender, RenderEventArgs e)
        {
            // CollisionSystem.UpdateBoundingBoxes(subModel);

            Simulator simulator = modelContainer.GetSimulator(0);

            e.Context.DrawInfiniteGrid2D();

            var subModel = simulator.GetSubModel();
            subModel.EnterSubModel();
            SubModel.GetSubModel().BeginRenderContext(e.Context);

            PathAnimationSystem.Update(SubModel.GetSubModel().GetSimulator().GetCurrentTime());
            TransformSystem.UpdateGlobals(subModel);
            // InterpreterRenderSystem.Render();

            var conveyorView = SubModel.GetSubModel().GetView<ConveyorComponent, PathComponent>([]);
            while (conveyorView.Next())
            {
                var path    = conveyorView.GetComponent<PathComponent>();
                var segment = path.Value.GetSegment(0);

                switch (segment.Type)
                {
                    case PathSegmentType.Straight:
                        Vector3 tf         = segment.To - segment.From;
                        Vector2 beltCenter = (segment.To.XY() + segment.From.XY()) / 2.0f;
                        float length       = tf.XY().Length();
                        float angle        = -Vector2.Normalize(tf.XY()).Angle(Vector2.UnitX) / (2.0f * MathF.PI);
                        Vector2 beltDims   = new Vector2(length, ConveyorBuilder.beltWidth);

                        Vector2 railDims = beltDims + new Vector2(0, ConveyorBuilder.railWidth * 2.0f);
                        e.Context.DrawRect2D(beltCenter, railDims, angle, ConveyorBuilder.railColor);

                        e.Context.DrawRect2D(beltCenter, beltDims, angle, ConveyorBuilder.beltColor);
                        break;

                    case PathSegmentType.Helical:
                        // Convert to turns
                        float diff = segment.EndAngle - segment.BeginAngle;
                        float sign = (diff > 0) ? 1 : -1;
                        float abs  = MathF.Abs(diff);
                        if (abs > 1)
                            abs = abs % 1 + 1;
                        float endAngle  = segment.BeginAngle + abs * sign;
                        float railWidth = ConveyorBuilder.beltWidth + 2.0f * ConveyorBuilder.railWidth;
                        e.Context.DrawArc2D(
                            segment.Center.XY(), segment.Radius, railWidth, segment.BeginAngle, endAngle, ConveyorBuilder.railColor, 20);
                        e.Context.DrawArc2D(
                            segment.Center.XY(), segment.Radius, ConveyorBuilder.beltWidth, segment.BeginAngle, endAngle,
                            ConveyorBuilder.beltColor, 20);
                        break;
                }
            }
            conveyorView.Dispose();

            // Draw loads
            var transportableView = SubModel.GetSubModel().GetView<TransformComponent, TransportableComponent, RelationComponent>([]);
            while (transportableView.Next())
            {
                var transform = transportableView.GetComponent<TransformComponent>();
                var load      = transportableView.GetComponent<TransportableComponent>();

                int type = load.Value.Type;
                Vector2 dims;
                Vector4 color;
                // TODO: Do this in a nicer way. Quick hack for promat
                if (type < 8)
                {
                    // Simple load
                    dims  = new Vector2(dimensions[type].X, dimensions[type].Y);
                    color = new Vector4(colors[type].X, colors[type].Y, colors[type].Z, 1.0f);
                }
                else if (type < 16)
                {
                    // Pallet with load
                    dims           = new Vector2(1.2f, 0.8f);
                    int palletType = type % 8;
                    color          = new Vector4(colors[palletType].X, colors[palletType].Y, colors[palletType].Z, 2.0f);
                }
                else
                {
                    // Empty pallet - light brown (wood color)
                    dims  = new Vector2(1.2f, 0.8f);
                    color = new Vector4(0.76f, 0.60f, 0.42f, 1.0f); // Light brown RGBA
                }

                e.Context.DrawRect2D(transform.Value.GetGlobalPosition2D(), dims, transform.Value.GetGlobalRotation3D().Z, color);
            }
            transportableView.Dispose();

            var bayView = SubModel.GetSubModel().GetView<TransformComponent, BayComponent>([]);
            while (bayView.Next())
            {
                var transform = bayView.GetComponent<TransformComponent>();
                var bay       = bayView.GetComponent<BayComponent>();
                Entity entity = bayView.GetEntity();
                float width   = bay.Value.XPositions * 1.2f;
                float height  = bay.Value.YPositions;
                Vector2 pos   = transform.Value.GetPosition2D();
                pos.Y -= bay.Value.IsMirrored ? height : 0;
                // THis is again a hack!! TODO: Calculate the correct width based on pallet number and offsets.
                pos.X += width * 0.2f;
                pos.X += width / 2.0f;
                pos.Y += height / 2.0f;
                Vector4 color = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
                e.Context.DrawRect2D(pos, new Vector2(width, height), 0, color);
                e.Context.DrawText2D(entity.GetName(), new Vector2(pos.X - 3, pos.Y - 1.0f), 3);
            }
            bayView.Dispose();

            SubModel.GetSubModel().EndRenderContext();
            subModel.ExitSubModel();
        }

        private void Render3D(object? sender, RenderEventArgs e)
        {
            e.Context.DrawInfiniteGrid3D();

            var subModel = modelContainer.GetSimulator(0).GetSubModel();
            CollisionSystem.UpdateBoundingBoxes(subModel);

            //// Visualize conveyors
            e.Context.DrawMesh(convMesh);

            foreach (Model3D textModel in textModels)
            {
                e.Context.DrawModel3D(textModel);
            }

            subModel.EnterSubModel();

            // EASTER EGG: Tracking a load.
            // Entity track = CEntity.Find("load 3");
            // if(track.IsValid())
            //{
            //    var pos = track.GetComponent<TransformComponent>().Value.GetGlobalPosition3D();
            //    pos.Z += 0.5f;
            //    ersVisualization1.Camera3D.Position = pos;
            //}
            TransformSystem.UpdateGlobals(subModel);
            PathAnimationSystem.Update(SubModel.GetSubModel().GetSimulator().GetCurrentTime());
            var viewLoads = subModel.GetView<TransportableComponent, TransformComponent, RelationComponent>([]);
            while (viewLoads.Next())
            {
                Ref<TransformComponent> transform = viewLoads.GetComponent<TransformComponent>();
                Ref<TransportableComponent> load  = viewLoads.GetComponent<TransportableComponent>();
                int type                          = load.Value.Type;
                var transportableInstanceModel    = loadInstanceModels[type];
                e.Context.DrawInstanced(
                    transportableInstanceModel, transform.Value.GetGlobalPosition3D(), transform.Value.GetGlobalRotation3D());
            }
            viewLoads.Dispose();

            // SubModel.GetSubModel().PrintGCStats();
            subModel.ExitSubModel();
        }

        private void Visualization_SelectedEntityChanged(object? sender, SelectedEntityEventArgs e)
        {
            propertyGrid1.SetSelectedEntity(ersVisualization1.SelectedEntity, e.ModelContainer.GetSimulator(e.SimulatorID));
        }

        private void ErsTreeView_AfterSelect(object? sender, ErsTreeViewEventArgs e)
        {
            if (ersTreeView1.SelectedNode.Tag == null)
            {
                propertyGrid1.SelectedObject = new object();
                return;
            }

            object selected = ersTreeView1.SelectedNode.Tag;
            if (selected.GetType() == typeof(Entity))
                propertyGrid1.SetSelectedEntity((Entity)selected, e.ModelContainer!.GetSimulator(e.SimulatorID!.Value));
            else
                propertyGrid1.SelectedObject = selected;
        }

        private void comboBox2D3D_SelectedIndexChanged(object sender, EventArgs e)
        {
            ersVisualization1.RenderMode      = (RenderMode)((ComboBox)sender).SelectedIndex;
            this.comboBox3DCameraMode.Enabled = ersVisualization1.RenderMode == RenderMode.Render3D;
        }

        private void comboBox3DCameraMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.comboBox3DCameraMode.Enabled)
                return;

            int index = ((ComboBox)sender).SelectedIndex;
            ersVisualization1.SwitchCamera3DMode((Camera3DMode)index);
            this.splitContainerLeftMiddle.Panel2.Focus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void scriptToolStripMenuItem_Click(object sender, EventArgs e) => new FormScriptEditor().Show();

        private void ReloadPythonModules()
        {
            // Note: This should be different in release!!! BE CAREFUL. Only for quick hotreloading during development.
            // string directoryPath = "../../../Assets/Scripts/Library";
            // string modelPath = "../../../Assets/Scripts/Model/model.py";
            string directoryPath = "Assets/Scripts/Library";
            string modelPath     = "Assets/Scripts/Model/model.py";

            foreach (string file in Directory.EnumerateFiles(directoryPath, "*.py", SearchOption.AllDirectories))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                SubModel.GetSubModel().LoadModuleFromString(fileNameWithoutExtension, "");

                Logger.Debug($"Preloaded python module {fileNameWithoutExtension}");
            }

            foreach (string file in Directory.EnumerateFiles(directoryPath, "*.py", SearchOption.AllDirectories))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                string fileContent              = File.ReadAllText(file);
                SubModel.GetSubModel().LoadModuleFromString(fileNameWithoutExtension, fileContent);

                Logger.Debug($"Loaded python module {fileNameWithoutExtension}");
            }

            string modelContent = File.ReadAllText(modelPath);
            SubModel.GetSubModel().LoadModuleFromString("model", modelContent);
        }

        private void RestartModel()
        {
            // Set up simulation model
            modelContainer = ModelContainer.CreateModelContainer();
            modelContainer.AddSimulator("main", SimulatorType.DiscreteEvent);
            ersVisualization1.AttachedModelContainer = modelContainer;
            ersClock1.AttachedModelContainer         = modelContainer;
            ersTreeView1.AttachedModelContainer      = modelContainer;

            EditingSubModel subModel = modelContainer.GetSimulator(0).GetSubModel();

            subModel.AddComponentType<InterpreterAtomBehavior>();
            subModel.AddComponentType<TransportableComponent>();
            subModel.AddComponentType<BayComponent>();

            subModel.EnterSubModel();
            SubModel.GetSubModel().CreateInterpreter();
            subModel.ExitSubModel();
            subModel.EnterSubModel();

            SubModel.GetSubModel().AddComponentType<ConveyorComponent>();
            SubModel.GetSubModel().AddComponentType<PathComponent>();
            ReloadPythonModules();

            // Update all transforms before building the mesh.
            TransformSystem.UpdateGlobals(subModel);

            // Build the static conveyor mesh.
            convMesh = new Mesh();

            // Origin depalletizer (fake)
            Vector3 color     = new Vector3(0.6f, 0.6f, 0.6f);
            float height      = 2.5f;
            float width       = 2.0f;
            float length      = 2.0f;
            float beamSize    = 0.25f;
            float totalHeight = 12.0f;
            convMesh.PushCube(new Vector3(0, 0, height / 2), new Vector3(width, length, height), color);
            convMesh.PushCube(new Vector3(-width / 2, -length / 2, totalHeight / 2), new Vector3(beamSize, beamSize, totalHeight), color);
            convMesh.PushCube(new Vector3(width / 2, -length / 2, totalHeight / 2), new Vector3(beamSize, beamSize, totalHeight), color);
            convMesh.PushCube(new Vector3(width / 2, length / 2, totalHeight / 2), new Vector3(beamSize, beamSize, totalHeight), color);
            convMesh.PushCube(new Vector3(-width / 2, length / 2, totalHeight / 2), new Vector3(beamSize, beamSize, totalHeight), color);
            convMesh.PushCube(new Vector3(0, 0, totalHeight - beamSize / 2), new Vector3(width + 0.2f, length + 0.2f, beamSize), color);

            // Add the highbays
            BayComponent.AddToMesh(convMesh);

            var conveyorView = SubModel.GetSubModel().GetView<ConveyorComponent, PathComponent>([]);
            while (conveyorView.Next())
            {
                var path    = conveyorView.GetComponent<PathComponent>();
                var segment = path.Value.GetSegment(0);

                switch (segment.Type)
                {
                    case PathSegmentType.Straight:
                        ConveyorBuilder.AddStraightConveyor(convMesh, segment.From, segment.To);
                        break;

                    case PathSegmentType.Helical:
                        // TODO: Intelligent subdivision calculation.
                        ConveyorBuilder.AddHelicalConveyor(
                            convMesh, segment.Center, segment.Radius, segment.BeginAngle, segment.EndAngle, segment.EndZ, 500);
                        break;
                }
            }
            conveyorView.Dispose();
            convMesh.SendToRenderDevice();

            // EASTER EGG: Send this factory over the factory heheh
            // var pModel = new Model3D();
            // pModel.AddMesh(convMesh);
            // var pInstanceModel = new InstancedModel(pModel);
            // loadInstanceModels[0] = pInstanceModel;

            subModel.ExitSubModel();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e) { RestartModel(); }
    }
}
