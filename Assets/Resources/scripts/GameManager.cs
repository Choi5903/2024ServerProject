using UnityEngine;
using TMPro; // TMP ����� ���� �߰�
using UnityEngine.Networking;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TMP_Text currentLevelText;  // ���� ������ ǥ���ϴ� �ؽ�Ʈ
    public GameObject[] levelImages;  // �� ������ �ش��ϴ� �̹��� �迭
    private string baseUrl = "http://localhost:3000";
    private int loggedInUserId;  // �α��� �� �������� ���� ���� ID

    // Ž�� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void Explore()
    {
        StartCoroutine(ExploreCoroutine());
    }

    // Ž�縦 ó���ϴ� �ڷ�ƾ
    IEnumerator ExploreCoroutine()
    {
        // Ž�� ��û �� ���� JSON ������ �غ�
        string jsonData = "{\"userId\": " + loggedInUserId + "}";

        // POST ��û�� ���� UnityWebRequest ��ü ����
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/explore", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);  // JSON�� ����Ʈ �迭�� ��ȯ
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);  // JSON �����͸� ������ �ڵ鷯 ����
        request.downloadHandler = new DownloadHandlerBuffer();  // ������ ���� �ڵ鷯 ����
        request.SetRequestHeader("Content-Type", "application/json");  // ��û ����� Content-Type ����

        // ������ ��û�� ������ ���� ���
        yield return request.SendWebRequest();

        // ���� ���� ó��
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("���� ����: " + responseText);

            // ���� ���信 ���� ����/���� �޽��� ó�� �� UI ������Ʈ
            if (responseText.Contains("Ž�� ����"))
            {
                Debug.Log("Ž�� ����!");
                // ���� ������Ʈ (���� ������ ȭ�鿡 �ݿ�)
                UpdateUI(loggedInUserId);  // �������� ��ȯ�� ���� ������ ����� UI ������Ʈ
            }
            else
            {
                Debug.Log("Ž�� ����");
            }
        }
        else
        {
            Debug.LogError("���� ����: " + request.error);
        }
    }

    // UI ������Ʈ �޼��� (���� ������ �ݿ�)
    public void UpdateUI(int userId)
    {
        // �������� ���� ������ �޾ƿ��� GET ��û
        StartCoroutine(GetUserLevel(userId));
    }

    // �������� ���� ������ �������� �ڷ�ƾ
    IEnumerator GetUserLevel(int userId)
    {
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/getUserLevel?userId=" + userId, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            int level = int.Parse(responseText);  // �������� ���� ������ �Ľ�

            // ���� ������Ʈ (���� ������ ȭ�鿡 �ݿ�)
            currentLevelText.text = "LEVEL: " + level;

            // �ش� ������ �´� �̹����� Ȱ��ȭ
            for (int i = 0; i < levelImages.Length; i++)
            {
                levelImages[i].SetActive(i == level - 1);  // �ش� ������ �´� �̹����� Ȱ��ȭ
            }
        }
        else
        {
            Debug.LogError("���� ����: " + request.error);
        }
    }

    // �α��� �� userId�� �����ϴ� �޼���
    public void SetLoggedInUserId(int userId)
    {
        loggedInUserId = userId;  // �α��� �� ���� ���� ID�� ����
    }
}
