using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class AbilityInstanceUI : MonoBehaviour
{
    [Header("Ability 实例")] 
    public Ability_Scriptable ability_instance;
    
    [Header("UI元素")]
    [SerializeField] private TMP_Text _Label;
    [SerializeField] private TMP_Text _Level;
    [SerializeField] private TMP_Text _Exp;
    [SerializeField] private Slider _Progress;


    private void Start()
    {
        BindAbility(ability_instance.Ability_Id, _Label, _Level, _Exp, _Progress);
    }

    private void BindAbility(string abilityId, TMP_Text labelText, TMP_Text levelText, TMP_Text expText, Slider progress)
    {
        var ability = GameManager.GM.Player_Ability[abilityId];

        _Label.text = ability.Ability_Label;
        
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