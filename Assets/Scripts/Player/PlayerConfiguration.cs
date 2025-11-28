using UnityEngine;
using System.Collections;

public class PlayerConfiguration : MonoBehaviour
{
    [Header("Configuration")]

    #region Movement
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    public float MOVESPEED => moveSpeed;

    [SerializeField] float turnSpeed;
    public float TURNSPEED => turnSpeed;

    [SerializeField] float airControl;
    public float AIRCONTROL => airControl;
    #endregion

    #region Jump
    [Header("Jumps")]
    [SerializeField] float jumpForce;
    public float JUMPFORCE => jumpForce;

    [SerializeField] float maxJumps;
    public float MAXJUMPS => maxJumps;

    [SerializeField] float jumpModifier = 0.8f;
    public float JUMPMODIFIER => jumpModifier;

    public float remainingJumps;


    public void ResetJumps()
    {
        remainingJumps = MAXJUMPS;
    }
    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float dashForce;
    public float DASHFORCE => dashForce;

    [SerializeField] float dashDuration;
    public float DASHDURATION => dashDuration;

    [SerializeField] float dashCooldown;
    public float DASHCOOLDOWN => dashCooldown;

    [HideInInspector] public float lastDashTime;
    [HideInInspector] public bool isDashing;
    #endregion

    #region Stomp
    [Header("Stomp")]
    [SerializeField] float stompForce;
    public float STOMPFORCE => stompForce;

    [HideInInspector] public bool isStomping = false;

    public void ResetStomp()
    {
        StartCoroutine(DelayedResetStomp());
    }

    private IEnumerator DelayedResetStomp()
    {
        yield return new WaitForSeconds(0.2f);

        isStomping = false;
    }
    #endregion

    #region Push
    [Header("Push")]
    // Push Modifier
    [SerializeField] float pushForce;
    public float PUSHFORCE => pushForce;

    // Horizontal Push
    [SerializeField] float horizontalPush = 25f;
    public float HORIZONTALPUSH => horizontalPush;

    // Vertical Push
    [SerializeField] float verticalPush = 7.5f;
    public float VERTICALPUSH => verticalPush;

    [HideInInspector] public Vector3 pusherPosition;
    [HideInInspector] public bool isPushed;

    // Duration of Push
    [SerializeField] float pushDuration = 0.3f;
    public float PUSHDURATION => pushDuration;

    // Duration of the Pushed State
    [SerializeField] float stateDuration = 1.2f;
    public float PUSHEDSTATEDURATION => stateDuration;



    public void SetPush(Vector3 position)
    {
        pusherPosition = position;
        isPushed = true;
    }
    #endregion

    #region Quack
    [Header("Quack")]
    public float quackDelay;
    public bool canQuack;


    public void Quack()
    {
        if (!GameManager.instance.gameStarted)
            GameManager.instance.StartGame();

        canQuack = false;

        StartCoroutine(ResetQuack());
    }

    private IEnumerator ResetQuack()
    {
        yield return new WaitForSeconds(quackDelay);

        canQuack = true;
    }
    #endregion

    #region Rotations
    private Quaternion horizontalRotation;
    [HideInInspector] public Quaternion HORIZONTAL => horizontalRotation;

    private Quaternion invertedRotation;
    [HideInInspector] public Quaternion INVERTED => invertedRotation;

    private void SetRotations()
    {
        horizontalRotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        invertedRotation = Quaternion.Inverse(transform.rotation);
    }
    #endregion

    private void Awake()
    {
        SetRotations();
    }
}
