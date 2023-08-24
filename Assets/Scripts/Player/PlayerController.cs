using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Stats
    [field: Header("Stats")]
    [field: SerializeField] public float _moveSpeed { get; private set; } = 5f;
    [field: SerializeField] public int _jumpHeight { get; private set; } = 5;
    #endregion

    #region Components & Movement stuff
    private Rigidbody2D _rb;
    private Vector2 _moveDirection = Vector2.zero;
    #endregion

    #region Grounded Check
    private Vector3 _boxSize;
    private float _maxDistance;
    [Header("Ground Layer Mask")][SerializeField] private LayerMask _layerMask;
    #endregion

    #region InputActions & Player Controls
    private PlayerControls _playerControls;
    private InputAction _move;
    private InputAction _attack;
    private InputAction _jump;
    #endregion

    #region Buffs
    [SerializeField] private Buff _damageBuff;

    private List<Buff> _buffs = new List<Buff>();
    #endregion

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _move.performed += StartMove;
        _move.canceled += StopMove;
        _move.Enable();

        _attack = _playerControls.Player.Attack;
        _attack.Enable();
        _attack.performed += Attack;

        _jump = _playerControls.Player.Jump;
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
        PlayerCombat.main.Attack();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (GroundCheck())
        {
            _rb.AddForce(transform.up * _jumpHeight, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * _maxDistance, _boxSize);
    }

    private bool GroundCheck()
    {
        return (Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _maxDistance, _layerMask));
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

    private void AddAllBuffs()
    {
        //_bfs.Add()
    }
}
