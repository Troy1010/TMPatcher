using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace TMPatcher;

public class Settings
{
    [MaintainOrder]

    [SettingName("FeatureGuaranteeOneIngredientEffect")]
    [Tooltip("Some UI mods with CTD if you open alchemy menu while an ingredient has 0 effects. This makes sure that doesn't occur.")]
    public bool FeatureGuaranteeOneIngredientEffect = true;
}