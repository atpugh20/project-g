using UnityEngine;

namespace TController
{
    [CreateAssetMenu]
    public class ScriptableStats : ScriptableObject {
        #region Base Movement

        [Header("LAYERS")] [Tooltip("Set this to the layer your player is on")]
        public LayerMask PlayerLayer;

        [Header("INPUT")] [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keybaord parity.")]
        public bool SnapInput = false;

        [Tooltip("Minimum input required before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
        public float VerticalDeadZoneThreshold = 0.4f;

        [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
        public float HorizontalDeadZoneThreshold = 0.4f;

        [Header("MOVEMENT")] [Tooltip("The top horizontal movement speed")]
        public float MaxSpeed = 25;

        [Tooltip("The player's capacity to gain horizontal speed")]
        public float Acceleration = 200;

        [Tooltip("The pace at which the player comes to a stop")]
        public float GroundDeceleration = 500;

        [Tooltip("Deceleration in air only after stopping input mid-air")]
        public float AirDeceleration = 50;

        [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
        public float GroundingForce = -5.2f;

        [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
        public float GrounderDistance = 0.055f;

        [Header("JUMP")] [Tooltip("The immediate velocity applied when jumping")]
        public float JumpPower = 50;

        [Tooltip("The maximum vertical movement speed")]
        public float MaxFallSpeed = 120;

        [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
        public float FallAcceleration = 110;

        [Tooltip("The gravity multiplier added when jump is released early")]
        public float JumpEndEarlyGravityModifier = 3;

        [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
        public float CoyoteTime = .15f;

        [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
        public float JumpBuffer = .2f;

        #endregion

        [Header("ABILITIES")]

        #region Flame

        [Tooltip("Base Strength of the flame dash")]
        public float DashPower = 60;

        [Tooltip("Base time of the flame dash")]
        public float DirectionalFlameTime = 0.27f;

        [Tooltip("Base time of the Neutral flame")]
        public float NeutralFlameTime = 0.5f;

        [Tooltip("Velocity for the player shooting out of the side of the Big Fireball")]
        public float NeutralFlameThrust = 50;

        #endregion

        #region Earth

        [Tooltip("Base time of the Directional Earth")]
        public float DirectionalEarthTime = 0.5f;

        [Tooltip("Base power of the Directional Earth")]
        public float ThrustPower = 50;

        [Tooltip("Base time of the Neutral Earth")]
        public float NeutralEarthTime = 1f;

        #endregion

        #region Water

        [Tooltip("Base time of the Directional Earth")]
        public float DirectionalWaterTime = 0.38f;

        [Tooltip("Base time of the Directional Earth")]
        public float BlastBackPower = -50;

        [Tooltip("Base time of the Neutral Earth")]
        public float NeutralWaterTime = 1f;

        #endregion


    }
}