using UnityEngine;
using UnityEngine.Events;

public class StaminaSystem : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterStatsSO _stats;

    [Header("Events")]
    public UnityAction<float, float> OnStaminaChanged;
    public UnityAction OnStaminaEmpty;

    [SerializeField] private float _currentStamina;
    private float _timeSinceLastAction;

    public float CurrentStamina => _currentStamina;

    private void Awake()
    {
        if (_stats == null)
        {
            Debug.LogError($"{gameObject.name}: Thiếu CharacterStatsSO trong StaminaSystem!");
            return;
        }
        _currentStamina = _stats.MaxStamina;
    }

    private void Start()
    {
        // Gửi dữ liệu ban đầu cho UI
        OnStaminaChanged?.Invoke(_currentStamina, _stats.MaxStamina);
    }

    private void Update()
    {
        HandleRegeneration();
    }

    private void HandleRegeneration()
    {
        if (_currentStamina >= _stats.MaxStamina) return;

        _timeSinceLastAction += Time.deltaTime;

        if (_timeSinceLastAction >= _stats.StaminaRegenDelay)
        {
            _currentStamina += _stats.StaminaRegenRate * Time.deltaTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, _stats.MaxStamina);

            OnStaminaChanged?.Invoke(_currentStamina, _stats.MaxStamina);
        }
    }
    public bool TryUseStamina(float amount)
    {
        if (_currentStamina >= amount)
        {
            _currentStamina -= amount;

            _timeSinceLastAction = 0f;

            OnStaminaChanged?.Invoke(_currentStamina, _stats.MaxStamina);
            return true; 
        }
        else
        {
            Debug.Log("Không đủ Stamina!");
            OnStaminaEmpty?.Invoke();
            return false; 
        }
    }

    public void RestoreFull()
    {
        _currentStamina = _stats.MaxStamina;
        OnStaminaChanged?.Invoke(_currentStamina, _stats.MaxStamina);
    }
}