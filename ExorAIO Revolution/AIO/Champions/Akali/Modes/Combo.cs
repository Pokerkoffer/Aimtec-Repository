
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using Utilities;

    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Akali
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public static void Combo()
        {
            var heroTarget = Extensions.GetBestEnemyHeroTarget();
            if (!heroTarget.IsValidTarget() ||
                Invulnerable.Check(heroTarget, DamageType.Magical))
            {
                return;
            }

            /// <summary>
            ///     The R Combo Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                heroTarget.IsValidTarget(SpellClass.R.Range) &&
                MenuClass.Spells["r"]["combo"].As<MenuBool>().Enabled)
            {
                if ((!heroTarget.IsUnderEnemyTurret() ||
                    !MenuClass.Spells["r"]["customization"]["safe"].As<MenuBool>().Enabled) &&
                    (UtilityClass.Player.GetBuffCount("AkaliShadowDance") >
                        MenuClass.Spells["r"]["customization"]["keepstacks"].As<MenuSliderBool>().Value ||
                    !MenuClass.Spells["r"]["customization"]["keepstacks"].As<MenuSliderBool>().Enabled) &&
                    MenuClass.Spells["r"]["whitelist"][heroTarget.ChampionName.ToLower()].Enabled)
                {
                    SpellClass.R.CastOnUnit(heroTarget);
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                !heroTarget.HasBuff("AkaliMota") &&
                heroTarget.IsValidTarget(SpellClass.Q.Range) &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.CastOnUnit(heroTarget);
            }

            /// <summary>
            ///     The R Gapclose Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                !heroTarget.IsValidTarget(SpellClass.R.Range) &&
                heroTarget.IsValidTarget(SpellClass.R.Range * 2) &&
                UtilityClass.Player.GetBuffCount("AkaliShadowDance")
                    >= MenuClass.Spells["r"]["customization"]["gapclose"].As<MenuSliderBool>().Value &&
                MenuClass.Spells["r"]["customization"]["gapclose"].As<MenuSliderBool>().Enabled)
            {
                var bestMinion = Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.R.Range)
                    .Where(m => m.Distance(heroTarget) < SpellClass.Q.Range)
                    .OrderBy(m => m.Distance(heroTarget))
                    .FirstOrDefault();

                if (bestMinion != null)
                {
                    SpellClass.R.CastOnUnit(bestMinion);
                }
            }
        }

        #endregion
    }
}