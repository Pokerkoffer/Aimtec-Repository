namespace AIO.Utilities
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;

    /// <summary>
    ///     The Bools class.
    /// </summary>
    internal static class Bools
    {
        #region Public Methods and Operators

        /// <returns>
        ///     true if the champion is supported by the AIO; otherwise, false.
        /// </returns>
        public static bool IsChampionSupported = true;

        /// <returns>
        ///     true if an unit has a Sheen-Like buff; otherwise, false.
        /// </returns>
        public static bool HasSheenLikeBuff(this Obj_AI_Hero unit)
        {
            return
                unit.HasBuff("sheen") ||
                unit.HasBuff("LichBane") ||
                unit.HasBuff("dianaarcready") ||
                unit.HasBuff("ItemFrozenFist") ||
                unit.HasBuff("sonapassiveattack");
        }

        /// <returns>
        ///     true if an unit is a Building; otherwise, false.
        /// </returns>
        public static bool IsBuilding(this GameObject unit)
        {
            return unit is Obj_AI_Turret;
        }

        /// <summary>
        ///     Checks whether the unit should preserve the sheen buff.
        /// </summary>
        public static bool ShouldPreserveSheen(this Obj_AI_Hero source)
        {
            return source.HasSheenLikeBuff() && source.ActionState.HasFlag(ActionState.CanAttack);
        }

        /// <summary>
        ///     Gets a value indicating whether a determined hero has a stackable item.
        /// </summary>
        public static bool HasTearLikeItem(this Obj_AI_Hero unit)
        {
            return
                unit.HasItem(ItemId.Manamune) ||
                unit.HasItem(ItemId.ArchangelsStaff) ||
                unit.HasItem(ItemId.TearoftheGoddess) ||
                unit.HasItem(ItemId.ManamuneQuickCharge) ||
                unit.HasItem(ItemId.ArchangelsStaffQuickCharge) ||
                unit.HasItem(ItemId.TearoftheGoddessQuickCharge);
        }

        /// <summary>
        ///     Gets a value indicating whether a determined champion can move or not.
        /// </summary>
        public static bool IsImmobile(this Obj_AI_Base target)
        {
            if (target.Buffs.Any(
                buff =>
                    buff.Type == BuffType.Stun ||
                    buff.Type == BuffType.Fear ||
                    buff.Type == BuffType.Flee ||
                    buff.Type == BuffType.Snare ||
                    buff.Type == BuffType.Taunt ||
                    buff.Type == BuffType.Charm ||
                    buff.Type == BuffType.Knockup ||
                    buff.Type == BuffType.Suppression))
            {
                return true;
            }

            if (!target.Name.Equals("Target Dummy"))
            {
                switch (target.ActionState)
                {
                    case ActionState.Asleep:
                    case ActionState.Feared:
                    case ActionState.Charmed:
                    case ActionState.Fleeing:
                    case ActionState.Surpressed:
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}