/****************************************************
    文件：UIOfflineData.cs
    作者：TA94
    日期：2021/10/27 21:52:4
    功能：Nothing
*****************************************************/
using INFramework.Framework.AssetBundles;
using UnityEngine;

public class UIOfflineData : OfflineData
{
    public Vector2[] mAnchorMax;
    public Vector2[] mAnchorMin;
    public Vector2[] mPivot;
    public Vector2[] mSizeDelta;
    public Vector3[] mAnchoredPos;
    public ParticleSystem[] mParticle;

    public override void BindData()
    {
        Transform[] allTrs = gameObject.GetComponentsInChildren<Transform>(true);
        int allPointCount = allTrs.Length;
        for (int i = 0; i < allPointCount; i++)
        {
            if(!(allTrs[i] is RectTransform))
            {
                allTrs[i].gameObject.AddComponent<RectTransform>();
            }
        }

        mAllPoints = gameObject.GetComponentsInChildren<RectTransform>(true);
        mParticle = gameObject.GetComponentsInChildren<ParticleSystem>(true);

        mAllPointChildCounts = new int[allPointCount];
        mAllPointActive = new bool[allPointCount];
        mPos = new Vector3[allPointCount];
        mScale = new Vector3[allPointCount];
        mRot = new Quaternion[allPointCount];
        mAnchorMax = new Vector2[allPointCount];
        mAnchorMin = new Vector2[allPointCount];
        mPivot = new Vector2[allPointCount];
        mSizeDelta = new Vector2[allPointCount];
        mAnchoredPos = new Vector3[allPointCount];
        mParticle = new ParticleSystem[allPointCount];

        for (int i = 0; i < mAllPoints.Length; i++)
        {
            RectTransform rects = mAllPoints[i] as RectTransform;
            mAllPointChildCounts[i] = rects.childCount;
            mAllPointActive[i] = rects.gameObject.activeSelf;
            mPos[i] = rects.localPosition;
            mScale[i] = rects.localScale;
            mRot[i] = rects.localRotation;
            mAnchorMax[i] = rects.anchorMax;
            mAnchorMin[i] = rects.anchorMin;
            mPivot[i] = rects.pivot;
            mSizeDelta[i] = rects.sizeDelta;
            mAnchoredPos[i] = rects.anchoredPosition3D;          
        }
    }

    public override void ResetProp()
    {
        int allPoint = mAllPoints.Length;
        for (int i = 0; i < allPoint; i++)
        {
            RectTransform tempTrs = mAllPoints[i] as RectTransform;
            if(tempTrs != null)
            {
                tempTrs.localPosition = mPos[i];
                tempTrs.localScale = mScale[i];
                tempTrs.localRotation = mRot[i];
                tempTrs.anchorMin = mAnchorMin[i];
                tempTrs.anchorMax = mAnchorMax[i];
                tempTrs.pivot = mPivot[i];
                tempTrs.sizeDelta = mSizeDelta[i];
                tempTrs.anchoredPosition3D = mAnchoredPos[i];

                if (mAllPointActive[i])
                {
                    if (!tempTrs.gameObject.activeSelf)
                    {
                        tempTrs.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (tempTrs.gameObject.activeSelf)
                    {
                        tempTrs.gameObject.SetActive(false);
                    }
                }
                if (tempTrs.childCount > mAllPointChildCounts[i])
                {
                    int childCount = tempTrs.childCount;
                    for (int j = mAllPointChildCounts[i]; j < childCount; j++)
                    {
                        GameObject tempObj = tempTrs.GetChild(i).gameObject;
                        if (!ObjectManager.Instance.IsObjectManagerCreate(tempObj))
                        {
                            GameObject.Destroy(tempObj);
                        }
                    }
                }
            }

        }

        for (int i = 0; i < mParticle.Length; i++)
        {
            mParticle[i].Clear();
            mParticle[i].Play();
        }
    }
}
