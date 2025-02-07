using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private TMP_Text _staminaText;

    private void Start()
    {
        GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Stamina)
            .CombineLatest(
                GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Stamina_Max),
                (current, max) => new { current, max })
            .Subscribe(data => {
                _staminaSlider.maxValue = data.max;
                _staminaSlider.value = data.current;
                _staminaText.text = $"{data.current:F0} / {data.max:F0}";
            })
            .AddTo(this);
    }
}