using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The attributes define the power level of the entities. All attributes but bonus armor and magic resistance are 10 by default, and 1 point increase by 10% the statistic.
/// </summary>
public struct Attributes
{
    /// <param name="level">The current level of the entity</param>
    public Attributes(int level)
    {
        // The attributes without any modifier exept the current level of the entity
        Strength = 10 + (level - 1);
        Dexterity = 10;
        Intelligence = 10 + (level - 1);
        Constitution = 10 + (level - 1);
        BonusArmor = 0;
        BonusMagicResistance = 0;
    }
    /// <summary>
    /// The strength changes the amount of physical damages done by the attacks (10 = 100%).
    /// </summary>
    public int Strength;
    /// <summary>
    /// The dexterity changes the number of attack by seconds (10 = 100%).
    /// </summary>
    public int Dexterity;
    /// <summary>
    /// The intelligence changes the effectiveness of the abilities (10 = 100%).
    /// </summary>
    public int Intelligence;
    /// <summary>
    /// The constitution changes the health of the entity (10 = 100%).
    /// </summary>
    public int Constitution;
    /// <summary>
    /// The bonus armor is added to the base armor of the entity.
    /// </summary>
    public int BonusArmor;
    /// <summary>
    /// The bonus magic resistance is added to the base armor of the entity.
    /// </summary>
    public int BonusMagicResistance;
}
