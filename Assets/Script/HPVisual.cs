using UnityEngine;
using UnityEngine.UI;

public class HPVisual : MonoBehaviour
{
    public Image hpImage;

    public Sprite state1Sprite;
    public Sprite state2Sprite;
    public Sprite state3Sprite;
    public Sprite state4Sprite;
    public Sprite state5Sprite;

    public void UpdateHPVisual(int currentHP, int maxHP)
    {
        if (currentHP <= 0)
        {
            hpImage.sprite = state5Sprite;
            return;
        }

        float ratio = (float)currentHP / maxHP;

        if (ratio >= 0.8f)
        {
            hpImage.sprite = state1Sprite;
        }
        else if (ratio >= 0.6f)
        {
            hpImage.sprite = state2Sprite;
        }
        else if (ratio >= 0.4f)
        {
            hpImage.sprite = state3Sprite;
        }
        else
        {
            hpImage.sprite = state4Sprite;
        }
    }
}