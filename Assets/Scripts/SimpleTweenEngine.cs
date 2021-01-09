using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SimpleTweenEngine
{

    public enum TransformTweenType
    {
        Move,
        LocalMove,
        Scale,
        Rotate
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
        private static void SetStandardJobValues(float time, System.Action onStart, System.Action onComplete, System.Action onInterrupt, System.Action onChange, TweenJob job)
        {
            job.counter = 0;
            job.time = time;
            job.onComplete = onComplete;
            job.onInterrupt = onInterrupt;
            job.onChange = onChange;
            if (onStart != null) onStart.Invoke();

        }


        public static void Timer(float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onChange = null)
        {
            if (time == 0) return;
            TweenJobTimer job = new TweenJobTimer();
            SetStandardJobValues(time, onStart, onComplete, onInterrupt, onChange, job);
            Engine.AddJob(job);
        }

        public static void FloatValue(FloatField val, float to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onChange = null)
        {
            if (time == 0 || val == null) return;
            TweenJobValueFloat job = new TweenJobValueFloat();
            SetStandardJobValues(time, onStart, onComplete, onInterrupt, onChange, job);
            job.floatfield = val;
            job.target = to;
            Engine.AddJob(job);
        }

        public static void CallBackOverTime(System.Action callback, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onChange = null)
        {
            if (time == 0 || callback == null) return;

            TweenJobCallBackOverTime job = new TweenJobCallBackOverTime();
            SetStandardJobValues(time, onStart, onComplete, onInterrupt, onChange, job);
            job.callback = callback;
            Engine.AddJob(job);

        }
        public static void Scale(GameObject scaledObject, Vector3 to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onChange = null)
        {
            if (time == 0 || scaledObject == null) return;
            TransformTweenValues(to, scaledObject, TransformTweenType.Scale, time, onStart, onComplete, onInterrupt, onChange);
        }

        public static void Move(GameObject movedObject, Vector3 to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onChange = null)
        {
            if (time == 0 || movedObject == null) return;
            TransformTweenValues(to, movedObject, TransformTweenType.Move, time, onStart, onComplete, onInterrupt, onChange);

        }

        public static void MoveLocal(GameObject movedObject, Vector3 to, float time, System.Action onStart = null, System.Action onComplete = null, System.Action onInterrupt = null, System.Action onChange = null)
        {
            if (time == 0 || movedObject == null) return;
            TransformTweenValues(to, movedObject, TransformTweenType.LocalMove, time, onStart, onComplete, onInterrupt, onChange);
        }

        private static void TransformTweenValues(Vector3 to, GameObject gameObject, TransformTweenType type, float time, System.Action onStart, System.Action onComplete, System.Action onInterrupt, System.Action onChange)
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
            SetStandardJobValues(time, onStart, onComplete, onInterrupt, onChange, job);
            Engine.AddJob(job);
        }

        public static void EndJob(int id)
        {
            Engine.RemoveJob(Engine.GetJob(id));
        }
    }

    public abstract class TweenJob
    {
        public float time = 0;
        internal int jobID = -1;
        internal float counter = 0;
        private bool finished = false;
        public System.Action onComplete;
        public System.Action onInterrupt;
        public System.Action onChange;

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
            onChange?.Invoke();
            finished = (counter >= time);
            return finished;
        }


    }
    public class TweenJobTimer : TweenJob
    {
        public override void Work(float dt, LerpEngine engine)
        {
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

        public override void Work(float dt, LerpEngine engine)
        {


            switch (type)
            {
                case TransformTweenType.Move:
                    objectTransform.position = Vector3.Lerp(/*objectTransform.position*/start, target, counter / time);
                    break;
                case TransformTweenType.LocalMove:
                    objectTransform.localPosition = Vector3.Lerp(start/*objectTransform.localPosition*/, target, counter / time);
                    break;
                case TransformTweenType.Scale:
                    objectTransform.localScale = Vector3.Lerp(start/*objectTransform.localScale*/, target, counter / time);
                    break;
            }

            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                if (onComplete != null) onComplete.Invoke();
            }
        }
    }

    public class TweenJobValueFloat : TweenJob
    {
        internal FloatField floatfield;
        internal float target;
        public override void Work(float dt, LerpEngine engine)
        {
            floatfield.Value += (target * dt) / time;

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
            callback.Invoke();

            if (Step(dt))
            {
                if (onComplete != null) onComplete.Invoke();
                engine.RemoveJob(jobID);
            }
            
        }
    }
}

