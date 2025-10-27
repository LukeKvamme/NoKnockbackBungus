using BepInEx;
using R2API;
using RoR2;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ExamplePlugin
{

    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class NoKnockbackBungus : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Ovalsquare";
        public const string PluginName = "NoKnockbackBungus";
        public const string PluginVersion = "1.0.0";

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);

            On.RoR2.HealthComponent.TakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            // Run custom code before the original method
            // Check if the attacker has Bungus and the victim is not moving (IDK how to do the IL hooking stuff to get the private mushroomWardHealing thing, but this is close enough I think and still works for the purpose of no knockback)
            if (self.body.inventory && self.body.inventory.GetItemCount(RoR2Content.Items.Mushroom) > 0 && self.body.GetNotMoving())
            {
                damageInfo.force = Vector3.zero;
                // Log.Info("Negated knockback -- character has bungus + not moving (using public getnotmoving() inside characterbody)");
            }

            // Call the original method, now that we have removed the Vector3 force
            orig(self, damageInfo);
        }
    }
}
