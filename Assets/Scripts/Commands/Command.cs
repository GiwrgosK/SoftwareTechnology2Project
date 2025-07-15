using UnityEngine;
using System;
using System.Collections.Generic;

namespace RTS.Commands {
    /// <summary>
    /// Represents the type of command that can be issued
    /// </summary>
    public enum CommandType {
        Move,
        Attack,
        Stop,
        Patrol
    }

    /// <summary>
    /// Represents the current status of a command in the network
    /// </summary>
    public enum CommandStatus {
        Created,
        InTransit,
        Delivered,
        Executed,
        Failed
    }

    /// <summary>
    /// Base class for all commands in the game
    /// </summary>
    public class Command {
        // Unique identifier for the command
        public Guid Id { get; private set; }

        // Type of command
        public CommandType Type { get; private set; }

        // Current status of the command
        public CommandStatus Status { get; set; }

        // The player who issued the command
        public int IssuingPlayerId { get; private set; }

        // The unit or units that should receive this command
        public List<int> TargetUnitIds { get; set; }

        // When the command was created
        public float TimeIssued { get; private set; }

        // Priority of the command (higher numbers = higher priority)
        public int Priority { get; set; }

        // Command-specific parameters
        public Dictionary<string, object> Parameters { get; set; }

        // Constructor
        public Command(CommandType type, int issuingPlayerId, List<int> targetUnitIds, int priority = 1) {
            Id = Guid.NewGuid();
            Type = type;
            Status = CommandStatus.Created;
            IssuingPlayerId = issuingPlayerId;
            TargetUnitIds = targetUnitIds;
            TimeIssued = Time.time;
            Priority = priority;
            Parameters = new Dictionary<string, object>();
        }

        // Clone the command (useful when intercepting/modifying)
        public virtual Command Clone() {
            Command clone = new Command(Type, IssuingPlayerId, new List<int>(TargetUnitIds), Priority);
            clone.Status = Status;

            // Copy parameters
            foreach (var kvp in Parameters) {
                clone.Parameters[kvp.Key] = kvp.Value;
            }

            return clone;
        }
    }
}