
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using AIO.Utilities;

    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ahri
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic()
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["logical"].As<MenuBool>().Value)
            {
                var range = SpellClass.E.Range;
                var target = ImplementationClass.ITargetSelector.GetOrderedTargets(range)
                    .FirstOrDefault(t => t.IsImmobile() && !Invulnerable.Check(t) && t.IsValidTarget(range));
                if (target != null)
                {
                    SpellClass.E.Cast(target);
                }
            }
        }

        #endregion
    }
}