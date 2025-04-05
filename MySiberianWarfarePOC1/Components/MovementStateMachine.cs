using MySiberianWarfarePOC1.GameObjects;
using MySiberianWarfarePOC1.Interfaces;

namespace MySiberianWarfarePOC1.Components {
    public enum MovementState {
        STOPPED,
        MOVING
    }

    public enum Actions {
        STOP,
        MOVE
    }

    public interface IState {
        List<Actions> GetAvailableActions();
        void ExecuteCommand(CommandArgs commandArguments);
    }

    public interface IStateHandler {
         void SetState(MovementState state);
    }

    public abstract class AState : IState, ICommandHandler {
        protected IComponentHandler unit;
        protected IStateHandler stateHandler;

        public AState(IComponentHandler unit, IStateHandler stateMachine) {
            this.unit = unit;
            stateHandler = stateMachine;
        }

        public abstract List<Actions> GetAvailableActions();
        public abstract void ExecuteCommand(CommandArgs commandArguments);
    }

    public class MovingState : AState {
        public MovingState(IComponentHandler unit, IStateHandler state) : base(unit, state) {
        }

        public override List<Actions> GetAvailableActions() {
            return new List<Actions> { Actions.STOP };
        }

        public override void ExecuteCommand(CommandArgs commandArguments) {
            if (commandArguments is MoveCommand.MoveArgs moveArgs) {
                // TODO: MOVE IT.
            }
        }
    }

    public class StoppedState : AState {
        public StoppedState(IComponentHandler unit, IStateHandler state) : base(unit, state) {
        }

        public override List<Actions> GetAvailableActions() {
            return new List<Actions> { Actions.MOVE };
        }

        public override void ExecuteCommand(CommandArgs commandArguments) {
            if (commandArguments is MoveCommand.MoveArgs moveArgs) {
                unit.GetComponent<IMutableTransform>().Position = moveArgs.NewPosition;
                stateHandler.SetState(MovementState.MOVING);
            }
        }
    }

    public interface IStateFactory {
        IState CreateMovingState(IComponentHandler unit, IStateHandler stateMachine);
        IState CreateStoppedState(IComponentHandler unit, IStateHandler stateMachine);
    }

    public class MoveStateFactory : IStateFactory {
        public IState CreateMovingState(IComponentHandler unit,
            IStateHandler stateMachine) {
            return new MovingState(unit, stateMachine);
        }

        public IState CreateStoppedState(IComponentHandler unit,
            IStateHandler stateMachine) {
            return new StoppedState(unit, stateMachine);
        }
    }

    public class MovementStateMachine : IStateHandler, IComponent, ICommandHandler {
        private Dictionary<MovementState, IState> allStates;
        private IState mCurrentState;

        public MovementStateMachine(IComponentHandler unit,IStateFactory factory) {
            allStates = new Dictionary<MovementState, IState> {
                {MovementState.MOVING, factory.CreateMovingState(unit,this)},
                {MovementState.STOPPED, factory.CreateStoppedState(unit,this)}
            };
            mCurrentState = allStates[MovementState.STOPPED];
        }

        public void SetState(MovementState state) {
            mCurrentState = allStates[state];
        }

        public List<Actions> GetAvailableActions() {
            return mCurrentState.GetAvailableActions();
        }

        public void ExecuteCommand(CommandArgs commandArguments) {
            mCurrentState.ExecuteCommand(commandArguments);
        }
    }
}