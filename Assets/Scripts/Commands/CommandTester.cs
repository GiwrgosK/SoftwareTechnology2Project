using RTS.Commands;
using System.Collections.Generic;
using UnityEngine;

public class CommandTester : MonoBehaviour {
    // Reference to objects in the scene
    public Transform targetMarker;

    // Player ID for testing
    private int playerId = 0;

    // Sample unit IDs for testing
    private List<int> unitIds = new List<int>();

    // Test commands
    private Command moveCommand;
    private Command attackCommand;

    void Start() {
        // Generate some sample unit IDs
        for (int i = 0; i < 3; i++) {
            unitIds.Add(i + 1000);
        }

        // Create test commands
        CreateTestCommands();

        // Log command details
        LogCommandDetails();
    }

    // Create some test commands
    private void CreateTestCommands() {
        // Create a move command
        Vector3 destination = targetMarker != null ? targetMarker.position : new Vector3(10, 0, 10);
        moveCommand = new MoveCommand(playerId, unitIds, destination);

        // Create an attack command
        int targetId = 2000; // Some arbitrary target ID
        attackCommand = new AttackCommand(playerId, unitIds, targetId);
    }

    // Log command details to verify they are working
    private void LogCommandDetails() {
        Debug.Log("=== Command System Test ===");

        // Log move command details
        Debug.Log($"Move Command ID: {moveCommand.Id}");
        Debug.Log($"Type: {moveCommand.Type}");
        Debug.Log($"Status: {moveCommand.Status}");
        Debug.Log($"Issuing Player: {moveCommand.IssuingPlayerId}");
        Debug.Log($"Target Units: {string.Join(", ", moveCommand.TargetUnitIds)}");
        Debug.Log($"Time Issued: {moveCommand.TimeIssued}");
        Debug.Log($"Priority: {moveCommand.Priority}");

        if (moveCommand is MoveCommand moveCmd) {
            Debug.Log($"Destination: {moveCmd.Destination}");
        }

        // Log attack command details
        Debug.Log("---");
        Debug.Log($"Attack Command ID: {attackCommand.Id}");
        Debug.Log($"Type: {attackCommand.Type}");
        Debug.Log($"Status: {attackCommand.Status}");
        Debug.Log($"Issuing Player: {attackCommand.IssuingPlayerId}");
        Debug.Log($"Target Units: {string.Join(", ", attackCommand.TargetUnitIds)}");
        Debug.Log($"Time Issued: {attackCommand.TimeIssued}");
        Debug.Log($"Priority: {attackCommand.Priority}");

        if (attackCommand is AttackCommand attackCmd) {
            Debug.Log($"Target ID: {attackCmd.TargetId}");
        }

        // Test cloning
        Command clonedMove = moveCommand.Clone();
        Debug.Log("---");
        Debug.Log($"Cloned Move Command ID: {clonedMove.Id}");
        Debug.Log($"Original == Clone: {moveCommand == clonedMove}");

        if (clonedMove is MoveCommand clonedMoveCmd) {
            Debug.Log($"Cloned Destination: {clonedMoveCmd.Destination}");

            // Modify the clone to demonstrate independence
            clonedMoveCmd.Destination = new Vector3(20, 0, 20);
            Debug.Log($"Modified Cloned Destination: {clonedMoveCmd.Destination}");
            Debug.Log($"Original Destination: {((MoveCommand)moveCommand).Destination}");
        }
    }

    void Update() {
        // Optional: Add runtime testing here
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Creating new test commands...");
            CreateTestCommands();
            LogCommandDetails();
        }
    }
}
