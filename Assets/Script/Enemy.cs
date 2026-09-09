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
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.DamageEnemy(battleDamage);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BattleArea"))
        {
            // 敵がプレイヤーエリアに入ったSE
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySE("Hit",1.2f);
            }

            // プレイヤーにダメージ
            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.DamagePlayer(playerDamage);
            }

            // 敵を削除
            Destroy(gameObject);
        }
    }
}