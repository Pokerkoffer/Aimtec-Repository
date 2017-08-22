﻿// ReSharper disable ArrangeMethodOrOperatorBody


#pragma warning disable 1587

namespace AIO.Champions
{
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;

    using AIO.Utilities;

    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the FlashFrost Object.
        /// </summary>
        public GameObject FlashFrost = null;

        /// <summary>
        ///     Gets the GlacialStorm Object.
        /// </summary>
        public GameObject GlacialStorm = null;

        /// <summary>
        ///     Gets the total missile damage on a determined unit.
        /// </summary>
        public double GetFrostBiteDamage(Obj_AI_Base unit)
        {
            var player = UtilityClass.Player;
            return this.IsChilled(unit)
                       ? player.GetSpellDamage(unit, SpellSlot.E, DamageStage.Empowered)
                       : player.GetSpellDamage(unit, SpellSlot.E);
        }

        /// <summary>
        ///     Returns true if the target is marked by Q or R, else false.
        /// </summary>
        public bool IsChilled(Obj_AI_Base unit)
        {
            return unit.HasBuff("chilled");
        }

        #endregion
    }
}