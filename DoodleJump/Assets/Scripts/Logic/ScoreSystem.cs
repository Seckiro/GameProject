using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScoreSystem : SystemBase
{
    private Action<int> _updateScore;

    public ReactiveProperty<int> Score = new ReactiveProperty<int>(0);

    public override bool SystemActive { get; set; }

    public event Action<int> UpdateScore { add => _updateScore += value; remove => _updateScore -= value; }

    public override void SystemInit()
    {
        Score.Subscribe(var => _updateScore?.Invoke(var));
    }




}
