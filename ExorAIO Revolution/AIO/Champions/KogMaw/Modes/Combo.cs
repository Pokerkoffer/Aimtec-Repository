
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
    internal partial class KogMaw
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                GameObjects.EnemyHeroes.Any(t => t.IsValidTarget(SpellClass.W.Range)) &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.W.Cast();
            }

            /// <summary>
            ///     The R Combo Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                UtilityClass.Player.Mana
                    > UtilityClass.Player.SpellBook.GetSpell(SpellSlot.R).Cost + 50 * (UtilityClass.Player.GetBuffCount("kogmawlivingartillerycost") + 1) &&
                MenuClass.Spells["r"]["combo"].As<MenuSliderBool>().Enabled &&
                MenuClass.Spells["r"]["combo"].As<MenuSliderBool>().Value
                    > UtilityClass.Player.GetBuffCount("kogmawlivingartillerycost"))
            {
                foreach (var target in Extensions.GetBestEnemyHeroesTargets())
                {
                    if (target.IsValidTarget() &&
                        target.HealthPercent() <= 40 &&
                        !Invulnerable.Check(target, DamageType.Magical) ||
                        MenuClass.Spells["r"]["whitelist"][target.ChampionName.ToLower()].As<MenuBool>().Enabled)
                    {
                        SpellClass.R.Cast(target);
                    }
                }
            }
        }

        #endregion
    }
}