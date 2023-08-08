using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace SynSetESL
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "SynSetESL.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            uint uintMin = 0x800;
            uint uintMax = 0xFFF;
            bool isESPFE = true;
            ModKey patchModKey = state.PatchMod.ModKey;

            foreach (var rec in state.PatchMod.EnumerateMajorRecords())
            {
                if (!rec.FormKey.ModKey.Equals(patchModKey)) continue;

                if (rec.FormKey.ID < uintMin) isESPFE = false;
                if (rec.FormKey.ID > uintMax) isESPFE = false;
            }

            if (isESPFE)
            {
                state.PatchMod.ModHeader.Flags |= SkyrimModHeader.HeaderFlag.LightMaster;
            }
        }
    }
}
