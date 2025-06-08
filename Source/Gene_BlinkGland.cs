using System;
using AutoBlink;
using Verse;

namespace BlinkGene
{
    public class Gene_BlinkGland : Gene
    {
        //Is there a better way to do this? Probably. Do I care? No, not really.
        private static readonly Lazy<GeneDef> lazyGene = new Lazy<GeneDef>(() => DefDatabase<GeneDef>.GetNamed("kd8lvt_BlinkGland"));
        public static GeneDef gene => lazyGene.Value;

        public override bool Active
        {
            get
            {
                //Allows me to just use a `HasActiveGene` check, instead of that AND the below.
                //Plus it makes it obvious for the user when a HAR race is incompatible.
                return base.Active && pawn.def.modExtensions.Any(ext => ext is BlinkableExtension);
            }
        }

        public override void Tick()
        {
            if (pawn.GetComp<CompBlinkWatcher>() is CompBlinkWatcher comp)
                //This should never nullref, because Gene.Tick() shouldn't ever be called on anything with a null `genes` field. 
                comp.autoBlinkMaster = pawn.genes.HasActiveGene(gene);

            base.Tick();
        }
    }
}