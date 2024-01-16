using MessagePipe;
using UnityEngine;
using VContainer.Unity;

public class MassagePipe : IStartable
{
    readonly IPublisher<MassagePipe, int> publisher;
    readonly ISubscriber<MassagePipe, int> subscriber;

    public MassagePipe(IPublisher<MassagePipe, int> publisher, ISubscriber<MassagePipe, int> subscriber)
    {
        this.publisher = publisher;
        this.subscriber = subscriber;
    }

    public void Start()
    {
        subscriber.Subscribe(this, (x) => Debug.Log("S1:" + x));
        publisher.Publish(this, 10);
        publisher.Publish(this, 20);
        publisher.Publish(this, 30);

        (var publicKey, var privateKey) = RSAUtility.GenerateRSAKey();

        string data = "先帝创业未半而中道崩殂，今天下三分，益州疲弊，此诚危急存亡之秋也。然侍卫之臣不懈于内，忠志之士忘身于外者，盖追先帝之殊遇，欲报之于陛下也。诚宜开张圣听，以光先帝遗德，恢弘志士之气，不宜妄自菲薄，引喻失义，以塞忠谏之路也。宫中府中，俱为一体，陟罚臧否，不宜异同。";

        Debug.Log(GZipUtility.ByteToString(GZipUtility.StringToByte(data, false), false));

        Debug.Log(GZipUtility.DecompressStringToString(GZipUtility.CompressStringToString(data)));

        string encrypt = RSAUtility.Encrypt(data, publicKey);
        string decrypt = RSAUtility.Decrypt(encrypt, privateKey);

        Debug.Log($"publicKey:{publicKey}\n privateKey:{privateKey}\n encrypt:{encrypt}\n decrypt:{decrypt}");

    }
}
