using Gram.Rpg.Client.Core;

namespace Gram.Rpg.Client.Presentation.Input
{
    public class IInput
    {
        bool     Detected           { get; }
        bool     Released           { get; }
        bool     SecondaryDetected  { get; }
        bool     SecondarySustained { get; }
        bool     Sustained          { get; }
        GVector3 Vector             { get; }
    }
}
