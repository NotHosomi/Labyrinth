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
        Debug.Log("Place @ " + pos);
        transform.position = new Vector3(pos.x, pos.y, 0);
        if(current_tile != null)
            current_tile.players.Remove(this);
        current_tile = Tile.board.getTile(pos);
        current_tile.players.Add(this);

        transform.parent = current_tile.transform;
        // check treasure
    }

    void checkOOB()
    {
        bool hit = false;
        Vector3 pos = transform.position;
        if (pos.x >= Tile.board.size)
        {
            pos.x = 0;
            hit = true;
        }
        else if (pos.x < 0)
        { 
            pos.x = Tile.board.size - 1;
            hit = true;
        }
        if (pos.y >= Tile.board.size)
        { 
            pos.y = 0;
            hit = true;
        }
        else if (pos.y < 0)
        { 
            pos.y = Tile.board.size - 1;
            hit = true;
        }
        if (hit)
            place(pos);
    }
}
