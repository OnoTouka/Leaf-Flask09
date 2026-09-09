using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class CustamuButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("見た目")]
    [SerializeField]
    private Transform _root;

    [SerializeField]
    private GameObject _FreamePrefub;

    [SerializeField]
    private float hoverScale = 1.2f;

    [SerializeField]
    private float pressScale = 1.1f;


    [Header("サウンド")]
    [SerializeField]
    private string onClickSeID = "Select";


    [Header("ボタンアクション")]
    [SerializeField]
    private ButtonAction buttonAction = ButtonAction.None;

    [SerializeField]
    private string sceneName;


    [Header("ダイアログ")]
    [SerializeField]
    private GameObject dialog;

    [SerializeField]
    private Vector3 dialogStartScale = Vector3.zero;

    [SerializeField]
    private float dialogAnimationTime = 0.2f;


    private Vector3 defaultScale;

    private bool isHover;
    private bool isPressed;

    private Coroutine dialogCoroutine;


    public enum ButtonAction
    {
        None,
        SceneChange,
        StartGame,
        OpenDialog,
        CloseDialog,
        Exit
    }


    private void Start()
    {
        if (_root != null)
        {
            defaultScale = _root.localScale;
        }

        // OpenDialogボタンの場合、
        // 最初はダイアログを非表示にする
        if (buttonAction == ButtonAction.OpenDialog)
        {
            if (dialog != null)
            {
                dialog.SetActive(false);
            }
        }
    }


    // ==============================
    // Hover
    // ==============================

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;

        if (_root != null)
        {
            _root.localScale =
                defaultScale * hoverScale;
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;

        // 押している途中なら、
        // OnPointerUpで処理する
        if (isPressed)
        {
            return;
        }

        if (_root != null)
        {
            _root.localScale =
                defaultScale;
        }
    }


    // ==============================
    // Pointer Down
    // ==============================

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;

        if (_root != null)
        {
            _root.localScale =
                defaultScale * pressScale;
        }

        if (_FreamePrefub != null)
        {
            _FreamePrefub.SetActive(false);
        }
    }


    // ==============================
    // Pointer Up
    // ==============================

    public void OnPointerUp(PointerEventData eventData)
    {
        // 押されていなければ何もしない
        if (!isPressed)
        {
            return;
        }

        isPressed = false;


        // ==================================
        // ボタンの外で離した場合
        // ==================================

        if (!isHover)
        {
            if (_root != null)
            {
                _root.localScale =
                    defaultScale;
            }

            if (_FreamePrefub != null)
            {
                _FreamePrefub.SetActive(true);
            }

            // クリック不成立なので
            // SEもアクションも実行しない
            return;
        }


        // ==================================
        // ボタンの上で離した場合
        // ==================================

        if (_root != null)
        {
            _root.localScale =
                defaultScale * hoverScale;
        }

        if (_FreamePrefub != null)
        {
            _FreamePrefub.SetActive(true);
        }


        // ==================================
        // クリック成立
        // ==================================

        // AudioManagerからSEを再生
        if (AudioManager.Instance != null)
        {
            if (!string.IsNullOrEmpty(onClickSeID))
            {
                AudioManager.Instance.PlaySE(onClickSeID);
            }
        }


        // 元のボタンアクションを実行
        ExecuteButtonAction();
    }


    // ==============================
    // ボタンアクション
    // ==============================

    private void ExecuteButtonAction()
    {
        switch (buttonAction)
        {
            case ButtonAction.None:

                break;


            case ButtonAction.SceneChange:

                if (string.IsNullOrEmpty(sceneName))
                {
                    Debug.LogWarning(
                        "CustamuButton: sceneNameが設定されていません。"
                    );

                    return;
                }

                SceneManager.LoadScene(sceneName);

                break;


            case ButtonAction.StartGame:

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.StartGame();
                }


                if (ItemList.GetInstance() != null)
                {
                    ItemList.GetInstance()
                        .ClearGameObtainedItems();
                }


                if (string.IsNullOrEmpty(sceneName))
                {
                    Debug.LogWarning(
                        "CustamuButton: sceneNameが設定されていません。"
                    );

                    return;
                }

                SceneManager.LoadScene(sceneName);

                break;


            case ButtonAction.OpenDialog:

                OpenDialog();

                break;


            case ButtonAction.CloseDialog:

                CloseDialog();

                break;


            case ButtonAction.Exit:

#if UNITY_EDITOR

                EditorApplication.isPlaying = false;

#else

                Application.Quit();

#endif

                break;
        }
    }


    // ==============================
    // Dialog Open
    // ==============================

    private void OpenDialog()
    {
        if (dialog == null)
        {
            return;
        }


        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine);
        }


        dialog.SetActive(true);

        dialog.transform.localScale =
            dialogStartScale;


        dialogCoroutine =
            StartCoroutine(
                OpenDialogAnimation()
            );
    }


    private IEnumerator OpenDialogAnimation()
    {
        Vector3 startScale =
            dialogStartScale;

        Vector3 endScale =
            Vector3.one;


        float time = 0f;


        while (time < dialogAnimationTime)
        {
            time += Time.deltaTime;


            float t =
                Mathf.Clamp01(
                    time / dialogAnimationTime
                );


            // SmoothStep
            t =
                t * t *
                (3f - 2f * t);


            if (dialog != null)
            {
                dialog.transform.localScale =
                    Vector3.Lerp(
                        startScale,
                        endScale,
                        t
                    );
            }


            yield return null;
        }


        if (dialog != null)
        {
            dialog.transform.localScale =
                endScale;
        }


        dialogCoroutine = null;
    }


    // ==============================
    // Dialog Close
    // ==============================

    private void CloseDialog()
    {
        if (dialog == null)
        {
            return;
        }


        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine);
        }


        dialogCoroutine =
            StartCoroutine(
                CloseDialogAnimation()
            );
    }


    private IEnumerator CloseDialogAnimation()
    {
        Vector3 startScale =
            dialog != null
                ? dialog.transform.localScale
                : Vector3.one;


        Vector3 endScale =
            dialogStartScale;


        float time = 0f;


        while (time < dialogAnimationTime)
        {
            time += Time.deltaTime;


            float t =
                Mathf.Clamp01(
                    time / dialogAnimationTime
                );


            // SmoothStep
            t =
                t * t *
                (3f - 2f * t);


            if (dialog != null)
            {
                dialog.transform.localScale =
                    Vector3.Lerp(
                        startScale,
                        endScale,
                        t
                    );
            }


            yield return null;
        }


        if (dialog != null)
        {
            dialog.transform.localScale =
                endScale;

            dialog.SetActive(false);
        }


        dialogCoroutine = null;
    }
}