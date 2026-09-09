using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioVolumeSlider :
    MonoBehaviour,
    IPointerUpHandler
{
    [SerializeField] private Slider slider;
    [SerializeField] private bool isBGM = true;

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (AudioManager.Instance == null)
            return;

        if (isBGM)
        {
            slider.value =
                AudioManager.Instance.GetBGMVolume();

            slider.onValueChanged.AddListener(
                OnBGMVolumeChanged
            );
        }
        else
        {
            slider.value =
                AudioManager.Instance.GetSEVolume();

            slider.onValueChanged.AddListener(
                OnSEVolumeChanged
            );
        }
    }

    private void OnBGMVolumeChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    private void OnSEVolumeChanged(float value)
    {
        AudioManager.Instance.SetSEVolume(value);
    }

    public void OnPointerUp(
        PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySE("Select");
        }
    }

    private void OnDestroy()
    {
        if (slider == null)
            return;

        slider.onValueChanged.RemoveListener(
            OnBGMVolumeChanged
        );

        slider.onValueChanged.RemoveListener(
            OnSEVolumeChanged
        );
    }
}