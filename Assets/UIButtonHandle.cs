using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TController;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandle : MonoBehaviour {
    public GameObject Player;
    public GameObject XButton;
    public GameObject YButton;
    public GameObject AButton;
    public GameObject BButton;
    private PlayerController _pController;
    private Button _xButtonModifier; 
    private Button _yButtonModifier;
    private Button _aButtonModifier;
    private Button _bButtonModifier;

    // Start is called before the first frame update
    void Start() {
        _pController = Player.GetComponent<PlayerController>();
        _xButtonModifier = XButton.GetComponent<Button>();
        _yButtonModifier = YButton.GetComponent<Button>();
        _aButtonModifier = AButton.GetComponent<Button>();
        _bButtonModifier = BButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update() {
        buttonActivate();
    }

    void buttonActivate() {
        print(_pController._jumpToConsume);
        
        // X Button
        if (_pController._canDash) {
            _xButtonModifier.interactable = true;
        } else {
            _xButtonModifier.interactable = false;
        }
        // A Button
        if (_pController._jumpToConsume) {
            _aButtonModifier.interactable = true;
        } else {
            _aButtonModifier.interactable = false;
        }
    }
}
