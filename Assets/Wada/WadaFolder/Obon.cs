using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obon : MonoBehaviour
{
    [Tooltip("プレイヤーがゲーム開始時に持っているお菓子の配列"), SerializeField]
    private GameObject[] _startOkasis;

    [Tooltip("プレイヤーがプレイ中に持っているお菓子の配列")]
    private List<GameObject> _okasis = new List<GameObject>();

    [Tooltip("プレイヤーがゲーム開始時に持っているお菓子の配列"), SerializeField]
    private int[] _int;


    private void Awake()
    {
        for (int i = 0; i < _startOkasis.Length; i++)
        {
            if(_okasis.Count == 0)
            {
                _okasis.Add(_startOkasis[0]);
                _okasis[0].transform.position = this.transform.position;
            }
            else
            {
                _okasis.Add(_startOkasis[i]);
                _okasis[i].transform.position = _okasis[i - 1].GetComponent<Sweets>().NextPos.position;
            }
        }
    }
    
    public void SweetsAdd(GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (_okasis.Count == 0)
            {
                _okasis.Add(gameObjects[0]);
                _okasis[0].transform.position = this.transform.position;
            }
            else
            {
                _okasis.Add(gameObjects[i]);
                _okasis[i].transform.position = _okasis[i - 1].GetComponent<Sweets>().NextPos.position;
            }
        }
    }
}
