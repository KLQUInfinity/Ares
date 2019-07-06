using UnityEngine;
public interface IJobWorkflow
{
    void startWorkflow();
    void pauseWorkflow();
    void updateWorkflow();
    void finishWorkflow();
    Job getWorkFlowJob();
    JobLoadState? getJobLoadState();
    void setJobLoadState(JobLoadState loadState);
    void setLoadPosition(Vector3 DestinationPosition);
    JobWorkflowSate getWorkflowState();
    void setIsCharacterAtInitialWorkflowPos(bool value);
}
