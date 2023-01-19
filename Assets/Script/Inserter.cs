using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inserter : MonoBehaviour
{
    // NESW
    [SerializeField] int dir;
    [SerializeField] int pos;
    public bool disabled;
    [SerializeField] Inserter opposite;
    public static Inserter last;

    public static Inserter current_inactive = null;
    public static List<Inserter> inserters;

    public static float nudge_dist = 0.25f;

    public void Start()
    {
        inserters.Add(this);
    }

    public void OnClick()
    {
        if (disabled)
            return;
        opposite.disabled = true;
        opposite.GetComponent<Image>().color = Color.gray;

        if (current_inactive != null)
        {
            current_inactive.GetComponent<Image>().color = Color.red;
        }
        current_inactive = opposite;
        GetComponent<Image>().color = Color.red * 0.8f;

        switch (dir)
        {
            case 0:
                Tile.board.insertUp(Mathf.RoundToInt(transform.position.x));
                break;
            case 1:
                Tile.board.insertRight(Mathf.RoundToInt(transform.position.y));
                break;
            case 2:
                Tile.board.insertDown(Mathf.RoundToInt(transform.position.x));
                break;
            case 3:
                Tile.board.insertLeft(Mathf.RoundToInt(transform.position.y));
                break;
        }
        last = this;
        GM.gm.onTilePlace();
    }

    public void OnHover()
    {
        if (disabled)
            return;

        List<Tile> slice;
        if ((dir & 1) == 1)
            slice = Tile.board.getRow(pos);
        else
            slice = Tile.board.getCol(pos);
        foreach(Tile t in slice)
        {
            t.slide(dir, nudge_dist);
        }

        GetComponent<Image>().color = Color.red;

    }

    public void OnExitHover()
    {
        if (disabled)
            return;

        List<Tile> slice;
        if ((dir & 1) == 1)
            slice = Tile.board.getRow(pos);
        else
            slice = Tile.board.getCol(pos);
        foreach (Tile t in slice)
        {
            t.slide(dir, -nudge_dist);
        }
        GetComponent<Image>().color = Color.red * 0.8f;
    }
}