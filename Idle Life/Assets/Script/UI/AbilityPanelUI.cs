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
    
    [Header("图片编辑UI")]
    [SerializeField] private TMP_Text _imageCraftingLevel;
    [SerializeField] private TMP_Text _imageCraftingExp;
    [SerializeField] private Slider _imageCraftingProgress;
    
    [Header("审美UI")]
    [SerializeField] private TMP_Text _aestheticLevel;
    [SerializeField] private TMP_Text _aestheticExp;
    [SerializeField] private Slider _aestheticProgress;
    
    [Header("通用计算机UI")]
    [SerializeField] private TMP_Text _generalComputerLevel;
    [SerializeField] private TMP_Text _generalComputerExp;
    [SerializeField] private Slider _generalComputerProgress;
    
    [Header("绘画UI")]
    [SerializeField] private TMP_Text _drawingLevel;
    [SerializeField] private TMP_Text _drawingExp;
    [SerializeField] private Slider _drawingProgress;

    private void Start()
    {
        BindAbility("Ability_Math", _mathLevel, _mathExp, _mathProgress);
        BindAbility("Ability_Physics", _physicsLevel, _physicsExp, _physicsProgress);
        BindAbility("Ability_ImageCrafting", _imageCraftingLevel, _imageCraftingExp, _imageCraftingProgress);
        BindAbility("Ability_Aesthetic", _aestheticLevel, _aestheticExp, _aestheticProgress);
        BindAbility("Ability_GeneralComputer", _generalComputerLevel, _generalComputerExp, _generalComputerProgress);
        BindAbility("Ability_Drawing", _drawingLevel, _drawingExp, _drawingProgress);
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