using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField usernameField;   // TMP InputField - Username
    public TMP_InputField passwordField;   // TMP InputField - Password
    public TMP_Text resultText;             // TMP Text - Result message (�α���/ȸ������ ���� �޽���)

    private string baseUrl = "http://localhost:3000";  // ���� URL
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();  // GameManager ����
    }

    // ȸ������ ��ư Ŭ�� �� ȣ��� �޼���
    public void Register()
    {
        StartCoroutine(RegisterCoroutine());
    }

    // �α��� ��ư Ŭ�� �� ȣ��� �޼���
    public void Login()
    {
        StartCoroutine(LoginCoroutine());
    }

    // ȸ�������� ó���ϴ� �ڷ�ƾ
    IEnumerator RegisterCoroutine()
    {
        // ������ �غ�
        UserData data = new UserData
        {
            username = usernameField.text,  // ���̵� �Է°�
            password = passwordField.text   // ��й�ȣ �Է°�
        };

        // JSON���� ��ȯ
        string jsonData = JsonUtility.ToJson(data);

        // ��û ����
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/register", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData); // JSON�� ����Ʈ �迭�� ��ȯ
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");  // JSON �������� ��û ��� ����

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // ���� ���� ó��
            string jsonResponse = request.downloadHandler.text;
            var response = JsonUtility.FromJson<ServerResponse>(jsonResponse);
            resultText.text = response.success ? $"ȸ������ ����! ������ ���̵� {response.username} " : "ȸ������ ����: " + response.message;
        }
        else
        {
            resultText.text = "ȸ������ ����: " + request.error;
        }
    }

    // �α��� ó���� ���� �ڷ�ƾ
    IEnumerator LoginCoroutine()
    {
        // ������ �غ�
        UserData data = new UserData
        {
            username = usernameField.text,  // ���̵� �Է°�
            password = passwordField.text   // ��й�ȣ �Է°�
        };

        // JSON���� ��ȯ
        string jsonData = JsonUtility.ToJson(data);

        // ��û ����
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/login", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData); // JSON�� ����Ʈ �迭�� ��ȯ
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");  // JSON �������� ��û ��� ����

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // ���� ���� ó��
            string jsonResponse = request.downloadHandler.text;
            var response = JsonUtility.FromJson<ServerResponse>(jsonResponse);
            resultText.text = response.success ? "�α��� ����!" : "�α��� ����: " + response.message;
            if (response.success)
            {
                gameManager.SetLoggedInUserId(response.user.id, response.user.username);  // �α��� �� ���� ���� ID ����
                Debug.Log("�α��� ���� ID: " + response.user.id);
            }
        }
        else
        {
            resultText.text = "�α��� ����: " + request.error;
        }
    }
}

// ���� ������ ó���� Ŭ����
[System.Serializable]
public class ServerResponse
{
    public bool success;
    public string message;
    public string username; // ������ ���̵�
    public User user;  // �α��� ���� �� ��ȯ�Ǵ� ���� ����
}

// ���� �����͸� ���� Ŭ����
[System.Serializable]
public class UserData
{
    public string username;
    public string password;
}

[System.Serializable]
public class User
{
    public int id;  // ���� ID �߰�
    public string username;
    public string password;
    public int level;
}
