using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("HQ References")]
    [SerializeField] private Health playerHQ;
    [SerializeField] private Health aiHQ;

    [Header("Game Over UI (Optional)")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMPro.TextMeshProUGUI resultText;

    private bool gameEnded = false;

    private void Start() {
        if (playerHQ != null) {
            playerHQ.OnDeath += PlayerHQ_OnDeath;    
        }

        if (aiHQ != null) {
            aiHQ.OnDeath += AiHQ_OnDeath;    
        }
    }

    private void PlayerHQ_OnDeath() {
        if (gameEnded) return;

        gameEnded = true;
        EndGame("You Lost!");
    }

    private void AiHQ_OnDeath() {
        if (gameEnded) return;

        gameEnded = true;
        EndGame("You Won!");
    }

    private void EndGame(string message) {
        Debug.Log("Game Over: " + message);

        if (gameOverPanel != null) {
            gameOverPanel.SetActive(true);
        }

        if (resultText != null) {
            resultText.text = message;
        }
    }
}