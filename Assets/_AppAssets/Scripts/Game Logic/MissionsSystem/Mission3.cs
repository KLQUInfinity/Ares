using UnityEngine;
namespace Assets.Scripts.MissionComponent
{
    class Mission3 :Mission
    {
        protected internal Mission3(GameObject Room): base(Room/*new Training3Logic()*/)
        {
            this.missionName = MissionName.ResourceErgentProduction;
        }

        public override void startMission()
        {
        }
    }
}
