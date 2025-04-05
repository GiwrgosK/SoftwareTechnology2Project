using System.Numerics;

namespace MySiberianWarfarePOC1.Interfaces {
    public class InitializationArgs {  
    }

    public interface IComponent{ 
    }

    public interface IComponentHandler {
        T GetComponent<T>() where T : class;
    }

    public interface IImmutableTransform {
        Vector3 Position { get; }
        Vector3 Rotation { get; }
        Vector3 Scale { get; }
    }

    public interface IMutableTransform {
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
    }
}