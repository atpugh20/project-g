using System;
using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

 

public class AbilityController : MonoBehaviour {
    
    #region INIT

    [SerializeField] private ScriptableStats _stats;

    // Components
    private Rigidbody2D _rb;
    private Animator _anim;
    private Transform _transform;
    private PlayerController _pC;

    public bool hasStaff;
    public bool hasFlame;
    public bool hasWater;
    public bool hasEarth;

    // Animations
    GameObject PlayerShape;
    FlameDashActivate flameDashActivate;

    // Audio
    public GameObject FlameDashSound;
    private AudioSource _flameDashSound;

    private void Awake() {
        // Components
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _pC = GetComponent<PlayerController>();

        // Abilities
        ShootingPoint = GameObject.Find("WaterBulletSpawner").transform;

        // Animations
        PlayerShape = GameObject.Find("PlayerShape");
        flameDashActivate = GameObject.Find("FlameDash").GetComponent<FlameDashActivate>();

        // Audio
        _flameDashSound = FlameDashSound.GetComponent<AudioSource>();
    }

    #endregion

    private void FixedUpdate() => HandlePowers();

    public void Grab()
    {
        hasStaff = true;
        GameObject staffhand = GameObject.Find("staff2");
        print(staffhand);
        staffhand.GetComponent<SpriteRenderer>().enabled = true;
        GameObject staffped = GameObject.Find("staffinstone");
        staffped.SetActive(false);
    }

    private void HandlePowers() {
        if (hasFlame) HandleFlame();
        if (hasEarth) HandleEarth();
        if (hasWater) HandleWater();
    }

    #region Flame

    private float _dFlameRefreshTime = 0.5f;
    private float _nFlameRefreshTime = 30f;
    private float _timeSinceDFlameUse = 0;
    private float _timeSinceNFlameUse = 0;
    public bool CanUseDirFlame = true;
    public bool CanUseNeuFlame = true;
    public bool UsingNeutralFlame = false;
    public bool UsingDirectionalFlame = false;
    private Vector3 _originalAngle;
    private Vector2 _flameThrustDir;
    public Vector2 NeutralMoveDir;
    public GameObject Fireball;

    private void HandleFlame() {
        _timeSinceDFlameUse += Time.deltaTime;
        _timeSinceNFlameUse +=Time.deltaTime;

        // Checks flamedash conditions
        if (_pC._grounded && 
            !UsingDirectionalFlame && 
            !UsingNeutralFlame && 
            !_pC._frameInput.FlameHeld && 
            _timeSinceDFlameUse > _dFlameRefreshTime) 
            CanUseDirFlame = true;

        // Checks neutral flame conditions
        if (!UsingDirectionalFlame && 
            !UsingNeutralFlame && 
            !_pC._frameInput.FlameHeld && 
            _timeSinceNFlameUse > _nFlameRefreshTime) 
            CanUseNeuFlame = true;

        // Decide which flame ability
        if ((Mathf.Abs(_pC._frameInput.Move.x) > 0.2 || Mathf.Abs(_pC._frameInput.Move.y) > 0.2 || 
            UsingDirectionalFlame) && 
            !UsingNeutralFlame) {
            HandleDirectionalFlame();
        } else {
            HandleNeutralFlame();
        }
    }

    private void HandleNeutralFlame() {

        // Checks timing of the ability
        if (UsingNeutralFlame && _stats.NeutralFlameTime <= _timeSinceNFlameUse) {
            _stats.FallAcceleration = 110;
            if (_pC.transform.eulerAngles.y > 0) {
                _flameThrustDir = new(_stats.NeutralFlameThrust, 50);
                NeutralMoveDir = new(-1, 0);
            } else {
                _flameThrustDir = new(-_stats.NeutralFlameThrust, 50);
                NeutralMoveDir = new(1, 0);
            }
            _pC._frameVelocity = _flameThrustDir;
            _anim.SetTrigger("takeoff");
            _anim.SetBool("isJumping", true);
            UsingNeutralFlame = false;
            UsingDirectionalFlame = false;
        }

        // Exit case
        if (!CanUseNeuFlame || !_pC._frameInput.FlameHeld) return;

        // Uses ability
        UsingNeutralFlame = true;
        UsingDirectionalFlame = false;
        CanUseNeuFlame = false;
        _pC._endedJumpEarly = true;
        _timeSinceNFlameUse = 0;
        FreezeInPlace();
        Instantiate(Fireball, gameObject.transform.position, Quaternion.identity);
        NeutralMoveDir = Vector2.zero;
    }

