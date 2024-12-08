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

        // 인풋 필드에 기본 텍스트 설정
        usernameInput.text = "ID";
        passwordInput.text = "PASSWORD";

        // 버튼 이벤트 설정
        registerButton.onClick.AddListener(OnRegisterClick);
        loginButton.onClick.AddListener(OnLoginClick);
        logoutButton.onClick.AddListener(OnLogoutClick);

        // 인풋 필드 선택 시 초기화하는 이벤트 설정
        usernameInput.onSelect.AddListener(delegate { OnInputFieldSelected(usernameInput, "ID"); });
        passwordInput.onSelect.AddListener(delegate { OnInputFieldSelected(passwordInput, "PASSWORD"); });
    }

    private void OnInputFieldSelected(TMP_InputField inputField, string defaultText)
    {
        // 인풋 필드가 선택되었을 때, 텍스트가 기본값일 경우에만 지움
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
        statusText.text = "로그인 중...";
        yield return StartCoroutine(authManager.Login(usernameInput.text, passwordInput.text));
        statusText.text = "로그인 성공";
    }

    private void OnRegisterClick()
    {
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator RegisterCoroutine()
    {
        statusText.text = "회원가입 중...";
        yield return StartCoroutine(authManager.Register(usernameInput.text, passwordInput.text));
        statusText.text = "회원가입 성공, 로그인 해주세요";
    }

    private void OnLogoutClick()
    {
        StartCoroutine(LogoutCoroutine());
    }

    private IEnumerator LogoutCoroutine()
    {
        statusText.text = "로그아웃 중...";
        yield return StartCoroutine(authManager.Logout());
        statusText.text = "로그아웃 성공";
    }
}
