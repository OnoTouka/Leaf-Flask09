using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Battle Damage")]
    public int battleDamage = 10;

    [Header("Player Damage")]
    public int playerDamage = 10;

    private void Update()
    {
        // 下方向へ移動
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    public void HitByBullet()
    {
        // 敵全体HPを減らす
        BattleManager.Instance.DamageEnemy(battleDamage);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BattleArea"))
        {
            // プレイヤーにダメージ
            BattleManager.Instance.DamagePlayer(playerDamage);

            Destroy(gameObject);
        }
    }
}
