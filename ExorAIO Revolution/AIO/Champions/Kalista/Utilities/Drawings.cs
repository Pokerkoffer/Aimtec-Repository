﻿
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void Drawings()
        {
            /// <summary>
            ///     Loads the Q drawing.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Drawings["q"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.Q.Range, 30, Color.LightGreen);
            }

            /// <summary>
            ///     Loads the W drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["w"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.W.Range, 30, Color.Yellow);
            }

            /// <summary>
            ///     Loads the E drawings.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                /// <summary>
                ///     Loads the E Range.
                /// </summary>
                if (MenuClass.Drawings["e"].As<MenuBool>().Enabled)
                {
                    Render.Circle(UtilityClass.Player.Position, SpellClass.E.Range, 30, Color.Cyan);
                }

                /// <summary>
                ///     Loads the E damage to healthbar.
                /// </summary>
                if (MenuClass.Drawings["edmg"].As<MenuBool>().Enabled)
                {
                    foreach (var unit in ObjectManager.Get<Obj_AI_Base>().Where(h =>
                        IsPerfectRendTarget(h) &&
                        (h is Obj_AI_Hero || UtilityClass.JungleList.Contains(h.UnitSkinName)) &&
                        h.FloatingHealthBarPosition.OnScreen()))
                    {
                        var heroUnit = unit as Obj_AI_Hero;
                        var jungleList = UtilityClass.JungleList;
                        var mobOffset = DrawingClass.JungleHpBarOffsetList.FirstOrDefault(x => x.UnitSkinName.Equals(unit.UnitSkinName));

                        int width;
                        if (jungleList.Contains(unit.UnitSkinName))
                        {
                            width = mobOffset?.Width ?? DrawingClass.SWidth;
                        }
                        else
                        {
                            width = DrawingClass.SWidth;
                        }

                        int height;
                        if (jungleList.Contains(unit.UnitSkinName))
                        {
                            height = mobOffset?.Height ?? DrawingClass.SHeight;
                        }
                        else
                        {
                            height = DrawingClass.SHeight;
                        }

                        int xOffset;
                        if (jungleList.Contains(unit.UnitSkinName))
                        {
                            xOffset = mobOffset?.XOffset ?? DrawingClass.SxOffset(heroUnit);
                        }
                        else
                        {
                            xOffset = DrawingClass.SxOffset(heroUnit);
                        }

                        int yOffset;
                        if (jungleList.Contains(unit.UnitSkinName))
                        {
                            yOffset = mobOffset?.YOffset ?? DrawingClass.SyOffset(heroUnit);
                        }
                        else
                        {
                            yOffset = DrawingClass.SyOffset(heroUnit);
                        }

                        var barPos = unit.FloatingHealthBarPosition;
                        barPos.X += xOffset;
                        barPos.Y += yOffset;

                        var unitHealth = unit.GetRealHealth();
                        var totalDamage = GetTotalRendDamage(unit);

                        var barLength = 0;
                        if (unitHealth > totalDamage)
                        {
                            barLength = (int)(width * ((unitHealth - totalDamage) / unit.MaxHealth * 100 / 100));
                        }

                        var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                        var drawStartXPos = barPos.X + barLength;

                        Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true, unitHealth < totalDamage ? Color.Blue : Color.Orange);
                        Render.Line(drawStartXPos, barPos.Y, drawStartXPos, barPos.Y + height + 1, 1, true, Color.Lime);
                    }
                }
            }

            /// <summary>
            ///     Loads the R drawing.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["r"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.R.Range, 30, Color.Red);
            }

            /// <summary>
            ///     Loads the Soulbound drawing.
            /// </summary>
            if (SoulBound != null &&
                MenuClass.Drawings["soulbound"].As<MenuSliderBool>().Enabled)
            {
                for (var i = 0; i < MenuClass.Drawings["soulbound"].As<MenuSliderBool>().Value; i++)
                {
                    Render.Circle(SoulBound.Position, SoulBound.BoundingRadius + 5 * i, (uint)(5 + i), Color.Cyan);
                }
            }
        }

        #endregion
    }
}