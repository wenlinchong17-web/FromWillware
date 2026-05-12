using UnityEngine;
using UnityEngine.InputSystem;

public class InputModeManager : MonoBehaviour
{
    public PlayerInput playerInput;

    public void EnterUI()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }

    public void ExitUI()
    {
        playerInput.SwitchCurrentActionMap("Gameplay");
    }
}