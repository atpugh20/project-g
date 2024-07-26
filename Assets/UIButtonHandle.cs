using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandle : MonoBehaviour {
 
    // Player
    private GameObject _player;
    private PlayerController _pC;
    private AbilityController _aC;

    // Buttons
    public GameObject XButton;
    public GameObject YButton;
    public GameObject AButtonText;
    public GameObject BButton;
    private Button _xButtonModifier;
    private Button _yButtonModifier;
    private TMP_Text _aButtonModifier;
    private Button _bButtonModifier;
    public Color EnabledButtonColor = new (255f, 255f, 255f);
    public Color DisabledButtonColor = new (120f, 120f, 120f);

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.Find("Player");
        _pC = _player.GetComponent<PlayerController>();
        _aC = _player.GetComponent<AbilityController>();
        _xButtonModifier = XButton.GetComponent<Button>();
        _yButtonModifier = YButton.GetComponent<Button>();
        _aButtonModifier = AButtonText.GetComponent<TMP_Text>();
        _bButtonModifier = BButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update() {
        ButtonActivate();
    }

    void ButtonActivate() {   
        // Changes the opacity/color of the buttons when they are available

        // Flame Button
        if (_aC.CanUseFlame && _aC.hasFlame) {
            _xButtonModifier.interactable = true;
        } else {
            _xButtonModifier.interactable = false;
        }
        if (_aC.CanUseEarth && _aC.hasEarth) {
            _yButtonModifier.interactable = true;
        } else {
            _yButtonModifier.interactable = false;
        }
        if (_aC.CanUseWater && _aC.hasWater) {
            _bButtonModifier.interactable = true;
        } else {
            _bButtonModifier.interactable = false;
        }
        // A Button
        if (_pC._grounded || _pC.CanUseCoyote) {
            _aButtonModifier.color = EnabledButtonColor;
        } else {
            _aButtonModifier.color = DisabledButtonColor;
        }
    }
}
