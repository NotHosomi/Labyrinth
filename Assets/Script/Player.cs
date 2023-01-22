using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile current_tile;
    public int id;
    List<int> goals;
    List<GameObject> cards;
    Vector2 card_pile;
    Vector2 discard_pile;
    Vector2 stack_dir;

    private void Awake()
    {
        cards = new List<GameObject>();
    }

    public void init(Vector2 pos, Color col, int _id)
    {
        place(pos);
        GetComponent<SpriteRenderer>().color = col;
        id = _id;
        // set card positions
        switch (id)
        {
            case 0:
                card_pile = new Vector2(7.5f, 6);
                discard_pile = new Vector2(9f, 6);
                stack_dir = new Vector2(1, -1);
                break;
            case 1:
                card_pile = new Vector2(7.5f, 0);
                discard_pile = new Vector2(9f, 0);
                stack_dir = new Vector2(1, 1);
                break;
            case 2:
                card_pile = new Vector2(-1.5f, 0);
                discard_pile = new Vector2(-3f, 0);
                stack_dir = new Vector2(-1, 1);
                break;
            case 3:
                card_pile = new Vector2(-1.5f, 6);
                discard_pile = new Vector2(-3f, 6);
                stack_dir = new Vector2(-1, -1);
                break;
        }
        for(int i = 0; i < cards.Count; ++i)
        {
            cards[i].transform.position = card_pile;
            cards[i].transform.position += new Vector3(stack_dir.x, stack_dir.y, i+1) * 0.1f;
        }
    }

    public void place(Vector2 pos)
    {
        Debug.Log("Place @ " + pos);
        if(current_tile != null)
            current_tile.players.Remove(this);
        current_tile = Tile.board.getTile(pos);
        current_tile.players.Add(this);
        transform.position = current_tile.transform.position;

        transform.parent = current_tile.transform;
        // check treasure
        if(current_tile.treasure_id == goals[goals.Count-1])    // these two values are different, probs a mistake in the assigning processing
        {
            onGetTreasure();
        }
    }

    public void setGoals(List<int> g)
    {
        goals = g;
    }

    void onGetTreasure()
    {
        Debug.Log("Player " + id + " collected goal " + goals[goals.Count - 1]);
        goals.RemoveAt(goals.Count - 1);
        GameObject c = cards[cards.Count - 1];
        c.transform.position = discard_pile;
    }
    public void addCard(GameObject card)
    {
        cards.Add(card);
    }
}
