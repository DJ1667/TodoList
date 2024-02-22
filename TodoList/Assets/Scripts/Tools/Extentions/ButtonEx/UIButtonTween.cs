using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct GraphicParam
{
    public MaskableGraphic graphic;
    public Color originalColor;

}
/// <summary>
/// 按钮显示动画效果
/// </summary>
public class UIButtonTween : Button
{
    // 按钮效果
    private GraphicParam[] graphicParams;

    [Header("需要获取原始尺寸吗，不需要则默认为Vector.one")]
    public bool needGetOriginalScale = true;
    private Vector3 oriScale = Vector3.zero;
    
    public List<Tween> tweens = new List<Tween>();

    protected override void Awake()
    {
        base.Awake();

        MaskableGraphic[] tempGraphics = gameObject.GetComponentsInChildren<MaskableGraphic>();
        graphicParams = new GraphicParam[tempGraphics.Length];

        // 记录原始数据
        for (int i = 0; i < tempGraphics.Length; i++)
        {
            GraphicParam gp = new GraphicParam();
            gp.graphic = tempGraphics[i];
            gp.originalColor = tempGraphics[i].color;
            graphicParams[i] = gp;
        }
    }

    //protected override void Start()
    //{
    //    MaskableGraphic[] tempGraphics = gameObject.GetComponentsInChildren<MaskableGraphic>();
    //    graphicParams = new GraphicParam[tempGraphics.Length];

    //    // 记录原始数据
    //    for (int i = 0; i < tempGraphics.Length; i++)
    //    {
    //        GraphicParam gp = new GraphicParam();
    //        gp.graphic = tempGraphics[i];
    //        gp.originalColor = tempGraphics[i].color;
    //        graphicParams[i] = gp;
    //    }
    //    oriScale = transform.localScale;
    //}

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (!interactable)
            return;

        //gameObject.transform.DOKill();
        ClearTween();

        //tweens.Add(gameObject.transform.DOScale(oriScale, 0.12f).SetEase(Ease.InSine));
        //添加松开按钮回弹效果
        tweens.Add(gameObject.transform.DOScale(oriScale, 0.7f).SetEase(Ease.OutBounce));

        if (base.transition == Transition.ColorTint)
        {
            for (int i = 0; i < graphicParams.Length; i++)
            {
                if (graphicParams[i].graphic.transform.GetComponent<IgnoreButtonColorTween>() != null)
                    continue;

                graphicParams[i].graphic.DOKill();
                graphicParams[i].graphic.DOColor(graphicParams[i].originalColor, 0.2f);
            }
        }
    }

    public void ClearTween()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            tweens[i].Kill();
        }
        tweens.Clear();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (!interactable)
            return;
        
        //在首次点击的那刻才记录
        if (needGetOriginalScale && oriScale == Vector3.zero)
            oriScale = transform.localScale;

        //tweens.Add(gameObject.transform.DOScale(new Vector3(oriScale.x * 1.16f, oriScale.y * 1.16f, 1), 0.17f).SetEase(Ease.OutSine));
        //修改所有按钮点击后缩小
        tweens.Add(gameObject.transform.DOScale(new Vector3(oriScale.x * 0.90f, oriScale.y * 0.90f, 1), 0.17f).SetEase(Ease.OutSine));

        if (base.transition == Transition.ColorTint)
        {
            for (int i = 0; i < graphicParams.Length; i++)
            {
                if (graphicParams[i].graphic.transform.GetComponent<IgnoreButtonColorTween>() != null)
                    continue;

                graphicParams[i].graphic.DOKill();
                Color c = new Color(graphicParams[i].originalColor.r * 0.8f,
                    graphicParams[i].originalColor.g * 0.8f,
                    graphicParams[i].originalColor.b * 0.8f, 1);
                graphicParams[i].graphic.DOColor(c, 0.2f);
            }
        }
    }
}
