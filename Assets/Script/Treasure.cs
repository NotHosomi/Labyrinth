using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Treasure : MonoBehaviour
{
    public static Treasure _i;
    private void Awake()
    {
        if (_i == null)
            _i = this;
    }


    public const int num_treasures = 24;
    [SerializeField] List<GameObject> available_treasure;
    List<GameObject> determined_treasures;
    List<int> unplaced_treasures;

    [SerializeField] GameObject card_prefab;
    List<List<int>> player_goals;
    public void instatiateTreasures(ref List<Player> players)
    {
        player_goals = new List<List<int>>();
        for (int i = 0; i < players.Count; ++i)
        {
            player_goals.Add(new List<int>());
        }
        // assign goal IDs to players
        int player_id = 0;
        List<int> ids = Enumerable.Range(0, num_treasures).ToList();
        for (;ids.Count > 0;)
        {
            int selection = Random.Range(0, ids.Count);
            player_goals[player_id].Add(ids[selection]);
            ids.RemoveAt(selection);
            player_id = (player_id + 1) % players.Count;
        }

        // generate treasure objects
        determined_treasures = new List<GameObject>();
        for (int i = 0; i < num_treasures; ++i)
        {
            int selection = Random.Range(0, available_treasure.Count);
            determined_treasures.Add(available_treasure[selection]);
            available_treasure.RemoveAt(selection);
        }

        // create cards
        for(int p = 0; p < player_goals.Count; ++p)
        {
            players[p].setGoals(player_goals[p]);
            foreach(int id in player_goals[p])
            {
                GameObject card = Instantiate(card_prefab);
                players[p].addCard(card);
                GameObject icon = Instantiate(determined_treasures[id], card.transform, false);
                icon.transform.position += new Vector3(0, 0, -0.01f);
            }
        }

        unplaced_treasures = Enumerable.Range(0, num_treasures).ToList();
    }
     
    public void placeTreasure(Tile t)
    {
        int selection = Random.Range(0, unplaced_treasures.Count);
        t.treasure_id = unplaced_treasures[selection];
        unplaced_treasures.RemoveAt(selection);
        GameObject treasure = Instantiate(determined_treasures[t.treasure_id], t.transform, false);
        treasure.transform.position += new Vector3(0, 0, 0.05f);
        treasure.name = "treasure";
    }
}
