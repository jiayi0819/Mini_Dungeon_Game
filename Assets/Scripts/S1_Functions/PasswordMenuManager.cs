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

    private void Update()
    {
        // --- ADDED: Press ESC to close the menu and back away ---
        if (isPasswordInputActive && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void ShowPasswordMenu(string correctPassword, Action onSuccess)
    {
        _correctPassword = correctPassword;
        _onSuccessCallback = onSuccess;

        isPasswordInputActive = true;
        panel.SetActive(true);
        inputField.text = "";

        StartCoroutine(ForceFocusSequence());
    }

    private System.Collections.IEnumerator ForceFocusSequence()
    {
        yield return null;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        }

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
            StartCoroutine(ForceFocusSequence());

            // NOTE: If you ALSO want the panel to completely close on a wrong password, 
            // uncomment the line below:
            // CloseMenu();
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