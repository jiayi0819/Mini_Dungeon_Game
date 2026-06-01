using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class PasswordMenuManager : MonoBehaviour
{
    public static PasswordMenuManager Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_InputField inputField;

    private string _correctPassword;
    private Action _onSuccessCallback;

    public bool isPasswordInputActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
        inputField.characterLimit = 4;
        inputField.onEndEdit.AddListener(OnHitEnter);
    }

    public void ShowPasswordMenu(string correctPassword, Action onSuccess)
    {
        _correctPassword = correctPassword;
        _onSuccessCallback = onSuccess;

        isPasswordInputActive = true;
        panel.SetActive(true);
        inputField.text = "";

        // Start the focus sequence safely
        StartCoroutine(ForceFocusSequence());
    }

    private System.Collections.IEnumerator ForceFocusSequence()
    {
        // 1. Wait exactly 1 frame to let the Panel turn on and layout update
        yield return null;

        // 2. Clear any weird selection states
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);

            // 3. Force select the actual text box object
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        }

        // 4. Force the blinking cursor alive
        inputField.ActivateInputField();
        inputField.Select();
    }

    private void OnHitEnter(string currentText)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckPassword();
        }
    }

    public void CheckPassword()
    {
        if (inputField.text == _correctPassword)
        {
            Debug.Log("Access Granted!");
            _onSuccessCallback?.Invoke();
            CloseMenu();
        }
        else
        {
            Debug.Log("Wrong Code!");
            inputField.text = "";
            StartCoroutine(ForceFocusSequence()); // Refocus automatically on fail
        }
    }

    public void CloseMenu()
    {
        panel.SetActive(false);
        isPasswordInputActive = false;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        inputField.DeactivateInputField();
    }
}