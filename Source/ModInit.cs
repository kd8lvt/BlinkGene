using AutoBlink;
using HarmonyLib;
using Verse;

namespace BlinkGene
{
    [StaticConstructorOnStartup]
    public class ModInit
    {
        public const string MOD_ID = "kd8lvt.blink.gene";
        public static readonly Harmony HARMONY;

        static ModInit()
        {
            HARMONY = new Harmony(MOD_ID);
            HARMONY.Patch(new HarmonyMethod(typeof(Pawn),nameof(Pawn.Tick)).method,new HarmonyMethod(typeof(ModInit),nameof(PawnTick)));
        }
        public static void PawnTick(ref Pawn __instance)
        {
            /*
             * We need this patch to disable auto-blinking for pawns who don't have the gene.
             * I ALMOST did this whole project without Harmony. Oh well.
             */
            if (__instance.def.defName == "Human" && __instance.GetComp<CompBlinkWatcher>() is CompBlinkWatcher comp &&
                !__instance.genes.HasActiveGene(Gene_BlinkGland.gene))
            {
                comp.autoBlinkMaster = false;
            }
        }
    }
}