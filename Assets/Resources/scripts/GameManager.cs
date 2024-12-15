using UnityEngine;
using TMPro; // TMP ����� ���� �߰�
using UnityEngine.Networking;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TMP_Text crewIdText;        // CREW ID �ؽ�Ʈ
    public TMP_Text currentLocationText;  // CURRENT LOCATION �ؽ�Ʈ
    public TMP_Text explorationLogText;// EXPLORATION LOG �ؽ�Ʈ
    public TMP_Text situationReportText; // SITUATION REPROT �ؽ�Ʈ
    public TMP_Text nextAreaText;        // NEXT AREA �ؽ�Ʈ
    public TMP_Text riskLevelText;       // RISK LEVEL �ؽ�Ʈ
    public GameObject[] levelImages;     // ���� �̹��� �迭


    private string baseUrl = "http://localhost:3000";
    private int loggedInUserId;  // �α��� �� �������� ���� ���� ID
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

    public void SetLoggedInUserId(int userId)
    {
        loggedInUserId = userId;
        crewIdText.text = userId.ToString();  //������ ���̵� ���ڿ�ȭ ���� ������.
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

            // ���� ���信 ���� ����/���� �޽��� ó�� �� UI ������Ʈ
            if (responseText.Contains("Ž�� ����"))
            {
                currentLevel++;
                explorationLogText.text += "\nŽ�� ����! ���� " + currentLevel + "�� �̵�."; //���� �̵��ߴ��� ����ִ� �α� �ؽ�Ʈ
                situationReportText.text += "\n" + GetLocationName(currentLevel - 1) + " -> " + GetLocationName(currentLevel);
                //Debug.Log("Ž�� ����!");
                //// ���� ������Ʈ (���� ������ ȭ�鿡 �ݿ�)
                //UpdateUI(loggedInUserId);  // �������� ��ȯ�� ���� ������ ����� UI ������Ʈ
            }
            else
            {
                currentLevel = 1;
                explorationLogText.text += "\nŽ�� ����! ���� �ʱ�ȭ.";
                situationReportText.text += "\n���� �ʱ�ȭ. �ٽ� ����";
                Debug.Log("Ž�� ����");
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
