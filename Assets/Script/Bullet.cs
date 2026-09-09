using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    public float destroyY = 2f;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y >= destroyY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            // 敵に弾が当たったSE
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySE("Hit",1.2f);
            }

            enemy.HitByBullet();

            Destroy(gameObject);
        }
    }
}