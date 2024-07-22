using System;
using UnityEngine;

namespace TController {
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// I have a premium version on Patreon, which has every feature you'd expect from a polished controller. Link: https://www.patreon.com/tarodev
    /// You can play and compete for best times here: https://tarodev.itch.io/extended-ultimate-2d-controller
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/tarodev
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private Animator _anim;
        private Transform _transform;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            _anim = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update() {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput() {
            _frameInput = new FrameInput {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                DashDown = Input.GetButtonDown("Fire3") || Input.GetKey(KeyCode.C),
                DashHeld = Input.GetButton("Fire3") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput) {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown) {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
            //if (_frameInput.DashDown) _canDash = true;
            
        }

        private void FixedUpdate() {
            CheckCollisions();
            HandleJump();
            HandleDash();
            HandleDirection();
            HandleGravity();
            ApplyMovement();
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions() {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit) {
                _anim.SetBool("isJumping", false);
                _grounded = true;
                //_canDash = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));

            }
            // Left the Ground
            else if (_grounded && !groundHit) {
                _anim.SetBool("isJumping", true);
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;


        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump() {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;
            if (_grounded || CanUseCoyote) ExecuteJump(); 
           
            _jumpToConsume = false;
        }

        private void ExecuteJump() {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            _anim.SetTrigger("takeoff");

            Jumped?.Invoke();
        }

        #endregion

        #region Dashing

        private bool _canDash = true;
        private bool _isDashing = false;
        private int _dashRefreshTime = 1;
        private int _timeSinceDash = 0;

        private void HandleDash() {
            _timeSinceDash++;
            print(_frameInput.Move);
            if (_stats.DashLength <= _timeSinceDash * Time.deltaTime) {
                _stats.FallAcceleration = 110;
                _isDashing = false; 
            }

            if (_grounded && !_isDashing && !_frameInput.DashHeld && _timeSinceDash * Time.deltaTime > _dashRefreshTime) _canDash = true;
            if (!_canDash || !_frameInput.DashHeld) return;
            _frameVelocity = new Vector2(-_rb.velocity.x, -_rb.velocity.y);
            _stats.FallAcceleration = 0;
            _timeSinceDash = 0;
            _canDash = false;
            _isDashing = true;
            _endedJumpEarly = true;
            _frameVelocity = new Vector2(_stats.DashPower * _frameInput.Move.x, _stats.DashPower * _frameInput.Move.y);
            
        }


        #endregion

        #region Horizontal

        private void HandleDirection() {
            if (_frameInput.Move.x == 0) {
                _anim.SetFloat("runSpeedMultiplier", 1);
                _anim.SetBool("isRunning", false);
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            } else {
                _anim.SetFloat("runSpeedMultiplier", Mathf.Abs(_rb.velocity.x / 20));
                if (_frameInput.Move.x > 0) {
                    _transform.eulerAngles = new Vector3(0, 0, 0);
                } else if (_frameInput.Move.x < 0) {
                    _transform.eulerAngles = new Vector3(0, 180, 0);
                }
                if (!_isDashing) _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
                _anim.SetBool("isRunning", true);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity() {
            if (_grounded && _frameVelocity.y <= 0f) {
                _frameVelocity.y = _stats.GroundingForce;
            } else {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    

    public void Die()
        {
            Debug.Log("Player has died!");
        }

    public void Bounce(float bounceForce)
    {
        _frameVelocity.y = bounceForce;
        _anim.SetTrigger("takeoff");
    }

   }

    public struct FrameInput {
        public bool JumpDown;
        public bool JumpHeld;
        public bool DashDown;
        public bool DashHeld;
        public Vector2 Move;
    }

    public interface IPlayerController {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}