using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    [SerializeField] private string bgmID = "Title";

    private void Start()
    {
        Debug.Log("SceneBGM Start : " + bgmID);

        if (AudioManager.Instance != null)
        {
            Debug.Log("BGMを再生します : " + bgmID);
            AudioManager.Instance.PlayBGM(bgmID);
        }
        else
        {
            Debug.LogError("AudioManager.Instance がありません");
        }
    }
}