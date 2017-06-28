
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Collections.Generic;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using AIO.Utilities;

    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public static void Combo()
        {
            /// <summary>
            ///     Orbwalk on minions.
            /// </summary>
            var minionsInAaRange = UtilityClass.GetAllGenericMinionsTargetsInRange(UtilityClass.Player.AttackRange);
            if (minionsInAaRange.Any() &&
                UtilityClass.Player.HasItem(ItemId.RunaansHurricane) &&
                !GameObjects.EnemyHeroes.Any(t => t.IsValidTarget(UtilityClass.Player.AttackRange)) &&
                MenuClass.Miscellaneous["minionsorbwalk"].As<MenuBool>().Enabled)
            {
                UtilityClass.Player.IssueOrder(OrderType.AttackUnit, minionsInAaRange.FirstOrDefault());
            }

            var bestTarget = UtilityClass.GetBestEnemyHeroTarget();
            if (!bestTarget.IsValidTarget() ||
                Invulnerable.Check(bestTarget, DamageType.Physical))
            {
                return;
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            var playerSpellbook = UtilityClass.Player.SpellBook;
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.Mana <
                    playerSpellbook.GetSpell(SpellSlot.Q).Cost +
                    playerSpellbook.GetSpell(SpellSlot.E).Cost &&
                bestTarget.IsValidTarget(SpellClass.Q.Range) &&
                !bestTarget.IsValidTarget(UtilityClass.Player.AttackRange) &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var collisions = (IList<Obj_AI_Base>)SpellClass.Q.GetPrediction(bestTarget).Collisions;
                if (collisions.Any())
                {
                    if (collisions.All(c => UtilityClass.GetAllGenericUnitTargets().Contains(c) && c.GetRealHealth() < UtilityClass.Player.GetSpellDamage(c, SpellSlot.Q)))
                    {
                        SpellClass.Q.Cast(bestTarget);
                    }
                }
                else
                {
                    SpellClass.Q.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}