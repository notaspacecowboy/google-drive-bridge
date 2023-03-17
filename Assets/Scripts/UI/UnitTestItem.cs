using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GoogleDriveBridge
{
    public class UnitTestItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _apiName;

        [SerializeField]
        private TextMeshProUGUI _testResult;

        public void SetAPIName(string name)
        {
            _apiName.text = name;
        }

        public void SetResult(bool result)
        {
            if (result)
            {
                _testResult.text = "PASS";
                _testResult.color = Color.blue;
            }
            else
            {
                _testResult.text = "FAILED";
                _testResult.color = Color.red;
            }

        }
    }
}
