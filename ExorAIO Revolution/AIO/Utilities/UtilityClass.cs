﻿
// ReSharper disable ArrangeMethodOrOperatorBody
#pragma warning disable 1587

namespace AIO.Utilities
{
    using Aimtec;
    using Aimtec.SDK.Extensions;

    /// <summary>
    ///     The UtilityData class.
    /// </summary>
    internal static class UtilityClass
    {
        #region Static Fields

        /// <summary>
        ///     Gets the Player.
        /// </summary>
        public static Obj_AI_Hero Player = ObjectManager.GetLocalPlayer();

        /// <summary>
        ///     The last tick.
        /// </summary>
        public static int LastTick = 0;

        /// <summary>
        ///     The jungle HP bar offset list.
        /// </summary>
        internal static readonly string[] JungleList =
            {
                "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water",
                "SRU_Dragon_Earth", "SRU_Dragon_Elder", "SRU_Baron",
                "SRU_RiftHerald", "SRU_Red", "SRU_Blue", "SRU_Gromp",
                "Sru_Crab", "SRU_Krug", "SRU_Razorbeak", "SRU_Murkwolf"
            };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if there is a Wall between X pos and Y pos.
        /// </summary>
        public static bool AnyWallInBetween(Vector3 startPos, Vector3 endPos)
        {
            for (var i = 0; i < startPos.Distance(endPos); i++)
            {
                var point = NavMesh.WorldToCell(startPos.Extend(endPos, i));
                if (point.Flags.HasFlag(NavCellFlags.Wall | NavCellFlags.Building))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Gets the health with Blitzcrank's Shield support.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The target Health with Blitzcrank's Shield support.
        /// </returns>
        public static float GetRealHealth(this Obj_AI_Base unit)
        {
            var debuffer = 0f;

            /// <summary>
            ///     Gets the predicted reduction from Blitzcrank Shield.
            /// </summary>
            var hero = unit as Obj_AI_Hero;
            if (hero != null)
            {
                if (hero.ChampionName.Equals("Blitzcrank") && !hero.HasBuff("BlitzcrankManaBarrierCD"))
                {
                    debuffer += hero.Mana / 2;
                }
            }
            return unit.Health + unit.PhysicalShield + unit.HPRegenRate + debuffer;
        }

        #endregion
    }
}