// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Represents a virtual joystick that aggregates input from multiple sources to simulate a joystick input.
/// </summary>
public sealed class VirtualJoystick : VirtualInput, IVirtualJoystickBuilder
{
    private readonly List<Node> _nodes;
    private readonly bool _snap;
    private readonly bool _normalize;

    /// <summary>
    /// Gets the value of this virtual joystick.
    /// </summary>
    public Vector2 Value { get; private set; }

    /// <summary>
    /// Gets the value of this virtual joystick during the previous update frame.
    /// </summary>
    public Vector2 PreviousValue { get; private set; }

    /// <summary>
    /// Gets the difference in this virtual joystick's value between the current and previous update frames.
    /// </summary>
    public Vector2 Delta => Value - PreviousValue;

    internal VirtualJoystick(string name, InputService input) : this(name, input, false, false) { }

    internal VirtualJoystick(string name, InputService input, bool snapped, bool normalized)
        : base(name, input)
    {
        _nodes = new List<Node>();
        _snap = snapped;
        _normalize = normalized;
    }

    internal override void Update(GameTime gameTime)
    {
        Span<Node> nodes = CollectionsMarshal.AsSpan(_nodes);

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].Update(Input, gameTime);
        }

        PreviousValue = Value;
        Value = Vector2.Zero;

        for (int i = 0; i < nodes.Length; i++)
        {
            Vector2 newValue = nodes[i].Value;

            if (newValue != Vector2.Zero)
            {
                if (_normalize)
                {
                    if (_snap)
                    {
                        //  Snap using normalized length
                        newValue = Snap(newValue, 8, 1.0f);
                    }
                    else
                    {
                        newValue.Normalize();
                    }
                }
                else if (_snap)
                {
                    //  Snap using vector length
                    newValue = Snap(newValue, 8, newValue.Length());
                }

                Value = newValue;
                break;
            }
        }
    }

    private static Vector2 Snap(Vector2 vector, float divisions, float length)
    {
        float angle = (float)Math.Atan2(vector.Y, vector.X);
        float angleIncrements = MathHelper.TwoPi / divisions;
        float snapTo = (float)Math.Round(angle / angleIncrements) * angleIncrements;
        float x = (float)Math.Cos(snapTo) * length;
        float y = (float)Math.Sin(snapTo) * length;
        return new Vector2(x, y);
    }

    /// <inheritdoc/>
    public IVirtualJoystickBuilder RegisterKeyboardKeys(Keys up, Keys down, Keys left, Keys right, OverlapBehavior behavior)
    {
        KeyNode node = new KeyNode(up, down, left, right, behavior);
        _nodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IVirtualJoystickBuilder RegisterGamePadButtons(PlayerIndex player, Buttons up, Buttons down, Buttons left, Buttons right, OverlapBehavior behavior)
    {
        GamePadButtonNode node = new GamePadButtonNode(player, up, down, left, right, behavior);
        _nodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IVirtualJoystickBuilder RegisterGamePadLeftStick(PlayerIndex player) => RegisterGamePadLeftStick(player, 0.0f);

    /// <inheritdoc/>
    public IVirtualJoystickBuilder RegisterGamePadLeftStick(PlayerIndex player, float deadZone)
    {
        GamePadLeftStickNode node = new GamePadLeftStickNode(player, deadZone);
        _nodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IVirtualJoystickBuilder RegisterGamePadRightStick(PlayerIndex player) => RegisterGamePadRightStick(player, 0.0f);

    /// <inheritdoc/>
    public IVirtualJoystickBuilder RegisterGamePadRightStick(PlayerIndex player, float deadZone)
    {
        GamePadRightStickNode node = new GamePadRightStickNode(player, deadZone);
        _nodes.Add(node);
        return this;
    }

    private abstract class Node
    {
        public abstract Vector2 Value { get; }
        public abstract void Update(InputService input, GameTime gameTime);
    }

    private static float ChooseValue(bool negative, bool positive, OverlapBehavior behavior)
    {
        if (positive)
        {
            if (negative)
            {
                //  Both positive and negative are true, so value is determined by the overlap behavior
                return behavior switch
                {
                    OverlapBehavior.Positive => 1,
                    OverlapBehavior.Negative => -1,
                    _ => 0
                };
            }

            return 1;
        }
        else if (negative)
        {
            return -1;
        }

        return 0;
    }

    private sealed class KeyNode : Node
    {
        private readonly Keys _up;
        private readonly Keys _down;
        private readonly Keys _left;
        private readonly Keys _right;
        private readonly OverlapBehavior _behavior;
        private Vector2 _value;

        public override Vector2 Value => _value;

        public KeyNode(Keys up, Keys down, Keys left, Keys right, OverlapBehavior behavior) =>
            (_up, _down, _left, _right, _behavior) = (up, down, left, right, behavior);


        public override void Update(InputService input, GameTime gameTime)
        {
            bool isUp = input.Keyboard.Check(_up);
            bool isDown = input.Keyboard.Check(_down);
            bool isLeft = input.Keyboard.Check(_left);
            bool isRight = input.Keyboard.Check(_right);

            _value.X = ChooseValue(isLeft, isRight, _behavior);
            _value.Y = ChooseValue(isDown, isUp, _behavior);
        }
    }

    private sealed class GamePadButtonNode : Node
    {
        private readonly PlayerIndex _player;
        private readonly Buttons _up;
        private readonly Buttons _down;
        private readonly Buttons _left;
        private readonly Buttons _right;
        private readonly OverlapBehavior _behavior;
        private Vector2 _value;

        public override Vector2 Value => _value;

        public GamePadButtonNode(PlayerIndex player, Buttons up, Buttons down, Buttons left, Buttons right, OverlapBehavior behavior) =>
            (_player, _up, _down, _left, _right, _behavior) = (player, up, down, left, right, behavior);

        public override void Update(InputService input, GameTime gameTime)
        {
            GamePadInfo gamePad = input.GetGamePad(_player);
            bool isUp = gamePad.Check(_up);
            bool isDown = gamePad.Check(_down);
            bool isLeft = gamePad.Check(_left);
            bool isRight = gamePad.Check(_right);

            _value.X = ChooseValue(isLeft, isRight, _behavior);
            _value.Y = ChooseValue(isDown, isUp, _behavior);
        }
    }

    private sealed class GamePadLeftStickNode : Node
    {
        private readonly PlayerIndex _player;
        private readonly float _deadZone;
        private Vector2 _value;

        public override Vector2 Value => _value;

        public GamePadLeftStickNode(PlayerIndex player, float deadZone) => (_player, _deadZone) = (player, deadZone);

        public override void Update(InputService input, GameTime gameTime)
        {
            ThumbStickInfo thumbStick = input.GetGamePad(_player).LeftThumbStick;
            _value = thumbStick.CurrentValue(_deadZone);
        }
    }

    private sealed class GamePadRightStickNode : Node
    {
        private readonly PlayerIndex _player;
        private readonly float _deadZone;
        private Vector2 _value;

        public override Vector2 Value => _value;

        public GamePadRightStickNode(PlayerIndex player, float deadZone) => (_player, _deadZone) = (player, deadZone);

        public override void Update(InputService input, GameTime gameTime)
        {
            ThumbStickInfo thumbStick = input.GetGamePad(_player).RightThumbStick;
            _value = thumbStick.CurrentValue(_deadZone);
        }
    }
}
