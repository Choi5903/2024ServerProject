using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [System.Serializable]
    public class UIObject
    {
        public RectTransform uiElement; // UI 오브젝트
        public Vector2 targetPosition;   // 이동할 목표 위치 (화면 밖 등)
        public Vector2 moveInPosition;   // 들어올 위치 (화면 밖에서 설정된 위치)
        private Vector2 initialPosition; // 최초 위치 저장용

        public void SetInitialPosition()
        {
            initialPosition = uiElement.anchoredPosition;
        }

        public Tween MoveOut()
        {
            // 최초 위치에서 targetPosition으로 이동하는 Tween을 반환
            return uiElement.DOAnchorPos(targetPosition, 0.5f);
        }

        public Tween MoveIn()
        {
            // moveInPosition에서 최초 위치로 이동하는 Tween을 반환
            uiElement.anchoredPosition = moveInPosition; // 시작 위치를 moveInPosition으로 설정
            return uiElement.DOAnchorPos(targetPosition, 0.5f);
        }
    }

    public List<UIObject> uiObjectsToMoveOut; // 화면 밖으로 나갈 UI 리스트
    public List<UIObject> uiObjectsToMoveIn;  // 화면 밖에서 나타날 UI 리스트

    private void Start()
    {
        // 각 UI 오브젝트의 최초 위치를 설정합니다.
        foreach (var obj in uiObjectsToMoveOut)
        {
            obj.SetInitialPosition();
        }
        foreach (var obj in uiObjectsToMoveIn)
        {
            obj.SetInitialPosition();
        }
    }

    public void OnButtonClick()
    {
        Sequence sequence = DOTween.Sequence();

        // MoveOut() 실행
        foreach (var obj in uiObjectsToMoveOut)
        {
            sequence.Append(obj.MoveOut());
        }

        // 1초 딜레이 후 MoveIn() 실행
        sequence.AppendInterval(1f);

        foreach (var obj in uiObjectsToMoveIn)
        {
            sequence.Append(obj.MoveIn());
        }
    }
}