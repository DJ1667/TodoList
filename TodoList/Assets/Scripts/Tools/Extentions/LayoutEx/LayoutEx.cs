using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutEx : MonoBehaviour
{
    // public RectOffset m_Padding;
    public LayoutType m_layoutType = LayoutType.Horizontal;
    public float m_spacing = 0;
    public AnchorPresets m_childAlignmen = AnchorPresets.MiddleCenter;

    private RectTransform m_rectTransform;
    private List<RectTransform> m_childList = new List<RectTransform>();
    private bool _isInited = false;

    public enum LayoutType
    {
        Horizontal = 0,
        Vertical = 1
    }

    private void Awake()
    {
        Init();
        ReBuild();
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        Init();
        ReBuild();
    }

    [ContextMenu("重构布局")]
    public void ReBuild_Editor()
    {
        Init();
        ReBuild();
    }

#endif

    private void Init()
    {
        _isInited = true;

        m_rectTransform = GetComponent<RectTransform>();
        m_childList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i) as RectTransform;
            m_childList.Add(child);
            SetAnchorAndPivot(child);
        }
    }

    private void SetAnchorAndPivot(RectTransform rectTrans)
    {
        if (m_layoutType == LayoutType.Horizontal)
        {
            rectTrans.SetAnchor(AnchorPresets.TopLeft);
        }
    }

    public void ReBuild()
    {
        if (m_layoutType == LayoutType.Horizontal)
        {
            ReBuild_Horizontal();
        }
    }

    private void ReBuild_Horizontal()
    {
        switch (m_childAlignmen)
        {
            case AnchorPresets.TopLeft:
                ReBuild_TopLeft_Horizontal();
                break;
            case AnchorPresets.TopCenter:
                ReBuild_TopCenter_Horizontal();
                break;
            case AnchorPresets.TopRight:
                break;
            case AnchorPresets.MiddleLeft:
                break;
            case AnchorPresets.MiddleCenter:
                break;
            case AnchorPresets.MiddleRight:
                break;
            case AnchorPresets.BottomLeft:
                break;
            case AnchorPresets.BottomCenter:
                break;
            case AnchorPresets.BottomRight:
                break;
            case AnchorPresets.VertStretchLeft:
                break;
            case AnchorPresets.VertStretchRight:
                break;
            case AnchorPresets.VertStretchCenter:
                break;
            case AnchorPresets.HorStretchTop:
                break;
            case AnchorPresets.HorStretchMiddle:
                break;
            case AnchorPresets.HorStretchBottom:
                break;
            case AnchorPresets.StretchAll:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ReBuild_TopLeft_Horizontal()
    {
        float preTotalWidth = 0;
        for (int i = 0; i < m_childList.Count; i++)
        {
            var child = m_childList[i];
            var rect = child.rect;
            var width = rect.width;
            var height = rect.height;
            var pos = new Vector2(width / 2 + preTotalWidth, -height / 2);
            preTotalWidth += width;
            child.anchoredPosition = pos;
        }
    }

    private void ReBuild_TopCenter_Horizontal()
    {
        List<float> xList = new List<float>();

        if (CheckIsOutOfBounds(xList))
        {
            //退化成TopLeft排序
            ReBuild_TopLeft_Horizontal();
        }
        else
        {
            SetPosByList(xList);
        }
    }

    #region Helper

    /// <summary>
    /// 计算左边第一个会不会超出0的边界，如果超出直接以0为基准向右排
    /// </summary>
    /// <param name="xList"></param>
    /// <returns></returns>
    private bool CheckIsOutOfBounds(List<float> xList)
    {
        var widthParent = m_rectTransform.rect.width;

        var childCount = m_childList.Count;
        var half = childCount / 2;
        bool isOutOfBounds = false;
        float preTotalWidthR = 0;
        float preTotalWidthL = 0;


        float centerWidth = 0;
        bool isOdd = childCount % 2 != 0;
        if (isOdd) //奇数特殊处理
        {
            var centerChild = m_childList[half];
            centerWidth = centerChild.rect.width;
            xList.Add(widthParent / 2);
        }

        for (int i = isOdd ? half + 1 : half, j = half - 1; i < childCount && j >= 0; i++, j--)
        {
            var childR = m_childList[i];
            var childL = m_childList[j];
            var rectR = childR.rect;
            var rectL = childL.rect;
            var xr = widthParent / 2 + centerWidth / 2 + rectR.width / 2 + preTotalWidthR;
            var xl = widthParent / 2 - centerWidth / 2 - rectL.width / 2 - preTotalWidthL;

            xList.Add(xl);
            xList.Add(xr);
            // Debug.Log(j + "  xl: " + xl + "  xr: " + xr);
            preTotalWidthR += rectR.width;
            preTotalWidthL += rectL.width;
            if (xl - rectL.width / 2 < 0)
                isOutOfBounds = true;
        }

        return isOutOfBounds;
    }

    /// <summary>
    /// 对xList进行排序 从小到大,然后根据xList对子节点排序
    /// </summary>
    /// <param name="xList"></param>
    private void SetPosByList(List<float> xList)
    {
        xList.Sort();
        for (int i = 0; i < xList.Count; i++)
        {
            var child = m_childList[i];
            var pos = new Vector2(xList[i], -child.rect.height / 2);
            child.anchoredPosition = pos;
        }
    }

    #endregion
}