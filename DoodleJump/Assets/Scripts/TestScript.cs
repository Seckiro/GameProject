using System;
using UniRx;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public event Action<int> UpdataHighEventA;
    public event Action<int> UpdataHighEventB;

    void Start()
    {

        IObservable<int> observableA = Observable.FromEvent<int>(
            h => { UpdataHighEventA += h; Debug.Log($"AAAA{h}----"); },
            h => { UpdataHighEventA -= h; Debug.Log($"{h}AAAA"); });
        var disposA = observableA.Subscribe(value => Debug.Log("ReceivedA value: " + value));
        UpdataHighEventA += (value) => Debug.Log("ReceivedA1 value: " + value);
        UpdataHighEventA?.Invoke(1);
        disposA.Dispose();

        IObservable<int> observableB = Observable.FromEvent<Action<int>, int>(
           handler => value => { handler(value * 2); Debug.Log($"CCCC{handler}----"); },
            h => { UpdataHighEventB += h; Debug.Log($"BBBB{h}----"); },
            h => { UpdataHighEventB -= h; Debug.Log($"{h}BBBB"); });
        var disposB = observableB.Subscribe(value => Debug.Log("ReceivedB value: " + value));
        UpdataHighEventB += (value) => Debug.Log("ReceivedB1 value: " + value);
        UpdataHighEventB?.Invoke(1);
        disposB.Dispose();

    }




}
