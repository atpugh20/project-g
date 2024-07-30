using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class WoodHandler : MonoBehaviour
{
    public GameObject Player;
    private AbilityController _aC;
    private Animator _anim;

    // Start is called before the first frame update
    void Start() {
        _aC = Player.GetComponent<AbilityController>();
        _anim = GetComponent<Animator>();
    }

    void OnCollisionStay2D(Collision2D col) {
        print(col.gameObject.tag);
        if (_aC.UsingDirectionalFlame || col.gameObject.CompareTag("FlameBall")) { 
            _anim.SetTrigger("break");
        }
    }

    public void Kill() => Destroy(gameObject);
}
