using UnityEngine;
using TMPro; // TMP ����� ���� �߰�
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text crewIdText;        // CREW ID �ؽ�Ʈ
    public TMP_Text currentLocationText;  // CURRENT LOCATION �ؽ�Ʈ
    public TMP_Text explorationLogText;// EXPLORATION LOG �ؽ�Ʈ
    public TMP_Text situationReportText; // SITUATION REPROT �ؽ�Ʈ
    public TMP_Text nextAreaText;        // NEXT AREA �ؽ�Ʈ
    public TMP_Text riskLevelText;       // RISK LEVEL �ؽ�Ʈ
    public GameObject[] levelImages;     // ���� �̹��� �迭

    public ScrollRect explorationLogScrollRect; // Ž�� �α��� ScrollRect
    public ScrollRect situationReportScrollRect; // ��Ȳ ������ ScrollRect

    private string baseUrl = "http://localhost:3000";
    private int loggedInUserId;  // �α��� �� �������� ���� ���� ID
    private string loggedInUsername; // �α��� �� �������� ���� ���� �̸� (CREW ID)
    private int currentLevel = 1;
    private string[] locations = {"Lv1.�༺ �˵� ����", "Lv2.��� ����", "Lv3.���� �ܰ�", "Lv4.��ǥ�� Ž��", "Lv5.���� ���� ����", "Lv6.Ž�� ���� ����",
        "Lv7.���� ����", "Lv8.���� ����", "Lv9.���� ����ü ���� ����", "Lv10.�Ļ� ���� ä��", "Lv11.��ȭ �߰�", "Lv12.��� �ܰ� �ü� �߰�",
        "Lv13.�ü� ����", "Lv14.���κ� ����", "Lv15.�ü� ������ ���̽� �м�", "Lv16.���� ���������� ž��", "Lv17.�༺ �ھ� ����",
        "Lv18.�ھ� ���� Ž��", "Lv19.�ھ� ����ȭ", "Lv20.������ ȹ�� �� ö��"};
    private string[] riskLevels = { "�ſ� ����", "����", "����", "����", "�ſ� ����" };

    void Start()
    {
        // �ʱ� UI ������Ʈ
        UpdateUI();
    }

    public void SetLoggedInUserId(int userId, string username)
    {
        loggedInUserId = userId;
        loggedInUsername = username;
        crewIdText.text = username; // CREW ID�� username ǥ��
        UpdateUI();
    }

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

            if (responseText.Contains("Ž�� ����"))
            {
                currentLevel++;
                AddExplorationLog($"Ž�� ����! ���� {currentLevel}�� �̵�.");
                AddSituationReport($"{GetLocationName(currentLevel - 1)} -> {GetLocationName(currentLevel)}");
            }
            else
            {
                currentLevel = 1;
                AddExplorationLog("Ž�� ����! ���� �ʱ�ȭ.");
                AddSituationReport("���� �ʱ�ȭ. �ٽ� ����.");
            }

            UpdateUI();
        }
        else
        {
            Debug.LogError("���� ����: " + request.error);
        }
    }

    // UI ������Ʈ �޼���
    public void UpdateUI()
    {
        //���� �ؽ�Ʈ ������Ʈ 
        currentLocationText.text = GetLocationName(currentLevel).ToString();

        //���� �̹��� ������Ʈ
        for (int i = 0; i < levelImages.Length; i++)
        {
            levelImages[i].SetActive(i == currentLevel - 1);
        }

        // �������� �� ���赵 ������Ʈ
        if (currentLevel < locations.Length)
        {
            nextAreaText.text = GetLocationName(currentLevel + 1).ToString();
            riskLevelText.text = GetRiskLevel(currentLevel + 1).ToString();
        }
        else
        {
            nextAreaText.text = "None (�ְ� ����)";
            riskLevelText.text = "None";
        }

        // �������� ���� ������ �޾ƿ��� GET ��û
        //StartCoroutine(GetUserLevel(userId));
    }

    string GetLocationName(int level)
    {
        return locations[(level - 1) % locations.Length]; // ������ ���� ��� �̸� ��ȯ
    }
    
    string GetRiskLevel(int level)
    {
        return riskLevels[(level - 1) % riskLevels.Length]; // ������ ���� ���赵 ��ȯ
    }

    private void AddExplorationLog(string message)
    {
        explorationLogText.text += message + "\n";

        // �ؽ�Ʈ �߰� �� Content ũ�� ���� ������Ʈ
        LayoutRebuilder.ForceRebuildLayoutImmediate(explorationLogText.GetComponent<RectTransform>());

        // ��ũ���� �� �Ʒ��� �̵�
        Canvas.ForceUpdateCanvases();
        explorationLogScrollRect.verticalNormalizedPosition = 0f;
    }

    private void AddSituationReport(string message)
    {
        situationReportText.text += message + "\n";

        // �ؽ�Ʈ �߰� �� Content ũ�� ���� ������Ʈ
        LayoutRebuilder.ForceRebuildLayoutImmediate(situationReportText.GetComponent<RectTransform>());

        // ��ũ���� �� �Ʒ��� �̵�
        Canvas.ForceUpdateCanvases();
        situationReportScrollRect.verticalNormalizedPosition = 0f;
    }

    // �������� ���� ������ �������� �ڷ�ƾ
    //IEnumerator GetUserLevel(int userId)
    //{
    //    UnityWebRequest request = new UnityWebRequest(baseUrl + "/getUserLevel?userId=" + userId, "GET");
    //    request.downloadHandler = new DownloadHandlerBuffer();

    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.Success)
    //    {
    //        string responseText = request.downloadHandler.text;
    //        int level = int.Parse(responseText);  // �������� ���� ������ �Ľ�

    //        // ���� ������Ʈ (���� ������ ȭ�鿡 �ݿ�)
    //        currentLocationText.text = "LEVEL: " + level;

    //        // �ش� ������ �´� �̹����� Ȱ��ȭ
    //        for (int i = 0; i < levelImages.Length; i++)
    //        {
    //            levelImages[i].SetActive(i == level - 1);  // �ش� ������ �´� �̹����� Ȱ��ȭ
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("���� ����: " + request.error);
    //    }
    //}


}
