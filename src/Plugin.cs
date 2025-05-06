using BepInEx;
using IL;
using MoreSlugcats;
using RainMeadow;
using RWCustom;
using System;
using System.Security.Permissions;
using UnityEngine;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace LizardOnBackMod
{
    [BepInPlugin(modID, modeName, version)]
    public partial class LizardOnBack : BaseUnityPlugin
    {
        public const string modID = "ShanKa.LizardOnBack";
        public const string modeName = "LizardOnBack";
        public const string version = "0.0.1";
        public static LizardOnBack instance;
        // public static LizardOnBackOptions options;
        private bool init;
        private bool fullyInit;
        private bool addedMod = false;

        public void OnEnable()
        {
            instance = this;
            // options = new LizardOnBackOptions(this);

            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
        }

        private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            if (init) return;
            init = true;

            try
            {
                // 添加配置选项到机器连接器
                // MachineConnector.SetRegisteredOI(modID, options);
                LizardOnBackHook.Hook();

                fullyInit = true;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                fullyInit = false;
            }
        }
    }
}