    private void HandleDirectionalFlame() {

        // Checks timing of the ability
        if (UsingDirectionalFlame && _stats.DirectionalFlameTime <= _timeSinceDFlameUse) {
            UsingDirectionalFlame = false;
            UsingNeutralFlame = false;
            flameDashActivate.FlameDashOff();
            _anim.SetBool("isFlameDashing", false);
            PlayerShape.SetActive(true);
            _stats.FallAcceleration = 110;
            _transform.eulerAngles = _originalAngle;
        }

        // Exit Case
        if (!CanUseDirFlame || !_pC._frameInput.FlameHeld) return;

        // Uses ability
        PlayFlameDashSound();
        UsingDirectionalFlame = true;
        UsingNeutralFlame = false;
        CanUseDirFlame = false;
        _anim.SetBool("isFlameDashing", true);
        flameDashActivate.FlameDash();
        PlayerShape.SetActive(false);
        _pC._endedJumpEarly = true;
        _timeSinceDFlameUse = 0;
        // dash calculation
        _stats.FallAcceleration = 0;
        _pC._frameVelocity = new Vector2(-_rb.velocity.x, -_rb.velocity.y); // sets velocity to 0
        _originalAngle = new(_transform.rotation.x, _transform.rotation.y, _transform.rotation.z);
        float newAngle = Mathf.Atan2(_pC._frameInput.Move.y, _pC._frameInput.Move.x) * Mathf.Rad2Deg;
        _transform.eulerAngles = new Vector3(0f, _transform.rotation.y, newAngle);
        _pC._frameVelocity = new Vector2(_stats.DashPower * _pC._frameInput.Move.x, _stats.DashPower * _pC._frameInput.Move.y);
    }


    #endregion

    #region Earth

    readonly float EarthRefreshTime = 1f;
    private float _timeSinceEarthUse = 0;
    public bool CanUseEarth = true;
    public bool UsingNeutralEarth = false;
    public bool UsingDirectionalEarth = false;
    public float ThrustPower;
    private Vector3 _originalEarthAngle = new(0, 0, 0);
    private Vector2 _stickDir;
    private Vector2 _thrustDir;

    private void HandleEarth() {
        _timeSinceEarthUse+=Time.deltaTime;

        // Checks elemental conditions
        if (_pC._grounded && !UsingNeutralEarth && !UsingDirectionalEarth && !_pC._frameInput.EarthHeld && _timeSinceEarthUse > EarthRefreshTime) CanUseEarth = true;

        if (Mathf.Abs(_pC._frameInput.Move.x) > 0.2 || Mathf.Abs(_pC._frameInput.Move.y) > 0.2 || UsingDirectionalEarth) {
            HandleDirectionalEarth();
        } else {
            HandleNeutralEarth();
        }
    }
    private void HandleNeutralEarth() {
        // Checks timing of the ability
        if (_stats.NeutralEarthTime <= _timeSinceEarthUse) {
            UsingNeutralEarth = false;
        }

        // Exit case
        if (!CanUseEarth || !_pC._frameInput.EarthHeld) return;

        // Uses ability
        CanUseEarth = false;
        UsingNeutralEarth = true;
        _timeSinceEarthUse = 0;
        print("NEUTRAL EARTH");
    }

