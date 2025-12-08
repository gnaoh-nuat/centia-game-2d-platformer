using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameHUD : MonoBehaviour
{
    private PlayerController _player;

    [Header("UI Elements")]
    [SerializeField] private Image _staminaFillImage;
    [SerializeField] private Transform _heartContainer;
    [SerializeField] private GameObject _heartPrefab;

    private List<GameObject> _hearts = new List<GameObject>();
    public void InitializeHUD(PlayerController player)
    {
        if (_player != null)
        {
            _player.Health.OnHealthChanged -= UpdateHealthUI;
            _player.Stamina.OnStaminaChanged -= UpdateStaminaUI;
        }

        _player = player;

        if (_player != null)
        {
            InitializeHearts(_player.Health.MaxHealth);
            UpdateHealthUI(_player.Health.CurrentHealth, _player.Health.MaxHealth);
            UpdateStaminaUI(_player.Stamina.CurrentStamina, _player.Stats.MaxStamina);

            _player.Health.OnHealthChanged += UpdateHealthUI;
            _player.Stamina.OnStaminaChanged += UpdateStaminaUI;
        }
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.Health.OnHealthChanged -= UpdateHealthUI;
            _player.Stamina.OnStaminaChanged -= UpdateStaminaUI;
        }
    }

    private void InitializeHearts(int maxHealth)
    {
        foreach (Transform child in _heartContainer) Destroy(child.gameObject);
        _hearts.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(_heartPrefab, _heartContainer);
            _hearts.Add(newHeart);
        }
    }

    private void UpdateHealthUI(int current, int max)
    {
        if (_hearts.Count != max) InitializeHearts(max);

        for (int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].SetActive(i < current);
        }
    }

    private void UpdateStaminaUI(float current, float max)
    {
        if (_staminaFillImage == null) return;

        float ratio = current / max;

        _staminaFillImage.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}