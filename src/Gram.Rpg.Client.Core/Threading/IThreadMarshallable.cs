namespace Gram.Rpg.Client.Core.Threading
{
    /// <summary>
    /// Despite similar naming, this is not related to IThreadMarshaller.
    /// </summary>
    public interface IThreadMarshallable
    {
        void Marshall();
    }
}
