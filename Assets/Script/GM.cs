using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM gm;
    [SerializeField] GameObject player_prefab;
    int active_player = 0;
    bool move_stage;

    // Start is called before the first frame update
    void Awake()
    {
        if (gm == null)
            gm = this;

        Inserter.inserters = new List<Inserter>();
    }

    private void Start()
    {
        createPlayers();
    }

    List<Player> players;
    void createPlayers()
    {
        players = new List<Player>();
        for(int i = 0; i < 4; ++i)
        {
            players.Add(Instantiate(player_prefab).GetComponent<Player>());
        }
        players[0].init(new Vector2(0, 0), Color.blue, 0);
        players[1].init(new Vector2(0, Tile.board.size - 1), Color.red, 1);
        players[2].init(new Vector2(Tile.board.size - 1, Tile.board.size - 1), new Color(0.5f, 0, 0.5f), 2);
        players[3].init(new Vector2(Tile.board.size - 1, 0), Color.green, 3);
    }

    public void onPlayerMove()
    {
        // prep next turn
        active_player++;
        foreach (Inserter i in Inserter.inserters)
            i.disabled = false;
        Inserter.current_inactive.disabled = true;
    }
    public void onTilePlace()
    {
        // prep movement
        Tile.board.navReset(players[active_player].current_tile);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile target = Tile.board.getTile(pos);
            if (target.navigable)
            {
                players[active_player].place(target.transform.position);
            }
        }
    }
    List<Tile> pathable;
}
