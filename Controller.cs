using UnityEngine;
using UnityEngine.InputSystem;
public enum ControllerAction
{
    NULL,
    Move,
    Roll,
    ActionWithMainHand,
    ActionWithOffHand,
    Look,

}

public enum ControllerState
{
    mainControlls,
    ui,
    Dead,
}

public class Controller : Singleton<Controller>
{

    private bool buttonHeld;
    private InputAction.CallbackContext currentContext;
    public InputActionAsset inputActions;

    public void SwitchState(ControllerState controllerMode)
    {
        switch (controllerMode)
        {
            case ControllerState.mainControlls:
                inputActions.FindActionMap("Player").Enable();

                break;
            case ControllerState.ui:
                inputActions.FindActionMap("Player").Disable();

                break;
            case ControllerState.Dead:
                inputActions.FindActionMap("Player").Disable();

                break;
            default:
                break;
        }
    }
    private void Awake()
    {
        foreach (var map in inputActions.actionMaps)
        {
            map.Enable();
            foreach (var action in map.actions)
            {
                action.Enable();
                var method = typeof(Controller).GetMethod(action.name);
                if (method != null)
                {
                    action.performed += delegate { method.Invoke(this, new object[0]); };
                }
                else
                {
                    Debug.Log("Method not found for " + action.name);
                }
            }
        }
    }

    public bool IsCurrentControllerStateAllowAction(ControllerAction action)
    {
        return true;
    }

    private void Update()
    {
        if (PlayersRaft.instance != null)
        {
            var moveAction = inputActions.FindAction("Move");

            PlayersRaft.instance.Move(moveAction.ReadValue<Vector2>());
            if (!KatabasisUtillsClass.IsPointerOverUIElement(Mouse.current.position.ReadValue()))
            {
                var mousePosition = Mouse.current.position.ReadValue();

                Ray ray = MainCameraHandler.instance.cam.ScreenPointToRay(mousePosition);
                Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
                float distance;
                xy.Raycast(ray, out distance);
                ;
                var realMousePos = ray.GetPoint(distance);
                PlayersRaft.instance.Look(realMousePos);
            }

        }
    }

    public void Move()
    {

    }

    public void Fire()
    {
        if (!KatabasisUtillsClass.IsPointerOverUIElement(Mouse.current.position.ReadValue()))
        {
            var mousePosition = Mouse.current.position.ReadValue();
            Ray ray = MainCameraHandler.instance.cam.ScreenPointToRay(mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            float distance;
            xy.Raycast(ray, out distance);
            ;
            var mousePos = ray.GetPoint(distance);
            PlayersRaft.instance.Fire(mousePos);
        }
    }
    public void Look()
    {

    }

}
