using UnityEngine;
using WorldGraphEditor;

namespace ZenjectDemo
{
    public class ZenjectPlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _acceleration = 50f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _linearDrag;
        
        [Header("Jumping")]
        [SerializeField] private float _jumpForce = 150f;
        [SerializeField] private float _hangTime;
        [SerializeField] private float _jumpBuffer;
        [SerializeField] private float _fallTolerance;
        [SerializeField] private float _fallMultiplier;
        [SerializeField] private float _lowJumpFallMultiplier;
        [SerializeField] private float _airLinearDrag;
        
        [Header("Ground Detection")]
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _yOffset;
        
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private BoxCollider2D _collider;
        
        private bool _isGrounded;
        private bool _isControllerEnabled;
        private bool _isJumpPressed;
        private bool _isMovementPressed;

        private float _hangTimeCounter;
        private float _jumpBufferCounter;
        private float _direction;
        private float _horizontalCashedDirection;

        private bool _isChangingDirection => (_rigidbody.linearVelocity.x > 0f && _horizontalCashedDirection < 0f) ||
                                             (_rigidbody.linearVelocity.x < 0f && _horizontalCashedDirection > 0f);
        
        private bool _canJump => _jumpBufferCounter > 0f && _hangTimeCounter > 0f && _isControllerEnabled;

        public void SetPushForce(PushData pushData)
        {
            if (pushData.Exists) 
                _rigidbody.AddForce(pushData.Force, ForceMode2D.Impulse);
        }

        public void Enable()
        {
            _isControllerEnabled = true;
        }
        
        private void Awake()
        {
            _isControllerEnabled = false;
        }
        
        private void FixedUpdate()
        {
            _isGrounded = IsGrounded();

            if (_isControllerEnabled)
                Move();
            
            if (_isGrounded)
            {
                ApplyGroundDrag();
                _hangTimeCounter = _hangTime;
            }
            else
            {
                ApplyAirDrag();
                ApplyFallMultiplier();

                _hangTimeCounter -= Time.fixedDeltaTime;
            }

            if (_canJump)
                Jump();
        }

        private void Update()
        {
            ProcessInput();

            _jumpBufferCounter = _isJumpPressed ? _jumpBuffer : 0;
            
            if (_jumpBufferCounter > 0)
                _jumpBufferCounter -= Time.deltaTime;
        }

        private void ProcessInput()
        {
            _horizontalCashedDirection = Input.GetAxisRaw("Horizontal");
            _isJumpPressed = Input.GetButton("Jump");

            _isMovementPressed = _horizontalCashedDirection != 0;
        }

        private bool IsGrounded()
        {
            return Physics2D.BoxCast(transform.position, _collider.size, 0,
                Vector2.down, _yOffset, _layerMask);
        }
        
        private void ApplyFallMultiplier()
        {
            if (_rigidbody.linearVelocity.y < _fallTolerance)
            {
                _rigidbody.gravityScale = _fallMultiplier;
            }
            else if (_rigidbody.linearVelocity.y > _fallTolerance && !_isJumpPressed)
            {
                _rigidbody.gravityScale = _lowJumpFallMultiplier;
            }
            else
            {
                _rigidbody.gravityScale = 1f;
            }
        }

        private void ApplyAirDrag()
        {
            _rigidbody.linearDamping = _airLinearDrag;
        }
        
        private void Jump()
        {
            ApplyAirDrag();
            _hangTimeCounter = 0f;
            _jumpBufferCounter = 0f;
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        
        private void Move()
        {
            if (!_isMovementPressed)
                return;
            
            if (Mathf.Abs(_rigidbody.linearVelocity.x) >= _maxSpeed)
            {
                _rigidbody.linearVelocity = new Vector2(Mathf.Sign(_rigidbody.linearVelocity.x) * _maxSpeed, _rigidbody.linearVelocity.y);
            }
            else
            {
                _rigidbody.AddForce(new Vector2(_horizontalCashedDirection, 0f) * _acceleration);
            }
        }

        private void ApplyGroundDrag()
        {
            if (Mathf.Abs(_horizontalCashedDirection) < .4f || _isChangingDirection)
            {
                _rigidbody.linearDamping = _linearDrag;
            }
            else
            {
                _rigidbody.linearDamping = 0;
            }
        }
    }   
}