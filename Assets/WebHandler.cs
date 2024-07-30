using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class WebHandler : MonoBehaviour {
    [SerializeField] private ScriptableStats _stats;

    private GameObject _player;
    private PlayerController _pC;
    private AbilityController _aC;
    private Animator _anim;

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.Find("Player");
        _pC = _player.GetComponent<PlayerController>();
        _aC = _player.GetComponent<AbilityController>();
        _anim = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D col) {
        if (_aC.UsingDirectionalFlame) _anim.SetTrigger("break");
        if (col.CompareTag("Player")) _pC._frameVelocity /= 50;
    }
    
    public void Kill() => Destroy(gameObject); 
}
