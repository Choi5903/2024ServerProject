using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    // MAX_LEVEL ���� 20���� ����
    private const int MAX_LEVEL = 20;

    public GameView gameView;    // GameView�� ����
    private PlayerModel playerModel;    // PlayerModel�� ����

    void Start()
    {
        // ��ư Ŭ�� ������ ����
        gameView.SetExploreButtonListener(OnExploreButtonClicked);
        StartCoroutine(RegisterPlayer("CREW#" + Random.Range(1000, 9999).ToString())); // ȸ������ �� �÷��̾� �̸� ����
    }

    // ȸ������ ó�� (�÷��̾� ID �ο�)
    private IEnumerator RegisterPlayer(string playerName)
    {
        // �⺻ Ž�� �ܰ� 1�� ����
        playerModel = new PlayerModel(playerName, 1);
        UpdateUI();
        yield return null;
    }

    // Ž���ϱ� ��ư Ŭ�� �� ó��
    public void OnExploreButtonClicked()
    {
        StartCoroutine(ExploreNextStage());
    }

    // ������ ����Ͽ� Ž�� ����� ó���ϰ�, ���� �� ����
    private IEnumerator ExploreNextStage()
    {
        // ������ ����Ͽ� Ž�� ���� ���θ� Ȯ�� (IEnumerator�� ȣ��)
        yield return StartCoroutine(ServerCommunication.Explore(playerModel.currentStage));

        // Ž�� ���� ���� ��ȯ�� ó��
        if (ServerCommunication.explorationSuccess)
        {
            // ���� ��, ���� �ܰ�� ����
            if (playerModel.currentStage < MAX_LEVEL)
            {
                playerModel.currentStage++;
            }
            else
            {
                // �̹� ������ �ܰ��� ���, �ش� �޽����� ���
                Debug.Log("�̹� ��� �ܰ踦 �Ϸ��߽��ϴ�.");
            }
            UpdateUI();
        }
        else
        {
            // ���� ��, ù �ܰ�� �ǵ�����
            playerModel.currentStage = 1;
            UpdateUI();
        }

        yield return null;
    }

    // UI ������Ʈ
    private void UpdateUI()
    {
        string currentStageName = playerModel.GetStageName(playerModel.currentStage);
        string description = playerModel.GetStageDescription(playerModel.currentStage);
        Sprite stageImage = playerModel.GetStageImage(playerModel.currentStage);

        // ���Ӻ信 ���� ������Ʈ
        gameView.UpdatePlayerInfo(
            playerModel.playerName,
            currentStageName,
            stageImage,
            description,
            "Exploration log text",  // Ž�� �α� �ؽ�Ʈ (�ʿ��� �����͸� �־�� ��)
            "Next area",  // ���� �ܰ� ���� (�ܰ� �̸� ��)
            playerModel.currentStage
        );

        // ���൵ �����̴� �� ������Ʈ
        gameView.SetProgress(playerModel.GetProgress(playerModel.currentStage));
    }
}