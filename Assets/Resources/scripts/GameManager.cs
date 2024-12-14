using UnityEngine;
using TMPro; // TMP 사용을 위해 추가
using UnityEngine.Networking;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TMP_Text currentLevelText;  // 현재 레벨을 표시하는 텍스트
    public GameObject[] levelImages;  // 각 레벨에 해당하는 이미지 배열
    private string baseUrl = "http://localhost:3000";
    private int loggedInUserId;  // 로그인 후 서버에서 받은 유저 ID

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
                Debug.Log("탐사 성공!");
                // 레벨 업데이트 (유저 레벨을 화면에 반영)
                UpdateUI(loggedInUserId);  // 서버에서 반환된 유저 레벨을 사용해 UI 업데이트
            }
            else
            {
                Debug.Log("탐사 실패");
            }
        }
        else
        {
            Debug.LogError("서버 오류: " + request.error);
        }
    }

    // UI 업데이트 메서드 (현재 레벨을 반영)
    public void UpdateUI(int userId)
    {
        // 서버에서 유저 레벨을 받아오는 GET 요청
        StartCoroutine(GetUserLevel(userId));
    }

    // 서버에서 유저 레벨을 가져오는 코루틴
    IEnumerator GetUserLevel(int userId)
    {
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/getUserLevel?userId=" + userId, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            int level = int.Parse(responseText);  // 서버에서 받은 레벨을 파싱

            // 레벨 업데이트 (유저 레벨을 화면에 반영)
            currentLevelText.text = "LEVEL: " + level;

            // 해당 레벨에 맞는 이미지만 활성화
            for (int i = 0; i < levelImages.Length; i++)
            {
                levelImages[i].SetActive(i == level - 1);  // 해당 레벨에 맞는 이미지만 활성화
            }
        }
        else
        {
            Debug.LogError("서버 오류: " + request.error);
        }
    }

    // 로그인 후 userId를 설정하는 메서드
    public void SetLoggedInUserId(int userId)
    {
        loggedInUserId = userId;  // 로그인 후 받은 유저 ID를 설정
    }
}
