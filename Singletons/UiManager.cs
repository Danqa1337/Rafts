using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : Singleton<UiManager>
{
    public enum Menu
    {
        Inventory,
        Pause,
        Console,
        Describer,
        Death,
        Log,
        CharacterSellection,
        Hud,
        Victory,
    }
    public enum UICanvasState
    {
        On,
        Off,
        Null,
    }
    public Camera mainCam;

    public Canvas hudCanvas;
    public Canvas victoryCanvas;
    public Canvas inventoryCanvas;
    public Canvas pauseCanvas;
    public Canvas consoleCanvas;
    public Canvas describerCanvas;
    public Canvas deathCanvas;
    public Canvas logCanvas;
    public Canvas CharacterSellectionCanvas;

    public static Action OnInventoryOpened;
    public static Action OnInventoryClosed;

    public static Stack<Menu> menuStack;
    private void Awake()
    {

        mainCam = Camera.main;
        menuStack = new Stack<Menu>();

        ToggleUICanvas(Menu.Hud, UICanvasState.On);
    }

    public static void UpdatePositions()
    {
        instance.gameObject.SetActive(false);
        instance.gameObject.SetActive(true);
    }
    public static void ToggleUICanvas(Menu menu, UICanvasState state = UICanvasState.Null)
    {

        Canvas[] canvases = null;
        Action openAction = null;
        Action closeAction = null;
        switch (menu)
        {
            case Menu.Inventory:
                canvases = new Canvas[] { instance.inventoryCanvas };
                openAction = OnInventoryOpened;
                closeAction = OnInventoryClosed;
                break;
            case Menu.Pause:
                canvases = new Canvas[] { instance.pauseCanvas };
                break;
            case Menu.Console:
                canvases = new Canvas[] { instance.consoleCanvas };
                break;
            case Menu.Describer:
                canvases = new Canvas[] { instance.describerCanvas };
                break;
            case Menu.Death:
                canvases = new Canvas[] { instance.deathCanvas };
                break;
            case Menu.Log:
                canvases = new Canvas[] { instance.logCanvas };
                break;
            case Menu.CharacterSellection:
                canvases = new Canvas[] { instance.CharacterSellectionCanvas };
                break;
            case Menu.Hud:
                canvases = new Canvas[] { instance.hudCanvas };
                break;
            case Menu.Victory:
                canvases = new Canvas[] { instance.victoryCanvas };
                break;

        }

        if (state == UICanvasState.Null)
        {

            if (canvases[0].renderMode == RenderMode.ScreenSpaceOverlay) CloseLast();
            else ToggleUICanvas(menu, UICanvasState.On);
        }

        switch (state)
        {
            case UICanvasState.On:
                foreach (var canvas in canvases)
                {
                    canvas.enabled = true;
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    if (openAction != null) openAction?.Invoke();
                    Debug.Log(menu + " opened");
                }
                menuStack.Push(menu);
                break;
            case UICanvasState.Off:
                foreach (var camera in canvases)
                {
                    camera.enabled = false;

                    camera.renderMode = RenderMode.WorldSpace;
                    camera.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
                    if (openAction != null) closeAction?.Invoke();
                    Debug.Log(menu + " closed");

                }
                if (menuStack.Count > 0) menuStack.Pop();
                break;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (menuStack.Count == 0)
            {
                Controller.instance.SwitchState(ControllerState.mainControlls);
            }
            else
            {
                Controller.instance.SwitchState(GetStateFromMenu(menuStack.Peek()));
            }
        }
        UpdatePositions();
    }
    public static void CloseAll()
    {
        ToggleUICanvas(Menu.Console, UICanvasState.Off);
        ToggleUICanvas(Menu.Describer, UICanvasState.Off);
        ToggleUICanvas(Menu.Death, UICanvasState.Off);
        ToggleUICanvas(Menu.Inventory, UICanvasState.Off);
        ToggleUICanvas(Menu.Pause, UICanvasState.Off);
    }
    private static ControllerState GetStateFromMenu(Menu menu) => (menu) switch
    {
        _ => ControllerState.ui,
    };

    public static void CloseLast()
    {
        if (menuStack.Count > 0)
        {
            Menu menu = menuStack.Pop();
            ToggleUICanvas(menu, UICanvasState.Off);
            if (menuStack.Count == 0) Controller.instance.SwitchState(ControllerState.mainControlls);
        }
    }

}
