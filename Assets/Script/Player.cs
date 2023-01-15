using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile current_tile;
    public int id;

    private void Awake()
    {
    }

    public void init(Vector2 pos, Color col, int _id)
    {
        place(pos);
        GetComponent<SpriteRenderer>().color = col;
        id = _id;
    }

    public void place(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, 0);
        if(current_tile != null)
            current_tile.players.Remove(this);
        current_tile = Tile.board.getTile(pos); // returning null ???
        Debug.Log(current_tile);
        current_tile.players.Add(this);

        // check treasure
    }
}
