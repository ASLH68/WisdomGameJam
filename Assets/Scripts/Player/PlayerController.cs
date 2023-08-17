using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerControls PlayerControls;
    public Vector2 moveDirection = Vector2.zero;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;

    private InputAction _move;
    private InputAction _attack;
    private void Awake()
    {
        PlayerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _move = PlayerControls.Player.Move;
        _move.Enable();

        _attack = PlayerControls.Player.Attack;
        _attack.Enable();
        _attack.performed += Attack;
    }

    private void OnDisable()
    {
        _move.Disable();
        _attack.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, moveDirection.y * moveSpeed * Time.deltaTime);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("We attacked");
    }
}
