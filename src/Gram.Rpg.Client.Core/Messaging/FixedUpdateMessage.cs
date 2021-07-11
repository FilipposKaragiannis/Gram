namespace Gram.Rpg.Client.Core.Messaging
{
    public class FixedUpdateMessage : IMessage
    {
        public static readonly FixedUpdateMessage Instance;

        static FixedUpdateMessage()
        {
            Instance = new FixedUpdateMessage();
        }

        private FixedUpdateMessage()
        {
        }

        public float DeltaTime { get; private set; }

        public static float FixedDeltaTime
        {
            get => Instance.DeltaTime;
            set => Instance.DeltaTime = value;
        }
    }
}
