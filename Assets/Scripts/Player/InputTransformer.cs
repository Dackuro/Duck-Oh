using UnityEngine;

public class InputTransformer
{
    public InputTransformer() { }
    private bool inputsEnabled = true;


    // Movement
    Vector2 inputVector;
    public Vector2 INPUTVECTOR => inputVector;
    public Vector2 INPUTVECTORNORMAL => inputVector.normalized;

    // Jump
    float inputJump;
    public float INPUTJUMP => inputJump;

    // Dash
    float inputDash;
    public float INPUTDASH => inputDash;

    // Stomp
    float inputStomp;
    public float INPUTSTOMP => inputStomp;

    // Quack
    float inputQuack;
    public float INPUTQUACK => inputQuack;
    
    
    public void EnableInputs(bool areEnabled)
    {
        inputsEnabled = areEnabled;
    }

    public void ProcessInputVector(Vector2 value)
    {
        if (!inputsEnabled)
            return;

        this.inputVector = value;
    }

    public void ProcessInputJump(float value)
    {
        if (!inputsEnabled)
            return;

        this.inputJump = value;
    }

    public void ProcessInputDash(float value)
    {
        if (!inputsEnabled)
            return;

        this.inputDash = value;
    }
    
    public void ProcessInputStomp(float value)
    {
        if (!inputsEnabled)
            return;

        this.inputStomp = value;
    }

    public void ProcessInputQuack(float value)
    {
        this.inputQuack = value;
    }
}