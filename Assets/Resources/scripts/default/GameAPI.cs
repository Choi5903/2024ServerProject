using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json; // JSON ���̺귯�� �߰�
using System;

public class GameAPI : MonoBehaviour
{
    private string baseUrl = "http://localhost:4000/api"; // Node.js ���� URL
    private PlayerModel playerModel; // PlayerModel ������ Ŭ������ �߰�

    // �÷��̾� ��� �޼���
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

    // �÷��̾� �α��� �޼���
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
                // ������ ó���Ͽ� PlayerModel ����
                string responseBody = request.downloadHandler.text;
                try
                {
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                    // �������� Ž�� �ܰ踸 ���Ե� ������ PlayerModel ����
                    playerModel = new PlayerModel("CREW#" + UnityEngine.Random.Range(1000, 9999).ToString(), 1); // �⺻ Ž�� �ܰ� 1�� ����

                    onSuccess?.Invoke(playerModel); // PlayerModel ��ȯ
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
