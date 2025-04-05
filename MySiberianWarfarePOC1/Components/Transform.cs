using MySiberianWarfarePOC1.Interfaces;
using System.Numerics;

namespace MySiberianWarfarePOC1.Components {
    internal abstract class Transform : IComponent {
        protected Vector3 position;
        protected Vector3 rotation;
        protected Vector3 scale;

        protected Transform(Vector3 position, Vector3 rotation, Vector3 scale) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

    internal class TransformComponent : Transform, IImmutableTransform, IMutableTransform {
        public TransformComponent(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale) {
        }

        Vector3 IImmutableTransform.Position => position;
        Vector3 IImmutableTransform.Rotation => rotation;
        Vector3 IImmutableTransform.Scale => scale;

        Vector3 IMutableTransform.Position {
            get => position;
            set => position = value;
        }

        Vector3 IMutableTransform.Rotation {
            get => rotation;
            set => rotation = value;
        }
        
        Vector3 IMutableTransform.Scale {
            get => scale;
            set => scale = value;
        }
    }
}