namespace Gram.Rpg.Client.Core.Messaging
{
    public class Every10thUpdateMessage : IMessage
    {
        private static readonly Every10thUpdateMessage instance = new Every10thUpdateMessage();

        public static Every10thUpdateMessage Instance(float deltaTime)
        {
            instance.DeltaTime = deltaTime;

            return instance;
        }

        private Every10thUpdateMessage()
        {
        }

        public float DeltaTime { get; private set; }
    }
}
