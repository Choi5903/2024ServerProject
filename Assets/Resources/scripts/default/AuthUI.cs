using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour
{
    public TMP_InputField usernameInput; // TMP_InputField�� ����
    public TMP_InputField passwordInput; // TMP_InputField�� ����

    public Button registerButton;
    public Button loginButton;
    public Button logoutButton;
    public Button GetDataButton;

    public TextMeshProUGUI statusText; // TextMeshProUGUI�� ����

    private AuthManager authManager;

    void Start()
    {
        authManager = GetComponent<AuthManager>();
        registerButton.onClick.AddListener(OnRegisterClick);
        loginButton.onClick.AddListener(OnLoginClick);
        logoutButton.onClick.AddListener(OnLogoutClick);
        GetDataButton.onClick.AddListener(OnGetDataClick);
    }

    private void OnLoginClick()
    {
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        statusText.text = "�α��� ��...";
        yield return StartCoroutine(authManager.Login(usernameInput.text, passwordInput.text));
        statusText.text = "�α��� ����";
    }

    private void OnRegisterClick()
    {
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator RegisterCoroutine()
    {
        statusText.text = "ȸ������ ��...";
        yield return StartCoroutine(authManager.Register(usernameInput.text, passwordInput.text));
        statusText.text = "ȸ������ ����, �α��� ���ּ���";
    }

    private void OnLogoutClick()
    {
        StartCoroutine(LogoutCoroutine());
    }

    private IEnumerator LogoutCoroutine()
    {
        statusText.text = "�α׾ƿ� ��...";
        yield return StartCoroutine(authManager.Logout());
        statusText.text = "�α׾ƿ� ����";
    }

    private void OnGetDataClick()
    {
        StartCoroutine(GetDataCoroutine());
    }

    private IEnumerator GetDataCoroutine()
    {
        statusText.text = "������ ��û ��...";
        yield return StartCoroutine(authManager.GetProtectedData());
        statusText.text = "������ ��û �Ϸ�";
    }
}