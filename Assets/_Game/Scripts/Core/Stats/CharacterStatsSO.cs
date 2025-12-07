using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Centia/Stats/CharacterStats")]
public class CharacterStatsSO : ScriptableObject
{
    [Header("Health Config")]
    [Tooltip("Lượng máu tối đa")]
    public int MaxHealth = 5;

    public float ImmunityTime = 1.5f;

    [Header("Stamina Config")]
    public float MaxStamina = 100f;
    public float StaminaRegenRate = 10f;
    public float StaminaRegenDelay = 2f;
    public float DashStaminaCost = 15f;
}
