using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button Btn_Finish, Btn_UnFinish, Btn_Delete;
    public TextMeshProUGUI Tmp_Info;
    public RectTransform RectTrans_Bg, RectTrans_Delete;

    private ScrollRect Obj_ScrollRect;
    private TaskData _data;
    public TaskData Data => _data;

    private bool _scrollDirX = false;
    public float _speed = 10;
    private bool _lockDelete = false;

    private void Awake()
    {
        Btn_Finish.onClick.AddListener(BtnOnClick_Finish);
        Btn_UnFinish.onClick.AddListener(BtnOnClick_UnFinish);
        Btn_Delete.onClick.AddListener(BtnOnClick_Delete);
        Obj_ScrollRect = transform.GetComponentInParent<ScrollRect>();
    }

    public void UpdateInfo(TaskData data, bool isCreating)
    {
        _data = data;
        Tmp_Info.text = data.Title;

        if (isCreating)
        {
            Btn_Finish.interactable = false;
            Btn_UnFinish.gameObject.SetActiveEx(false);
        }
        else
        {
            Btn_Finish.gameObject.SetActiveEx(!data.IsFinish);
            Btn_UnFinish.gameObject.SetActiveEx(data.IsFinish);

            Tmp_Info.color = data.IsFinish ? new Color32(210, 210, 210, 255) : Color.black;
        }
    }

    private void BtnOnClick_Finish()
    {
        _data.IsFinish = true;

        var panel = UIManager.Instance.GetPanel<UITaskPanel>();
        panel.UpdateInfo();
    }

    private void BtnOnClick_UnFinish()
    {
        _data.IsFinish = false;

        var panel = UIManager.Instance.GetPanel<UITaskPanel>();
        panel.UpdateInfo();
    }

    private void BtnOnClick_Delete()
    {
        var panel = UIManager.Instance.GetPanel<UITaskPanel>();
        panel.UpdateInfo();
        //É¾³ý_data
        panel.DeleteSubItem(_data);

        GameObject.Destroy(this.gameObject);
    }


    #region ÍÏ¶¯Âß¼­

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta;
        _scrollDirX = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);

        if (!_scrollDirX)
        {
            Obj_ScrollRect.OnBeginDrag(eventData);
        }
        else
        {
            DOTween.Kill("EndDragTween");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_scrollDirX)
        {
            Obj_ScrollRect.OnEndDrag(eventData);

        }
        else
        {
            if (!_lockDelete)
                RectTrans_Bg.DOAnchorPosX(0, 0.2f).SetId("EndDragTween");
            else
                RectTrans_Bg.DOAnchorPosX(-RectTrans_Delete.rect.width, 0.2f).SetId("EndDragTween");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_scrollDirX)
        {
            Obj_ScrollRect.OnDrag(eventData);
        }
        else
        {
            Vector2 delta = ((PointerEventData)eventData).delta;
            var movePos = _speed * delta.x * Time.unscaledDeltaTime;
            var targetPos = RectTrans_Bg.anchoredPosition.x + movePos;

            var width = RectTrans_Delete.rect.width;

            if (targetPos > 0)
            {
                RectTrans_Bg.anchoredPosition = Vector2.zero;
                return;
            }
            else
            {
                if (delta.x < 0 && targetPos < -width / 3)
                {
                    _lockDelete = true;
                }
                else
                {
                    _lockDelete = false;
                }

                if (targetPos < -width)
                {
                    targetPos = -width;
                }

                RectTrans_Bg.anchoredPosition = new Vector2(targetPos, 0);
            }
        }
    }

    #endregion
}
