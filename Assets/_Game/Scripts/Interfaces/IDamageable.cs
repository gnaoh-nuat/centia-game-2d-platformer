using UnityEngine;

public interface IDamageable
{
    // Method to apply damage to the object
    // damageAmount: The amount of damage to apply
    // hitDirection: The direction from which the damage is applied
    void TakeDamage(int damageAmount, Vector2 hitDirection);
}
