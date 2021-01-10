using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTweenEngine;

public enum TweenAxis
{
    X,
    Y,
    Z,
    All
}

public static class TweenEngine
{
    static LerpEngine engine;
    public static int jobIndex = 0;
    public static LerpEngine Engine
    {
        get
        {
            if (engine == null)
            {
                GameObject engineObject = new GameObject("SimpleTweenEngine");
                engineObject.transform.position = Vector3.zero;
                engine = engineObject.AddComponent<LerpEngine>();
            }
            return engine;
        }
    }
    private static void SetStandardJobValues(float time, System.Action onStart, System.Action onComplete, System.Action onInterrupt, System.Action onUpdate, TweenJob job)
    {
        job.counter = 0;
        job.time = time;
        job.onComplete = onComplete;
        job.onInterrupt = onInterrupt;
        job.onUpdate = onUpdate;
        if (onStart != null) onStart.Invoke();

    }
    private static TweenJobTransform SetStandardTransformTweenValues(Vector3 to, GameObject gameObject, TransformTweenType type, float time, System.Action onStart, System.Action onComplete, System.Action onInterrupt, System.Action onUpdate)
    {
        TweenJobTransform job = new TweenJobTransform();
        job.type = type;
        job.target = to;
        job.objectTransform = gameObject.transform;
        switch (type)
        {
            case TransformTweenType.Move:
                job.start = gameObject.transform.position;
                break;
            case TransformTweenType.LocalMove:
                job.start = gameObject.transform.localPosition;
                break;
            case TransformTweenType.Scale:
                job.start = gameObject.transform.localScale;
                break;
            case TransformTweenType.Rotate:
                job.start = gameObject.transform.eulerAngles;
                break;
        }
        SetStandardJobValues(time, onStart, onComplete, onInterrupt, onUpdate, job);
        Engine.AddJob(job);
        return job;
    }


    public static TweenJobTimer Timer(float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onUpdate = null)
    {
        if (time == 0) return null;
        TweenJobTimer job = new TweenJobTimer();
        SetStandardJobValues(time, onStart, onComplete, onInterrupt, onUpdate, job);
        Engine.AddJob(job);
        return job;
    }

    public static TweenJobValueFloat FloatValue(FloatField val, float to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onUpdate = null)
    {
        if (time == 0 || val == null) return null;
        TweenJobValueFloat job = new TweenJobValueFloat();
        SetStandardJobValues(time, onStart, onComplete, onInterrupt, onUpdate, job);
        job.floatfield = val;
        job.target = to;
        Engine.AddJob(job);
        return job;
    }

    public static TweenJobCallBackOverTime CallBackOverTime(System.Action callback, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onUpdate = null)
    {
        if (time == 0 || callback == null) return null;

        TweenJobCallBackOverTime job = new TweenJobCallBackOverTime();
        SetStandardJobValues(time, onStart, onComplete, onInterrupt, onUpdate, job);
        job.callback = callback;
        Engine.AddJob(job);
        return job;

    }

    public static TweenJobTransform Scale(GameObject scaledObject, Vector3 to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onUpdate = null)
    {
        if (time == 0 || scaledObject == null) return null;
        return SetStandardTransformTweenValues(to, scaledObject, TransformTweenType.Scale, time, onStart, onComplete, onInterrupt, onUpdate);
    }

    public static TweenJobTransform Move(GameObject movedObject, Vector3 to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onUpdate = null)
    {
        if (time == 0 || movedObject == null) return null;
        return SetStandardTransformTweenValues(to, movedObject, TransformTweenType.Move, time, onStart, onComplete, onInterrupt, onUpdate);

    }

    public static TweenJobTransform MoveLocal(GameObject movedObject, Vector3 to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onUpdate = null)
    {
        if (time == 0 || movedObject == null) return null;
        return SetStandardTransformTweenValues(to, movedObject, TransformTweenType.LocalMove, time, onStart, onComplete, onInterrupt, onUpdate);
    }


    public static void EndJob(TweenJob job)
    {
        EndJob(job.jobID);
    }
    public static void EndJob(int id)
    {
        Engine.RemoveJob(Engine.GetJob(id));
    }
}
namespace SimpleTweenEngine
{

    public enum TransformTweenType
    {
        Move,
        LocalMove,
        Scale,
        Rotate
    }



   

    public abstract class TweenJob
    {
        public float time = 0;
        internal int jobID = -1;
        internal float counter = 0;
        private bool finished = false;
        public System.Action onComplete;
        public System.Action onInterrupt;
        public System.Action onUpdate;

        public bool Finished
        {
            get
            {
                return finished;
            }
        }

