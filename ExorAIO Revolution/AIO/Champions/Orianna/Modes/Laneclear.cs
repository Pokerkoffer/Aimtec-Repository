
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using Utilities;

    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public static void Laneclear()
        {
            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["laneclear"]) &&
                MenuClass.Spells["w"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                if (Extensions.GetEnemyLaneMinionsTargets().Count(m => m.IsValidTarget(SpellClass.W.Width, false, BallPosition))
                        >= MenuClass.Spells["w"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.W.Cast();
                }
            }

            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var polygon = new Geometry.Rectangle(
                    (Vector2)UtilityClass.Player.Position,
                    (Vector2)UtilityClass.Player.Position.Extend(BallPosition, UtilityClass.Player.Distance(BallPosition)),
                    SpellClass.E.Width);

                if (Extensions.GetEnemyLaneMinionsTargets().Count(t => t.IsValidTarget() && !polygon.IsOutside((Vector2)t.Position))
                        >= MenuClass.Spells["e"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.E.CastOnUnit(UtilityClass.Player);
                }
            }

            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.Q.GetLinearFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.Q.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["q"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.Q.Cast(farmLocation.Position);
                }
                */
            }
        }

        #endregion
    }
}