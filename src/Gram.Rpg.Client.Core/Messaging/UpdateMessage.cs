namespace Gram.Rpg.Client.Core.Messaging
{
    public class UpdateMessage : IMessage
    {
        private static readonly UpdateMessage instance = new UpdateMessage();

        public static UpdateMessage Instance(float deltaTime)
        {
            instance.DeltaTime = deltaTime;

            return instance;
        }

        private UpdateMessage()
        {
        }

        public float DeltaTime { get; private set; }
    }
}
