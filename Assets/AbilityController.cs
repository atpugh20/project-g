using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

 

public class AbilityController : MonoBehaviour {
    [SerializeField] private ScriptableStats _stats;
    // Game Objects


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

    // Update is called once per frame
    void Update() {}

    private void FixedUpdate() {
        HandlePowers();
    }

    public void ToggleShowPlayer() {
        PlayerShape.SetActive(true);
    }

    private void HandlePowers() {
        if (hasFlame) HandleFlame();
        if (hasEarth) HandleEarth();
        if (hasWater) HandleWater();
    }

    #region Flame

    private float _flameRefreshTime = 0.5f;
    private int _timeSinceFlameUse = 0;
    public bool CanUseFlame = true;
    public bool UsingNeutralFlame = false;
    public bool UsingDirectionalFlame = false;
    private Vector3 _originalAngle;


    private void HandleFlame() {
        _timeSinceFlameUse++;

        // Checks flame conditions
        if (_pC._grounded && !UsingDirectionalFlame && !UsingNeutralFlame && !_pC._frameInput.FlameHeld && _timeSinceFlameUse * Time.deltaTime > _flameRefreshTime) CanUseFlame = true;

        if (Mathf.Abs(_pC._frameInput.Move.x) > 0.2 || Mathf.Abs(_pC._frameInput.Move.y) > 0.2 || UsingDirectionalFlame) {
            HandleDirectionalFlame();
        } else {
            HandleNeutralFlame();
        }
    }

    private void HandleNeutralFlame() {

        // Checks timing of the ability
        if (_stats.NeutralFlameTime <= _timeSinceFlameUse * Time.deltaTime) {
            UsingNeutralFlame = false;
            UsingDirectionalFlame = false;
        }

        // Exit case
        if (!CanUseFlame || !_pC._frameInput.FlameHeld) return;

        // Uses ability
        UsingNeutralFlame = true;
        UsingDirectionalFlame = false;
        CanUseFlame = false;
        _timeSinceFlameUse = 0;
    }

    private void HandleDirectionalFlame() {

        // Checks timing of the ability
        if (UsingDirectionalFlame && _stats.DirectionalFlameTime <= _timeSinceFlameUse * Time.deltaTime) {
            UsingDirectionalFlame = false;
            UsingNeutralFlame = false;
            flameDashActivate.FlameDashOff();
            _anim.SetBool("isFlameDashing", false);
            PlayerShape.SetActive(true);
            _stats.FallAcceleration = 110;
            _transform.eulerAngles = _originalAngle;
        }

        // Exit Case
        if (!CanUseFlame || !_pC._frameInput.FlameHeld) return;

        // Uses ability
        PlayFlameDashSound();
        UsingDirectionalFlame = true;
        UsingNeutralFlame = false;
        CanUseFlame = false;
        _anim.SetBool("isFlameDashing", true);
        flameDashActivate.FlameDash();
        PlayerShape.SetActive(false);
        _pC._endedJumpEarly = true;
        _timeSinceFlameUse = 0;
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

    private float _earthRefreshTime = 1f;
    private int _timeSinceEarthUse = 0;
    public bool CanUseEarth = true;
    public bool UsingNeutralEarth = false;
    public bool UsingDirectionalEarth = false;
    private Vector3 _originalEarthAngle = new(0, 0, 0);

    private void HandleEarth() {
        _timeSinceEarthUse++;

        // Checks elemental conditions
        if (_pC._grounded && !UsingNeutralEarth && !UsingDirectionalEarth && !_pC._frameInput.EarthHeld && _timeSinceEarthUse * Time.deltaTime > _earthRefreshTime) CanUseEarth = true;

        if (Mathf.Abs(_pC._frameInput.Move.x) > 0.2 || Mathf.Abs(_pC._frameInput.Move.y) > 0.2 || UsingDirectionalEarth) {
            HandleDirectionalEarth();
        } else {
            HandleNeutralEarth();
        }
    }
    private void HandleNeutralEarth() {
        // Checks timing of the ability
        if (_stats.NeutralEarthTime <= _timeSinceEarthUse * Time.deltaTime) {
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
        if (UsingDirectionalEarth && _stats.DirectionalEarthTime <= _timeSinceEarthUse * Time.deltaTime) {
            UsingNeutralEarth = false;
        }

        // Exit case
        if (!CanUseEarth || !_pC._frameInput.EarthHeld) return;

        // Uses ability
        CanUseEarth = false;
        _timeSinceEarthUse = 0;
        print("SIDE EARTH");
    }

    #endregion

    #region Water

    private float _waterRefreshTime = 1f;
    private int _timeSinceWaterUse = 0;
    private Vector3 _originalWaterAngle = new(0, 0, 0);
    public bool CanUseWater = true;
    public bool UsingNeutralWater = false;
    public bool UsingDirectionalWater = false;
    public Transform ShootingPoint;
    public GameObject WaterBullet;

    private void HandleWater() {
        _timeSinceWaterUse++;

        // Checks elemental conditions
        if (_pC._grounded && !UsingNeutralWater && !UsingDirectionalWater && !_pC._frameInput.WaterHeld && _timeSinceWaterUse * Time.deltaTime > _waterRefreshTime) CanUseWater = true;

        if (Mathf.Abs(_pC._frameInput.Move.x) > 0.2 || Mathf.Abs(_pC._frameInput.Move.y) > 0.2 || UsingDirectionalWater) {
            HandleDirectionalWater();
        } else {
            HandleNeutralWater();
        }
    }
    private void HandleNeutralWater() {
        // Checks timing of the ability
        if (_stats.NeutralWaterTime <= _timeSinceWaterUse * Time.deltaTime) {
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
        if (UsingDirectionalWater && _stats.DirectionalWaterTime <= _timeSinceWaterUse * Time.deltaTime) {
            UsingNeutralWater = false;
        }

        // Exit case
        if (!CanUseWater || !_pC._frameInput.WaterHeld) return;

        // Uses ability
        CanUseWater = false;
        _timeSinceWaterUse = 0;
        Instantiate(WaterBullet, ShootingPoint.position, Quaternion.identity);
    }

    #endregion

    // Audio Players
    public void PlayFlameDashSound() => _flameDashSound.Play();

#if UNITY_EDITOR
    private void OnValidate() {
        if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
    }
#endif

}

