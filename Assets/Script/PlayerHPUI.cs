using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    public HPBar hpBar;
    public HPVisual hpVisual;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        int hp = GameManager.Instance.currentHP;
        int maxHP = GameManager.Instance.maxHP;

        hpBar.SetMaxHP(maxHP);
        hpBar.SetHP(hp);

        hpVisual.UpdateHPVisual(hp, maxHP);
    }
}