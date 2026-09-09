using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MapSceneManeger : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] private Transform _root;

    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float pressScale = 1.1f;

    [SerializeField] private AudioClip onClickSeAudioClip;
    [SerializeField] private AudioSource audioSource;

    // 文章表示用
    [SerializeField] private TextMeshProUGUI descriptionText;

    // カーソルが乗っていないときの文章
    [SerializeField]
    [TextArea(2, 5)]
    private string defaultDescription = "行きたい場所を選択してください";
    [SerializeField]
    [TextArea]
    private string incapable = "まだ行けない";

    // このボタンにカーソルを乗せたときの文章
    [SerializeField]
    [TextArea(2, 5)]
    private string description;

    public enum ButtonAction
    {
        None,
        SceneChange,
        OpenDialog,
        CloseDialog
    }

    [SerializeField] private ButtonAction buttonAction;

    [SerializeField] private string sceneName;

    private Vector3 defaultScale;
    private bool isHover = false;

    private void Start()
    {
        defaultScale = _root.localScale;

        // 最初は通常時の文章を表示
        if (descriptionText != null)
        {
            descriptionText.text = defaultDescription;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;

        _root.localScale = defaultScale * hoverScale;

        // 専用の文章を表示
        if (descriptionText != null)
        {
            descriptionText.text = description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;

        _root.localScale = defaultScale;

        // カーソルが離れたら通常時の文章に戻す
        if (descriptionText != null)
        {
            descriptionText.text = defaultDescription;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _root.localScale = defaultScale * pressScale;

        // クリックSE
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySE("Select");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _root.localScale = isHover
            ? defaultScale * hoverScale
            : defaultScale;


        switch (buttonAction)
        {
            case ButtonAction.SceneChange:
                SceneManager.LoadScene(sceneName);
                break;

            case ButtonAction.None:
                if (descriptionText != null)
                {
                    descriptionText.text = incapable;
                }
                break;
        }
    }
}