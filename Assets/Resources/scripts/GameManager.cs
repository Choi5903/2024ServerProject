using UnityEngine;
using TMPro; // TMP 사용을 위해 추가
using UnityEngine.Networking;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TMP_Text crewIdText;        // CREW ID 텍스트
    public TMP_Text currentLocationText;  // CURRENT LOCATION 텍스트
    public TMP_Text explorationLogText;// EXPLORATION LOG 텍스트
    public TMP_Text situationReportText; // SITUATION REPROT 텍스트
    public TMP_Text nextAreaText;        // NEXT AREA 텍스트
    public TMP_Text riskLevelText;       // RISK LEVEL 텍스트
    public GameObject[] levelImages;     // 레벨 이미지 배열


    private string baseUrl = "http://localhost:3000";
    private int loggedInUserId;  // 로그인 후 서버에서 받은 유저 ID
    private int currentLevel = 1;
    private string[] locations = {"Lv1.행성 궤도 진입", "Lv2.대기 진입", "Lv3.착륙 단계", "Lv4.지표면 탐색", "Lv5.토착 생물 조우", "Lv6.탐사 기지 구축",
        "Lv7.협곡 진입", "Lv8.동굴 진입", "Lv9.지하 생명체 무리 조우", "Lv10.식생 샘플 채취", "Lv11.벽화 발견", "Lv12.고대 외계 시설 발견",
        "Lv13.시설 진입", "Lv14.방어로봇 조우", "Lv15.시설 데이터 베이스 분석", "Lv16.지하 엘레베이터 탑승", "Lv17.행성 코어 접근",
        "Lv18.코어 근접 탐사", "Lv19.코어 안정화", "Lv20.데이터 획득 및 철수"};
    private string[] riskLevels = { "매우 낮음", "낮음", "보통", "높음", "매우 높음" };

    void Start()
    {
        // 초기 UI 업데이트
        UpdateUI();
    }

    public void SetLoggedInUserId(int userId)
    {
        loggedInUserId = userId;
        crewIdText.text = userId.ToString();  //유저의 아이디를 문자열화 시켜 보여줌.
        UpdateUI();
    }

    // 탐사 버튼 클릭 시 호출되는 메서드
    public void Explore()
    {
        StartCoroutine(ExploreCoroutine());
    }

    // 탐사를 처리하는 코루틴
    IEnumerator ExploreCoroutine()
    {
        // 탐사 요청 시 보낼 JSON 데이터 준비
        string jsonData = "{\"userId\": " + loggedInUserId + "}";

        // POST 요청을 위한 UnityWebRequest 객체 생성
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/explore", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);  // JSON을 바이트 배열로 변환
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);  // JSON 데이터를 보내는 핸들러 설정
        request.downloadHandler = new DownloadHandlerBuffer();  // 응답을 받을 핸들러 설정
        request.SetRequestHeader("Content-Type", "application/json");  // 요청 헤더에 Content-Type 설정

        // 서버에 요청을 보내고 응답 대기
        yield return request.SendWebRequest();

        // 서버 응답 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("서버 응답: " + responseText);

            // 서버 응답에 따라 성공/실패 메시지 처리 및 UI 업데이트
            if (responseText.Contains("탐사 성공"))
            {
                currentLevel++;
                explorationLogText.text += "\n탐사 성공! 레벨 " + currentLevel + "로 이동."; //어디로 이동했는지 찍어주는 로그 텍스트
                situationReportText.text += "\n" + GetLocationName(currentLevel - 1) + " -> " + GetLocationName(currentLevel);
                //Debug.Log("탐사 성공!");
                //// 레벨 업데이트 (유저 레벨을 화면에 반영)
                //UpdateUI(loggedInUserId);  // 서버에서 반환된 유저 레벨을 사용해 UI 업데이트
            }
            else
            {
                currentLevel = 1;
                explorationLogText.text += "\n탐사 실패! 레벨 초기화.";
                situationReportText.text += "\n레벨 초기화. 다시 시작";
                Debug.Log("탐사 실패");
            }

            UpdateUI();
        }
        else
        {
            Debug.LogError("서버 오류: " + request.error);
        }
    }

    // UI 업데이트 메서드
    public void UpdateUI()
    {
        //레벨 텍스트 업데이트 
        currentLocationText.text = GetLocationName(currentLevel).ToString();

        //레벨 이미지 업데이트
        for (int i = 0; i < levelImages.Length; i++)
        {
            levelImages[i].SetActive(i == currentLevel - 1);
        }

        // 다음지역 및 위험도 업데이트
        if (currentLevel < locations.Length)
        {
            nextAreaText.text = GetLocationName(currentLevel + 1).ToString();
            riskLevelText.text = GetRiskLevel(currentLevel + 1).ToString();
        }
        else
        {
            nextAreaText.text = "None (최고 레벨)";
            riskLevelText.text = "None";
        }

        // 서버에서 유저 레벨을 받아오는 GET 요청
        //StartCoroutine(GetUserLevel(userId));
    }

    string GetLocationName(int level)
    {
        return locations[(level - 1) % locations.Length]; // 레벨에 따라 장소 이름 반환
    }
    
    string GetRiskLevel(int level)
    {
        return riskLevels[(level - 1) % riskLevels.Length]; // 레벨에 따라 위험도 반환
    }



    // 서버에서 유저 레벨을 가져오는 코루틴
    //IEnumerator GetUserLevel(int userId)
    //{
    //    UnityWebRequest request = new UnityWebRequest(baseUrl + "/getUserLevel?userId=" + userId, "GET");
    //    request.downloadHandler = new DownloadHandlerBuffer();

    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.Success)
    //    {
    //        string responseText = request.downloadHandler.text;
    //        int level = int.Parse(responseText);  // 서버에서 받은 레벨을 파싱

    //        // 레벨 업데이트 (유저 레벨을 화면에 반영)
    //        currentLocationText.text = "LEVEL: " + level;

    //        // 해당 레벨에 맞는 이미지만 활성화
    //        for (int i = 0; i < levelImages.Length; i++)
    //        {
    //            levelImages[i].SetActive(i == level - 1);  // 해당 레벨에 맞는 이미지만 활성화
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("서버 오류: " + request.error);
    //    }
    //}

       
}
