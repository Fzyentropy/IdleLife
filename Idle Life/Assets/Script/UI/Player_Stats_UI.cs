using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Stats_UI : MonoBehaviour
{
    [Space(5)][Header("金钱")]
    [SerializeField] private TMP_Text _moneyText;       // 金钱
    [Space(5)][Header("体力值")]
    [SerializeField] private Slider _staminaSlider;     // 体力值 进度条
    [SerializeField] private TMP_Text _staminaText;     // 体力值 text
    [Space(5)][Header("饱腹值")]
    [SerializeField] private Slider _satietySlider;     // 饱腹值 进度条
    [SerializeField] private TMP_Text _satietyText;     // 饱腹值 text
    [Space(3)]
    [SerializeField] private Image _satietyFill;        // 饱腹值 进度条 Fill
    [SerializeField] private Color _satietyFullColor;   // 饱腹值 进度条 饱腹时颜色（亮橙）
    [SerializeField] private Color _satietyHungryColor; // 饱腹值 进度条 饥饿时颜色（暗橙）

    private void Start()
    {
        Start_Monitor_Stamina();
        Start_Monitor_Money();
        Start_Monitor_Satiety();
    }


    void Start_Monitor_Stamina()        // 体力值
    {
        GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Stamina)
            .CombineLatest(
                GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Stamina_Max),
                (current, max) => new { current, max })
            .Subscribe(stamina => {
                _staminaSlider.maxValue = stamina.max;
                _staminaSlider.value = stamina.current;
                _staminaText.text = $"{stamina.current:F0} / {stamina.max:F0}";
            })
            .AddTo(this);
    }

    void Start_Monitor_Money()      // 金钱
    {
        GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Money)
            .Subscribe(money => {
                _moneyText.text = $"{(int)money}";
            })
            .AddTo(this);
    }

    void Start_Monitor_Satiety()        // 饱腹值
    {
        GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Satiety)
            .CombineLatest(
                GameManager.GM.ObserveEveryValueChanged(gm => gm.Player_Satiety_Max),
                (current, max) => new { current, max })
            .Subscribe(satiety => {
                
                // 设置进度条
                _satietySlider.maxValue = satiety.max / 2;
                _satietySlider.value = satiety.current / 2;
                
                // 设置文本显示
                _satietyText.text = $"{satiety.current:F1} / {satiety.max:F0}";

                // 设置进度条颜色
                if (satiety.current < satiety.max / 2)
                    _satietyFill.color = _satietyHungryColor;
                else
                    _satietyFill.color = _satietyFullColor;
            })
            .AddTo(this);
    }
}