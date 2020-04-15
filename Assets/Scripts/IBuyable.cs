using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBuyable
{
    Sprite Icon { get; }
    string Name { get; }
    int Price { get; }
}
