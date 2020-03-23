using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Target.Utils
{
  enum TimerDirection
  {
    Forward,
    Backward
  }
  enum TimeoutBehaviour
  {
    None,
    Reset,
    StartOver,
    Destroy
  }
  struct TimeoutAction
  {
    public double timeout;
    public TimeoutBehaviour timeoutBehaviour;
    public Action action;
    public TimerDirection direction;
    public TimeoutAction(TimerDirection direction_, double timeout_, TimeoutBehaviour timeoutBehaviour_, Action action_)
    {
      direction = direction_;
      timeout = timeout_;
      timeoutBehaviour = timeoutBehaviour_;
      action = action_;
    }
  }

  class Timer
  {
    private bool _active;
    private double _duration;
    private TimerDirection _direction;
    private List<TimeoutAction> _actions;
    private List<int> _destroyBuffer;

    public Timer(TimerDirection direction = TimerDirection.Forward, float startDuration = 0)
    {
      Stop();
      _duration = startDuration;
      _actions = new List<TimeoutAction>();
      _destroyBuffer = new List<int>();
    }

    public double getDuration()
    {
      return (_duration);
    }
    public TimerDirection getDirection()
    {
      return (_direction);
    }

    public bool isActive()
    {
      return (_active);
    }

    public void setDuration(double duration)
    {
      _duration = duration;
    }
    public void setDirection(TimerDirection direction)
    {
      _direction = direction;
    }

    //Controls

    public void Reset()
    {
      Stop();
      _duration = 0;
    }
    public void StartOver()
    {
      Reset();
      Start();
    }

    public void Toggle()
    {
      _active = !_active;
    }

    public void Stop()
    {
      _active = false;
    }

    public void Start()
    {
      _active = true;
    }

    public void Reverse()
    {
      _direction = (_direction == TimerDirection.Forward ? TimerDirection.Backward : TimerDirection.Forward);
    }
    //Setters
    public void setTimeout(TimerDirection direction, double timeout, TimeoutBehaviour timeoutBehaviour, Action action)
    {
      Reset();
      addAction(direction, timeout, timeoutBehaviour, action);
      Start();
    }
    public void addAction(TimerDirection direction, double timeout, TimeoutBehaviour timeoutBehaviour, Action action)
    {
      _actions.Add(new TimeoutAction(direction, timeout, timeoutBehaviour, action));
    }

    public void addMilliseconds(double time)
    {
      _duration += time;
    }

    //Update
    public void Update(GameTime gameTime)
    {
      if (!_active) return; //Only when active
      if (_direction == TimerDirection.Forward) _duration += gameTime.ElapsedGameTime.TotalMilliseconds;
      else if (_direction == TimerDirection.Backward) _duration -= gameTime.ElapsedGameTime.TotalMilliseconds;
      foreach (var it in _actions)
      {
        if (_direction == it.direction
          && ((_direction == TimerDirection.Forward && _duration >= it.timeout)
          || (_direction == TimerDirection.Backward && _duration <= it.timeout)))
        {
          it.action();
          switch (it.timeoutBehaviour)
          {
            case TimeoutBehaviour.Reset:
              Reset();
              break;
            case TimeoutBehaviour.StartOver:
              StartOver();
              break;
            case TimeoutBehaviour.Destroy:
              _destroyBuffer.Add(_actions.IndexOf(it));
              break;
            default:
              break;
          }
        }
      }
      //Clear actions triggered
      if (_destroyBuffer.Count > 0)
      {
        _destroyBuffer.ForEach((idx) => { _actions.RemoveAt(idx); });
        _destroyBuffer.Clear();
      }
    }
  }
}
