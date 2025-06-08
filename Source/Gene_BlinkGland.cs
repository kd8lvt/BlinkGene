using System;
using AutoBlink;
using Verse;

namespace BlinkGene
{
    /** The class that actually does anything noticeable ingame */
    public class Gene_BlinkGland : Gene
    {
        /** Is there a better way to do this? Probably. Do I care? No, not really. */
        private static readonly Lazy<GeneDef> lazyGene = new Lazy<GeneDef>(() => DefDatabase<GeneDef>.GetNamed("kd8lvt_BlinkGland"));
        
        /** A lazily-initialized reference to the kd8lvt_BlinkGland GeneDef */
        public static GeneDef gene => lazyGene.Value;

        public override bool Active
        {
            get
            {
                /* Allows me to just use `HasActiveGene`, instead of that AND the below modExtensions check.
                 * Plus it makes it obvious for the user when a HAR race is incompatible.
                 * More people should override this, tbh.
                 */
                return base.Active && pawn.def.modExtensions.Any(ext => ext is BlinkableExtension);
            }
        }
        
        /** We need this patch to disable auto-blinking for pawns who don't have the gene at all.<br />
         *  I <i>almost</i> did this whole project without needing Harmony. Oh well.
         */
        public static void PawnTickPrefix(ref Pawn __instance)
        {
            //This weird if chain ensures they're not all evaluated at the same time.
            //I'm doing this to optimize for the lowest-impact but widest-net checks first.
            //There's certainly fewer humans than other animals (typically),
            //Only a few of those humans will have kd8lvt_BlinkGland,
            //and ideally none of them will have a null CompBlinkWatcher, but it's probably worth checking.
            if (__instance.def.defName == "Human") if (!__instance.genes.HasActiveGene(gene)) if (__instance.GetComp<CompBlinkWatcher>() is CompBlinkWatcher comp)
            {
                comp.autoBlinkMaster = false;
            }
        }
    }
}