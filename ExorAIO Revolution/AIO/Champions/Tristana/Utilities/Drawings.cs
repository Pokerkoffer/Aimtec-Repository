﻿
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Drawing;
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using Utilities;

    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Tristana
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the menus.
        /// </summary>
        public static void Drawings()
        {
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
                    RenderManager.RenderCircle(UtilityClass.Player.Position, SpellClass.E.Range, 100, Color.Cyan);
                }

                /// <summary>
                ///     Loads the E damage to healthbar.
                /// </summary>
                if (MenuClass.Drawings["edmg"].As<MenuBool>().Enabled)
                {
                    ObjectManager.Get<Obj_AI_Base>()
                                 .Where(h => h.IsValidTarget() && (h is Obj_AI_Hero && !Invulnerable.Check((Obj_AI_Hero)h) || UtilityClass.JungleList.Contains(h.UnitSkinName)))
                                 .ToList()
                                 .ForEach(unit =>
                                 {
                                     var heroUnit = unit as Obj_AI_Hero;
                                     var jungleList = UtilityClass.JungleList;
                                     var mobOffset = DrawingClass.JungleHpBarOffsetList.FirstOrDefault(x => x.UnitSkinName.Equals(unit.UnitSkinName));

                                     int width;
                                     if (jungleList.Contains(unit.UnitSkinName))
                                     {
                                         width = mobOffset != null ? mobOffset.Width : DrawingClass.SWidth;
                                     }
                                     else
                                     {
                                         width = DrawingClass.SWidth;
                                     }

                                     int height;
                                     if (jungleList.Contains(unit.UnitSkinName))
                                     {
                                         height = mobOffset != null ? mobOffset.Height : DrawingClass.SHeight;
                                     }
                                     else
                                     {
                                         height = DrawingClass.SHeight;
                                     }

                                     int xOffset;
                                     if (jungleList.Contains(unit.UnitSkinName))
                                     {
                                         xOffset = mobOffset != null ? mobOffset.XOffset : DrawingClass.SxOffset(heroUnit);
                                     }
                                     else
                                     {
                                         xOffset = DrawingClass.SxOffset(heroUnit);
                                     }

                                     int yOffset;
                                     if (jungleList.Contains(unit.UnitSkinName))
                                     {
                                         yOffset = mobOffset != null ? mobOffset.YOffset : DrawingClass.SyOffset(heroUnit);
                                     }
                                     else
                                     {
                                         yOffset = DrawingClass.SyOffset(heroUnit);
                                     }

                                     var barPos = unit.FloatingHealthBarPosition;
                                     barPos.X += xOffset;
                                     barPos.Y += yOffset;

                                     var drawEndXPos = barPos.X + width * (unit.HealthPercent() / 100);
                                     var drawStartXPos = (float)(barPos.X + (unit.GetRealHealth() > GetTotalExplosionDamage(unit)
                                                                                 ? width * ((unit.GetRealHealth() - GetTotalExplosionDamage(unit)) / unit.MaxHealth * 100 / 100)
                                                                                 : 0));

                                     RenderManager.RenderLine(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true, unit.GetRealHealth() < GetTotalExplosionDamage(unit) ? Color.Blue : Color.Orange);
                                     RenderManager.RenderLine(drawStartXPos, barPos.Y, drawStartXPos, barPos.Y + height + 1, 1, true, Color.Lime);
                                 });
                }
            }

            /// <summary>
            ///     Loads the W drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["w"].As<MenuBool>().Enabled)
            {
                RenderManager.RenderCircle(UtilityClass.Player.Position, SpellClass.W.Range, 100, Color.Purple);
            }
        }

        #endregion
    }
}