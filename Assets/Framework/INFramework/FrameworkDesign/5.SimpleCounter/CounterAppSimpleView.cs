using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INFrameworkDesign.Example
{
	public class CounterAppSimpleView : MonoBehaviour
	{
        public Text NumberTxt;
        public Button AddBtn;
        public Button SubBtn;

        private void Awake()
        {
            NumberTxt = transform.Find("TxtNumber").GetComponent<Text>();
            AddBtn = transform.Find("BtnAdd").GetComponent<Button>();
            SubBtn = transform.Find("BtnSub").GetComponent<Button>();
        }
    }
}
