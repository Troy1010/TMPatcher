using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace TMPatcher;

public class Settings
{
    [MaintainOrder]

    [SettingName("FeatureGuaranteeOneIngredientEffect")]
    [Tooltip("Some UI mods with CTD if you open alchemy menu while an ingredient has 0 effects. This makes sure that doesn't occur.")]
    public bool FeatureGuaranteeOneIngredientEffect = false;

    [SettingName("FeatureGuaranteeCreatureSpeed")]
    [Tooltip("Some creatures are way too low. This makes sure that all creatures have at least some speed.")]
    public bool FeatureGuaranteeCreatureSpeed = true;

    [SettingName("FeatureAdjustLocks")]
    [Tooltip("I felt like every lock was very easy or easy.")]
    public bool FeatureAdjustLocks = false;

    [SettingName("FeatureAdjustLocks_MinimumLockLevel")]
    [Tooltip("FeatureAdjustLocks - all locks will at least be this at this lock level")]
    public uint FeatureAdjustLocks_MinimumLockLevel = 40;
}