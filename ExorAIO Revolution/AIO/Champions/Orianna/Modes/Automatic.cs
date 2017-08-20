
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using AIO.Utilities;

    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Automatic()
        {
            if (this.BallPosition == null)
            {
                return;
            }

            /// <summary>
            ///     The Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["aoe"] != null &&
                MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Enabled)
            {
                var countValidTargets = GameObjects.EnemyHeroes.Count(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTarget(SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius, false, false, (Vector3)this.BallPosition) &&
                        SpellClass.R.GetPrediction(t).CastPosition.Distance((Vector3)this.BallPosition) < SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius);
            
                if (countValidTargets >= MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Value)
                {
                    SpellClass.R.Cast();
                }
            }
        }

        #endregion
    }
}