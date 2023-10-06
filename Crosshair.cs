using UnityEngine;
using UnityEngine.InputSystem;
public class Crosshair : MonoBehaviour
{
    private void Update()
    {
        if (PlayersRaft.instance != null)
        {
            transform.position = Mouse.current.position.ReadValue();

        }
    }
}
