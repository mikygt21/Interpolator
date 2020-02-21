using System.Collections;
using System.Collections.Generic;
using System;

public class Interpolator
{
    public enum State { MIN, MAX, SHRINKING, GROWING};
    public enum Type { LINEAR, SIN, COS, QUADRATIC, SMOOTH, SMOOTHER};

    private State interState = State.MIN;
    private Type interpType;

    private float currentTime = 0.0f;
    private readonly float epsilon = 0.05f;
    private float interpolationTime = 0.0f;

    public float Value { get; private set; }
    public float Inverse { get { return 1 - Value; } }

    public bool IsMaxPrecise { get { return this.interState != State.MAX; } }
    public bool IsMax { get { return Value > 1f - epsilon; } }

    public bool IsMinPrecise { get { return this.interState != State.MIN; } }
    public bool IsMin { get { return Value < epsilon; } }



    public Interpolator(float interpTime, Type type = Type.LINEAR)
    {
        interpolationTime = interpTime;
        interpType = type;
    }

    public void Update(float dt)
    {
        if (this.interState == State.MIN || this.interState == State.MAX)
            return;

        float modifiedDt = this.interState == State.GROWING ? dt : -dt;

        currentTime += modifiedDt;

        if(currentTime >= interpolationTime) 
        {
            ForceMax();
        }
        else if(currentTime <= 0.0f) {
            ForceMin();
        }

        Value = currentTime / interpolationTime;
        switch (interpType) {
            case Type.SIN:
                Value = (float)Math.Sin(Value * Math.PI * 0.5f);
                break;
            case Type.COS:
                Value = (float)Math.Cos(Value * Math.PI * 0.5f);
                break;
            case Type.QUADRATIC:
                Value *= Value;
                break;
            case Type.SMOOTH:
                Value = Value * Value * (3f - 2f * Value);
                break;
            case Type.SMOOTHER:
                Value = (float)Math.Pow(Value, 3f) * (Value * (6f * Value - 15f) + 10f);
                break;
            default:
                break;
        }
    }
    
    public void ToMax()    //Si no esta a max, GROWING
    {
        this.interState = this.interState != State.MAX ? State.GROWING : State.MAX;
        
    }

    public void ToMin()
    {
        this.interState = this.interState != State.MIN ? State.SHRINKING : State.MIN;
    }

    public void ForceMax()
    {
        this.currentTime = this.interpolationTime;
        this.interState = State.MAX;
    }

    public void ForceMin()
    {
        this.currentTime = 0f;
        this.interState = State.MIN;
    }
}
