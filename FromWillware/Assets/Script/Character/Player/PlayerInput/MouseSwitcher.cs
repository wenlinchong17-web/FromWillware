using UnityEngine;
using UnityEngine.InputSystem;

public class MouseSwitcher : MonoBehaviour
{
    public GameObject virtualCursor;

    void Update()
    {
        bool usingGamepad =
            Gamepad.current != null &&
            Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.01f;

        bool usingRealMouse =
            Mouse.current != null &&
            Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f;

        if (usingGamepad)
        {
            Cursor.visible = false;
            virtualCursor.SetActive(true);
        }

        if (usingRealMouse)
        {
            Cursor.visible = true;
            virtualCursor.SetActive(false);
        }
    }
}