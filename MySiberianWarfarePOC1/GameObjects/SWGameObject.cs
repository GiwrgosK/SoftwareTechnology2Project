using System.ComponentModel;

namespace MySiberianWarfarePOC1.GameObjects {
    public class SWGameObject {
        private List<IComponent> m_components = new List<IComponent>();

        public void AddComponent(IComponent newComponent) {
            m_components.Add(newComponent);
        }

        public void RemoveComponent(IComponent component) {
            m_components.Remove(component);
        }

        public T GetComponent<T>() where T : class {
            return m_components.OfType<T>().FirstOrDefault() ?? throw new NullReferenceException("Request of non-existing component");
        }
    }
}