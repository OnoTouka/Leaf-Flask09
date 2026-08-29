using UnityEngine;

public class EnemyHPUI : MonoBehaviour
{
    public HPBar hpBar;
    public HPVisual hpVisual;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (BattleManager.Instance == null)
            return;

        UpdateUI();
    }

    private void UpdateUI()
    {
        int hp = BattleManager.Instance.currentEnemyHP;
        int maxHP = BattleManager.Instance.maxEnemyHP;

        hpBar.SetMaxHP(maxHP);
        hpBar.SetHP(hp);

        hpVisual.UpdateHPVisual(hp, maxHP);
    }
}