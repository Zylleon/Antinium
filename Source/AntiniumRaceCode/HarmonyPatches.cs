using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using RimWorld;
using Verse;
using Harmony;



namespace AntiniumRaceCode
{

    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        // this static constructor runs to create a HarmonyInstance and install a patch.
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.antinium");

            // find the AddFoodPoisoningHediff method of the class RimWorld.FoodUtility
            MethodInfo targetmethod = AccessTools.Method(typeof(RimWorld.FoodUtility), "AddFoodPoisoningHediff");

            // find the static method to call before (i.e. Prefix) the targetmethod
            HarmonyMethod prefixmethod = new HarmonyMethod(typeof(AntiniumRaceCode.HarmonyPatches).GetMethod("AddFoodPoisoningHediff_Prefix"));

            // patch the targetmethod, by calling prefixmethod before it runs, with no postfixmethod (i.e. null)
            harmony.Patch(targetmethod, prefixmethod, null);

            targetmethod = AccessTools.Method(typeof(RimWorld.FoodUtility), "AddIngestThoughtsFromIngredient");
            prefixmethod = new HarmonyMethod(typeof(AntiniumRaceCode.HarmonyPatches).GetMethod("AddIngestThoughtsFromIngredient_Prefix"));
            harmony.Patch(targetmethod, prefixmethod, null);
        }


        

        
        public static void AddIngestThoughtsFromIngredient_Prefix(ThingDef ingredient, Pawn ingester, List<ThoughtDef> ingestThoughts)
        {
            if (ingester.story.traits.HasTrait(AntDefOf.Ant_BirdLover))
            {
                if (ingredient.ingestible.sourceDef.race.body.defName == "Bird" || ingredient.ingestible.sourceDef.race.leatherDef.defName == "Leather_Bird")
                {
                    ingestThoughts.Add(AntDefOf.Ant_AteBirdMeatAsIngredient);                // TODO: figure out how to identify my new thought so it can be added


                }

            }
            

            //if (ingester.story.traits.allTraits.Any(tr => tr.def.defName == "Ant_BirdLover"))
            //{
            //    if (ingredient.ingestible.sourceDef.race.body.defName == "Bird" || ingredient.ingestible.sourceDef.race.leatherDef.defName == "Leather_Bird")
            //    {
            //        ingestThoughts.Add(ThoughtDefOf.Ant_AteBirdMeatAsIngredient);                // TODO: figure out how to identify my new thought so it can be added

                    
            //    }

            //}



            //if (ingester.RaceProps.Humanlike && FoodUtility.IsHumanlikeMeat(ingredient))
            //{
            //    ingestThoughts.Add((!ingester.story.traits.HasTrait(TraitDefOf.Cannibal)) ? ThoughtDefOf.AteHumanlikeMeatAsIngredient : ThoughtDefOf.AteHumanlikeMeatAsIngredientCannibal);
            //}
            //else if (ingredient.ingestible.specialThoughtAsIngredient != null)
            //{
            //    ingestThoughts.Add(ingredient.ingestible.specialThoughtAsIngredient);
            //}
            //
        }


        // This method is now always called right before RimWorld.FoodUtility.AddFoodPoisoningHediff.
        public static bool AddFoodPoisoningHediff_Prefix(Pawn pawn)
        {
            if (pawn.kindDef.race.defName == "Ant_AntiniumRace")
            {
                return false;
            }
            return true;

        }


    }
}