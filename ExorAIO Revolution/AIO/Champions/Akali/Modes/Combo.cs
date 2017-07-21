
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using AIO.Utilities;

    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Akali
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (heroTarget.IsValidTarget() &&
                    !heroTarget.HasBuff("AkaliMota") &&
                    !Invulnerable.Check(heroTarget, DamageType.Magical))
                {
                    SpellClass.Q.CastOnUnit(heroTarget);
                }
            }

            /// <summary>
            ///     The R Combo Logic.
            /// </summary>
            if (SpellClass.R.Ready)
            {
                Obj_AI_Hero heroTarget;
                if (Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range * 2).Any(t => t.HasBuff("AkaliMota")))
                {
                    heroTarget = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range).FirstOrDefault(t => t.HasBuff("AkaliMota"));
                }
                else
                {
                    heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range * 2);
                }


                if (heroTarget == null ||
                    !heroTarget.IsValidTarget() ||
                    Invulnerable.Check(heroTarget, DamageType.Magical))
                {
                    return;
                }

                if (heroTarget.IsUnderEnemyTurret() &&
                    MenuClass.Spells["r"]["customization"]["safe"].As<MenuBool>().Enabled)
                {
                    return;
                }

                if (heroTarget.IsValidTarget(SpellClass.R.Range) &&
                    MenuClass.Spells["r"]["combo"].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.GetBuffCount("AkaliShadowDance")
                            <= MenuClass.Spells["r"]["customization"]["keepstacks"].As<MenuSlider>().Value)
                    {
                        return;
                    }

                    if (MenuClass.Spells["r"]["whitelist"][heroTarget.ChampionName.ToLower()].Enabled)
                    {
                        SpellClass.R.CastOnUnit(heroTarget);
                    }
                }
                else
                {
                    if (UtilityClass.Player.GetBuffCount("AkaliShadowDance")
                            <= MenuClass.Spells["r"]["customization"]["gapclose"].As<MenuSliderBool>().Value ||
                        !MenuClass.Spells["r"]["customization"]["gapclose"].As<MenuSliderBool>().Enabled)
                    {
                        return;
                    }

                    var bestEnemy = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range * 2)
                        .FirstOrDefault(t => MenuClass.Spells["r"]["whitelist"][t.ChampionName.ToLower()].Enabled);
                    if (bestEnemy != null)
                    {
                        var bestMinion = Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.R.Range)
                            .Where(m => m.Distance(bestEnemy) < SpellClass.Q.Range)
                            .OrderBy(m => m.Distance(heroTarget))
                            .FirstOrDefault();

                        if (bestMinion != null)
                        {
                            SpellClass.R.CastOnUnit(bestMinion);
                        }
                    }
                }
            }
        }

        #endregion
    }
}