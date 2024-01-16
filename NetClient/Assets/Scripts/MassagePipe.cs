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

        string data = "�ȵ۴�ҵδ����е����㣬���������֣�����ƣ�ף��˳�Σ������֮��Ҳ��Ȼ����֮����и���ڣ���־֮ʿ���������ߣ���׷�ȵ�֮����������֮�ڱ���Ҳ�����˿���ʥ�����Թ��ȵ��ŵ£��ֺ�־ʿ֮�����������ԷƱ�������ʧ�壬��������֮·Ҳ�����и��У���Ϊһ�壬�췣갷񣬲�����ͬ��";

        Debug.Log(GZipUtility.ByteToString(GZipUtility.StringToByte(data, false), false));

        Debug.Log(GZipUtility.DecompressStringToString(GZipUtility.CompressStringToString(data)));

        string encrypt = RSAUtility.Encrypt(data, publicKey);
        string decrypt = RSAUtility.Decrypt(encrypt, privateKey);

        Debug.Log($"publicKey:{publicKey}\n privateKey:{privateKey}\n encrypt:{encrypt}\n decrypt:{decrypt}");

    }
}
