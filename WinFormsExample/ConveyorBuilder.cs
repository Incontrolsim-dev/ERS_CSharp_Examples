using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ers.Visualization;

namespace WinFormsExample
{
    public class ConveyorBuilder
    {
        static float beltThickness = 0.1f;  // Pretty thin
        public static float beltWidth = 1.0f;      // 1 meter wide
        public static Vector4 beltColor = new Vector4(0.8f, 0.8f, 0.8f, 1f); // bright white
        public static Vector4 railColor = new Vector4(0.6f, 0.6f, 0.6f, 1f); // Slightly darker
        // Parameters for the guard rails (side beams)
        public static float railWidth = 0.1f;    // Narrow rail
        static float railThickness = 0.3f; // Matching belt thickness

        // Instance state
        private Vector3 currentPosition;
        private Vector3 currentForward;
        private Vector3 upDirection = Vector3.UnitZ;

        public ConveyorBuilder(Vector3 startPosition, Vector3 startForward)
        {
            currentPosition = startPosition;
            currentForward = Vector3.Normalize(startForward);
            if (Math.Abs(currentForward.Z) > 0.001f)
            {
                throw new ArgumentException("Start forward must be in the XY plane.");
            }
        }

        public void Forward(Mesh mesh, float amount)
        {
            Vector3 newPosition = currentPosition + amount * currentForward;
            AddStraightConveyor(mesh, currentPosition, newPosition);
            currentPosition = newPosition;
        }

        public void Turn(Mesh mesh, float radians, float up, float turnRadius, int segments = 16)
        {
            if (segments < 1)
            {
                throw new ArgumentException("Segments must be at least 1.");
            }
            if (turnRadius <= 0)
            {
                throw new ArgumentException("Turn radius must be positive.");
            }

            // Compute perpendicular direction for the helix center
            Vector3 perpendicular = Math.Sign(radians) * -Vector3.Cross(currentForward, upDirection);
            Vector3 centralCenter = currentPosition + turnRadius * perpendicular;

            // Compute begin and end angles
            Vector3 startVector = currentPosition - centralCenter;
            float beginAngle = (float)Math.Atan2(startVector.Y, startVector.X);
            float endAngle = beginAngle + radians;

            // Define Z coordinates
            float endZ = currentPosition.Z + up;

            // Add helical conveyor section
            AddHelicalConveyor(mesh, centralCenter, turnRadius, beginAngle, endAngle, endZ, segments);

            // Compute new position
            Vector3 endPosition = centralCenter + new Vector3(
                turnRadius * (float)Math.Cos(endAngle),
                turnRadius * (float)Math.Sin(endAngle),
                endZ
            );

            // Compute new forward direction
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            Vector3 newForward = new Vector3(
                currentForward.X * cos - currentForward.Y * sin,
                currentForward.X * sin + currentForward.Y * cos,
                0
            );

            // Update state
            currentPosition = endPosition;
            currentForward = Vector3.Normalize(newForward);
        }

        public static void AddHelicalConveyor(Mesh mesh,
                                       Vector3 centralCenter,
                                       float radius,
                                       float beginAngle,
                                       float endAngle,
                                       float endZ,
                                       int segments)
        {
            // Create the central helical conveyor belt.
            mesh.PusHelicalBeam(centralCenter, radius, beginAngle, endAngle, endZ, beltWidth, beltThickness, segments, beltColor);

            // Calculate rail positions:
            float innerRailRadius = radius - beltWidth / 2.0f - railWidth / 2.0f;
            float outerRailRadius = radius + beltWidth / 2.0f + railWidth / 2.0f;

            // Create the helical guard rails.
            mesh.PusHelicalBeam(centralCenter, innerRailRadius, beginAngle, endAngle, endZ, railWidth, railThickness, segments, railColor);
            mesh.PusHelicalBeam(centralCenter, outerRailRadius, beginAngle, endAngle, endZ, railWidth, railThickness, segments, railColor);
        }

        public static void AddStraightConveyor(Mesh mesh, Vector3 from, Vector3 to)
        {
            // Create the central conveyor belt.
            mesh.PushBeam(from, to, Vector3.UnitZ, beltWidth, beltThickness, new Vector3(beltColor.X, beltColor.Y, beltColor.Z));

            // Compute the direction of the belt.
            Vector3 direction = Vector3.Normalize(to - from);
            // Compute the perpendicular right vector (assuming up is Vector3.UnitZ).
            Vector3 right = Vector3.Normalize(Vector3.Cross(direction, Vector3.UnitZ));

            // Calculate offset distance: half the belt plus half the rail width.
            float offset = (beltWidth / 2.0f) + (railWidth / 2.0f);

            // Compute endpoints for the left guard rail.
            Vector3 leftFrom = from - right * offset;
            Vector3 leftTo   = to   - right * offset;

            // Compute endpoints for the right guard rail.
            Vector3 rightFrom = from + right * offset;
            Vector3 rightTo   = to   + right * offset;

            // Create the guard rails.
            mesh.PushBeam(leftFrom, leftTo, Vector3.UnitZ, railWidth, railThickness, new Vector3(railColor.X, railColor.Y, railColor.Z));
            mesh.PushBeam(rightFrom, rightTo, Vector3.UnitZ, railWidth, railThickness, new Vector3(railColor.X, railColor.Y, railColor.Z));
        }
    }
}