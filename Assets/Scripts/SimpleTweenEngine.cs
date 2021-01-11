using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTweenEngine;
using System;

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
    private static int jobIndex = 0;
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

    public static int JobIndex
    {
        get
        {
            return jobIndex;
        }
    }

    public static TweenJobTimer Timer(float time)
    {
        TweenJobTimer job = new TweenJobTimer(time, jobIndex++);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobValueFloat FloatValue(FloatField val, float to, float time)
    {
        if (time == 0 || val == null) return null;
        TweenJobValueFloat job = new TweenJobValueFloat(val,to ,time,jobIndex++);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobCallBackOverTime CallBackOverTime(System.Action callback, float time, float rate = 0)
    {
        if (time == 0 || callback == null) return null;

        TweenJobCallBackOverTime job = new TweenJobCallBackOverTime(callback, time, jobIndex++, rate);
        Engine.AddJob(job);
        return job;

    }
    //

    //Scale
    public static TweenJobTransform Scale(GameObject scaledObject, Vector3 to, float time)
    {
        if (time == 0 || scaledObject == null) return null;
        TweenJobTransform job = new TweenJobTransform(to, time, jobIndex++, scaledObject, TweenType.Scale);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle ScaleX(GameObject scaledObject, float to, float time)
    {
        if (time == 0 || scaledObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, scaledObject, TweenTypeSingle.ScaleX);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle ScaleY(GameObject scaledObject, float to, float time)
    {
        if (time == 0 || scaledObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, scaledObject, TweenTypeSingle.ScaleY);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle ScaleZ(GameObject scaledObject, float to, float time)
    {
        if (time == 0 || scaledObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, scaledObject, TweenTypeSingle.ScaleZ);
        Engine.AddJob(job);
        return job;
    }
    //

    //Move
    public static TweenJobTransform Move(GameObject movedObject, Vector3 to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransform job = new TweenJobTransform(to, time, jobIndex++, movedObject, TweenType.Move);
        Engine.AddJob(job);
        return job;

    }
    public static TweenJobTransformSingle MoveX(GameObject movedObject, float to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, movedObject, TweenTypeSingle.MoveX);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle MoveY(GameObject movedObject, float to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, movedObject, TweenTypeSingle.MoveY);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle MoveZ(GameObject movedObject, float to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, movedObject, TweenTypeSingle.MoveZ);
        Engine.AddJob(job);
        return job;
    }

    public static TweenJobTransform MoveLocal(GameObject movedObject, Vector3 to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransform job = new TweenJobTransform(to, time, jobIndex++, movedObject, TweenType.LocalMove);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle MoveLocalX(GameObject movedObject, float to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, movedObject, TweenTypeSingle.MoveX);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle MoveLocalY(GameObject movedObject, float to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, movedObject, TweenTypeSingle.MoveY);
        Engine.AddJob(job);
        return job;
    }
    public static TweenJobTransformSingle MoveLocalZ(GameObject movedObject, float to, float time)
    {
        if (time == 0 || movedObject == null) return null;
        TweenJobTransformSingle job = new TweenJobTransformSingle(to, time, jobIndex++, movedObject, TweenTypeSingle.MoveZ);
        Engine.AddJob(job);
        return job;
    }
    //

    public static void EndJob(TweenJob job)
    {
        EndJob(job.JobID);
    }
    public static void EndJob(int id)
    {
        Engine.RemoveJob(Engine.GetJob(id));
    }
}


namespace SimpleTweenEngine
{

    public enum TweenType
    {
        Move,
        LocalMove,
        Scale
    }

    public enum TweenTypeSingle
    {
        MoveX,
        MoveY,
        MoveZ,
        LocalMoveX,
        LocalMoveY,
        LocalMoveZ,
        ScaleX,
        ScaleY,
        ScaleZ
    }



    public abstract class TweenJob
    {
        protected float time = 0;
        protected int jobID = -1;
        protected float counter = 0;
        protected bool finished = false;
        protected System.Action onComplete;
        protected System.Action onInterrupt;
        protected System.Action onUpdate;
        public int JobID
        {
            get
            {
                return jobID;
            }
        }

        public abstract void Work(float dt, LerpEngine engine);
        protected bool Step(float dt)
        {
            counter += dt;
            onUpdate?.Invoke();
            return (counter >= time);
        }

        public void EndJob()
        {
            onInterrupt?.Invoke();
        }
    }


    #region Tween Job Types
    public class TweenJobTimer : TweenJob
    {
        public TweenJobTimer(float time, int jobID)
        {
            this.time = time;
            this.jobID = jobID;
            counter = 0;
        }

        public override void Work(float dt, LerpEngine engine)
        {
            onUpdate?.Invoke();
            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                if (onComplete != null) onComplete.Invoke();
            }
        }

        public TweenJobTimer SetOnComplete(System.Action action)
        {
            onComplete = action;
            return this;
        }
        public TweenJobTimer SetOnUpdate(System.Action action)
        {
            onUpdate = action;
            return this;
        }
        public TweenJobTimer SetOnInterrupt(System.Action action)
        {
            onInterrupt = action;
            return this;
        }
    }




    public class TweenJobTransform: TweenJob
    {
        public TweenJobTransform(Vector3 target, float time, int jobID, GameObject gameObject, TweenType type)
        {

            this.target = target;
            this.time = time;
            this.jobID = jobID;
            if (gameObject != null) objectTransform = gameObject.transform;
            this.type = type;

            if (objectTransform != null)
            {
                switch (type)
                {
                    case TweenType.Move:
                        start = objectTransform.position;
                        break;
                    case TweenType.LocalMove:
                        start = objectTransform.localPosition;
                        break;
                    case TweenType.Scale:
                        start = objectTransform.localScale;
                        break;
                }

            }

            tempTarget = target;
            counter = 0;
        }

        public TweenType type;
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
        private float t;
        public override void Work(float dt, LerpEngine engine)
        {

            onUpdate?.Invoke();

            Move();

            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                Move(true);
                if (onComplete != null) onComplete.Invoke();

            }
        }

        private void Move(bool last = false)
        {
            tempTarget = target;
            t = last ? 1 : counter / time;
            if (curveX != null) tempTarget.x += (curveX.Evaluate(t) * amplitudeX);
            if (curveY != null) tempTarget.y += (curveY.Evaluate(t) * amplitudeY);
            if (curveZ != null) tempTarget.z += (curveZ.Evaluate(t) * amplitudeZ);

            switch (type)
            {
                case TweenType.Move:
                    objectTransform.position = last ? target : Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenType.LocalMove:
                    objectTransform.localPosition = last ? target : Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenType.Scale:
                    objectTransform.localScale = last ? target : Vector3.Lerp(start, tempTarget, t);
                    break;
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
    public class TweenJobTransformSingle : TweenJob
    {

        public TweenJobTransformSingle(float target, float time, int jobID, GameObject gameObject, TweenTypeSingle type)
        {
            this.target = target;
            this.time = time;
            this.jobID = jobID;
            if (gameObject != null) objectTransform = gameObject.transform;
            this.type = type;
            if(objectTransform != null)
            {
                switch (type)
                {
                    case TweenTypeSingle.MoveX:
                    case TweenTypeSingle.MoveY:
                    case TweenTypeSingle.MoveZ:
                        start = objectTransform.position;
                        tempTarget = new Vector3(
                            type == TweenTypeSingle.MoveX ? target : start.x,
                            type == TweenTypeSingle.MoveY ? target : start.y,
                            type == TweenTypeSingle.MoveZ ? target : start.z);
                        break;
                    case TweenTypeSingle.LocalMoveX:
                    case TweenTypeSingle.LocalMoveY:
                    case TweenTypeSingle.LocalMoveZ:
                        start = objectTransform.localPosition;
                        tempTarget = new Vector3(
                            type == TweenTypeSingle.LocalMoveX ? target : start.x,
                            type == TweenTypeSingle.LocalMoveY ? target : start.y,
                            type == TweenTypeSingle.LocalMoveZ ? target : start.z);
                        break;
                    case TweenTypeSingle.ScaleX:
                    case TweenTypeSingle.ScaleY:
                    case TweenTypeSingle.ScaleZ:
                        start = objectTransform.localScale;
                        tempTarget = new Vector3(
                            type == TweenTypeSingle.ScaleX ? target : start.x,
                            type == TweenTypeSingle.ScaleY ? target : start.y,
                            type == TweenTypeSingle.ScaleZ ? target : start.z);
                        break;
                }
                
            }
            counter = 0;
        }
        protected TweenTypeSingle type;
        protected float target;
        protected Vector3 start;
        protected Transform objectTransform;

        protected AnimationCurve curve;
        protected float amplitude = 1;
        Vector3 tempTarget;
        float curveValue = 0;
        float t;
        public override void Work(float dt, LerpEngine engine)
        {
            if (objectTransform == null) engine.RemoveJob(jobID);
            onUpdate?.Invoke();

            Move();

            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                Move(true);
                if (onComplete != null) onComplete.Invoke();

                #region old
                //if (curve != null) curveValue = (curve.Evaluate(1) * amplitude);
                ////Just snap to the target to clear fractions
                //switch (type)
                //{
                //    case TweenTypeSingle.MoveX:
                //        tempTarget.x = target + curveValue;
                //        objectTransform.position = tempTarget;
                //        break;
                //    case TweenTypeSingle.MoveY:
                //        tempTarget.y = target + curveValue;
                //        objectTransform.position = tempTarget;
                //        break;
                //    case TweenTypeSingle.MoveZ:
                //        tempTarget.z = target + curveValue;
                //        objectTransform.position = tempTarget;
                //        break;
                //    case TweenTypeSingle.LocalMoveX:
                //        tempTarget.x = target + curveValue;
                //        objectTransform.localPosition = tempTarget;
                //        break;
                //    case TweenTypeSingle.LocalMoveY:
                //        tempTarget.y = target + curveValue;
                //        objectTransform.localPosition = tempTarget;
                //        break;
                //    case TweenTypeSingle.LocalMoveZ:
                //        tempTarget.z = target + curveValue;
                //        objectTransform.localPosition = tempTarget;
                //        break;
                //    case TweenTypeSingle.ScaleX:
                //        tempTarget.x = target + curveValue;
                //        objectTransform.localScale = tempTarget;
                //        break;
                //    case TweenTypeSingle.ScaleY:
                //        tempTarget.x = target + curveValue;
                //        objectTransform.localScale = tempTarget;
                //        break;
                //    case TweenTypeSingle.ScaleZ:
                //        tempTarget.x = target + curveValue;
                //        objectTransform.localScale = tempTarget;
                //        break;
                //}
                #endregion
            }
        }

        private void Move(bool last = false)
        {
            t = last ? 1 : counter / time;
            if (curve != null) curveValue = (curve.Evaluate(t) * amplitude);

            switch (type)
            {
                case TweenTypeSingle.MoveX:
                    tempTarget.x = last? target + curveValue : tempTarget.x + curveValue;
                    objectTransform.position = Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenTypeSingle.MoveY:
                    tempTarget.y = last ? target + curveValue : tempTarget.y + curveValue;
                    objectTransform.position = Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenTypeSingle.MoveZ:
                    tempTarget.z = last ? target + curveValue : tempTarget.z + curveValue;
                    objectTransform.position = Vector3.Lerp(start, tempTarget, t);
                    break;

                case TweenTypeSingle.LocalMoveX:
                    tempTarget.x = last ? target + curveValue : tempTarget.x + curveValue;
                    objectTransform.localPosition = Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenTypeSingle.LocalMoveY:
                    tempTarget.y = last ? target + curveValue : tempTarget.y + curveValue;
                    objectTransform.localPosition = Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenTypeSingle.LocalMoveZ:
                    tempTarget.z = last ? target + curveValue : tempTarget.z + curveValue;
                    objectTransform.localPosition = Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenTypeSingle.ScaleX:
                    tempTarget.x = last ? target + curveValue : tempTarget.x + curveValue;
                    objectTransform.localScale = Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenTypeSingle.ScaleY:
                    tempTarget.y = last ? target + curveValue : tempTarget.y + curveValue;
                    objectTransform.localScale = Vector3.Lerp(start, tempTarget, t);
                    break;
                case TweenTypeSingle.ScaleZ:
                    tempTarget.z = last ? target + curveValue : tempTarget.z + curveValue;
                    objectTransform.localScale = Vector3.Lerp(start, tempTarget, t);
                    break;
            }
        }

        /// <summary>
        /// Set an Animation Curve to be added to the Target Vector. 
        /// ie Target Vector3 010 and Curve ending with 3 and Amplitude 1 will result in final target position of 040. 
        /// Play with the curves
        /// </summary>
        /// <param name="curve">Unity Animation Curve can be set in Editor</param>
        /// <param name="amplitude">Can be more than 1 if you want to multiply the values of the curve</param>
        /// <returns></returns>
        public TweenJobTransformSingle SetCurve(AnimationCurve curve, float amplitude = 1)
        {

            this.curve = curve;
            this.amplitude = amplitude;

            return this;
        }
        public TweenJobTransformSingle SetOnComplete(System.Action callback)
        {
            onComplete = callback;
            return this;
        }
        public TweenJobTransformSingle SetOnUpdate(System.Action callback)
        {
            onUpdate = callback;
            return this;
        
        }
        public TweenJobTransformSingle SetOnInterrupt(System.Action callback)
        {
            onInterrupt = callback;
            return this;
        }
    }




    public class TweenJobValueFloat : TweenJob
    {

        public TweenJobValueFloat(FloatField floatField, float target,float time, int jobID)
        {
            this.floatField = floatField;
            this.target = target;
            this.time = time;
            this.jobID = jobID;
            counter = 0;
        }

        internal FloatField floatField;
        internal float target;
        protected AnimationCurve curve;
        protected float amplitude;
        public override void Work(float dt, LerpEngine engine)
        {
            floatField.Value += (target * dt) / time;
            onUpdate?.Invoke();
            if (Step(dt))
            {
                engine.RemoveJob(jobID);
                if (onComplete != null) onComplete.Invoke();
            }
        }

        public TweenJobValueFloat SetCurve(AnimationCurve curve, float amplitude = 1)
        {
            this.curve = curve;
            this.amplitude = amplitude;
            return this;
        }
        public TweenJobValueFloat SetOnComplete(System.Action action)
        {
            onComplete = action;
            return this;
        }
        public TweenJobValueFloat SetOnUpdate(System.Action action)
        {
            onUpdate = action;
            return this;
        }
        public TweenJobValueFloat SetOnInterrupt(System.Action action)
        {
            onInterrupt = action;
            return this;
        }
    }
    public class TweenJobCallBackOverTime : TweenJob
    {

        public TweenJobCallBackOverTime(System.Action callback, float time, int jobID, float rate = 0)
        {
            this.callback = callback;
            this.time = time;
            this.jobID = jobID;
            this.rate = Mathf.Max(0, rate);
            counter = 0;
            rateCounter = 0;
        }
        protected System.Action callback;
        protected float rate;
        protected float rateCounter = 0;
        public override void Work(float dt, LerpEngine engine)
        {
            onUpdate?.Invoke();

            rateCounter += dt;
            if (rate > 0 && rateCounter >= rate)
            {
                rateCounter = 0;
                callback?.Invoke();
            }
            else
            {
                callback?.Invoke();
            }

            if (Step(dt))
            {
                if (onComplete != null) onComplete.Invoke();
                engine.RemoveJob(jobID);
            }
            
        }

        public TweenJobCallBackOverTime SetOnComplete(System.Action action)
        {
            onComplete = action;
            return this;
        }
        public TweenJobCallBackOverTime SetOnUpdate(System.Action action)
        {
            onUpdate = action;
            return this;
        }
        public TweenJobCallBackOverTime SetOnInterrupt(System.Action action)
        {
            onInterrupt = action;
            return this;
        }
    }
    #endregion
}

