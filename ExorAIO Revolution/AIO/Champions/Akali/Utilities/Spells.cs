namespace AIO.Champions
{
    using Aimtec;
    using Aimtec.SDK.Prediction.Skillshots;

    using Utilities;

    using Spell = Aimtec.SDK.Spell;

    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Akali
    {

        #region Public Methods and Operators
        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public static void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 600f);
            SpellClass.W = new Spell(SpellSlot.W, 270f);
            SpellClass.E = new Spell(SpellSlot.E, 300f);
            SpellClass.R = new Spell(SpellSlot.R, 700f);

            SpellClass.W.SetSkillshot(0.25f, 400f, float.MaxValue, false, SkillType.Circle);
        }

        #endregion
    }
}