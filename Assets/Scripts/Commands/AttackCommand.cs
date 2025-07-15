using RTS.Commands;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specialized command for attacking
/// </summary>
public class AttackCommand : Command {
    public AttackCommand(int issuingPlayerId, List<int> targetUnitIds, int targetId, int priority = 1)
        : base(CommandType.Attack, issuingPlayerId, targetUnitIds, priority) {
        Parameters["TargetId"] = targetId;
    }

    public int TargetId {
        get => (int)Parameters["TargetId"]; 
        set => Parameters["TargetId"] = value; 
    }

    public override Command Clone() {
        return new AttackCommand(IssuingPlayerId, new List<int>(TargetUnitIds), TargetId, Priority) {
            Status = this.Status
        };
    }
}
