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

    static Inserter prev_inactive = null;

    public void OnClick()
    {
        if (disabled)
            return;
        disabled = true;
        GetComponent<Image>().color = Color.gray;

        if (prev_inactive != null)
        {
            prev_inactive.disabled = false;
            prev_inactive.GetComponent<Image>().color = Color.red;
        }
        prev_inactive = this;

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
            t.slide(dir, 0.3f);
        }
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
            t.slide(dir, -0.3f);
        }
    }
}
