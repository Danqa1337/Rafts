using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public class PlayersRaft : Singleton<PlayersRaft>
{
    public float speed;
    public float2 target;
    public Raft raft;
    private float cooldown;

    private void Awake()
    {
        raft = GetComponent<Raft>();

        CameraFolow.instance.target = transform;

    }
    private void Update()
    {
        cooldown -= Time.deltaTime;
    }
    public void Die()
    {
        MainCameraHandler.instance.transform.SetParent(null);
        if (Controller.instance != null)
        {
            GameManager.currentGameState = GameState.Dead;
            Controller.instance.SwitchState(ControllerState.Dead);
            UiManager.ToggleUICanvas(UiManager.Menu.Death, UiManager.UICanvasState.On);
        }
    }
    public Entity entity => raft.GetComponent<EntityLink>().entity;
    public void Move(Vector2 movementVector)
    {

        if (cooldown < 0 && movementVector != Vector2.zero)
        {
            cooldown = 0.1f;
            OrderSetSystem.SetOrder(new OrderComponent(Order.Push, movementVector));

        }
    }
    public void Fire(Vector2 fireVector)
    {
        OrderSetSystem.SetOrder(new OrderComponent(Order.Attack, fireVector));
    }
    public void Look(Vector2 mousePosition)
    {
        target = mousePosition;
    }
}
