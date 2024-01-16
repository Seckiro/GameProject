using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GamePresenter : IStartable, ITickable
{
    [Inject]
    readonly IPublisher<GamePresenter, int> publisher;

    [Inject]
    readonly ISubscriber<GamePresenter, int> subscriber;



    public void Start()
    {

        subscriber.Subscribe(this, x => Debug.Log("G1:" + x));
        subscriber.Subscribe(this, x => Debug.Log("G2:" + x));

        publisher.Publish(this, 123);

    }

    public void Tick()
    {

    }
}
