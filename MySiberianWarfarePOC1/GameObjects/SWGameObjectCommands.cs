using System.Numerics;

namespace MySiberianWarfarePOC1.GameObjects {
    public class CommandArgs {

    }

    public interface ICommandHandler {
        void ExecuteCommand(CommandArgs commandArguments);
    }

    public abstract class ACommand {
        protected ICommandHandler handler;

        public ACommand(ICommandHandler receiver) {
            handler = receiver;
        }

        public abstract void Execute(CommandArgs args);
    }

    public class MoveCommand : ACommand {

        public class MoveArgs : CommandArgs {
            private Vector3 newPosition;

            public Vector3 NewPosition {
                get => newPosition;
                set => newPosition = value;
            }
        }
        private MoveArgs targetPosition;

        public MoveCommand(Vector3 newPosiiton, ICommandHandler handler): base(handler) {
            targetPosition = new MoveArgs(){NewPosition = newPosiiton};
        }

        public override void Execute(CommandArgs args) {
            if (args is MoveArgs moveArgs) {
                handler.ExecuteCommand(moveArgs);
            }
        }
    }
}