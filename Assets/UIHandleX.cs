using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandleX : MonoBehaviour {

    public Animator _playerAnimator;
    private Button _xButton;

    // Start is called before the first frame update
    void Start() {
        _playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        _xButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update() {
        if (_playerAnimator.GetBool("isJumping")) {
            _xButton.interactable = false;
        } else {
            _xButton.interactable = true;
        }
    }
}
