﻿
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Collections.Generic;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using AIO.Utilities;

    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Fields

        /// <summary>
        ///     Returns the MineField position.
        /// </summary>
        public Dictionary<int, Vector3> MineField = new Dictionary<int, Vector3>();

        /// <summary>
        ///     Returns the WorkedGrounds position.
        /// </summary>
        public Dictionary<int, Vector3> WorkedGrounds = new Dictionary<int, Vector3>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if there are any worked grounds in a determined range from the player.
        /// </summary>
        public bool AnyTerrainInRange(float range)
        {
            return this.CountTerrainsInRange(range) > 0;
        }

        /// <summary>
        ///     Returns the number of worked grounds in a determined range from the player.
        /// </summary>
        public int CountTerrainsInRange(float range)
        {
            return this.WorkedGrounds.Count(o => o.Value.Distance(UtilityClass.Player.Position) <= range);
        }

        /// <summary>
        ///     Reloads the MineField.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vector3 GetBestBouldersHitPosition(Obj_AI_Base unit)
        {
            var mostBouldersHit = 0;
            var mostBouldersHitPos = Vector3.Zero;
            foreach (var mine in this.MineField)
            {
                var unitToMineRectangle = new Geometry.Rectangle((Vector2)unit.Position, (Vector2)unit.Position.Extend((Vector2)mine.Value, 300f), unit.BoundingRadius);
                var bouldersHit = this.MineField.Count(o => unitToMineRectangle.IsInside((Vector2)o.Value));
                if (bouldersHit > mostBouldersHit)
                {
                    mostBouldersHit = bouldersHit;
                    mostBouldersHitPos = mine.Value;
                }
            }

            return mostBouldersHitPos;
        }

        /// <summary>
        ///     Reloads the MineField.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public int GetBestBouldersHitPositionHitBoulders(Obj_AI_Base unit)
        {
            var mostBouldersHit = 0;
            foreach (var mine in this.MineField)
            {
                var unitToMineRectangle = new Geometry.Rectangle((Vector2)unit.Position, (Vector2)unit.Position.Extend((Vector2)mine.Value, 300f), unit.BoundingRadius);
                var bouldersHit = this.MineField.Count(o => unitToMineRectangle.IsInside((Vector2)o.Value));
                if (bouldersHit > mostBouldersHit)
                {
                    mostBouldersHit = bouldersHit;
                }
            }

            return mostBouldersHit;
        }

        /// <summary>
        ///     Returns the position the target would have after being pulled by W.
        /// </summary>
        public Vector3 GetUnitPositionAfterPull(Obj_AI_Base unit)
        {
            var targetPred = SpellClass.W.GetPrediction(unit).CastPosition;
            return targetPred.Extend(UtilityClass.Player.Position, 300f);
        }

        /// <summary>
        ///     Returns the position the target would have after being pushed by W.
        /// </summary>
        public Vector3 GetUnitPositionAfterPush(Obj_AI_Base unit)
        {
            var targetPred = SpellClass.W.GetPrediction(unit).CastPosition;
            return targetPred.Extend(UtilityClass.Player.Position, -300f);
        }

        /// <summary>
        ///     Returns the position the target would have after being pushed by W, based on the user's option preference.
        /// </summary>
        public Vector3 GetTargetPositionAfterW(Obj_AI_Hero target)
        {
            var position = new Vector3();
            switch (MenuClass.Spells["w"]["selection"][target.ChampionName.ToLower()].As<MenuList>().Value)
            {
                case 0:
                    position = this.GetUnitPositionAfterPull(target);
                    break;
                case 1:
                    position = this.GetUnitPositionAfterPush(target);
                    break;

                /// <summary>
                ///     Pull if killable else Push.
                /// </summary>
                case 2:
                    var isKillable = target.GetRealHealth() < UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q) * (this.IsNearWorkedGround() ? 1 : 3) +
                                     UtilityClass.Player.GetSpellDamage(target, SpellSlot.W) +
                                     UtilityClass.Player.GetSpellDamage(target, SpellSlot.E);
                    if (isKillable)
                    {
                        position = this.GetUnitPositionAfterPull(target);
                    }
                    else
                    {
                        position = this.GetUnitPositionAfterPush(target);
                    }
                    break;

                /// <summary>
                ///     Pull if not near else Push.
                /// </summary>
                case 3:
                    if (UtilityClass.Player.Distance(this.GetUnitPositionAfterPull(target)) >= 200f)
                    {
                        position = this.GetUnitPositionAfterPull(target);
                    }
                    else
                    {
                        position = this.GetUnitPositionAfterPush(target);
                    }
                    break;

                /// <summary>
                ///     Ignore Target If Possible.
                /// </summary>
                case 4:
                    if (!GameObjects.EnemyHeroes.Any(
                            t =>
                                t.IsValidTarget(SpellClass.W.Range) &&
                                MenuClass.Spells["w"]["selection"][t.ChampionName.ToLower()].As<MenuList>().Value < 3))
                    {
                        if (UtilityClass.Player.Distance(this.GetUnitPositionAfterPull(target)) >= 200f)
                        {
                            position = this.GetUnitPositionAfterPull(target);
                        }
                        else
                        {
                            position = this.GetUnitPositionAfterPush(target);
                        }
                    }
                    break;
            }

            return position;
        }

        /// <summary>
        ///     Returns true if the player is near a worked ground.
        /// </summary>
        public bool IsNearWorkedGround()
        {
            return this.AnyTerrainInRange(412.5f);
        }

        /// <summary>
        ///     Reloads the MineField.
        /// </summary>
        public void ReloadMineField()
        {
            foreach (var mine in ObjectManager.Get<GameObject>().Where(o => o != null && o.IsValid))
            {
                switch (mine.Name)
                {
                    case "Taliyah_Base_E_Mines.troy":
                        this.MineField.Add(mine.NetworkId, mine.Position);
                        break;
                }
            }
        }

        /// <summary>
        ///     Reloads the WorkedGrounds.
        /// </summary>
        public void ReloadWorkedGrounds()
        {
            foreach (var ground in ObjectManager.Get<GameObject>().Where(o => o != null && o.IsValid))
            {
                switch (ground.Name)
                {
                    case "Taliyah_Base_Q_aoe.troy":
                    case "Taliyah_Base_Q_aoe_river.troy":
                        this.WorkedGrounds.Add(ground.NetworkId, ground.Position);
                        break;
                }
            }
        }

        #endregion
    }
}