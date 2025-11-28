using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerGeneral PLAYER;

    private bool inputsEnabled = true;


    public void EnableInputs(bool areEnabled)
    {
        inputsEnabled = areEnabled;
    }

    #region Movement
    public void InputMovement(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.canceled)
            PLAYER.INPUTTRANSFORMER.ProcessInputVector(Vector2.zero);
        else
            PLAYER.INPUTTRANSFORMER.ProcessInputVector(context.ReadValue<Vector2>());
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.started)
        {
            float time = (float)context.time;
            PLAYER.INPUTTRANSFORMER.ProcessInputJump(time);
        }
        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputJump(0f);
        }
    }

    public void InputDash(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.started)
        {
            float time = (float)context.time;
            PLAYER.INPUTTRANSFORMER.ProcessInputDash(time);
        }
        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputDash(0f);
        }
    }

    public void InputStomp(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.started)
        {
            float time = (float)context.time;
            PLAYER.INPUTTRANSFORMER.ProcessInputStomp(time);
        }
        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputStomp(0f);
        }
    }
    #endregion

    #region Others
    public void InputQuack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float time = (float)context.time;
            PLAYER.INPUTTRANSFORMER.ProcessInputQuack(time);
        }
        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputQuack(0f);
        }
    }
    #endregion

    #region UI
    public void InputPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!GameManager.instance.gameEnded)
                GameManager.instance.TogglePause("pause");
        }
    }
    #endregion
}
