using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class ServerCommunication
{
    public static bool explorationSuccess;

    public static IEnumerator Explore(int currentStage)
    {
        string url = "http://localhost:4000/explore";
        string token = "your-jwt-token";  // ���� �α��� �� ���� JWT ��ū���� ��ü

        // ��û ����
        WWWForm form = new WWWForm();
        form.AddField("currentStage", currentStage);

        // HTTP POST ��û
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader("Authorization", "Bearer " + token);

        // ��û ������ �� ���� ���
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // JSON ���� ó��
            string response = request.downloadHandler.text;
            ExploreResponse exploreResponse = JsonUtility.FromJson<ExploreResponse>(response);

            if (exploreResponse.success)
            {
                explorationSuccess = true;
                Debug.Log("Ž�� ����: " + exploreResponse.message);
            }
            else
            {
                explorationSuccess = false;
                Debug.Log("Ž�� ����: " + exploreResponse.message);
            }
        }
        else
        {
            // ���� ó��
            Debug.LogError($"���� ��û ����: {request.responseCode} - {request.error}");
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
