using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    // MAX_LEVEL 값을 20으로 설정
    private const int MAX_LEVEL = 20;

    public GameView gameView;    // GameView를 참조
    private PlayerModel playerModel;    // PlayerModel을 관리

    void Start()
    {
        // 버튼 클릭 리스너 설정
        gameView.SetExploreButtonListener(OnExploreButtonClicked);
        StartCoroutine(RegisterPlayer("CREW#" + Random.Range(1000, 9999).ToString())); // 회원가입 시 플레이어 이름 설정
    }

    // 회원가입 처리 (플레이어 ID 부여)
    private IEnumerator RegisterPlayer(string playerName)
    {
        // 기본 탐사 단계 1로 설정
        playerModel = new PlayerModel(playerName, 1);
        UpdateUI();
        yield return null;
    }

    // 탐사하기 버튼 클릭 시 처리
    public void OnExploreButtonClicked()
    {
        StartCoroutine(ExploreNextStage());
    }

    // 서버와 통신하여 탐사 결과를 처리하고, 성공 시 진행
    private IEnumerator ExploreNextStage()
    {
        // 서버와 통신하여 탐사 성공 여부를 확인 (IEnumerator로 호출)
        yield return StartCoroutine(ServerCommunication.Explore(playerModel.currentStage));

        // 탐사 성공 여부 반환을 처리
        if (ServerCommunication.explorationSuccess)
        {
            // 성공 시, 다음 단계로 진행
            if (playerModel.currentStage < MAX_LEVEL)
            {
                playerModel.currentStage++;
            }
            else
            {
                // 이미 마지막 단계인 경우, 해당 메시지를 출력
                Debug.Log("이미 모든 단계를 완료했습니다.");
            }
            UpdateUI();
        }
        else
        {
            // 실패 시, 첫 단계로 되돌리기
            playerModel.currentStage = 1;
            UpdateUI();
        }

        yield return null;
    }

    // UI 업데이트
    private void UpdateUI()
    {
        string currentStageName = playerModel.GetStageName(playerModel.currentStage);
        string description = playerModel.GetStageDescription(playerModel.currentStage);
        Sprite stageImage = playerModel.GetStageImage(playerModel.currentStage);

        // 게임뷰에 정보 업데이트
        gameView.UpdatePlayerInfo(
            playerModel.playerName,
            currentStageName,
            stageImage,
            description,
            "Exploration log text",  // 탐사 로그 텍스트 (필요한 데이터를 넣어야 함)
            "Next area",  // 다음 단계 정보 (단계 이름 등)
            playerModel.currentStage
        );

        // 진행도 슬라이더 값 업데이트
        gameView.SetProgress(playerModel.GetProgress(playerModel.currentStage));
    }
}