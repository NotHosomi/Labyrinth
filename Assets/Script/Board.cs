using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance;
    [SerializeField] GameObject prefabL;
    [SerializeField] GameObject prefabR;
    [SerializeField] GameObject prefabT;
    [SerializeField] GameObject overlay;
    public int size = 7;
    public int size2 = 49;
    Tile extraTile;
    Tile[] tiles;

    void Awake()
    {
        Tile.board = this;
        tiles = new Tile[size2];
        for(int i = 0; i < size2; ++i)
        {
            if (i % (2 * size) < size && i % 2 == 0)
                continue;
            tiles[i] = randomTile();
            tiles[i].transform.position = getPosFromIndex(i);
            Instantiate(overlay, tiles[i].transform, false).GetComponent<SpriteRenderer>();
        }
        // row 0
        tiles[0] = newTile('r', 0, 0, 1);
        tiles[2] = newTile('T', 2, 0, 1);
        tiles[4] = newTile('T', 4, 0, 1);
        tiles[6] = newTile('r', 6, 0, 2);
        // row 2
        tiles[14] = newTile('T', 0, 2, 0);
        tiles[16] = newTile('T', 2, 2, 0);
        tiles[18] = newTile('T', 4, 2, 1);
        tiles[20] = newTile('T', 6, 2, 2);
        // row 4
        tiles[28] = newTile('T', 0, 4, 0);
        tiles[30] = newTile('T', 2, 4, 3);
        tiles[32] = newTile('T', 4, 4, 2);
        tiles[34] = newTile('T', 6, 4, 2);
        // row 6
        tiles[42] = newTile('r', 0, 6, 0);
        tiles[44] = newTile('T', 2, 6, 3);
        tiles[46] = newTile('T', 4, 6, 3);
        tiles[48] = newTile('r', 6, 6, 3);



        extraTile = randomTile();
        extraTile.transform.position = new Vector3(-2.5f, 4.5f, 0);
        extraTile.transform.localScale = new Vector3(2f, 2f, 1);
    }

    int L_count = 12;
    int R_count = 16;
    int T_count = 6;
    Tile randomTile()
    {
        Tile t;
        int roll = Random.Range(0, L_count + R_count + T_count); //34
        int rot = Random.Range(0, 3);
        if (roll < L_count) // l 12
        {
            t = Instantiate(prefabL).GetComponent<Tile>();
            t.rotate(rot);
            --L_count;
        }
        else if (roll < L_count + R_count) // r 16
        {
            t = Instantiate(prefabR).GetComponent<Tile>();
            t.rotate(rot);
            --R_count;
        }
        else // t 6
        {
            t = Instantiate(prefabT).GetComponent<Tile>();
            t.rotate(rot);
            --T_count;
        }
        return t;
    }

    Tile newTile(char type, int x, int y, int rot)
    {
        Tile t;
        switch (type)
        {
            case 'l':
                t = Instantiate(prefabL).GetComponent<Tile>();
                break;
            case 'r':
                t = Instantiate(prefabR).GetComponent<Tile>();
                break;
            default: // T
                t = Instantiate(prefabT).GetComponent<Tile>();
                break;
        }
        t.rotate(rot);
        t.transform.position = new Vector2(x, y);
        return t;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
            extraTile.rotate(1);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow))
            extraTile.rotate(3);
    }

    public Tile getTile(Vector2 pos)
    {
        return getTile(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }
    public Tile getTile(int x, int y)
    {
        if(x < 0 || x >= size || y < 0 || y >= size)
            return null;
        return tiles[getIndexFromPos(x,y)];
    }
    Vector2 getPosFromIndex(int i)
    {
        Vector2 pos;
        pos.y = i / size;
        pos.x = i % size;
        return pos;
    }
    int getIndexFromPos(int x, int y)
    {
        return y * size + x;
    }
    int getIndexFromPos(Vector2 pos)
    {
        return Mathf.RoundToInt(pos.y) * size + Mathf.RoundToInt(pos.x);
    }
    public List<Tile> getRow(int y)
    {
        List<Tile> row = new List<Tile>();
        for (int i = 0; i < size; ++i)
            row.Add(tiles[getIndexFromPos(i, y)]);
        return row;
    }
    public List<Tile> getCol(int x)
    {
        List<Tile> col = new List<Tile>();
        for (int i = 0; i < size; ++i)
            col.Add(tiles[getIndexFromPos(x, i)]);
        return col;
    }

    // foreach p in players
    //   Vector3 pos = p.transform.position
    //   if(pos.x == 7)
    //      pos.x = 0
    //   if(pos.x == -1)
    //      pos.x = 6
    //   if(pos.y == 6)
    //      pos.y = 0
    //   if(pos.y == -1)
    //      pos.y = 6
    //   p.transform.position = pos;

    public void insertDown(int col)
    {
        Tile buffer = tiles[getIndexFromPos(col, 0)];
        for (int i = getIndexFromPos(col, 0); i < getIndexFromPos(col, size-1); i += size)
        {
            tiles[i] = tiles[i + size];
        }
        extraTile.transform.position = new Vector3(col, 7 - Inserter.nudge_dist, 0);
        extraTile.transform.localScale = new Vector3(1, 1, 1);
        tiles[getIndexFromPos(col, size-1)] = extraTile;
        extraTile = buffer;
        for (int i = 0; i < extraTile.players.Count; ++i)
        {
            Vector3 pos = extraTile.players[0].transform.position;
            if (pos.y < 0)
            {
                pos.y = size - 1;
                extraTile.players[0].place(pos);
            }
            Debug.Log("Pos: " + pos);
        }

        List<Tile> slice = getCol(col);
        foreach (Tile t in slice)
        {
            t.slide(2, 1 - Inserter.nudge_dist);
        }
        extraTile.exitSlide(2);
    }
    public void insertUp(int col)
    {
        Tile buffer = tiles[getIndexFromPos(col, size - 1)];
        for (int i = getIndexFromPos(col, size-1); i > getIndexFromPos(col, 0); i -= size)
        {
            tiles[i] = tiles[i - size];
            Debug.Log("i=" + i + " " + getPosFromIndex(i));
        }
        extraTile.transform.position = new Vector3(col, -1 + Inserter.nudge_dist, 0);
        extraTile.transform.localScale = new Vector3(1, 1, 1);
        tiles[getIndexFromPos(col, 0)] = extraTile;
        extraTile = buffer;
        for (int i = 0; i < extraTile.players.Count; ++i)
        {
            Vector3 pos = extraTile.players[0].transform.position;
            if (pos.y >= size)
            {
                pos.y = 0;
                extraTile.players[0].place(pos);
            }
        }

        List<Tile> slice = getCol(col);
        foreach (Tile t in slice)
        {
            t.slide(0, 1 - Inserter.nudge_dist);
        }
        extraTile.exitSlide(0);
        extraTile.transform.position = new Vector3(-2, 3, 0);
    }
    public void insertRight(int row)
    {
        Tile buffer = tiles[getIndexFromPos(size - 1, row)];
        for (int i = getIndexFromPos(size - 1, row); i > getIndexFromPos(0, row); --i)
        {
            tiles[i] = tiles[i -1];
        }
        extraTile.transform.position = new Vector3(-1 + Inserter.nudge_dist, row, 0);
        extraTile.transform.localScale = new Vector3(1, 1, 1);
        tiles[getIndexFromPos(0, row)] = extraTile;
        extraTile = buffer;

        List<Tile> slice = getRow(row);
        foreach (Tile t in slice)
        {
            t.slide(1, 1 - Inserter.nudge_dist);
        }
        extraTile.exitSlide(1);
    }

    public void insertLeft(int row)
    {
        Tile buffer = tiles[getIndexFromPos(0, row)];
        for (int i = getIndexFromPos(0, row); i < getIndexFromPos(size - 1, row); ++i)
        {
            tiles[i] = tiles[i + 1];
        }
        extraTile.transform.position = new Vector3(7 - Inserter.nudge_dist, row, 0);
        extraTile.transform.localScale = new Vector3(1, 1, 1);
        tiles[getIndexFromPos(size - 1, row)] = extraTile;
        extraTile = buffer;


        List<Tile> slice = getRow(row);
        foreach (Tile t in slice)
        {
            t.slide(3, 1 - Inserter.nudge_dist);
        }
        extraTile.exitSlide(3);

    }

    public List<Tile> pathable;
    // Returns false if the player cannot move
    public bool navReset(Tile origin)
    {
        List<Tile> pathable = new List<Tile>();
        foreach (Tile t in tiles)
        {
            t.nav_checked = false;
            t.navigable = false;
        }
        // add navigable tiles to nav calc somewhere
        // find them thru iter
        // or add them on discovery
        origin.navCalc();
        foreach (Tile t in tiles)
        {
            if (t.navigable)
                pathable.Add(t);
        }
        if (pathable.Count == 1)
            return false;
        return true;
    }

    public void greyOut()
    {
        foreach(Tile t in tiles)
        {
            if(!t.navigable)
                t.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.5f);
        }
    }
    public void resetCol()
    {
        foreach (Tile t in tiles)
        {
            t.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
