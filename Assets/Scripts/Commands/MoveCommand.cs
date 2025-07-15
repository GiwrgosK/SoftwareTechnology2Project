using RTS.Commands;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command {
    public MoveCommand(int issuingPlayerId, List<int> targetUnitIds, Vector3 destination, int priority = 1)
        : base(CommandType.Move, issuingPlayerId, targetUnitIds, priority) {
        Parameters["Destination"] = destination;
    }

    public Vector3 Destination {
        get => (Vector3)Parameters["Destination"]; 
        set => Parameters["Destination"] = value; 
    }

    public override Command Clone() {
        return new MoveCommand(IssuingPlayerId, new List<int>(TargetUnitIds), Destination, Priority) {
            Status = this.Status
        };
    }
}
