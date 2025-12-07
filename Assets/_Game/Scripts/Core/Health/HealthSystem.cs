using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] private CharacterStatsSO _stats;

    [Header("Events")]
    public UnityAction<int, int> OnHealthChanged; 
    public UnityAction OnDeath;
    public UnityAction<Vector2> OnDamaged;

    [SerializeField] private int _currentHealth;
    private bool _isInvulnerable = false;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _stats.MaxHealth;

    private void Awake()
    {
        if (_stats == null)
        {
            Debug.LogError($"{gameObject.name}: Chưa gán CharacterStatsSO!");
            return;
        }
        
        _currentHealth = _stats.MaxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(_currentHealth, _stats.MaxHealth);
    }

    public void TakeDamage(int damageAmount, Vector2 hitDirection)
    {
        if (_currentHealth <= 0 || _isInvulnerable)
            return;

        _currentHealth -= damageAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _stats.MaxHealth);

        OnHealthChanged?.Invoke(_currentHealth, _stats.MaxHealth);

        if (_currentHealth <= 0)
        {
            HandleDeath();
        }
        else
        {
            HandleDamage(hitDirection);
        }
    }

    private void HandleDamage(Vector2 hitDirection)
    {
        OnDamaged?.Invoke(hitDirection);
        StartCoroutine(InvulnerabilityCoroutine());
    }

    private void HandleDeath()
    {
        OnDeath?.Invoke();
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(_stats.ImmunityTime);
        _isInvulnerable = false;
    }

    public void Heal(int healAmount)
    {
        if (_currentHealth <= 0)
            return;

        _currentHealth += healAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _stats.MaxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _stats.MaxHealth);
    }
}
