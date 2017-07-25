
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Collections.Generic;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Prediction.Skillshots;

    using AIO.Utilities;

    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The Extended Q Mixed Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["extendedq"]["combo"].As<MenuBool>().Enabled)
            {
                var unitsInQRange = Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range);
                var unitsToIterate =
                    unitsInQRange.Any(m => m.Health < UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q))
                        ? unitsInQRange.Where(m => m.Health < UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q))
                        : unitsInQRange;

                var objAiBases = unitsToIterate as IList<Obj_AI_Base> ?? unitsToIterate.ToList();
                foreach (var hero in Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.Q2.Range))
                {
                    foreach (var minion in objAiBases)
                    {
                        var polygon = this.QCone(minion);
                        if (polygon.IsInside((Vector2)hero.ServerPosition) &&
                            MenuClass.Spells["extendedq"]["whitelist"][hero.ChampionName.ToLower()].Enabled &&
                            (this.LoveTapTargetNetworkId == hero.NetworkId || GameObjects.EnemyMinions.All(m => polygon.IsOutside((Vector2)m.ServerPosition))) &&
                            polygon.IsInside((Vector2)Prediction.GetPrediction(hero, UtilityClass.Player.Distance(hero) / SpellClass.Q2.Speed + SpellClass.Q2.Delay).UnitPosition))
                        {
                            SpellClass.Q.CastOnUnit(minion);
                        }
                    }
                }
            }

            /// <summary>
            ///     The Harass E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical, false))
                {
                    SpellClass.E.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}