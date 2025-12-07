using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Cài đặt Sát thương")]
    public int damageAmount = 1; 
    public float timeToTrigger = 1.0f; // Thời gian giãn cách giữa các lần mất máu
    public bool isContinuous = true;   // Nếu tắt cái này, bẫy chỉ gây sát thương 1 lần lúc chạm

    private float timer;
    private bool isPlayerInside = false;
    private DamageDealer playerDamageable; 

    void Start()
    {
        // Khởi tạo timer
        timer = timeToTrigger;
    }

    void Update()
    {
        // Nếu nhân vật đang đứng trong bẫy VÀ bẫy này là loại gây sát thương liên tục
        if (isPlayerInside && isContinuous)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                ApplyDamageToPlayer();
                // Reset timer để chờ đợt sát thương kế tiếp
                timer = timeToTrigger;
            }
        }
    }

    void ApplyDamageToPlayer()
    {
        if (playerDamageable != null)
        {
            playerDamageable.TakeDamage(damageAmount);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerDamageable = collision.GetComponent<DamageDealer>();

            // --- THAY ĐỔI Ở ĐÂY ---
            // 1. Gây sát thương NGAY LẬP TỨC khi vừa bước vào
            ApplyDamageToPlayer();
            
            // 2. Reset timer ngay để bắt đầu đếm ngược cho lần sát thương sau
            timer = timeToTrigger; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerDamageable = null;
            
            // Khi đi ra, timer sẽ giữ nguyên giá trị (hoặc bạn có thể reset ở đây cũng được)
            // Nhưng quan trọng là ở OnTriggerEnter chúng ta đã có lệnh reset timer rồi.
        }
    }
}