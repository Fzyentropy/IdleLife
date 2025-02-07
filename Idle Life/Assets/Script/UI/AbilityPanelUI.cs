using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class AbilityPanelUI : MonoBehaviour
{
    [SerializeField] private Transform _mathPanel;
    [SerializeField] private Transform _physicsPanel;

    [Header("数学能力UI")]
    [SerializeField] private TMP_Text _mathLevel;
    [SerializeField] private TMP_Text _mathExp;
    [SerializeField] private Slider _mathProgress;

    [Header("物理能力UI")]
    [SerializeField] private TMP_Text _physicsLevel;
    [SerializeField] private TMP_Text _physicsExp;
    [SerializeField] private Slider _physicsProgress;

    private void Start()
    {
        BindAbility("Ability_Math", _mathLevel, _mathExp, _mathProgress);
        BindAbility("Ability_Physics", _physicsLevel, _physicsExp, _physicsProgress);
    }

    private void BindAbility(string abilityId, TMP_Text levelText, TMP_Text expText, Slider progress)
    {
        var ability = GameManager.GM.Player_Ability[abilityId];
        
        ability.ObserveEveryValueChanged(a => a.Ability_Level)
            .Subscribe(lv => levelText.text = $"Lv.{lv}")
            .AddTo(this);

        ability.ObserveEveryValueChanged(a => a.Ability_Current_Exp)
            .CombineLatest(
                ability.ObserveEveryValueChanged(a => a.ExpToNextLevel[a.Ability_Level+1]),
                (current, needed) => new { current, needed })
            .Subscribe(data => {
                expText.text = $"{data.current:F0}/{data.needed:F0}";
                progress.value = data.current / data.needed;
            })
            .AddTo(this);
    }
}