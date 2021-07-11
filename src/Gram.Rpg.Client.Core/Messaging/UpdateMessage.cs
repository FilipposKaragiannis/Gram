namespace Gram.Rpg.Client.Core.Messaging
{
    /// <summary>
    /// Singleton event used for Anim frame updates.
    /// Since this event will be raised at ~60Hz we've made it singleton to reduce heap transactions and GC calls.
    /// </summary>
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
