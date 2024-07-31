using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandle : MonoBehaviour {
 
    // Player
    public GameObject Player;
    private PlayerController _pC;
    private AbilityController _aC;

    // Buttons
    public GameObject XButton;
    public GameObject YButton;
    public GameObject BButton;
    public GameObject AButton;
    public GameObject AButtonText;

    // button modifiers
    private Button _xButtonModifier;
    private Button _yButtonModifier;
    private Button _bButtonModifier;
    private Button _aButtonModifier;
    private TMP_Text _aButtonTextModifier;
    
    // Colors
    public Color EnabledButtonColor = new (255f, 255f, 255f);
    public Color DisabledButtonColor = new (120f, 120f, 120f);

    // Start is called before the first frame update
    void Start() {
        _pC = Player.GetComponent<PlayerController>();
        _aC = Player.GetComponent<AbilityController>();
        _xButtonModifier = XButton.GetComponent<Button>();
        _yButtonModifier = YButton.GetComponent<Button>();
        _bButtonModifier = BButton.GetComponent<Button>();
        _aButtonModifier = AButton.GetComponent<Button>();
        _aButtonTextModifier = AButtonText.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update() => ButtonActivate();
    

    void ButtonActivate() {   
        // Changes the opacity/color of the buttons when they are available

        // Flame Button
        if (_aC.CanUseDirFlame && _aC.hasFlame) {
            _xButtonModifier.interactable = true;
        } else {
            _xButtonModifier.interactable = false;
        }
        // Earth Button
        if (_aC.CanUseEarth && _aC.hasEarth) {
            _yButtonModifier.interactable = true;
        } else {
            _yButtonModifier.interactable = false;
        }
        // Water Button
        if (_aC.CanUseWater && _aC.hasWater) {
            _bButtonModifier.interactable = true;
        } else {
            _bButtonModifier.interactable = false;
        }
        // Jump Button
        if (_pC._grounded || _pC.CanUseCoyote) {
            _aButtonModifier.interactable = true;
            _aButtonTextModifier.color = EnabledButtonColor;
        } else {
            _aButtonModifier.interactable= false;
            _aButtonTextModifier.color = DisabledButtonColor;
        }
    }
}
