using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;
public class InputManager : MonoBehaviour
{
    private Keybinds keyBinds;
    private AnimatorManager am;

    [Header("Monitor input values (do not edit)")]
    [Header("Main 2D Axis")]
    public Vector2 movementInput;
    public Vector2 cameraInput;

    [Header("Extras")]
    public bool isGamePaused = false;

    [Header("Debugging")]
    public bool drawRaycasts = false;

    private void Awake()
    {
        DisplayCursor(false);

        keyBinds = new Keybinds();
        keyBinds.Enable();

        am = GetComponent<AnimatorManager>();

        // Input events subscriptions
        keyBinds.UI.Pause.performed += Pause;
    }

    // Update values so that other classes can read them
    private void Update()
    {
        movementInput = keyBinds.Player.Move.ReadValue<Vector2>();
        cameraInput = keyBinds.Player.Look.ReadValue<Vector2>();

        //moveAmount = Mathf.Clamp01(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y));
        //am.UpdateAnimationVelocity(moveAmount);
        am.UpdateAnimationVelocity(movementInput.x, movementInput.y);
    }

    private void Pause(InputAction.CallbackContext context)
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            DisplayCursor(true);
        }
        else
        {
            Time.timeScale = 1f;
            DisplayCursor(false);
        }
    }

    // Helper functions

    public  void DisplayCursor(bool visibility)
    {
        if (visibility)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}