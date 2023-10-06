using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Playing,
    Won,
    Dead,

}
public class GameManager : Singleton<GameManager>
{
    public Grid grid;
    public int orderCancelDelay;
    public float playerStatsBonus;
    public static GameState currentGameState = GameState.Playing;
    private void Awake()
    {
        currentGameState = GameState.Playing;
        Pooler.RecreatePools();
        ItemDatabase.instance.Reimport();
        ItemDatabase.StartUp();

        if (PlayerPrefs.HasKey("SelectedRaft"))
        {
            Instantiate(rafts[PlayerPrefs.GetInt("SelectedRaft")], spawnPoint.position, Quaternion.identity);

        }
        else
        {
            Instantiate(rafts[0], spawnPoint.position, Quaternion.identity);

        }

    }

    public List<GameObject> rafts;

    public Transform spawnPoint;

}
