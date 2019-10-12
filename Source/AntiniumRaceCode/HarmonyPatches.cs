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


            // Bird lover eats bird
            targetmethod = AccessTools.Method(typeof(RimWorld.FoodUtility), "AddIngestThoughtsFromIngredient");
            prefixmethod = new HarmonyMethod(typeof(AntiniumRaceCode.HarmonyPatches).GetMethod("AddIngestThoughtsFromIngredient_Prefix"));
            harmony.Patch(targetmethod, prefixmethod, null);


            // Aberration
            targetmethod = AccessTools.Method(typeof(Verse.AI.MentalBreaker), "TryDoRandomMoodCausedMentalBreak");
            HarmonyMethod postfixmethod = new HarmonyMethod(typeof(AntiniumRaceCode.HarmonyPatches).GetMethod("MentalBreak_Abberation_Postfix"));
            harmony.Patch(targetmethod, null, postfixmethod);

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


        public static void AddIngestThoughtsFromIngredient_Prefix(ThingDef ingredient, Pawn ingester, List<ThoughtDef> ingestThoughts)
        {
            if (ingester.story.traits.HasTrait(AntDefOf.Ant_BirdLover))
            {
                if (ingredient.ingestible.sourceDef.race.body.defName == "Bird" || ingredient.ingestible.sourceDef.race.leatherDef.defName == "Leather_Bird")
                {
                    ingestThoughts.Add(AntDefOf.Ant_AteBirdMeatAsIngredient);                

                }

            }
        }



        public static void MentalBreak_Abberation_Postfix(Verse.AI.MentalBreaker __instance, ref bool __result)
        {
            Log.Message("aberration method fired");
            int intensity;
            int.TryParse("" + (byte)Traverse.Create(__instance).Property("CurrentDesiredMoodBreakIntensity").GetValue<MentalBreakIntensity>(), out intensity);
            Log.Message("Mental break had an intensity of " + intensity);
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();

            //TODO: check if pawn is already abberation

            if (pawn.kindDef.race.defName == "Ant_AntiniumRace" && __result && intensity >= 2)
            {

                Log.Message("it might be an aberration?");



                //TODO: some (weighted?) randomness to see if the pawn becomes abberation

                if (Rand.Chance(intensity * .06f - .1f))
                {
                    pawn.health.AddHediff(AntDefOf.Ant_Aberration);
                }



                // apply abberation
                pawn.health.AddHediff(AntDefOf.Ant_Aberration);
                //TODO: send letter
                //Verse.AI.MentalBreakWorker.TrySendLetter(pawn, LetterAberration)


            

            }

        }



    }
}