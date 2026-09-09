using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Enemy Battle HP")]
    public int maxEnemyHP = 100;
    public int currentEnemyHP;

    [Header("Result Message")]
    public GameObject victoryMessage;
    public GameObject defeatMessage;

    public float resultMessageTime = 2f;

    private bool battleEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentEnemyHP = maxEnemyHP;

        GameManager.Instance.StartBattle();

        victoryMessage.SetActive(false);
        defeatMessage.SetActive(false);
    }

    private void Update()
    {
        // 開発用・裏コマンド：Shift + W
        if (Keyboard.current.shiftKey.isPressed &&
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            SkipBattle();
        }
    }

    private void SkipBattle()
    {
        if (battleEnded)
            return;

        Debug.Log("裏コマンド：バトルをスキップしました");

        BattleVictory();
    }

    public void DamageEnemy(int damage)
    {
        if (battleEnded)
            return;

        currentEnemyHP -= damage;

        if (currentEnemyHP < 0)
            currentEnemyHP = 0;

        if (currentEnemyHP <= 0)
            BattleVictory();
    }

    public void DamagePlayer(int damage)
    {
        if (battleEnded)
            return;

        GameManager.Instance.TakeDamage(damage);

        if (GameManager.Instance.currentHP <= 0)
            BattleDefeat();
    }

    private void BattleVictory()
    {
        if (battleEnded)
            return;

        battleEnded = true;

        StopEnemySpawner();

        StartCoroutine(VictorySequence());
    }

    private void BattleDefeat()
    {
        if (battleEnded)
            return;

        battleEnded = true;

        if (GameManager.Instance != null)
            GameManager.Instance.SetBattleDefeat();

        StopEnemySpawner();

        StartCoroutine(DefeatSequence());
    }

    private IEnumerator VictorySequence()
    {
        victoryMessage.SetActive(true);

        yield return new WaitForSeconds(resultMessageTime);

        if (GameManager.Instance.battleStartSP <= 0)
        {
            SceneManager.LoadScene("ResultScene");
        }
        else
        {
            SceneManager.LoadScene("SearchScene");
        }
    }

    private IEnumerator DefeatSequence()
    {
        defeatMessage.SetActive(true);

        yield return new WaitForSeconds(resultMessageTime);

        SceneManager.LoadScene("ResultScene");
    }

    private void StopEnemySpawner()
    {
        EnemySpawner spawner =
            FindFirstObjectByType<EnemySpawner>();

        if (spawner != null)
        {
            spawner.StopSpawning();
        }
    }
}