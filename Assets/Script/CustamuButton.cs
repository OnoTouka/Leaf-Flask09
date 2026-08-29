using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustamuButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("ボタンのRoot")]
    [SerializeField]
    private Transform _root;

    [Header("押したときに消すFrame")]
    [SerializeField]
    private GameObject _FreamePrefub;


    [Header("ボタンアニメーション")]
    [SerializeField]
    private float hoverScale = 1.2f;

    [SerializeField]
    private float pressScale = 1.1f;


    [Header("サウンド")]
    [SerializeField]
    private AudioClip onClickSeAudioClip;

    [SerializeField]
    private AudioSource audioSource;

    // ボタンの動作
    public enum ButtonAction
    {
        None,
        SceneChange,
        StartGame,
        OpenDialog,
        CloseDialog
    }

    [SerializeField]
    private ButtonAction buttonAction;

    // シーン移動
    [SerializeField]
    private string sceneName;

    // ダイアログ
    [SerializeField]
    private GameObject dialog;

    [Header("ダイアログアニメーション")]
    [SerializeField]
    private float dialogStartScale = 0.1f;

    [SerializeField]
    private float dialogAnimationTime = 0.2f;

    private Vector3 defaultScale;

    private bool isHover;

    private Coroutine dialogCoroutine;

    // Start
    private void Start()
    {
        if (_root != null)
        {
            defaultScale =
                _root.localScale;
        }

        // OpenDialogボタンの場合
        // ダイアログだけ最初非表示
        if (buttonAction ==
            ButtonAction.OpenDialog)
        {
            if (dialog != null)
            {
                dialog.SetActive(false);

                dialog.transform.localScale =
                    Vector3.one;
            }
        }
    }

    // Hover
    public void OnPointerEnter(
        PointerEventData eventData)
    {
        isHover = true;


        if (_root != null)
        {
            _root.localScale =
                defaultScale * hoverScale;
        }
    }

    public void OnPointerExit(
        PointerEventData eventData)
    {
        isHover = false;


        if (_root != null)
        {
            _root.localScale =
                defaultScale;
        }
    }

    // Down
    public void OnPointerDown(
        PointerEventData eventData)
    {
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

    // Up
    public void OnPointerUp(
        PointerEventData eventData)
    {
        if (_root != null)
        {
            _root.localScale =
                isHover
                ? defaultScale * hoverScale
                : defaultScale;
        }

        if (_FreamePrefub != null)
        {
            _FreamePrefub.SetActive(true);
        }

        // サウンド
        if (audioSource != null &&
            onClickSeAudioClip != null)
        {
            audioSource.PlayOneShot(
                onClickSeAudioClip
            );
        }

        // ボタン処理
        switch (buttonAction)
        {
            case ButtonAction.SceneChange:

                SceneManager.LoadScene(
                    sceneName
                );

                break;

            case ButtonAction.StartGame:

                GameManager.Instance.ResetHP();

                SceneManager.LoadScene(
                    sceneName
                );

                break;

            case ButtonAction.OpenDialog:

                OpenDialog();

                break;

            case ButtonAction.CloseDialog:

                CloseDialog();

                break;

            case ButtonAction.None:

                break;
        }
    }

    // ダイアログを開く
    private void OpenDialog()
    {
        if (dialog == null)
        {
            Debug.LogWarning(
                "Dialogが設定されていません"
            );

            return;
        }

        // 現在のアニメーションを停止
        if (dialogCoroutine != null)
        {
            StopCoroutine(
                dialogCoroutine
            );

            dialogCoroutine = null;
        }

        // ダイアログを表示
        dialog.SetActive(true);

        // 必ず小さい状態から開始
        dialog.transform.localScale =
            Vector3.one *
            dialogStartScale;

        // 開くアニメーション
        dialogCoroutine =
            StartCoroutine(
                OpenDialogAnimation()
            );
    }

    // 開くアニメーション
    private IEnumerator OpenDialogAnimation()
    {
        Transform target =
            dialog.transform;

        Vector3 startScale =
            Vector3.one *
            dialogStartScale;

        Vector3 endScale =
            Vector3.one;

        float time = 0f;

        while (time <
               dialogAnimationTime)
        {
            time += Time.deltaTime;

            float t =
                time /
                dialogAnimationTime;

            t =
                Mathf.SmoothStep(
                    0f,
                    1f,
                    t
                );

            target.localScale =
                Vector3.Lerp(
                    startScale,
                    endScale,
                    t
                );

            yield return null;
        }

        // 最終状態
        target.localScale =
            Vector3.one;

        dialogCoroutine = null;
    }

    // ダイアログを閉じる
    private void CloseDialog()
    {
        if (dialog == null)
        {
            return;
        }

        // 現在のアニメーションを停止
        if (dialogCoroutine != null)
        {
            StopCoroutine(
                dialogCoroutine
            );

            dialogCoroutine = null;
        }

        // 閉じるアニメーション
        dialogCoroutine =
            StartCoroutine(
                CloseDialogAnimation()
            );
    }

    // 閉じるアニメーション
    private IEnumerator CloseDialogAnimation()
    {
        Transform target =
            dialog.transform;

        Vector3 startScale =
            target.localScale;

        Vector3 endScale =
            Vector3.one *
            dialogStartScale;

        float time = 0f;

        while (time <
               dialogAnimationTime)
        {
            time += Time.deltaTime;

            float t =
                time /
                dialogAnimationTime;

            t =
                Mathf.SmoothStep(
                    0f,
                    1f,
                    t
                );

            target.localScale =
                Vector3.Lerp(
                    startScale,
                    endScale,
                    t
                );

            yield return null;
        }

        // 完全に小さくする
        target.localScale =
            endScale;

        // 非表示
        dialog.SetActive(false);

        // 次回開くために元のサイズへ戻す
        target.localScale =
            Vector3.one;

        dialogCoroutine = null;
    }
}