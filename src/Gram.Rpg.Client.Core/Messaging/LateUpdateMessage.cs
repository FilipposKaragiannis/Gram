namespace Gram.Rpg.Client.Core.Messaging
{
    public class LateUpdateMessage : IMessage
    {
        public static readonly LateUpdateMessage Instance;

        static LateUpdateMessage()
        {
            Instance = new LateUpdateMessage();
        }

        private LateUpdateMessage()
        {
        }
    }
}
