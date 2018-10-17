using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentInputManager : MonoBehaviour {

    public static PersistentInputManager Instance { get; private set; }

    public enum InputEnum {
        kbm,
        gamepad,
        none
    }

    public InputEnum lastInput { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        lastInput = InputEnum.kbm;
    }

    private bool fire = false;
    private bool jump = false;
    private bool trans = false;
    private bool confirm = false;
    private bool exit = false;
    private bool pause = false;

    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float cannonX = 0.0f;
    private float cannonY = 0.0f;

    private float horizontalRaw = 0.0f;
    private float verticalRaw = 0.0f;
    private float cannonXRaw = 0.0f;
    private float cannonYRaw = 0.0f;

    // Use this for initialization
    void Start () {

    }

	// Update is called once per frame
	void Update () {

        checkInput("Fire", "Fire Gamepad", ref fire);
        checkInput("Jump", "Jump Gamepad", ref jump);
        checkInput("Transform", "Transform Gamepad", ref trans);
        checkInput("Confirm", "Confirm Gamepad", ref confirm);
        checkInput("Exit", "Exit Gamepad", ref exit);
        checkInput("Pause", "Pause Gamepad", ref pause);

        checkInputAxis("Horizontal", "Horizontal Gamepad", ref horizontal, ref horizontalRaw);
        checkInputAxis("Vertical", "Vertical Gamepad", ref vertical, ref verticalRaw);
        checkInputAxis("CannonX", "CannonX Gamepad", ref cannonX, ref cannonXRaw);
        checkInputAxis("CannonY", "CannonY Gamepad", ref cannonY, ref cannonYRaw);

    }

    void checkInput(string kbmString, string gamepadString, ref bool buttonBool) {
        if (Input.GetButtonDown(kbmString) || Input.GetButtonDown(gamepadString)) {
            buttonBool = true;
            if (Input.GetButtonDown(kbmString)) {
                lastInput = InputEnum.kbm;
            } else {
                lastInput = InputEnum.gamepad;
            }
        } else {
            buttonBool = false;
        }
    }

    void checkInputAxis(string kbmString, string gamepadString, ref float axisRef, ref float axisRefRaw) {
        if (Input.GetAxisRaw(gamepadString) != 0) {
            axisRefRaw = Input.GetAxisRaw(gamepadString);
            lastInput = InputEnum.gamepad;
        } else if (Input.GetAxisRaw(kbmString) != 0) {
            axisRefRaw = Input.GetAxisRaw(kbmString);
            lastInput = InputEnum.kbm;
        } else {
            axisRefRaw = 0.0f;
        }

        if (Input.GetAxis(gamepadString) != 0) {
            axisRef = Input.GetAxis(gamepadString);
            lastInput = InputEnum.gamepad;
        } else if (Input.GetAxis(kbmString) != 0) {
            axisRef = Input.GetAxis(kbmString);
            lastInput = InputEnum.kbm;
        } else {
            axisRef = 0.0f;
        }
    }

    public bool isFire() {
        return fire;
    }

    public bool isJump() {
        return jump;
    }

    public bool isTransform() {
        return trans;
    }

    public bool isConfirm() {
        return confirm;
    }

    public bool isExit() {
        return exit;
    }

    public bool isPause() {
        return pause;
    }

    public float getHorizontal() {
        return horizontal;
    }

    public float getVertical() {
        return vertical;
    }

    public float getCannonX() {
        return cannonX;
    }

    public float getCannonY() {
        return cannonY;
    }

    public float getHorizontalRaw() {
        return horizontalRaw;
    }

    public float getVerticalRaw() {
        return verticalRaw;
    }

    public float getCannonXRaw() {
        return cannonXRaw;
    }

    public float getCannonYRaw() {
        return cannonYRaw;
    }

}