    private void HandleDirectionalEarth() {
        // Checks timing of the ability
        if (UsingDirectionalEarth && _stats.DirectionalEarthTime <= _timeSinceEarthUse) {
            UsingDirectionalEarth = false;
            _stats.FallAcceleration = 110;
        }

        // Exit case
        if (!CanUseEarth || !_pC._frameInput.EarthHeld) return;

        // Uses ability
        _stickDir = _pC._frameInput.Move;
        UsingDirectionalEarth = true;
        CanUseEarth = false;
        _timeSinceEarthUse = 0;
        
        //FreezeInPlace();
        // Calculate Direction
        if (_pC.transform.eulerAngles.y > 0) { 
            _thrustDir = new(-_stats.ThrustPower, 10); 
        } else {
            _thrustDir = new(_stats.ThrustPower, 10);
        }
        Thrust();
    }

    public void Thrust() => _pC._frameVelocity = _thrustDir;

    #endregion

    #region Water

    readonly float _waterRefreshTime = 1f;
    private float _timeSinceWaterUse = 0;
    public bool CanUseWater = true;
    public bool UsingNeutralWater = false;
    public bool UsingDirectionalWater = false;
    public Transform ShootingPoint;
    public GameObject WaterBullet;
    public Vector2 BulletDirection;

    private void HandleWater() {
        _timeSinceWaterUse+=Time.deltaTime;

        // Checks elemental conditions
        if (_pC._grounded && !UsingNeutralWater && !UsingDirectionalWater && !_pC._frameInput.WaterHeld && _timeSinceWaterUse > _waterRefreshTime) CanUseWater = true;

        if (Mathf.Abs(_pC._frameInput.Move.x) > 0.2 || Mathf.Abs(_pC._frameInput.Move.y) > 0.2 || UsingDirectionalWater) {
            HandleDirectionalWater();
        } else {
            HandleNeutralWater();
        }
    }
    private void HandleNeutralWater() {
        // Checks timing of the ability
        if (_stats.NeutralWaterTime <= _timeSinceWaterUse) {
            UsingNeutralWater = false;
        }

        // Exit case
        if (!CanUseWater || !_pC._frameInput.WaterHeld) return;

        // Uses ability
        CanUseWater = false;
        UsingNeutralWater = true;
        _timeSinceWaterUse = 0;
        print("NEUTRAL WATER");
    }

    private void HandleDirectionalWater() {
        // Checks timing of the ability
        if (UsingDirectionalWater && _stats.DirectionalWaterTime <= _timeSinceWaterUse) {
            UsingDirectionalWater = false;
            _stats.FallAcceleration = 110;
        }

        // Exit case
        if (!CanUseWater || !_pC._frameInput.WaterHeld) return;

        // Uses ability
        UsingDirectionalWater = true;
        BulletDirection = _pC._frameInput.Move;
        
        CanUseWater = false;
        _timeSinceWaterUse = 0;
        FreezeInPlace();
        
        // direction
        if (BulletDirection.y > 0.5) _anim.SetTrigger("ShootUp");
        if (BulletDirection.y < -0.5) _anim.SetTrigger("ShootDown");
        if (-0.5 < BulletDirection.y && BulletDirection.y < 0.5) _anim.SetTrigger("ShootSide");
    }

    public void SpawnWaterBullet() => Instantiate(WaterBullet, ShootingPoint.position, Quaternion.identity);
    public void BlastBack() => _pC._frameVelocity = BulletDirection * _stats.BlastBackPower;

    #endregion

    #region Helper Methods

    private void FreezeInPlace() {
        _stats.FallAcceleration = 0;
        _pC._frameVelocity = Vector2.zero; // sets velocity to 0
    }

    #endregion

    #region Audio

    // Audio Players
    public void PlayFlameDashSound() => _flameDashSound.Play();

    #endregion

    #region Extra
#if UNITY_EDITOR
    private void OnValidate() {
        if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
    }
#endif

    #endregion

}

