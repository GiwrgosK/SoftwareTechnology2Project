using System.Numerics;
using MySiberianWarfarePOC1.Components;

namespace MySiberianWarfarePOC1.GameObjects {
    
    internal class SWGameState : SWGameObject {
        private static SWGameState instance;
        private static readonly object singletonLock = new object();
        private List<SWGameObject> allGameObjects = new List<SWGameObject>();

        private SWGameState() {
        }

        public static SWGameState Instance {
            get {
                lock (singletonLock) {
                    if (instance == null) {
                        instance = new SWGameState();
                    }
                    return instance;
                }
            }
        }

        public void RegisterNewGameObject(SWGameObject gameObject) {
            allGameObjects.Add(gameObject);
        }

        public void InitializeGame() {
            MoveStateFactory moveStateFactory = new MoveStateFactory();
            SWGameObject gameObject = new SWGameObject();

            gameObject.AddComponent(new TransformComponent(
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(1, 1, 1)));
            gameObject.AddComponent(
                new UnitTypeComponent(
                    "Infantry",
                    "Army Personnel",
                    "A basic soldier."));
            gameObject.AddComponent(new MovementStateMachine(gameObject, moveStateFactory));
            RegisterNewGameObject(gameObject);

            var warFactory = new SWGameObject();
            warFactory.AddComponent(new TransformComponent(
                new Vector3(10, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(1, 1, 1)));
            warFactory.AddComponent(
                new UnitTypeComponent(
                    "War Factory",
                    "Building",
                    "A building that produces tanks."
                ));
            warFactory.AddComponent(new MovementStateMachine(warFactory, moveStateFactory));
            RegisterNewGameObject(warFactory);

            var player = new Player();
            player.AddUnit(gameObject);
            player.AddUnit(warFactory);
            player.Scenario1_MoveUnit();
        }
    }
}