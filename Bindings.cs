using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Target
{
  //Mouse helper
  public enum MouseButton
  {
    None,
    Left,
    Right
  }
  static class MouseHelper
  {
    public static bool ButtonClicked(MouseState state, MouseButton button)
    {
      switch (button)
      {
        case MouseButton.Left:
          return (state.LeftButton == ButtonState.Pressed);
        case MouseButton.Right:
          return (state.RightButton == ButtonState.Pressed);
        case MouseButton.None:
        default:
          return (false);
      }
    }
  }

  public struct DeviceState
  {
    public MouseState mouse;
    public KeyboardState keyboard;
    public GamePadState gamepad;

    public DeviceState(MouseState mouse_, KeyboardState keyboard_, GamePadState gamepad_)
    {
      mouse = mouse_;
      keyboard = keyboard_;
      gamepad = gamepad_;
    }
  }

  //Bindings
  public enum GameAction
  {
    Fire,
    HoldBreath,
    Reload,
    Menu
  }

  public struct Control
  {
    public Keys key;
    public Buttons button;
    public MouseButton mouse;

    public bool IsControlHeld(DeviceState state, DeviceState prevState)
    {
      return (IsControlDown(state) && IsControlDown(prevState));
    }

    public bool IsControlPressed(DeviceState state, DeviceState prevState)
    {
      return (IsControlDown(state) && IsControlUp(prevState));
    }

    public bool IsControlReleased(DeviceState state, DeviceState prevState)
    {
      return (IsControlUp(state) && IsControlDown(prevState));
    }

    public bool IsControlDown(DeviceState state)
    {
      return (state.keyboard.IsKeyDown(key) || state.gamepad.IsButtonDown(button) || MouseHelper.ButtonClicked(state.mouse, mouse));
    }

    public bool IsControlUp(DeviceState state)
    {
      return (state.keyboard.IsKeyUp(key) && state.gamepad.IsButtonUp(button) && !MouseHelper.ButtonClicked(state.mouse, mouse));
    }
  }
}
