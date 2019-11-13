using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.Sound;

namespace AntiniumHiveQueen
{
    public class CompHQPresence : ThingComp
    {
        private int queenMaturity = 0;
        // 0 infant to maturity
        // 1 first year of maturity
        // 2? 2nd year of maturity+

        public int QueenMaturity
        {
            get
            {
                return this.queenMaturity;
            }
        }



        private bool Active
        {
            get
            {
                Pawn pawn = this.parent as Pawn;
                TraitDef queenTrait = DefDatabase<TraitDef>.GetNamed("Ant_HiveQueenTrait");
                return pawn.story.traits.HasTrait(queenTrait);
            }
        }


        public override void CompTick()
        {
            if (this.Active)
            {
                Pawn pawn = this.parent as Pawn;
                if (pawn != null)
                {
                    // to spread this out a bit
                    if (Find.TickManager.TicksGame % 3803 == 0)
                    {
                        // not exactly a gamecondition?
                        // apply relationship to colonists?
                        ApplyQueenRelation(pawn);
                        ApplyQueenHediff(pawn.Map);
                    }

                    switch (queenMaturity)
                    {
                        case 0:
                            // she's underage
                            // age queen if needed - 1 year per day?
                            // increase queenMaturity if needed

                            pawn.ageTracker.AgeBiologicalTicks += (long)60;
                            
                            if(pawn.ageTracker.AgeBiologicalYears >= 18)                
                            {
                                // Add adult backstory
                                queenMaturity++;
                            }
                            break;
                        case 1:
                            // This is the only adult phase to be implemented right now

                                break;
                        default:
                            break;
                    }


                }
            }
        }

        private static void ApplyQueenRelation(Pawn queen)
        {

            PawnRelationDef relation = DefDatabase<PawnRelationDef>.GetNamed("Ant_QueenRelation");
            // all free colonists on map
            IEnumerable<Pawn> colonists = queen.Map.mapPawns.FreeColonists;
            foreach (Pawn c in colonists.Where(p => p.RaceProps.Humanlike && p != queen))
            {
                if (c.relations.GetDirectRelation(relation, queen) == null)
                {
                    c.relations.AddDirectRelation(relation, queen);
                }

            }

        }


        private static void ApplyQueenHediff(Map map)
        {
            List<Pawn> colonists = map.mapPawns.FreeColonists.ToList();
            for (int i = 0; i < colonists.Count; i++)
            {
                Pawn pawn = colonists[i];
                if (pawn.RaceProps.Humanlike)
                {
                    SetQueenHediffSeverity(pawn);
                }
                //if (pawn.kindDef.race.defName == "Ant_AntiniumRace")
                //{
                //    SetQueenHediffSeverity(pawn, 1f);
                //}
                //else if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) >= 1.3)
                //{
                //    SetQueenHediffSeverity(pawn, 0.5f);
                //}
            }
        }


        private static void SetQueenHediffSeverity(Pawn pawn)
        {
            float severity = 0f;

            float hyper = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);

            // set severity
            if (pawn.kindDef.race.defName == "Ant_AntiniumRace")
            {
                severity = .5f;
            }

            if (hyper <= .3)
            {
                severity -= .35f;
            }

            else if (hyper <= .6)
            {
                severity -= .15f;
            }

            if (hyper >= 1.7)
            {
                severity += .35f;
            }

            else if (hyper >= 1.3)
            {
                severity += .15f;
            }


            // do hediff
            Hediff olddiff = pawn.health.hediffSet.GetFirstHediffOfDef(AntHQDefOf.Ant_HiveQueenInspHediff, false);
            if (olddiff != null)
            {
                pawn.health.RemoveHediff(olddiff);
            }

            if (severity > 0f)
            {
                Hediff hediff = HediffMaker.MakeHediff(AntHQDefOf.Ant_HiveQueenInspHediff, pawn, null);
                hediff.Severity = severity;
                pawn.health.AddHediff(hediff, null, null, null);
            }


        }

    }
}