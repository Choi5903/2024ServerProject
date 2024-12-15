using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField usernameField;   // TMP InputField - Username
    public TMP_InputField passwordField;   // TMP InputField - Password
    public TMP_Text resultText;             // TMP Text - Result message (로그인/회원가입 성공 메시지)

    private string baseUrl = "http://localhost:3000";  // 서버 URL
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();  // GameManager 참조
    }

    // 회원가입 버튼 클릭 시 호출될 메서드
    public void Register()
    {
        StartCoroutine(RegisterCoroutine());
    }

    // 로그인 버튼 클릭 시 호출될 메서드
    public void Login()
    {
        StartCoroutine(LoginCoroutine());
    }

    // 회원가입을 처리하는 코루틴
    IEnumerator RegisterCoroutine()
    {
        // 데이터 준비
        UserData data = new UserData
        {
            username = usernameField.text,  // 아이디 입력값
            password = passwordField.text   // 비밀번호 입력값
        };

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);

        // 요청 생성
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/register", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData); // JSON을 바이트 배열로 변환
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");  // JSON 형식으로 요청 헤더 설정

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 서버 응답 처리
            string jsonResponse = request.downloadHandler.text;
            var response = JsonUtility.FromJson<ServerResponse>(jsonResponse);
            resultText.text = response.success ? $"회원가입 성공! 생성된 아이디 {response.username} " : "회원가입 실패: " + response.message;
        }
        else
        {
            resultText.text = "회원가입 실패: " + request.error;
        }
    }

    // 로그인 처리를 위한 코루틴
    IEnumerator LoginCoroutine()
    {
        // 데이터 준비
        UserData data = new UserData
        {
            username = usernameField.text,  // 아이디 입력값
            password = passwordField.text   // 비밀번호 입력값
        };

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);

        // 요청 생성
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/login", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData); // JSON을 바이트 배열로 변환
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");  // JSON 형식으로 요청 헤더 설정

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 서버 응답 처리
            string jsonResponse = request.downloadHandler.text;
            var response = JsonUtility.FromJson<ServerResponse>(jsonResponse);
            resultText.text = response.success ? "로그인 성공!" : "로그인 실패: " + response.message;
            if (response.success)
            {
                gameManager.SetLoggedInUserId(response.user.id, response.user.username);  // 로그인 시 받은 유저 ID 설정
                Debug.Log("로그인 유저 ID: " + response.user.id);
            }
        }
        else
        {
            resultText.text = "로그인 실패: " + request.error;
        }
    }
}

// 서버 응답을 처리할 클래스
[System.Serializable]
public class ServerResponse
{
    public bool success;
    public string message;
    public string username; // 생성된 아이디
    public User user;  // 로그인 성공 시 반환되는 유저 정보
}

// 유저 데이터를 보낼 클래스
[System.Serializable]
public class UserData
{
    public string username;
    public string password;
}

[System.Serializable]
public class User
{
    public int id;  // 유저 ID 추가
    public string username;
    public string password;
    public int level;
}
