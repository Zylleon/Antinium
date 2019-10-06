using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace AntiniumRaceCode
{
    public class ThoughtWorker_BirdLoverApparel : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            string text = null;
            int num = 0;
            List<Apparel> wornApparel = p.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                if (wornApparel[i].Stuff.defName == "Leather_Bird")
                {
                    if (text == null)
                    {
                        text = wornApparel[i].def.label;
                    }
                    num++;
                }
            }

           
            if (num == 0)
            {
                return ThoughtState.Inactive;
            }
            if (num >= 5)
            {
                return ThoughtState.ActiveAtStage(4, text);
            }
            return ThoughtState.ActiveAtStage(num - 1, text);
        }
    }
}
