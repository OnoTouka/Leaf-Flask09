using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Player SP")]
    public int maxSP = 10;
    public int currentSP;

    // バトル開始時のSP
    public int battleStartSP;

    [Header("探索")]
    public int explorationDepth = 0;

    [Header("今回のゲームで獲得したアイテム")]
    public List<string> obtainedItems = new List<string>();

    [Header("ResultScene用")]
    // バトルからResultSceneへ来たか
    public bool fromBattle = false;

    // バトルで敗北したか
    public bool battleDefeat = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            currentHP = maxHP;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // ========================================
    // 新しいゲームを開始
    // ========================================
    public void StartGame()
    {
        // HPを最大にする
        currentHP = maxHP;

        // SPを最大にする
        currentSP = maxSP;

        explorationDepth = 0;

        // 前回のゲームで獲得したアイテムを消す
        obtainedItems.Clear();

        // ResultScene用の情報をリセット
        battleStartSP = 0;
        fromBattle = false;
        battleDefeat = false;
    }


    // ========================================
    // HPを減らす
    // ========================================
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }
    }


    // ========================================
    // HPを最大値に戻す
    // ========================================
    public void ResetHP()
    {
        currentHP = maxHP;
    }


    // ========================================
    // SPを増やす
    // ========================================
    public void AddSP(int amount)
    {
        currentSP += amount;

        currentSP = Mathf.Clamp(
            currentSP,
            0,
            maxSP
        );
    }


    // ========================================
    // SPを減らす
    // ========================================
    public void SubtractSP(int amount)
    {
        currentSP -= amount;

        currentSP = Mathf.Clamp(
            currentSP,
            0,
            maxSP
        );
    }


    // ========================================
    // SPを最大値に戻す
    // ========================================
    public void ResetSP()
    {
        currentSP = maxSP;
    }


    // ========================================
    // バトル開始時の処理
    // ========================================
    public void StartBattle()
    {
        // バトル開始時のSPを記録
        battleStartSP = currentSP;

        // バトルから来たことを記録
        fromBattle = true;

        // 敗北状態をリセット
        battleDefeat = false;

        // ★ここではアイテムを消さない
        // バトル前のSearchSceneで取ったアイテムも
        // ResultSceneまで残すため
    }


    // ========================================
    // アイテムを獲得
    // ========================================
    public void AddItem(string itemName)
    {
        obtainedItems.Add(itemName);
    }


    // ========================================
    // バトル敗北を記録
    // ========================================
    public void SetBattleDefeat()
    {
        battleDefeat = true;
    }
}