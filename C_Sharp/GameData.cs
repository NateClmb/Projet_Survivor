using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Projet_Survivor.C_Sharp
{
    public enum EnemyType
    {
        HandToHand,
        Distance
    }

    [Serializable]
    public class GameData
    {
        [XmlArray("Enemies")]
        [XmlArrayItem("Enemy")]
        public List<EnemyData> Enemies { get; set; }

        [XmlIgnore] public string GameName { get; } = "Survivor";
    }

    [Serializable]
    public class EnemyData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int HP { get; set; }
        public int AttackDamage { get; set; }
        public float Speed { get; set; }
        public int XPValue { get; set; }
    }
}