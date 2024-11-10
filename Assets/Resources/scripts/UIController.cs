using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [System.Serializable]
    public class UIObject
    {
        public RectTransform uiElement; // UI ������Ʈ
        public Vector2 targetPosition;   // �̵��� ��ǥ ��ġ (ȭ�� �� ��)
        public Vector2 moveInPosition;   // ���� ��ġ (ȭ�� �ۿ��� ������ ��ġ)
        private Vector2 initialPosition; // ���� ��ġ �����

        public void SetInitialPosition()
        {
            initialPosition = uiElement.anchoredPosition;
        }

        public Tween MoveOut()
        {
            // ���� ��ġ���� targetPosition���� �̵��ϴ� Tween�� ��ȯ
            return uiElement.DOAnchorPos(targetPosition, 0.5f);
        }

        public Tween MoveIn()
        {
            // moveInPosition���� ���� ��ġ�� �̵��ϴ� Tween�� ��ȯ
            uiElement.anchoredPosition = moveInPosition; // ���� ��ġ�� moveInPosition���� ����
            return uiElement.DOAnchorPos(targetPosition, 0.5f);
        }
    }

    public List<UIObject> uiObjectsToMoveOut; // ȭ�� ������ ���� UI ����Ʈ
    public List<UIObject> uiObjectsToMoveIn;  // ȭ�� �ۿ��� ��Ÿ�� UI ����Ʈ

    private void Start()
    {
        // �� UI ������Ʈ�� ���� ��ġ�� �����մϴ�.
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

        // MoveOut() ����
        foreach (var obj in uiObjectsToMoveOut)
        {
            sequence.Append(obj.MoveOut());
        }

        // 1�� ������ �� MoveIn() ����
        sequence.AppendInterval(1f);

        foreach (var obj in uiObjectsToMoveIn)
        {
            sequence.Append(obj.MoveIn());
        }
    }
}