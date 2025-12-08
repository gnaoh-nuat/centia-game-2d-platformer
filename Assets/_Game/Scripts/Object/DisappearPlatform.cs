using UnityEngine;
using System.Collections;

public class DisappearPlatform : MonoBehaviour
{
    public float timeToDisappear = 3.0f;
    [Header("Thời gian nhấp nháy trước khi biến mất")]
    public float timeToBlink = 2.0f;
    public float blinkSpeedDisappear = 0.1f;


    [Header("Thời gian nhấp nháy trước khi xuất hiện lại")]
    public float timeToRespawnWarning = 1.5f;
    public float blinkSpeedRespawn = 0.2f;
        

    private SpriteRenderer[] childSprites; 
    private Collider2D parentCollider;
    private bool isProcessing = false;

    [SerializeField] private AudioClip _clickSound;

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
            AudioManager.Instance.PlaySFX(_clickSound);
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

        float timer = 0;
        while (timer < timeToBlink)
        {
            foreach (var sprite in childSprites)
            {
                sprite.enabled = !sprite.enabled;
            }
            yield return new WaitForSeconds(blinkSpeedDisappear);
            timer += blinkSpeedDisappear;
        }

        SetSpritesEnabled(true);

        SetSpritesEnabled(false); 
        if(parentCollider != null) parentCollider.enabled = false; 

        yield return new WaitForSeconds(timeToDisappear);

        timer = 0;
        while (timer < timeToRespawnWarning)
        {
            foreach (var sprite in childSprites)
            {
                sprite.enabled = !sprite.enabled;
            }

            yield return new WaitForSeconds(blinkSpeedRespawn);
            timer += blinkSpeedRespawn;
        }

        SetSpritesEnabled(true); 
        if(parentCollider != null) parentCollider.enabled = true; 
        isProcessing = false;
    }
}