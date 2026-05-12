using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class GamepadVirtualMouse : MonoBehaviour
{
    public RectTransform cursorTransform;
    public Canvas canvas;

    public float cursorSpeed = 1000f;

    private Mouse virtualMouse;
    private Mouse realMouse;

    private bool previousMouseState;

    void OnEnable()
    {
        realMouse = Mouse.current;

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }

        InputSystem.EnableDevice(virtualMouse);

        Vector2 startPos = new Vector2(Screen.width / 2f, Screen.height / 2f);

        InputState.Change(virtualMouse.position, startPos);

        AnchorCursor(startPos);
    }

    void OnDisable()
    {
        if (virtualMouse != null && virtualMouse.added)
        {
            InputSystem.RemoveDevice(virtualMouse);
        }
    }

    void Update()
    {
        if (Gamepad.current == null)
        {
            HideVirtualMouse();
            return;
        }

        Vector2 stickValue = Gamepad.current.leftStick.ReadValue();

        bool usingGamepad = stickValue.sqrMagnitude > 0.01f;

        // 真鼠标移动
        if (realMouse.delta.ReadValue().sqrMagnitude > 0.01f)
        {
            HideVirtualMouse();
            Cursor.visible = true;
            return;
        }

        if (usingGamepad)
        {
            ShowVirtualMouse();
            Cursor.visible = false;

            Vector2 deltaValue = stickValue * cursorSpeed * Time.deltaTime;

            Vector2 currentPos = virtualMouse.position.ReadValue();

            Vector2 newPos = currentPos + deltaValue;

            newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
            newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

            InputState.Change(virtualMouse.position, newPos);

            AnchorCursor(newPos);

            // A键点击
            bool aButtonPressed = Gamepad.current.buttonSouth.isPressed;

            if (previousMouseState != aButtonPressed)
            {
                virtualMouse.CopyState<MouseState>(out var mouseState);

                mouseState.WithButton(MouseButton.Left, aButtonPressed);

                InputState.Change(virtualMouse, mouseState);

                previousMouseState = aButtonPressed;
            }

            InputSystem.Update();
        }
    }

    void AnchorCursor(Vector2 position)
    {
        cursorTransform.position = position;
    }

    void HideVirtualMouse()
    {
        cursorTransform.gameObject.SetActive(false);
    }

    void ShowVirtualMouse()
    {
        cursorTransform.gameObject.SetActive(true);
    }
}