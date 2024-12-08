using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public Button registerButton;
    public Button loginButton;
    public Button logoutButton;

    public TextMeshProUGUI statusText;

    private AuthManager authManager;

    void Start()
    {
        authManager = GetComponent<AuthManager>();

        // ��ǲ �ʵ忡 �⺻ �ؽ�Ʈ ����
        usernameInput.text = "ID";
        passwordInput.text = "PASSWORD";

        // ��ư �̺�Ʈ ����
        registerButton.onClick.AddListener(OnRegisterClick);
        loginButton.onClick.AddListener(OnLoginClick);
        logoutButton.onClick.AddListener(OnLogoutClick);

        // ��ǲ �ʵ� ���� �� �ʱ�ȭ�ϴ� �̺�Ʈ ����
        usernameInput.onSelect.AddListener(delegate { OnInputFieldSelected(usernameInput, "ID"); });
        passwordInput.onSelect.AddListener(delegate { OnInputFieldSelected(passwordInput, "PASSWORD"); });
    }

    private void OnInputFieldSelected(TMP_InputField inputField, string defaultText)
    {
        // ��ǲ �ʵ尡 ���õǾ��� ��, �ؽ�Ʈ�� �⺻���� ��쿡�� ����
        if (inputField.text == defaultText)
        {
            inputField.text = "";
        }
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
}
