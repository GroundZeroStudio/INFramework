using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace INFrameworkDesign.Example
{
	public class CounterAppSimpleController : MonoBehaviour
	{
        [SerializeField]
        private CounterAppSimpleView m_View;

        private CounterAppSimpleModel m_Model = new CounterAppSimpleModel();
        private void Start()
        {
            m_View = GetComponent<CounterAppSimpleView>();

            m_Model.Count.Select(count => count.ToString()).SubscribeToText(m_View.NumberTxt).AddTo(this);

            m_View.AddBtn.onClick.AddListener(() => m_Model.Count.Value++);
            m_View.SubBtn.onClick.AddListener(() => m_Model.Count.Value--);
        }

        void UpdateView(int nCount)
        {
            m_View.NumberTxt.text = m_Model.Count.ToString();
        }

        private void Update()
        {
        }
    }
}

