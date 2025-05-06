using BepInEx;
using IL;
using MoreSlugcats;
using RainMeadow;
using RWCustom;
using System;
using System.Security.Permissions;
using UnityEngine;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using Menu;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace LizardOnBackMod
{
    [BepInPlugin(modID, modeName, version)]
    public partial class LizardOnBack : BaseUnityPlugin
    {
        public const string modID = "ShanKa.LizardOnBack";
        public const string modeName = "LizardOnBack";
        public const string version = "0.0.2";
        public static LizardOnBack instance;
        public static LizardOnBackOptions options;
        private bool init;
        private bool fullyInit;
        private bool addedMod = false;

        public void OnEnable()
        {
            instance = this;
            options = new LizardOnBackOptions(this);

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
                MachineConnector.SetRegisteredOI(modID, options);
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

    public class LizardOnBackOptions : OptionInterface
    {
        // 配置选项
        public readonly Configurable<bool> EnableLongPressToDropLizard;
        public readonly Configurable<bool> EnableThrowLizard;

        private UIelement[] LizardOnBackSettings;

        public LizardOnBackOptions(LizardOnBack instance)
        {
            // 初始化配置选项
            EnableLongPressToDropLizard = config.Bind("EnableLongPressToDropLizard", true);
            EnableThrowLizard = config.Bind("EnableThrowLizard", true);
        }

        public override void Initialize()
        {
            try
            {
                // 创建配置选项卡
                OpTab lizardOnBackTab = new OpTab(this, "LizardOnBack");
                Tabs = new OpTab[1] { lizardOnBackTab };
                
                // 创建UI元素
                LizardOnBackSettings = new UIelement[]
                {
                    // 标题
                    new OpLabel(10f, 550f, Translate("LizardOnBack"), bigText: true),
                    
                    // 长按放下设置
                    new OpLabel(10f, 510f, Translate("Long Press To Drop Lizard"), bigText: false),
                    new OpCheckBox(EnableLongPressToDropLizard, new Vector2(10f, 480f)),
                    new OpLabel(40f, 480f, Translate("Enable Long Press To Drop Lizard From Back"), bigText: false),
                    
                    // 扔出蜥蜴设置
                    new OpLabel(10f, 450f, Translate("Throw Lizard"), bigText: false),
                    new OpCheckBox(EnableThrowLizard, new Vector2(10f, 420f)),
                    new OpLabel(40f, 420f, Translate("Enable Throwing Lizard From Back"), bigText: false)
                };
                
                // 将元素添加到选项卡
                lizardOnBackTab.AddItems(LizardOnBackSettings);
            }
            catch (Exception ex)
            {
                Debug.LogError("错误: 无法打开蜥蜴背负设置菜单: " + ex);
            }
        }
    }
}