using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro�� ����Ϸ��� �� ���ӽ����̽��� �ʿ��մϴ�.
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    // UI ��ҵ� (TextMeshProUGUI�� ����)
    public TMP_Text playerNameText;    // TextMeshPro�� ����
    public TMP_Text stageNameText;     // TextMeshPro�� ����
    public TMP_Text stageDescriptionText; // TextMeshPro�� ����
    public Image stageImage;
    public TMP_Text explorationLogText; // TextMeshPro�� ����
    public TMP_Text nextAreaText;      // TextMeshPro�� ����
    public Slider progressSlider;

    public Button exploreButton;

    // UI ������Ʈ �޼���
    public void UpdatePlayerInfo(string playerName, string stageName, Sprite stageSprite, string description, string logText, string nextArea, int currentStage)
    {
        playerNameText.text = playerName;
        stageNameText.text = stageName;
        stageDescriptionText.text = description;

        // �ܰ迡 �´� �̹����� ���ҽ� �������� �ҷ�����
        string imagePath = "StageImages/Stage" + currentStage; // "StageImages"�� Resources ���� ���� ����� ���� �̸�
        stageSprite = Resources.Load<Sprite>(imagePath); // �ش� ��ο��� �̹��� �ҷ�����

        // �̹����� ������ ��쿡�� ����
        if (stageSprite != null)
        {
            stageImage.sprite = stageSprite;
        }
        else
        {
            Debug.LogWarning($"Stage image not found for stage {currentStage}");
        }

        explorationLogText.text = logText;
        nextAreaText.text = nextArea;
    }

    public void SetProgress(float progress)
    {
        progressSlider.value = progress;
    }

    // Ž�� ��ư Ŭ�� ������ ����
    public void SetExploreButtonListener(UnityEngine.Events.UnityAction action)
    {
        exploreButton.onClick.AddListener(action);
    }
}
