using UnityEngine;
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

    // 敵全体にダメージ
    public void DamageEnemy(int damage)
    {
        if (battleEnded)
            return;

        currentEnemyHP -= damage;

        if (currentEnemyHP < 0)
            currentEnemyHP = 0;

        // 敵HPが0
        if (currentEnemyHP <= 0)
        {
            BattleVictory();
        }
    }

    // プレイヤーがダメージを受けた
    public void DamagePlayer(int damage)
    {
        if (battleEnded)
            return;

        GameManager.Instance.TakeDamage(damage);

        // プレイヤーHPが0
        if (GameManager.Instance.currentHP <= 0)
        {
            BattleDefeat();
        }
    }

    private void BattleVictory()
    {
        if (battleEnded)
            return;

        battleEnded = true;

        StartCoroutine(VictorySequence());
    }

    private void BattleDefeat()
    {
        if (battleEnded)
            return;

        battleEnded = true;

        StartCoroutine(DefeatSequence());
    }

    private IEnumerator VictorySequence()
    {
        victoryMessage.SetActive(true);

        yield return new WaitForSeconds(resultMessageTime);

        // バトル開始時SPが0
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
}