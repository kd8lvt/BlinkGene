using HarmonyLib;
using Verse;

namespace BlinkGene
{
    /** Mod initializer class */
    [StaticConstructorOnStartup]
    public class ModInit
    {
        /** There is quite literally no reason for me to have made this, yet here we are. */
        public const string MOD_ID = "kd8lvt.blink.gene";
        
        /** An ENTIRE harmony instance, for the SINGLE patch I use. */
        public static readonly Harmony HARMONY;
        
        /** Mod Initializer */
        static ModInit()
        {
            HARMONY = new Harmony(MOD_ID);
            HARMONY.Patch(typeof(Pawn).DeclaredMethod("Tick"),
                  typeof(Gene_BlinkGland).DeclaredMethod("PawnTickPrefix"));
        }
    }
}