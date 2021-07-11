namespace Gram.Rpg.Client.Presentation.Instance
{
    public interface IInstance : IBehaviour
    {
    }

    public class Instance : GBehaviour, ITimeSource, IInstance
    {
        protected override void DoDispose()
        {
            StopAllCoroutines();

            disposer.Dispose();

            OnDispose();

            Destroy(base.gameObject);
        }
    }
}
