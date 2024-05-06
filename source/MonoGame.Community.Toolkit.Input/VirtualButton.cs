// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Represents a virtual button that aggregates input from multiple sources to simulate a button input.
/// </summary>
public sealed class VirtualButton : VirtualInput, IVirtualButtonBuilder
{
    private bool _consumed;
    private readonly List<Node> _nodes;

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualButton"/> class.
    /// </summary>
    internal VirtualButton(string name, InputService input)
        : base(name, input)
    {
        _nodes = new List<Node>();
    }

    /// <summary>
    /// Returns a value that indicates whether this virtual button is down.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this virtual button is down; otherwise, <see langword="false"/>.  This method returns
    /// <see langword="true"/> for every frame that this virtual button is down.
    /// </returns>
    public bool Check()
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
            if (_nodes[i].Check(Input))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a value that indicates whether this virtual button was just pressed.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this virtual button was just pressed; otherwise, <see langword="false"/>.  This
    /// method will only return <see langword="true"/> on the first frame that this virtual button was pressed and
    /// if <see cref="ConsumePress"/> hasn't been called this frame.
    /// </returns>
    public bool Pressed()
    {
        if (_consumed) { return false; }

        for (int i = 0; i < _nodes.Count; i++)
        {
            if (_nodes[i].Pressed(Input))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a value that indicates whether this virtual button was just released.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this virtual button was just released; otherwise, <see langword="false"/>.  This
    /// method only returns <see langword="true"/> on the first frame that this virtual button was released.
    /// </returns>
    public bool Released()
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
            if (_nodes[i].Released(Input))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// When this is called, this virtual button will not return <see langword="true"/> for the <see cref="Pressed"/>
    /// method for the remainder of the current frame.
    /// </summary>
    public void ConsumePress() => _consumed = true;

    /// <inheritdoc/>
    public IVirtualButtonBuilder RegisterKeyboardKey(Keys key)
    {
        KeyNode node = new KeyNode(key);
        _nodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IVirtualButtonBuilder RegisterGamePadButton(PlayerIndex player, Buttons button)
    {
        GamePadButtonNode node = new GamePadButtonNode(player, button);
        _nodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IVirtualButtonBuilder RegisterMouseButton(MouseButton button)
    {
        MouseButtonNode node = new MouseButtonNode(button);
        _nodes.Add(node);
        return this;
    }

    private abstract class Node
    {
        public abstract bool Check(InputService input);
        public abstract bool Pressed(InputService input);
        public abstract bool Released(InputService input);
    }

    private sealed class KeyNode : Node
    {
        private readonly Keys _key;
        public KeyNode(Keys key) => _key = key;
        public override bool Check(InputService input) => input.Keyboard.Check(_key);
        public override bool Pressed(InputService input) => input.Keyboard.Pressed(_key);
        public override bool Released(InputService input) => input.Keyboard.Released(_key);
    }

    private sealed class MouseButtonNode : Node
    {
        private readonly MouseButton _button;
        public MouseButtonNode(MouseButton button) => _button = button;
        public override bool Check(InputService input) => input.Mouse.Check(_button);
        public override bool Pressed(InputService input) => input.Mouse.Pressed(_button);
        public override bool Released(InputService input) => input.Mouse.Released(_button);
    }

    private sealed class GamePadButtonNode : Node
    {
        private readonly PlayerIndex _player;
        private readonly Buttons _button;
        public GamePadButtonNode(PlayerIndex player, Buttons button) => (_player, _button) = (player, button);
        public override bool Check(InputService input) => input.GetGamePad(_player).Check(_button);
        public override bool Pressed(InputService input) => input.GetGamePad(_player).Pressed(_button);
        public override bool Released(InputService input) => input.GetGamePad(_player).Released(_button);
    }
}
