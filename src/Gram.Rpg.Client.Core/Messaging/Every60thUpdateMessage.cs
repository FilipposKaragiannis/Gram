namespace Gram.Rpg.Client.Core.Messaging
{
    public class Every60thUpdateMessage : IMessage
    {
        private static readonly Every60thUpdateMessage instance = new Every60thUpdateMessage();

        public static Every60thUpdateMessage Instance(float deltaTime)
        {
            instance.DeltaTime = deltaTime;

            return instance;
        }

        private Every60thUpdateMessage()
        {
        }

        public float DeltaTime { get; private set; }
    }
}
