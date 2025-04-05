using MySiberianWarfarePOC1.Interfaces;

namespace MySiberianWarfarePOC1.Components {
    public abstract class UnitType : IComponent {
        protected string name;
        protected string type;
        protected string description;

        protected UnitType(string name, string type, string description) {
            this.name = name;
            this.type = type;
            this.description = description;
        }
    }

    public interface IImmutableName{
        string ImmutableName { get; }
    }

    public interface IMutableName {
        string MutableName { get; set; }
    }

    public interface IImmutableType {
        string ImmutableType { get; }
    }

    public interface IMutableType {
        string MutableType { get; set; }
    }

    public interface IImmutableDescription {
        string ImmutableDescription { get; }
    }

    public interface IMutableDescription {
        string MutableDescription { get; set; }
    }

    public class UnitTypeComponent: UnitType, IImmutableType, IMutableType, IImmutableName, IMutableName, IImmutableDescription, IMutableDescription, IComparable<UnitTypeComponent> {
        public UnitTypeComponent(string name, string type, string description) : base(name, type, description) {
        }

        string IImmutableName.ImmutableName => name;
        string IImmutableType.ImmutableType => type;
        string IImmutableDescription.ImmutableDescription => description;

        string IMutableName.MutableName {
            get => name;
            set => name = value;
        }

        string IMutableType.MutableType {
            get => type;
            set => type = value;
        }

        string IMutableDescription.MutableDescription {
            get => description;
            set => description = value;
        }

        public bool Equals(UnitTypeComponent? other) {
            return other != null && name == other.name;
        }

        public override int GetHashCode() {
            return name.GetHashCode();
        }

        public int CompareTo(UnitTypeComponent? other) {
            if (other == null) {
                return 1;
            }
            return string.Compare(this.name, other.name, StringComparison.Ordinal);
        }
    }
}