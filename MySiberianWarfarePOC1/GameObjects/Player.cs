using System.Numerics;
using MySiberianWarfarePOC1.Components;
using MySiberianWarfarePOC1.Interfaces;

namespace MySiberianWarfarePOC1.GameObjects {

    public class Player : SWGameObject {
        List<SWGameObject> playerUnits = new List<SWGameObject>();

        public void AddUnit(SWGameObject unit) {
            playerUnits.Add(unit);
        }

        public void RemoveUnit(SWGameObject unit) {
            playerUnits.Remove(unit);
        }

        public void Scenario1_MoveUnit() {
            var infantry = playerUnits.FirstOrDefault(unit =>
                unit.GetComponent<IImmutableName>()?.ImmutableName == "Infantry");

            if (infantry == null) {
                Console.WriteLine("Infantry unit not found.");
                return;
            }

            var movementStateMachine = infantry.GetComponent<MovementStateMachine>();
            if (movementStateMachine == null) {
                Console.WriteLine("Movement state machine not found on infantry.");
                return;
            }

            if (!movementStateMachine.GetAvailableActions().Contains(Actions.MOVE)) {
                Console.WriteLine("Infantry cannot move at this time.");
                return;
            }
            
            var beforePos = infantry.GetComponent<IImmutableTransform>().Position;
            Console.WriteLine($"Infantry current position: {beforePos}");
            var targetPosition = new Vector3(5f, 0f, 0f);

            var moveCommand = new MoveCommand(targetPosition, movementStateMachine);
            moveCommand.Execute(new MoveCommand.MoveArgs { NewPosition = targetPosition });

            var newPos = infantry.GetComponent<IImmutableTransform>().Position;
            Console.WriteLine($"Infantry moved to position: {newPos}");
        }
    }
}