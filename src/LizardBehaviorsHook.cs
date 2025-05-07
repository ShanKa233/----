using System;
using System.Linq;
using MonoMod.Cil;

namespace LizardOnBackMod
{
    public class LizardBehaviorsHook
    {
        public static void Hook()
        {
            On.LizardAI.DetermineBehavior += LizardAI_DetermineBehavior;//防止在背上的蜥蜴像回家

            On.AbstractCreature.IsEnteringDen += PreventFriendDeathOnDenEnter;//防止拉着的时候进入巢穴被弄死
        }


        public static void PreventFriendDeathOnDenEnter(On.AbstractCreature.orig_IsEnteringDen orig, AbstractCreature self, WorldCoordinate den)
        {
            Creature realizedCreature = self.realizedCreature;
            Player player = realizedCreature as Player;
            if (player == null || !player.grasps.Any((Creature.Grasp x) => x.grabbed is Lizard liz && LizardOnBackHook.LikesPlayer(liz, player)))
            {
                orig.Invoke(self, den);
            }
        }


        private static LizardAI.Behavior LizardAI_DetermineBehavior(On.LizardAI.orig_DetermineBehavior orig, LizardAI self)
        {
            LizardAI.Behavior behavior = orig(self);
            if (self.lizard.GetExLizardData().ownerPlayer != null)
            {
                // 使用枚举类型判断蜥蜴的行为,如果打算在背上回家就阻止
                if (behavior == LizardAI.Behavior.ReturnPrey ||
                    behavior == LizardAI.Behavior.EscapeRain ||
                    behavior == LizardAI.Behavior.Injured)
                {
                    behavior = LizardAI.Behavior.FollowFriend;
                }
            }
            return behavior;
        }
    }
}