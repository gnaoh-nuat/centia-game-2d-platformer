using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Hàm này sẽ được script Bẫy gọi
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Máu còn lại: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Nhân vật đã hy sinh!");
        // Thêm code reset game hoặc ẩn nhân vật ở đây
        gameObject.SetActive(false); 
    }
}