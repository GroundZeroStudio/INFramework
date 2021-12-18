/****************************************************
    文件：OfflineData.cs
    作者：TA94
    日期：2021/10/27 21:4:9
    功能：Nothing
*****************************************************/
using INFramework.Framework.AssetBundles;
using UnityEngine;

public class OfflineData : MonoBehaviour
{
    public Rigidbody mRigidbody;
    public Collider mCollider;
    //所有节点
    public Transform[] mAllPoints;
    //所有节点子节点个数
    public int[] mAllPointChildCounts;
    public bool[] mAllPointActive;
    public Vector3[] mPos;
    public Vector3[] mScale;
    public Quaternion[] mRot;

    public virtual void BindData()
    {
        mRigidbody = gameObject.GetComponentInChildren<Rigidbody>(true);
        mCollider = gameObject.GetComponentInChildren<Collider>(true);
        mAllPoints = gameObject.GetComponentsInChildren<Transform>(true);
        int allPointCount = mAllPoints.Length;

        mAllPointChildCounts = new int[allPointCount];
        mAllPointActive = new bool[allPointCount];
        mPos = new Vector3[allPointCount];
        mScale = new Vector3[allPointCount];
        mRot = new Quaternion[allPointCount];

        for (int i = 0; i < allPointCount; i++)
        {
            Transform tempTrs = mAllPoints[i];
            mAllPointChildCounts[i] = tempTrs.childCount;
            mAllPointActive[i] = tempTrs.gameObject.activeSelf;
            mPos[i] = tempTrs.localPosition;
            mScale[i] = tempTrs.localScale;
            mRot[i] = tempTrs.localRotation;
        }

    }

    /// <summary>
    /// 还原属性
    /// </summary>
    public virtual void ResetProp()
    {
        int allPoints = mAllPoints.Length;
        for (int i = 0; i < allPoints; i++)
        {
            Transform tempTrs = mAllPoints[i];
            if(tempTrs != null)
            {
                tempTrs.localPosition = mPos[i];
                tempTrs.localScale = mScale[i];
                tempTrs.localRotation = mRot[i];
            }

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
            if(tempTrs.childCount > mAllPointChildCounts[i])
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
}