        public abstract void Work(float dt, LerpEngine engine);
        protected bool Step(float dt)
        {
            counter += dt;
            onUpdate?.Invoke();
            finished = (counter >= time);
            return finished;
        }



    }


    #region Tween Job Types
    public class TweenJobTimer : TweenJob
    {
        public override void Work(float dt, LerpEngine engine)
        {
            onUpdate?.Invoke();
            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                if (onComplete != null) onComplete.Invoke();
            }
        }
    }
    public class TweenJobTransform: TweenJob
    {
        public TransformTweenType type;
        public Vector3 target;
        public Vector3 start;
        public Transform objectTransform;

        protected AnimationCurve curveX;
        protected AnimationCurve curveY;
        protected AnimationCurve curveZ;
        protected float amplitudeX = 1;
        protected float amplitudeY = 1;
        protected float amplitudeZ = 1;
        private Vector3 tempTarget = Vector3.zero;
        public override void Work(float dt, LerpEngine engine)
        {

            onUpdate?.Invoke();

            tempTarget = target;
            if (curveX != null) tempTarget.x += (curveX.Evaluate(counter / time) * amplitudeX);
            if (curveY != null) tempTarget.y += (curveY.Evaluate(counter / time) * amplitudeY);
            if (curveZ != null) tempTarget.z += (curveZ.Evaluate(counter / time) * amplitudeZ);

            switch (type)
            {
                case TransformTweenType.Move:
                    objectTransform.position = Vector3.Lerp(start, tempTarget, counter / time);
                    break;
                case TransformTweenType.LocalMove:
                    objectTransform.localPosition = Vector3.Lerp(start, tempTarget, counter / time);
                    break;
                case TransformTweenType.Scale:
                    objectTransform.localScale = Vector3.Lerp(start, tempTarget, counter / time);
                    break;
            }

            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                if (onComplete != null) onComplete.Invoke();

                tempTarget = target;
                if (curveX != null) tempTarget.x += (curveX.Evaluate(1) * amplitudeX);
                if (curveY != null) tempTarget.y += (curveY.Evaluate(1) * amplitudeY);
                if (curveZ != null) tempTarget.z += (curveZ.Evaluate(1) * amplitudeZ);

                switch (type)
                {
                    case TransformTweenType.Move:
                        objectTransform.position = tempTarget;
                        break;
                    case TransformTweenType.LocalMove:
                        objectTransform.localPosition = tempTarget;
                        break;
                    case TransformTweenType.Scale:
                        objectTransform.localScale = tempTarget;
                        break;
                }
            }
        }

        /// <summary>
        /// Set an Animation Curve to be added to the Target Vector. 
        /// ie Target Vector3 010 and CurveY ending with 3 and Amplitude 1 will result in final target position of 040. 
        /// Play with the curves
        /// </summary>
        /// <param name="curve">Unity Animation Curve can be set in Editor</param>
        /// <param name="axis">Select which Axis to apply the curve to, TweenAxis.All applies the same curve to all Axes</param>
        /// <param name="amplitude">Can be more than 1 if you want to multiply the values of the curve</param>
        /// <returns></returns>
        public TweenJobTransform SetCurve(AnimationCurve curve, TweenAxis axis, float amplitude = 1)
        {
            switch (axis)
            {
                case TweenAxis.X:
                    curveX = curve;
                    amplitudeX = amplitude;
                    break;
                case TweenAxis.Y:
                    curveY = curve;
                    amplitudeY = amplitude;
                    break;
                case TweenAxis.Z:
                    curveZ = curve;
                    amplitudeZ = amplitude;
                    break;
                case TweenAxis.All:
                    curveX = curve;
                    curveY = curve;
                    curveZ = curve;
                    amplitudeX = amplitude;
                    amplitudeY = amplitude;
                    amplitudeZ = amplitude;
                    break;
            };

            return this;
        }
        public TweenJobTransform SetOnComplete(System.Action callback)
        {
            onComplete = callback;
            return this;
        }
        public TweenJobTransform SetOnUpdate(System.Action callback)
        {
            onUpdate = callback;
            return this;
        }

    }
    public class TweenJobValueFloat : TweenJob
    {
        internal FloatField floatfield;
        internal float target;
        public override void Work(float dt, LerpEngine engine)
        {
            floatfield.Value += (target * dt) / time;
            onUpdate?.Invoke();
            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                if (onComplete != null) onComplete.Invoke();
            }
        }
    }
    public class TweenJobCallBackOverTime : TweenJob
    {
        public System.Action callback;
        public override void Work(float dt, LerpEngine engine)
        {
            callback?.Invoke();
            onUpdate?.Invoke();

            if (Step(dt))
            {
                if (onComplete != null) onComplete.Invoke();
                engine.RemoveJob(jobID);
            }
            
        }
    }
    #endregion
}

