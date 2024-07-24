using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandle : MonoBehaviour {
    public GameObject Player;
    public GameObject XButton;
    public GameObject YButton;
    public GameObject AButtonText;
    public GameObject BButton;
    public Color EnabledButtonColor = new (255f, 255f, 255f);
    public Color DisabledButtonColor = new (120f, 120f, 120f);

    private PlayerController _pController;
    private Button _xButtonModifier; 
    private Button _yButtonModifier;
    private TMP_Text _aButtonModifier;
    private Button _bButtonModifier;

    // Start is called before the first frame update
    void Start() {
        _pController = Player.GetComponent<PlayerController>();
        _xButtonModifier = XButton.GetComponent<Button>();
        _yButtonModifier = YButton.GetComponent<Button>();
        _aButtonModifier = AButtonText.GetComponent<TMP_Text>();
        _bButtonModifier = BButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update() {
        buttonActivate();
    }

    void buttonActivate() {   
        // Changes the opacity/color of the buttons when they are available

        // Flame Button
        if (_pController._canUseFlame && _pController.hasFlame) {
            _xButtonModifier.interactable = true;
        } else {
            _xButtonModifier.interactable = false;
        }
        if (_pController._canUseEarth && _pController.hasEarth) {
            _yButtonModifier.interactable = true;
        } else {
            _yButtonModifier.interactable = false;
        }
        if (_pController._canUseWater && _pController.hasWater) {
            _bButtonModifier.interactable = true;
        } else {
            _bButtonModifier.interactable = false;
        }
        // A Button
        if (_pController._grounded || _pController.CanUseCoyote) {
            _aButtonModifier.color = EnabledButtonColor;
        } else {
            _aButtonModifier.color = DisabledButtonColor;
        }
    }
}
