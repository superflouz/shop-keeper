using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterAbility
{
    public Ability ability;
    public int manaCost;
    public int cooldown;
}

public class Ability : MonoBehaviour
{

}
