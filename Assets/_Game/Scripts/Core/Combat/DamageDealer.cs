using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDealDamage(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDealDamage(collision.gameObject);
    }

    private void TryDealDamage(GameObject target)
    {
        if (!target.CompareTag("Player")) return;

        if (target.TryGetComponent(out IDamageable damageable))
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            if (Mathf.Abs(direction.x) < 0.5f)
            {
                direction.x = target.transform.position.x > transform.position.x ? 1f : -1f;
            }
            if (direction.y < 0.2f) direction.y = 0.2f;
            damageable.TakeDamage(_damageAmount, direction.normalized * _knockbackForce);
        }
    }
}