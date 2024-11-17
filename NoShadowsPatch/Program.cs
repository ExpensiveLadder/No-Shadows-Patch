using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;

namespace NoShadowsPatch
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "YourPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var lightGetter in state.LoadOrder.PriorityOrder.Light().WinningOverrides())
            {
                if (lightGetter.Flags.HasFlag(Light.Flag.ShadowHemisphere) || lightGetter.Flags.HasFlag(Light.Flag.ShadowOmnidirectional) || lightGetter.Flags.HasFlag(Light.Flag.ShadowSpotlight)) {
                    var light = state.PatchMod.Lights.GetOrAddAsOverride(lightGetter);

                    if (lightGetter.Flags.HasFlag(Light.Flag.ShadowHemisphere)) {
                        light.Flags -= Light.Flag.ShadowHemisphere;
                    }

                    if (lightGetter.Flags.HasFlag(Light.Flag.ShadowOmnidirectional)) {
                        light.Flags -= Light.Flag.ShadowOmnidirectional;
                    }
     
                    if (lightGetter.Flags.HasFlag(Light.Flag.ShadowSpotlight)) {
                        light.Flags -= Light.Flag.ShadowSpotlight;
                    }

                    Console.WriteLine(light.EditorID);
                }
            }        
        }
    }
}
