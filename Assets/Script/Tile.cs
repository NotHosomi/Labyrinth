using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static Board board;
    public bool[] connections; // NESW
    public int treasure_id;

    public List<Player> players = new List<Player>();
    // Treasure treasure;

    public void Awake()
    {
        //players = new List<Player>();
    }

    public void init(char type)
    {
        switch(type)
        {
            case 'l': connections = new bool[4] { true, false, true, false };
                break;
            case 'r': connections = new bool[4] { true, true, false, false };
                break;
            case 'T': connections = new bool[4] { true, true, true, false };
                break;
            default: Debug.Log("Invalid tile type \"" + type + "\"");
                break;
        }
    }

    int rotation = 0;
    // right: times=1, left: times=3
    public void rotate(int times)
    {
        rotation += times;
        bool[] newCons = new bool[4];
        int offset;
        for (int i=0; i<4; ++i)
        {
            offset = i + times;
            offset %= 4;
            newCons[i] = connections[offset];
        }
        connections = newCons;

        Quaternion quat = transform.rotation;
        Vector3 rot = quat.eulerAngles;
        rot.z += 90 * times;
        quat.eulerAngles = rot;
        transform.rotation = quat;
    }

    public bool navigable = false;
    public bool nav_checked = false;
    // could track recursion parent for route path
    public void navCalc()
    {
        navigable = true;
        board.pathable.Add(this);
        navCalcDir(transform.position.x, transform.position.y + 1, 0);
        navCalcDir(transform.position.x + 1, transform.position.y, 1);
        navCalcDir(transform.position.x, transform.position.y - 1, 2);
        navCalcDir(transform.position.x - 1, transform.position.y, 3);
    }
    void navCalcDir(float x, float y, int dir)
    {
        Tile target = board.getTile(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        if (target == null)
            return;
        if (!target.navigable)
        {
            if (connections[dir] && target.connections[(dir+2)%4])
            {
                target.navigable = true;
                target.navCalc();
            }
        }
    }

    public void slide(int dir, float amount = 1)
    {
        Vector3 pos = transform.position;
        switch (dir)
        {
            case 0:
                pos.y += amount;
                break;
            case 1:
                pos.x += amount;
                break;
            case 2:
                pos.y -= amount;
                break;
            case 3:
                pos.x -= amount;
                break;
        }
        transform.position = pos;
    }
    public void exitSlide(int dir)//, int newPlayer)
    {
        Vector3 pos = transform.position;
        switch (dir)
        {
            case 0:
                pos.y += 0.7f;
                break;
            case 1:
                pos.x += 0.7f;
                break;
            case 2:
                pos.y -= 0.7f;
                break;
            case 3:
                pos.x -= 0.7f;
                break;
        }
        transform.position = pos;
        foreach(Player p in players)
            p.transform.position = pos;

        // yield return wait

        // TODO: go to player's tray
        //Vector2 tray_loc;
        //switch(GM.gm.getActiveIndex)
        //{
        //    default: tray_loc = 
        //        break;
        //    case 1:
        //    case 2:
        //    case 3:
        //}
        //transform.position = tray_loc;

        transform.position = new Vector3(-2.5f, 3, 0);
        transform.localScale = new Vector3(2, 2, 0);
        for (int i = 0; i < players.Count; ++i)
        {
            pos = players[0].transform.position;
            switch(dir)
            {
                case 1:
                    if (pos.x >= board.size)
                    {
                        pos.x = 0;
                        players[0].place(pos);
                    }
                    break;
                case 3:
                    if (pos.x < 0)
                    {
                        pos.x = size - 1;
                        extraTile.players[0].place(pos);
                    }
                    break;
            }
        }
    }
}
