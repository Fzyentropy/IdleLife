using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private TMP_Text _staminaText;
    [SerializeField] private TMP_Text _moneyText;

    private void Start()
    {
        Start_Monitor_Stamina();
        Start_Monitor_Money();
    }


    void Start_Monitor_Stamina()
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

    void Start_Monitor_Money()
    {
        GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Money)
            .Subscribe(data => {
                _moneyText.text = $"{(int)data}";
            })
            .AddTo(this);
    }
}