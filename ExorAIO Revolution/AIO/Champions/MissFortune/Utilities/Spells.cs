namespace AIO.Champions
{
    using Aimtec;
    using Aimtec.SDK.Prediction.Skillshots;

    using AIO.Utilities;

    using Spell = Aimtec.SDK.Spell;

    /// <summary>
    ///     The spells class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 650f + UtilityClass.Player.BoundingRadius);
            SpellClass.Q2 = new Spell(SpellSlot.Q, SpellClass.Q.Range + 500f - UtilityClass.Player.BoundingRadius);
            SpellClass.W = new Spell(SpellSlot.W);
            SpellClass.E = new Spell(SpellSlot.E, 1000f);
            SpellClass.R = new Spell(SpellSlot.R, 1400f);

            SpellClass.Q2.SetSkillshot(0.5f, UtilityClass.GetAngleByDegrees(40), 1400f, false, SkillshotType.Line);
            SpellClass.E.SetSkillshot(0.5f, 350f, 500f, false, SkillshotType.Circle);
            SpellClass.R.SetSkillshot(0.334f, UtilityClass.GetAngleByDegrees(17), 780f, false, SkillshotType.Cone);
        }

        #endregion
    }
}