using UnityEngine;
using System.Collections;

public class DisappearPlatform : MonoBehaviour
{
    public float timeToDisappear = 3.0f;      // Thời gian tàng hình hoàn toàn
    [Header("Thời gian nhấp nháy trước khi biến mất")]
    public float timeToBlink = 2.0f;          // Thời gian nhấp nháy trước khi mất
    public float blinkSpeedDisappear = 0.1f; // Tốc độ nháy lúc sắp mất (nên để nhanh, ví dụ 0.1)


    [Header("Thời gian nhấp nháy trước khi xuất hiện lại")]
    public float timeToRespawnWarning = 1.5f;
    public float blinkSpeedRespawn = 0.2f;   // Tốc độ nháy lúc sắp hiện (nên để chậm hơn, ví dụ 0.2)
        

    private SpriteRenderer[] childSprites; 
    private Collider2D parentCollider;
    private bool isProcessing = false;

    void Start()
    {
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        parentCollider = GetComponent<Collider2D>();

        if (parentCollider == null)
        {
            Debug.LogError("Bạn quên gắn Box Collider 2D cho vật thể Cha rồi!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isProcessing)
        {
            StartCoroutine(DisappearRoutine());
        }
    }

    void SetSpritesEnabled(bool isEnabled)
    {
        foreach (var sprite in childSprites)
        {
            sprite.enabled = isEnabled;
        }
    }

    IEnumerator DisappearRoutine()
    {
        isProcessing = true;

        // --- GIAI ĐOẠN 1: NHẤP NHÁY CẢNH BÁO MẤT ---
        float timer = 0;
        while (timer < timeToBlink)
        {
            foreach (var sprite in childSprites)
            {
                sprite.enabled = !sprite.enabled;
            }
            
            // THAY ĐỔI 2: Sử dụng tốc độ blinkSpeedDisappear
            yield return new WaitForSeconds(blinkSpeedDisappear);
            timer += blinkSpeedDisappear;
        }

        SetSpritesEnabled(true);

        // --- GIAI ĐOẠN 2: BIẾN MẤT ---
        SetSpritesEnabled(false); 
        if(parentCollider != null) parentCollider.enabled = false; 

        yield return new WaitForSeconds(timeToDisappear);

        // --- GIAI ĐOẠN 3: BÓNG MA (Sắp hiện lại) ---
        timer = 0;
        
        // Sử dụng biến timeToRespawnWarning đã đưa ra public
        while (timer < timeToRespawnWarning)
        {
            foreach (var sprite in childSprites)
            {
                sprite.enabled = !sprite.enabled;
            }

            // THAY ĐỔI 3: Sử dụng tốc độ blinkSpeedRespawn
            yield return new WaitForSeconds(blinkSpeedRespawn);
            timer += blinkSpeedRespawn;
        }

        // --- GIAI ĐOẠN 4: KHÔI PHỤC ---
        SetSpritesEnabled(true); 
        if(parentCollider != null) parentCollider.enabled = true; 
        isProcessing = false;
    }
}