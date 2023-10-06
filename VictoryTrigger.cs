using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayersRaft>() != null)
        {
            GameManager.currentGameState = GameState.Won;
            UiManager.ToggleUICanvas(UiManager.Menu.Victory);
        }
    }
}
