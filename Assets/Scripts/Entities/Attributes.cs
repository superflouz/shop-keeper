using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
        HealthRegeneration = 10 + (level - 1);
        ManaRegeneration = 10;
        BonusArmor = 0;
        BonusMagicResistance = 0;
    }
    /// <summary>
    /// Apply the factors to the attributes individually
    /// </summary>
    public static Attributes operator *(Attributes a, AttributesFactors b)
    {
        a.Strength = Mathf.RoundToInt(a.Strength * b.StrengthRatio);
        a.Dexterity = Mathf.RoundToInt(a.Dexterity * b.DexterityRatio);
        a.Intelligence = Mathf.RoundToInt(a.Intelligence * b.IntelligenceRatio);
        a.Constitution = Mathf.RoundToInt(a.Constitution * b.ConstitutionRatio);
        a.BonusArmor = Mathf.RoundToInt(a.BonusArmor * b.BonusArmorRatio);
        a.BonusMagicResistance = Mathf.RoundToInt(a.BonusMagicResistance * b.BonusMagicResistanceRatio);

        return a;
    }
    /// <summary>
    /// Apply a global factor to the attributes
    /// </summary>
    public static Attributes operator *(Attributes a, float b)
    {
        a.Strength = Mathf.RoundToInt(a.Strength * b);
        a.Dexterity = Mathf.RoundToInt(a.Dexterity * b);
        a.Intelligence = Mathf.RoundToInt(a.Intelligence * b);
        a.Constitution = Mathf.RoundToInt(a.Constitution * b);
        a.BonusArmor = Mathf.RoundToInt(a.BonusArmor * b);
        a.BonusMagicResistance = Mathf.RoundToInt(a.BonusMagicResistance * b);

        return a;
    }
    /// <summary>
    /// Add two attributes
    /// </summary>
    public static Attributes operator +(Attributes a, Attributes b)
    {
        a.Strength += b.Strength;
        a.Dexterity += b.Dexterity;
        a.Intelligence += b.Intelligence;
        a.Constitution += b.Constitution;
        a.BonusArmor += b.BonusArmor;
        a.BonusMagicResistance += b.BonusMagicResistance;

        return a;
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
    /// The health regeneration multiply the base health regeneration of the entity (10 = 100%).
    /// </summary>
    public int HealthRegeneration;
    /// <summary>
    /// The mana regeneration multiply the base mana regeneration of the entity (10 = 100%).
    /// </summary>
    public int ManaRegeneration;
    /// <summary>
    /// The bonus armor is added to the base armor of the entity.
    /// </summary>
    public int BonusArmor;
    /// <summary>
    /// The bonus magic resistance is added to the base armor of the entity.
    /// </summary>
    public int BonusMagicResistance;
}

[System.Serializable]
/// <summary>
/// The attributes factor can be applied to the attribute to multiply/divide their values.
/// </summary>
public struct AttributesFactors
{
    /// <summary>
    /// Return a base struct with all factor at 1.
    /// </summary>
    /// <returns>New structure with all factor at 1</returns>
    public static AttributesFactors One
    {
        get
        {
            AttributesFactors ratios = new AttributesFactors
            {
                StrengthRatio = 1,
                DexterityRatio = 1,
                IntelligenceRatio = 1,
                ConstitutionRatio = 1,
                BonusArmorRatio = 1,
                BonusMagicResistanceRatio = 1
            };

            return ratios;
        }
    }


    /// <summary>
    /// Multiply the factors individually.
    /// </summary>
    public static AttributesFactors operator *(AttributesFactors a, AttributesFactors b)
    {
        a.StrengthRatio *= b.StrengthRatio; 
        a.DexterityRatio *= b.DexterityRatio; 
        a.IntelligenceRatio *= b.IntelligenceRatio; 
        a.ConstitutionRatio *= b.ConstitutionRatio; 
        a.BonusArmorRatio *= b.BonusArmorRatio; 
        a.BonusMagicResistanceRatio *= b.BonusMagicResistanceRatio;

        return a;
    }

    /// <summary>
    /// The strength changes the amount of physical damages done by the attacks (10 = 100%).
    /// </summary>
    public float StrengthRatio;
    /// <summary>
    /// The dexterity changes the number of attack by seconds (10 = 100%).
    /// </summary>
    public float DexterityRatio;
    /// <summary>
    /// The intelligence changes the effectiveness of the abilities (10 = 100%).
    /// </summary>
    public float IntelligenceRatio;
    /// <summary>
    /// The constitution changes the health of the entity (10 = 100%).
    /// </summary>
    public float ConstitutionRatio;
    /// <summary>
    /// The bonus armor is added to the base armor of the entity.
    /// </summary>
    public float BonusArmorRatio;
    /// <summary>
    /// The bonus magic resistance is added to the base armor of the entity.
    /// </summary>
    public float BonusMagicResistanceRatio;
}