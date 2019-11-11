using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace AntiniumHiveQueen
{
    public class CompProperties_HQPresence : CompProperties
    {
        public int droneLevelIncreaseInterval = 150000;

        public CompProperties_HQPresence()
        {
            this.compClass = typeof(CompHQPresence);
        }
    }
}
