using System;
using AutoBlink;
using RimWorld.BaseGen;
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

        /** Disables auto-blink on pawns without the required gene.<br />
            I'll be real, this probably doesn't NEED to be called every tick. */
        public override void Tick()
        {
            //Only run once every 10t
            if (Find.TickManager.TicksGame % 10 > 0) { base.Tick(); return; }

            //This should never nullref, because Gene.Tick() shouldn't ever be called on anything with a null `genes` field. 
            //If it does, that's Ludeon's fault, not mine.
            if (pawn.GetComp<CompBlinkWatcher>() is CompBlinkWatcher comp && comp.autoBlinkMaster != pawn.genes.HasActiveGene(gene))
                comp.autoBlinkMaster = !comp.autoBlinkMaster;
            
            //Probably should let the base gene class tick... not that it probably does anything.
            base.Tick();
        }
        
        /** We need this patch to disable auto-blinking for pawns who don't have the gene.<br />
         *  I <i>almost</i> did this whole project without needing Harmony. Oh well.
         */
        public static void PawnTickPrefix(ref Pawn __instance)
        {
            if (__instance.def.defName == "Human" && __instance.GetComp<CompBlinkWatcher>() is CompBlinkWatcher comp &&
                !__instance.genes.HasActiveGene(Gene_BlinkGland.gene))
            {
                comp.autoBlinkMaster = false;
            }
        }
    }
}