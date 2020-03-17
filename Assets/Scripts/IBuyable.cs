using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyable
{
    Sprite Icon { get; }
    int Price { get; }
}
