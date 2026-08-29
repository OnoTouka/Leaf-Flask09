using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SearchButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public enum ButtonType
    {
        GetItem,
        SearchBackground
    }


    [Header("ボタンの種類")]
    [SerializeField]
    private ButtonType buttonType;


    [Header("文字の大きさ")]
    [SerializeField]
    private float hoverScale = 1.1f;

    [SerializeField]
    private float pressScale = 0.9f;


    [Header("SP")]
    [SerializeField]
    private int actionCost = 1;


    [Header("獲得アイテム表示")]
    [SerializeField]
    private TMP_Text itemText;

    [SerializeField]
    [TextArea(2, 5)]
    private string defaultText = "探索する";

    [SerializeField]
    private float itemDisplayTime = 2f;


    [Header("背景処理の確率")]
    [Range(0f, 100f)]
    [SerializeField]
    private float backgroundScaleProbability = 70f;


    [Header("背景")]
    [SerializeField]
    private Transform background;

    [SerializeField]
    private float backgroundScale = 1.1f;

    [SerializeField]
    private float scaleTime = 0.3f;


    [Header("シーン")]
    [SerializeField]
    private string nextSceneName;


    private Vector3 originalScale;

    private Coroutine textCoroutine;


    private void Start()
    {
        originalScale =
            transform.localScale;
    }

    // Hover
    public void OnPointerEnter(
        PointerEventData eventData)
    {
        transform.localScale =
            originalScale * hoverScale;
    }


    public void OnPointerExit(
        PointerEventData eventData)
    {
        transform.localScale =
            originalScale;
    }

    // Down
    public void OnPointerDown(
        PointerEventData eventData)
    {
        transform.localScale =
            originalScale * pressScale;
    }

    // Up
    public void OnPointerUp(
        PointerEventData eventData)
    {
        transform.localScale =
            originalScale;


        // SPを消費
        if (!UseActionPoint())
        {
            return;
        }


        switch (buttonType)
        {
            case ButtonType.GetItem:

                GetRandomItem();

                break;


            case ButtonType.SearchBackground:

                SearchBackground();

                break;
        }
    }

    // SP消費
    private bool UseActionPoint()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "GameManagerが見つかりません"
            );

            return false;
        }


        // SPが足りない
        if (GameManager.Instance.currentSP <
            actionCost)
        {
            Debug.Log(
                "SPが足りません"
            );

            return false;
        }


        GameManager.Instance.SubtractSP(
            actionCost
        );


        Debug.Log(
            "SPを " +
            actionCost +
            " 消費しました。" +
            " 現在のSP：" +
            GameManager.Instance.currentSP
        );


        // SPが0になった
        if (GameManager.Instance.currentSP <= 0)
        {
            SceneManager.LoadScene(
                "ResultScene"
            );

            return false;
        }


        return true;
    }

    // アイテム獲得
    private void GetRandomItem()
    {
        ItemList itemList =
            ItemList.GetInstance();


        int amount;


        ItemData item =
            itemList.GetRandomItem(
                out amount
            );


        if (item != null)
        {
            Debug.Log(
                item.itemName +
                " ×" +
                amount +
                " を獲得しました"
            );


            ShowObtainedItem(
                item,
                amount
            );
        }
    }


    private void ShowObtainedItem(
        ItemData item,
        int amount)
    {
        if (itemText == null)
        {
            Debug.LogWarning(
                "Item Textが設定されていません"
            );

            return;
        }


        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }


        itemText.text =
            item.itemName +
            " ×" +
            amount +
            "を獲得しました！";


        textCoroutine =
            StartCoroutine(
                ResetItemText()
            );
    }


    private IEnumerator ResetItemText()
    {
        yield return new WaitForSeconds(
            itemDisplayTime
        );


        itemText.text =
            defaultText;


        textCoroutine = null;
    }

    // 背景探索
    private void SearchBackground()
    {
        float random =
            Random.Range(
                0f,
                100f
            );


        if (random <
            backgroundScaleProbability)
        {
            StartCoroutine(
                ScaleBackground()
            );
        }
        else
        {
            SceneManager.LoadScene(
                nextSceneName
            );
        }
    }


    private IEnumerator ScaleBackground()
    {
        if (background == null)
        {
            Debug.LogWarning(
                "Backgroundが設定されていません"
            );

            yield break;
        }


        Vector3 originalBackgroundScale =
            background.localScale;


        Vector3 bigScale =
            originalBackgroundScale *
            backgroundScale;


        float time = 0f;


        while (time < scaleTime)
        {
            time += Time.deltaTime;


            float t =
                time / scaleTime;


            background.localScale =
                Vector3.Lerp(
                    originalBackgroundScale,
                    bigScale,
                    t
                );


            yield return null;
        }


        time = 0f;


        while (time < scaleTime)
        {
            time += Time.deltaTime;


            float t =
                time / scaleTime;


            background.localScale =
                Vector3.Lerp(
                    bigScale,
                    originalBackgroundScale,
                    t
                );


            yield return null;
        }


        background.localScale =
            originalBackgroundScale;
    }
}