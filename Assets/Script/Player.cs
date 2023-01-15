using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    List<Tile> pathable;
    Tile current_tile;

    private void Awake()
    {
        pathable = new List<Tile>();
    }
}
