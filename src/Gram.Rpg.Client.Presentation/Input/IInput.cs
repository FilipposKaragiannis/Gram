using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Input
{
    public interface IInput
    {
        bool    Detected           { get; }
        bool    Released           { get; }
        bool    Sustained          { get; }
        Vector3 Vector             { get; }
    }
}
