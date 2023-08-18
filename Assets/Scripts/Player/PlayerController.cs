using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Stats
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _jumpHeight = 5;
    #endregion

    #region Components & Movement stuff
    private Rigidbody2D _rb;
    private Vector2 _moveDirection = Vector2.zero;
    #endregion

    #region Grounded Check
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    #endregion

    #region InputActions & Player Controls
    public PlayerControls PlayerControls;
    private InputAction _move;
    private InputAction _attack;
    private InputAction _jump;
    #endregion

    private bool _jumped = false;

    private void Awake()
    {
        PlayerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _move = PlayerControls.Player.Move;
        _move.performed += StartMove;
        _move.canceled += StopMove;
        _move.Enable();

        _attack = PlayerControls.Player.Attack;
        _attack.Enable();
        _attack.performed += Attack;

        _jump = PlayerControls.Player.Jump;
        _jump.Enable();
        _jump.performed += Jump;
    }

    private void OnDisable()
    {
        _move.Disable();
        _attack.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("We attacked");
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (GroundCheck())
        {
            _rb.AddForce(transform.up * _jumpHeight, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

    private bool GroundCheck()
    {
        return (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask));
    }

    private void StartMove(InputAction.CallbackContext context)
    {
        StartCoroutine(MovementCoroutine());
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        StopCoroutine(MovementCoroutine());
    }

    IEnumerator MovementCoroutine()
    {
        while (true)
        {
            if(!GroundCheck())
            {
                _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed * Time.deltaTime, _rb.velocity.y);
            }
            else
            {
                _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed * Time.deltaTime, _rb.velocity.y);
            }
           
            yield return new WaitForFixedUpdate();
        }
    }
}
