using Mutagen.Bethesda;
using Mutagen.Bethesda.Oblivion;
using Mutagen.Bethesda.Plugins.Exceptions;
using Mutagen.Bethesda.Synthesis;

namespace TMPatcher
{
    public class Program
    {
        private static Lazy<Settings> _settings = null!;
        private static Settings Settings => _settings.Value;

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<IOblivionMod, IOblivionModGetter>(RunPatch)
                .SetAutogeneratedSettings("Settings", "settings.json", out _settings)
                .SetTypicalOpen(GameRelease.Oblivion, "TMPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<IOblivionMod, IOblivionModGetter> state)
        {
            Console.WriteLine("\n\nRunPatch`Open\n");
            // FeatureGuaranteeCreatureSpeed
            if (Settings.FeatureGuaranteeCreatureSpeed)
            {
                foreach (var oldCreature in state.LoadOrder.PriorityOrder.WinningOverrides<ICreatureGetter>())
                {
                    try
                    {
                        if (oldCreature.EditorID != null && oldCreature.EditorID!.StartsWith("Test"))
                        {
                            Console.WriteLine($"Skipping oldCreature because EditorID starts with Test. EditorID:{oldCreature.EditorID} Name:{oldCreature.Name}");
                            continue;
                        }
                        
                        if (oldCreature.Data == null)
                        {
                            Console.WriteLine($"Skipping oldCreature oldCreature.Data was null. EditorID:{oldCreature.EditorID} Name:{oldCreature.Name}");
                            continue;
                        }

                        if (oldCreature.Data?.Speed > 30)
                        {
                            Console.WriteLine($"Skipping oldCreature because Speed > 30. EditorID:{oldCreature.EditorID} Name:{oldCreature.Name}");
                            continue;
                        }

                        var newCreature = oldCreature.DeepCopy();
                        newCreature.Data!.Speed = 30;
                        state.PatchMod.Creatures.Set(newCreature);
                        Console.WriteLine($"Modified creature. EditorID:{newCreature.EditorID} Name:{newCreature.Name}");
                    }
                    catch (Exception ex)
                    {
                        throw RecordException.Enrich(ex, oldCreature);
                    }
                }
            }
            // FeatureGuaranteeOneIngredientEffect
            if (Settings.FeatureGuaranteeOneIngredientEffect)
            {
                // TODO: Find a better way to get an effect. Also, it might be better to get a different effect, like restore fatigue.
                IEffectGetter? nullableEffect = null;
                foreach (var ingredient in state.LoadOrder.PriorityOrder.WinningOverrides<IIngredientGetter>())
                {
                    try
                    {
                        Console.WriteLine("Before getting effect.");
                        nullableEffect = ingredient.Effects[0];
                        Console.WriteLine("After getting effect.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Couldn't get effect from ingredient.");
                    }
                }

                var effect = nullableEffect!;

                foreach (var oldIngredient in state.LoadOrder.PriorityOrder.WinningOverrides<IIngredientGetter>())
                {
                    try
                    {
                        if (oldIngredient.Effects.Count > 0)
                        {
                            Console.WriteLine($"Skipping ingredient because Count > 0. EditorID:{oldIngredient.EditorID} Name:{oldIngredient.Name}");
                            continue;
                        }

                        var newIngredient = oldIngredient.DeepCopy();
                        newIngredient.Effects.Add(effect.DeepCopy());
                        state.PatchMod.Ingredients.Set(newIngredient);
                        Console.WriteLine($"Modified ingredient. EditorID:{newIngredient.EditorID} Name:{newIngredient.Name}");
                    }
                    catch (Exception ex)
                    {
                        throw RecordException.Enrich(ex, oldIngredient);
                    }
                }
            }

            Console.WriteLine("\n\nRunPatch`Close\n");
        }
    }
}