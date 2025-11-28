using UnityEngine;


public class PlayerMovement
{
    Rigidbody rigidbody;
    PlayerConfiguration PlayerConfiguration;

    public PlayerMovement(Rigidbody rigidbody, PlayerConfiguration playerConfiguration)
    {
        this.rigidbody = rigidbody;
        this.PlayerConfiguration = playerConfiguration;
    }


    #region General
    private Vector3 MoveDirection(Vector2 inputVec, Transform cameraTransform)
    {
        // Forward
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // Right
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // Movement via Camera
        Vector3 moveDirection = (cameraForward * inputVec.y) + (cameraRight * inputVec.x);
        moveDirection.Normalize();

        return moveDirection;
    }
    
    public void PlayerRotation()
    {
        Vector3 velocityWithoutY = new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z);
        if (velocityWithoutY != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocityWithoutY.normalized, Vector3.up);
            Quaternion newRotation = Quaternion.Euler(0f, Quaternion.Lerp(rigidbody.rotation, targetRotation, PlayerConfiguration.TURNSPEED * Time.deltaTime).eulerAngles.y, 0f);
            rigidbody.MoveRotation(newRotation);
        }
    }
    #endregion

    #region Movement
    public void VelocityMovement(Vector2 inputVec, Transform cameraTransform)
    {
        // Movement with Speed Modifier
        Vector3 movement = MoveDirection(inputVec, cameraTransform) * PlayerConfiguration.MOVESPEED;

        rigidbody.linearVelocity = movement;

        // Rotation
        PlayerRotation();           
    }

    public void VelocityIdle()
    {
        rigidbody.linearVelocity = Vector3.zero;
    }
    #endregion

    #region Air
    public void VelocityJump(float multiplier = 1f)
    {
        Vector3 currentVelocity = rigidbody.linearVelocity;
        currentVelocity.y = PlayerConfiguration.JUMPFORCE * multiplier;
        rigidbody.linearVelocity = currentVelocity;
    }

    public float VELOCITYY => rigidbody.linearVelocity.y;

    public void VelocityMovementInAir(Vector2 inputVec, Transform cameraTransform)
    {
        // Movement with Speed Modifier
        Vector3 desired = MoveDirection(inputVec, cameraTransform) * PlayerConfiguration.MOVESPEED;

        Vector3 current = rigidbody.linearVelocity;
        Vector3 horizontal = new Vector3(current.x, 0f, current.z);

        Vector3 change = desired - horizontal;

        rigidbody.AddForce(change * PlayerConfiguration.AIRCONTROL, ForceMode.Acceleration);

        // Rotation
        PlayerRotation();
    }
    #endregion

    public void StartDash(Vector3 direction)
    {
        Vector3 currentVelocity = rigidbody.linearVelocity;

        Vector3 dashVelocity = new Vector3(direction.x, 0f, direction.z) * PlayerConfiguration.DASHFORCE;

        rigidbody.linearVelocity = new Vector3(dashVelocity.x, currentVelocity.y, dashVelocity.z);
    }

    public void StartStomp()
    {
        rigidbody.linearVelocity = new Vector3(0f, -1f, 0f) * PlayerConfiguration.STOMPFORCE;
    }

    public void StartPush(Vector3 pusherPosition)
    {
        // Direction
        Vector3 direction = rigidbody.position - pusherPosition;
        direction = direction.normalized;

        // Modified Speeds
        float horizontalSpeed = PlayerConfiguration.HORIZONTALPUSH * PlayerConfiguration.PUSHFORCE;
        float verticalSpeed = PlayerConfiguration.VERTICALPUSH * PlayerConfiguration.PUSHFORCE;

        Vector3 finalDirection = new Vector3(direction.x, 0f, direction.z).normalized * horizontalSpeed + Vector3.up * verticalSpeed;

        rigidbody.AddForce(finalDirection, ForceMode.VelocityChange);
    }
}