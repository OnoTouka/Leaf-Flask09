using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("移動")]
    [SerializeField]
    private float moveSpeed = 5f;

    [Header("移動範囲")]
    [SerializeField]
    private float minX = -5f;

    [SerializeField]
    private float maxX = 5f;

    [Header("弾")]
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletSpawnPoint;


    private void Update()
    {
        Move();
        Shoot();
    }


    // =========================
    // プレイヤー移動
    // =========================

    private void Move()
    {
        float horizontal = 0f;


        // A / D
        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1f;
        }


        // ← / →
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
        }


        Vector3 position =
            transform.position;


        position.x +=
            horizontal *
            moveSpeed *
            Time.deltaTime;


        // 移動範囲を制限
        position.x =
            Mathf.Clamp(
                position.x,
                minX,
                maxX
            );


        transform.position =
            position;
    }


    // =========================
    // 弾を撃つ
    // =========================

    private void Shoot()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (bulletPrefab == null)
            {
                Debug.LogWarning(
                    "Bullet Prefabが設定されていません"
                );

                return;
            }


            if (bulletSpawnPoint == null)
            {
                Debug.LogWarning(
                    "Bullet Spawn Pointが設定されていません"
                );

                return;
            }


            Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                Quaternion.identity
            );
        }
    }
}