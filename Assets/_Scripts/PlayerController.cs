using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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

        #region INIT

        [SerializeField] private ScriptableStats _stats;
        
        // Components
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private Animator _anim;
        private Transform _transform;
        private AbilityController _aC;

        //Respawn
        //private Vector3 respawnPoint;
        public GameObject fallDetector;
        public Vector3 Checkpoint;
        [SerializeField]
        GameObject Hero;

        // Movement
        public FrameInput _frameInput;
        public Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        // Audio
        public GameObject StepSound;
        public GameObject LandSound;
        private AudioSource _stepSound;
        private AudioSource _landSound;

        #endregion

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
            _aC = GetComponent<AbilityController>();
            _transform = GetComponent<Transform>();
            _stepSound = StepSound.GetComponent<AudioSource>();
            _landSound = LandSound.GetComponent<AudioSource>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            //respawnPoint = transform.position;
            Checkpoint = _rb.position;
        }

        private void Update() {
            _time += Time.deltaTime;
            GatherInput();
            print(Checkpoint);
            print(_rb.position);
        }

        private void GatherInput() {
            _frameInput = new FrameInput {
                // Jump button
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                // Earth button
                EarthDown = Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.C),
                EarthHeld = Input.GetButton("Fire1") || Input.GetKey(KeyCode.C),
                // Water button
                WaterDown = Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.C),
                WaterHeld = Input.GetButton("Fire2") || Input.GetKey(KeyCode.C),
                // Fire button
                FlameDown = Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.C),
                FlameHeld = Input.GetButton("Fire3") || Input.GetKey(KeyCode.C),
                // Control stick
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
        }

        private void FixedUpdate() {
            CheckCollisions();
            HandleJump();
            HandleDirection();
            HandleGravity();
            ApplyMovement();
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        public bool _grounded;

        private void CheckCollisions() {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit) {
                _landSound.Play();
                _anim.SetBool("isFalling", false);
                _anim.SetBool("isJumping", false);
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));

            }
            // Left the Ground
            else if (_grounded && !groundHit) {
                if (!_anim.GetBool("isJumping")) _anim.SetBool("isFalling", true);
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion

        #region Jumping

        public bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        public bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;


        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        public bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump() {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;
            if (!_jumpToConsume && !HasBufferedJump) return;
            if (_aC.UsingNeutralFlame) return;
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
            _anim.SetBool("isJumping", true);
            Jumped?.Invoke();
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
                if (!_aC.UsingDirectionalFlame && 
                    !_aC.UsingNeutralFlame && 
                    !_aC.UsingDirectionalWater && 
                    !_aC.UsingDirectionalEarth) {
                    _anim.SetFloat("runSpeedMultiplier", Mathf.Abs(_rb.velocity.x / 20));
                    if (_frameInput.Move.x > 0) {
                        _transform.eulerAngles = new Vector3(0f, 0f, _transform.rotation.z);
                    } else if (_frameInput.Move.x < 0) {
                        _transform.eulerAngles = new Vector3(0f, 180f, _transform.rotation.z);
                    }
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
                    _anim.SetBool("isRunning", true);
                }
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

        #region Audio

        public void PlayStepSound() { 
            if (_grounded && !_aC.UsingDirectionalFlame) _stepSound.Play();
        }

        #endregion


#if UNITY_EDITOR
        private void OnValidate() {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    

    public void Die() {
            Debug.Log("Player has died!");
            StartCoroutine(Respawn());
        }

    IEnumerator Respawn()
        {
            Destroy(gameObject, 1f);
            //_anim.SetTrigger("Death");
            yield return new WaitForSeconds(.9f);
            //Instantiate(Hero, Checkpoint, Quaternion.identity);
            SceneManager.LoadScene(2);
        }

    public void Bounce(float bounceForce){
        _frameVelocity.y = bounceForce;
        _anim.SetTrigger("takeoff");
        _anim.SetBool("isJumping", true);
    }

       private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "FallDetector")
            {
                Die();
            }
        }

    }

    public struct FrameInput {
        public bool JumpDown;
        public bool JumpHeld;
        public bool FlameDown;
        public bool FlameHeld;
        public bool WaterDown;
        public bool WaterHeld;
        public bool EarthDown;
        public bool EarthHeld;
        public Vector2 Move;
    }

    public interface IPlayerController {
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}