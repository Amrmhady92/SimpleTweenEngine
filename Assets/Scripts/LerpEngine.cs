using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SimpleTweenEngine
{
    public class LerpEngine : MonoBehaviour
    {

        List<TweenJob> jobsQueue;
        List<TweenJob> tempJobsQueue;

        TweenJob currentJob;
        int jobIndex = 0;

        public List<TweenJob> JobsQueue
        {
            get
            {
                if (jobsQueue == null) jobsQueue = new List<TweenJob>();
                return jobsQueue;
            }


        }
        public List<TweenJob> TempJobsQueue
        {
            get
            {
                if (tempJobsQueue == null) tempJobsQueue = new List<TweenJob>();
                return tempJobsQueue;
            }

        }
        public int ActiveJobsCount
        {
            get
            {
                return JobsQueue.Count + TempJobsQueue.Count;
            }
        }
        private void Update()
        {
            //Taking the jobs one by one as long as we have any jobs
            if (JobsQueue.Count == 0 && TempJobsQueue.Count > 0)
            {
                jobsQueue = new List<TweenJob>(TempJobsQueue);
                TempJobsQueue.Clear();
            }

            while (JobsQueue.Count > 0) 
            {
                currentJob = JobsQueue[0];
                JobsQueue.RemoveAt(0);
                if (currentJob == null) continue;

                TempJobsQueue.Add(currentJob);
                currentJob.Work(Time.deltaTime, this);
                currentJob = null;
            }

            
        }
        public void AddJob(TweenJob job)
        {
            if (job == null) return;

            job.jobID = jobIndex;
            jobIndex++;
            JobsQueue.Add(job);
        }
        public void RemoveJob(TweenJob job)
        {
            if (job != null)
            {
                RemoveJob(job.jobID);
            }
        }
        public void RemoveJob(int id)
        {
            TweenJob job = JobsQueue.Find(x => x.jobID == id); // look for the job in the active queue
            if (job != null)
            {
                JobsQueue.Remove(job);
            }

            job = TempJobsQueue.Find(x => x.jobID == id); // look in the temp as well
            if (job != null)
            {
                TempJobsQueue.Remove(job);
            }
        }
        public TweenJob GetJob(int id)
        {
            TweenJob job = JobsQueue.Find(x => x.jobID == id); // look for the job in the active queue
            if (job != null)
            {
                return job;
            }
            job = TempJobsQueue.Find(x => x.jobID == id); // look in the temp as well
            return job;
        }
        public bool EndJob(int id)
        {
            TweenJob job = GetJob(id);
            if (job == null) return false;
            RemoveJob(job.jobID);
            if (job.onInterrupt != null) job.onInterrupt.Invoke();
            return true;
        }
    }
}