using UnityEngine;

namespace Assets.Scripts.MissionComponent
{
    
    public enum MissionName
    {
        AsteroidAccident,
        FuelLeaking,
        ResourceErgentProduction
    }
    public class MissionFactory
    {
        static GameObject  Room;

        public static Mission createMission(MissionName MS_NAme)
        {
            Room = GetRandomroom();
            switch (MS_NAme)
            {
                case MissionName.AsteroidAccident:
                    return new Mission1(Room);
                case MissionName.FuelLeaking:
                    return new Mission2(Room);
                case MissionName.ResourceErgentProduction:
                    return new Mission3(Room);
                default:
                    return null;
            }
        }

        private static GameObject GetRandomroom()
        {
           return LevelManager.Instance.Environment.transform.GetChildren()[Random.Range(0,
               LevelManager.Instance.Environment.transform.GetChildren().Count)].gameObject;
        }
    }
}
