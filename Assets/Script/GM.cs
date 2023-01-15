using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    static GM gm;
    [SerializeField] GameObject player_prefab;

    // Start is called before the first frame update
    void Awake()
    {
        if (gm == null)
            gm = this;
    }

    List<Player> players;
    void createPlayers()
    {
        players = new List<Player>();
        for(int i = 0; i < 4; ++i)
        {
            players.Add(Instantiate(player_prefab).GetComponent<Player>());
        }
        players[0].transform.position = new Vector3(0, 0, 0);
        Tile.board.getTile(0, 0).players.Add(players[0]);
        players[1].transform.position = new Vector3(0, Tile.board.size - 1, 0);
        Tile.board.getTile(0, Tile.board.size - 1).players.Add(players[1]);
        players[2].transform.position = new Vector3(Tile.board.size - 1, Tile.board.size - 1, 0);
        Tile.board.getTile(Tile.board.size - 1, Tile.board.size - 1).players.Add(players[2]);
        players[3].transform.position = new Vector3(Tile.board.size - 1, 0, 0);
        Tile.board.getTile(Tile.board.size - 1, 0).players.Add(players[3]);
    }




    public int active_player = 0;
    public bool placing_tile = true;
    
}
