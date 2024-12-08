using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json; // JSON 라이브러리 추가
using System;

public class GameAPI : MonoBehaviour
{
    private string baseUrl = "http://localhost:4000/api"; // Node.js 서버 URL
    private PlayerModel playerModel; // PlayerModel 변수를 클래스에 추가

    // 플레이어 등록 메서드
    public IEnumerator RegisterPlayer(string playerName, string password)
    {
        var requestData = new { name = playerName, password = password };
        string jsonData = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error registering player: {request.error}");
            }
            else
            {
                Debug.Log("Player registered successfully");
            }
        }
    }

    // 플레이어 로그인 메서드
    public IEnumerator LoginPlayer(string playerName, string password, Action<PlayerModel> onSuccess)
    {
        var requestData = new { name = playerName, password = password };
        string jsonData = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error logging in: {request.error}");
            }
            else
            {
                // 응답을 처리하여 PlayerModel 생성
                string responseBody = request.downloadHandler.text;
                try
                {
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                    // 서버에서 탐사 단계만 포함된 정보로 PlayerModel 생성
                    playerModel = new PlayerModel("CREW#" + UnityEngine.Random.Range(1000, 9999).ToString(), 1); // 기본 탐사 단계 1로 설정

                    onSuccess?.Invoke(playerModel); // PlayerModel 반환
                    Debug.Log("Login successful");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error processing login response: {ex.Message}");
                }
            }
        }
    }
}
