using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM gm;
    [SerializeField] GameObject player_prefab;
    [SerializeField] int player_count = 4;
    [SerializeField] List<Color> player_cols;
    int active_player = 0;
    public int getActiveIndex()
    {
        return active_player;
    }
    public Player getActivePlayer()
    {
        return players[active_player];
    }
    bool move_stage;

    // Start is called before the first frame update
    void Awake()
    {
        if (gm == null)
            gm = this;
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
        for(int i = 0; i < player_count; ++i)
        {
            players.Add(Instantiate(player_prefab).GetComponent<Player>());
        }
        switch(player_count)
        {
            case 4:
                players[3].init(new Vector2(0, Tile.board.size - 1), player_cols[3], 1);
                goto case 3;
            case 3:
                players[2].init(new Vector2(0, 0), player_cols[2], 0);
                break;
        }
        players[1].init(new Vector2(Tile.board.size - 1, 0), player_cols[1], 3);
        players[0].init(new Vector2(Tile.board.size - 1, Tile.board.size - 1), player_cols[0], 2);
        //if(player_count > )
    }

    // begin movement phase
    public void onTilePlace()
    {
        // prep movement
        bool can_move = Tile.board.navReset(players[active_player].current_tile);
        if (can_move)
        {
            move_stage = true;
            foreach (Inserter i in Inserter.inserters)
                i.disabled = true;
            Tile.board.greyOut();
        }
        else
        {
            Inserter.last.OnHover();
            endTurn();
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && move_stage)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile target = Tile.board.getTile(pos);
            if (target != null && target.navigable)
            {
                players[active_player].place(target.transform.position);
                move_stage = false;
                endTurn();
            }
        }
    }
    List<Tile> pathable;

    void endTurn()
    {
        active_player = (active_player + 1) % player_count;

        foreach (Inserter i in Inserter.inserters)
            i.disabled = false;
        Inserter.current_inactive.disabled = true;
        Tile.board.resetCol();

        Color bg = player_cols[active_player] * 0.3f;
        for (int i = 0; i < 3; ++i)
            if (bg[i] == 0)
                bg[i] = 0.1f;
        bg.a = 255;
        Camera.main.backgroundColor = bg;
    }



    // ================
    // Treasure Manager
    // ================
    //void createTreasures()
}
