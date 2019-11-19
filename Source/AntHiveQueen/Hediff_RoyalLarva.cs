using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace AntiniumHiveQueen
{
    public class Hediff_RoyalLarva : HediffWithComps 
    {
        public override void Tick ()
        {
            switch(CurStageIndex)
            {

                case 0:
                    pawn.ageTracker.AgeBiologicalTicks += (long)15;
                    break;
                case 1:
                    pawn.ageTracker.AgeBiologicalTicks += (long)25;
                    break;
                case 2:
                    pawn.ageTracker.AgeBiologicalTicks += (long)40;
                    break;
                case 3:
                    pawn.ageTracker.AgeBiologicalTicks += (long)60;
                    break;

                //case 0:
                //    pawn.ageTracker.AgeBiologicalTicks += (long)25;
                //    break;
                //case 1:
                //    pawn.ageTracker.AgeBiologicalTicks += (long)50;
                //    break;
                //case 2:
                //    pawn.ageTracker.AgeBiologicalTicks += (long)70;
                //    break;
                //case 3:
                //    pawn.ageTracker.AgeBiologicalTicks += (long)95;
                //    break;

                default:
                    break;
            }

            if(pawn.ageTracker.AgeBiologicalYears >= 20)
            {
                // TODO: Add adult backstory!

                this.Severity = 0;
            }
        }
    }
}
