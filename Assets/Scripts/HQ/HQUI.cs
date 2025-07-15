using UnityEngine;
using UnityEngine.UI;

public class HQUI : MonoBehaviour {
    public static HQUI Instance { get; private set; }

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Button spawnButton;

    private HQSelectable _currentHQ;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("Something went wrong with the HQ UI singleton.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        uiPanel.SetActive(false);
        spawnButton.onClick.AddListener(OnSpawnClicked);
    }

    public void Show(HQSelectable hq) {
        _currentHQ = hq;
        uiPanel.SetActive(true);
    }

    public void Hide() {
        _currentHQ = null;
        uiPanel.SetActive(false);
    }

    private void OnSpawnClicked() {
        _currentHQ?.SpawnUnit();
    }
}