using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private TextMeshProUGUI _promptText;

    public bool IsDisplayed = false;
    
    // ADD THIS: A global way to lock the prompt when menus are open
    public bool IsLocked = false;

    private void Start()
    {
        _mainCam = Camera.main;
        _uiPanel.SetActive(false);
    }

    //private void LateUpdate()
    //{
    //    var rotation = _mainCam.transform.rotation;
    //    transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    //}



    public void SetUp(string promptText)
    {
        // MODIFY THIS: If it's locked, don't let it show up!
        if (IsLocked) return;

        _promptText.text = promptText;
        _uiPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        _promptText.text = "";
        _uiPanel.SetActive(false);
        IsDisplayed = false;

    }
}
