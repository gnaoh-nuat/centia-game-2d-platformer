using UnityEngine;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 1;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log($"{gameObject.name} bị thương! Máu còn: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ĐÃ CHẾT!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
