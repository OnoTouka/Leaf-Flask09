using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Movement Area")]
    public float minX = -5f;
    public float maxX = 5f;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 position = transform.position;

        position.x += horizontal * moveSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, minX, maxX);

        transform.position = position;
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                Quaternion.identity
            );
        }
    }
}
