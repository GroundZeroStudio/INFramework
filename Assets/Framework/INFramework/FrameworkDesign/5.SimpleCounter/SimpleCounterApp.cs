using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INFrameworkDesign.Example
{
	public class SimpleCounterApp : MonoBehaviour
	{
		private Text m_NumberTxt;
		private Button m_AddBtn;
		private Button m_SubBtn;

        private int m_Count = 0;
        private void Start()
        {
            m_NumberTxt = transform.Find("TxtNumber").GetComponent<Text>();
            m_AddBtn = transform.Find("BtnAdd").GetComponent<Button>();
            m_SubBtn = transform.Find("BtnSub").GetComponent<Button>();

            m_AddBtn.onClick.AddListener(() =>
            {
                m_Count++;
                m_NumberTxt.text = m_Count.ToString();
            });

            m_SubBtn.onClick.AddListener(() =>
            {
                m_Count--;
                m_NumberTxt.text = m_Count.ToString();
            });
        }
    }
}

