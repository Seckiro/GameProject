using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Threading;
using System;

using UniRx;

public class Score : MonoBehaviour
{

    float sScore = 0;
    int time = 2;
    CancellationTokenSource _cancellationTokenSource;

    AsyncReactiveProperty<int> _curScore = new AsyncReactiveProperty<int>(0);

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _curScore.Subscribe(UpdataScore, _cancellationTokenSource.Token);
        _curScore.WithoutCurrent();
        _curScore.Value = 0;

        Observable.Interval(TimeSpan.FromSeconds(5)).Subscribe(_ => _curScore.Value += 50);
    }

    private async void UpdataScore(int value)
    {
        float chazhi = value - sScore;
        while (value > sScore)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            sScore += Time.deltaTime / time * chazhi;
            Debug.Log(sScore);
        }

    }
}
