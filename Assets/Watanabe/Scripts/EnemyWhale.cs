﻿using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary> 鯨Enemy </summary>
public class EnemyWhale : MonoBehaviour
{
    [SerializeField] private GameObject _wavePrefab = default;
    [SerializeField] private Vector3 _waveStartPos = Vector3.zero;

    [Min(1f)]
    [Tooltip("何秒かけて波が降りきるか")]
    [SerializeField] private float _moveWaveTime = 1f;
    [Min(1f)]
    [Tooltip("何秒妨害するか")]
    [SerializeField] private float _sabotageTime = 1f;

    [SerializeField] private Transform[] _waves = default;

    private void Start()
    {
        _waves = new Transform[3];
        for (int i = 0; i < _waves.Length; i++)
        {
            var wave = Instantiate(_wavePrefab);
            wave.transform.SetParent(transform, false);

            _waves[i] = wave.GetComponent<Transform>();

            _waves[i].position = _waveStartPos * (i + 1);
        }
    }

    private void Update()
    {
        //以下テスト
        if (Input.GetKeyDown(KeyCode.Space)) Squirting();
        if (Input.GetKeyDown(KeyCode.Tab)) SquirtingScale();
    }

    /// <summary> 潮吹き妨害 </summary>
    private void Squirting()
    {
        var sequenceOne = DOTween.Sequence();
        var sequenceTwo = DOTween.Sequence();
        var sequenceThree = DOTween.Sequence();

        //強制終了（仮）
        //StartCoroutine(KillSequence(sequenceOne, sequenceTwo, sequenceThree));

        for (int i = 0; i < 2; i++)
        {
            sequenceOne.Append(_waves[0].DOMove(-_waveStartPos, _moveWaveTime))
                       .AppendCallback(() =>
                       {
                           Debug.Log("妨害終了0");
                           _waves[0].position = _waveStartPos;
                       });
            sequenceTwo.Append(_waves[1].DOMove(-_waveStartPos, _moveWaveTime * 1.5f))
                       .AppendCallback(() =>
                       {
                           Debug.Log("妨害終了1");
                           _waves[1].position = _waveStartPos;
                       });
            sequenceThree.Append(_waves[2].DOMove(-_waveStartPos, _moveWaveTime * 2f))
                         .AppendCallback(() =>
                         {
                             Debug.Log("妨害終了2");
                             _waves[2].position = _waveStartPos;
                         });
        }
    }

    private void SquirtingScale()
    {
        var sequence = DOTween.Sequence();

        //強制終了（仮）
        //StartCoroutine(KillSequence(sequence));

        sequence.Append(_waves[0].DOScale(new Vector3(1f, 3f, 1f), _sabotageTime))
                .Join(_waves[0].DOMove(new Vector3(0f, 0f, 0f), _sabotageTime))
                .Append(_waves[0].DOScale(new Vector3(1f, 1f, 1f), _sabotageTime))
                .Join(_waves[0].DOMove(-_waveStartPos, _sabotageTime))
                .AppendCallback(() =>
                {
                    Debug.Log("妨害終了");
                    _waves[0].localScale = new Vector3(1f, 1f, 1f);
                    _waves[0].position = _waveStartPos;
                });
    }

    /// <summary> 一定時間経ったら強制終了 </summary>
    private IEnumerator KillSequence(Tween tween)
    {
        yield return new WaitForSeconds(_sabotageTime);

        Debug.Log("終了");
        tween.Kill();
    }

    /// <summary> 水溜り </summary>
    private void WaterPuddle()
    {

    }
}
