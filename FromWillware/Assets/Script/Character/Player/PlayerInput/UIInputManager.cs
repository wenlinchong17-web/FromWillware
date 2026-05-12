using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputManager : MonoBehaviour
{
    public static UIInputManager Instance;

    public GameObject virtualMouse;

    public PlayerMove playerMove;
    public CameraFollow cameraFollow;

    private void Awake()
    {
        Instance = this;
    }

    public void EnterUI()
    {
        virtualMouse.SetActive(true);

        playerMove.enabled = false;

        cameraFollow.enabled = false;
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
         // 关键：禁用系统鼠标参与 UI
        //InputSystem.DisableDevice(Mouse.current);
    }

    public void ExitUI()
    {
        virtualMouse.SetActive(false);

        playerMove.enabled = true;

        cameraFollow.enabled = true;
        Cursor.visible = true;
        //InputSystem.EnableDevice(Mouse.current);
    }
}