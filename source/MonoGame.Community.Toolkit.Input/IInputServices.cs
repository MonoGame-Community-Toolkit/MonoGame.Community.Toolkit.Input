using Microsoft.Xna.Framework;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Defines the implementation for an input service.
/// </summary>
public interface IInputServices
{
    /// <summary>
    /// Gets the information about the state of keyboard input.
    /// </summary>
    KeyboardInfo Keyboard { get; }

    /// <summary>
    /// Gets the information about the state of mouse input.
    /// </summary>
    MouseInfo Mouse { get; }

    /// <summary>
    /// Returns the <see cref="GamePadInfo"/> specific to the player at the specified player index.
    /// </summary>
    /// <param name="index">The index of the player.</param>
    /// <returns>The <see cref="GamePadInfo"/> for the specified player.</returns>
    GamePadInfo GetGamePad(PlayerIndex index);

}
