using MySiberianWarfarePOC1.Components;
using MySiberianWarfarePOC1.Interfaces;

namespace MySiberianWarfarePOC1.GameObjects {
    public class SWGameObject : IComponentHandler, IComparable<SWGameObject> {
        private List<IComponent> components = new List<IComponent>();

        public void AddComponent(IComponent newComponent) {
            components.Add(newComponent);
        }

        public void RemoveComponent(IComponent component) {
            components.Remove(component);
        }

        public T GetComponent<T>() where T : class {
            return components.OfType<T>().FirstOrDefault() ?? throw new NullReferenceException("Something went wrong.");
        }

        public override bool Equals(object? obj) {
            if (obj is not SWGameObject other) {
                return false;
            }

            var thisName = GetComponent<IImmutableName>()?.ImmutableName;
            var otherName = other.GetComponent<IImmutableName>()?.ImmutableName;

            return thisName != null && thisName == otherName;
        }

        public int CompareTo(SWGameObject? other) {
            if (other == null) {
                return 1;
            }

            var thisName = GetComponent<IImmutableName>()?.ImmutableName;
            var otherName = other.GetComponent<IImmutableName>()?.ImmutableName;

            return string.Compare(thisName, otherName, StringComparison.Ordinal);
        }

        public override int GetHashCode() {
            return GetComponent<IImmutableName>()?.ImmutableName.GetHashCode() ?? base.GetHashCode();
        }
    }
}