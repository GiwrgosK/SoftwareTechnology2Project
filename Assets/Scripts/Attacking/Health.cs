using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour {
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBar;

    private int _currentHealth;

    public event Action OnDeath;

    private void Awake() {
        _currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(0, _currentHealth);
        UpdateHealthBar();

        if (_currentHealth <= 0) {
            OnDeath?.Invoke();
        }
    }

    private void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.fillAmount = (float)_currentHealth / maxHealth;
        }
    }
}