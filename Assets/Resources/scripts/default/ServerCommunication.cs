using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class ServerCommunication
{
    public static bool explorationSuccess;

    public static IEnumerator Explore(int currentStage)
    {
        string url = "http://localhost:4000/explore";
        string token = "your-jwt-token";  // 실제 로그인 후 받은 JWT 토큰으로 대체

        // 요청 본문
        WWWForm form = new WWWForm();
        form.AddField("currentStage", currentStage);

        // HTTP POST 요청
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader("Authorization", "Bearer " + token);

        // 요청 보내기 및 응답 대기
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // JSON 응답 처리
            string response = request.downloadHandler.text;
            ExploreResponse exploreResponse = JsonUtility.FromJson<ExploreResponse>(response);

            if (exploreResponse.success)
            {
                explorationSuccess = true;
                Debug.Log("탐험 성공: " + exploreResponse.message);
            }
            else
            {
                explorationSuccess = false;
                Debug.Log("탐험 실패: " + exploreResponse.message);
            }
        }
        else
        {
            // 실패 처리
            Debug.LogError($"서버 요청 실패: {request.responseCode} - {request.error}");
            explorationSuccess = false;
        }
    }
}

[System.Serializable]
public class ExploreResponse
{
    public bool success;
    public string message;
    public int progress;
}
