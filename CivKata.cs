using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivKata
{
    public enum Civilization
    {
        Other,
        Dido,
        Pachacuti,
        Hiawatha
    }

    public class Player
    {
        public bool HasDiscoveredEngineering { get; set; }
        public bool HasEarnedAGreatGeneral { get; set;  }
        public Civilization Civilization { get; set; }
    };

    public enum HexType
    {
        Coast,
        Desert,
        Grassland,
        Hills,
        Mountain,
        Plains,
        Snow,
        Tundra
    };

    public enum HexModifier
    {
        None,
        Fallout,
        Forest,
        Ice,
        Jungle,
        Marsh
    };

    public enum RoadType
    {
        None,
        Road,
        Railroad
    }

    public class Hex
    {
        public HexType Type { get; set; }
        public HexModifier Modifier { get; set; }
        public RoadType Road { get; set; }

        public bool IsFriendly { get; set; }
    }

    public enum UnitAttribute
    {
        Woodsman,
        MayEnterIceTiles,
        IgnoresTerrainCost,
        AltitudeTraining,
        BonusesInSnowHillsAndTundra,
        CannotEnterForestOrJungle,
        HoveringUnit,
        FlyingUnit,
        RoughTerrainPenalty
    }



    public class Unit
    {
        // Stored in fixed-point 'micromoves' - 30 micromoves = 1 MP, because we have 1/3 or 1/10 for roads and railroads
        // and don't want to deal with floats.
        public int RemainingMicroMoves;
        public List<UnitAttribute> Attributes = new List<UnitAttribute>();
    }

    public static class Logic
    {
        public const int MicroMoveFactor = 30;

        public static int CostOfHexType(HexType hexType)
        {
            if (hexType == HexType.Hills)
            {
                return 2 * MicroMoveFactor;
            }
            return 1 * MicroMoveFactor;
        }

        private static bool IsRoadlike(Hex hex, Player player)
        {
            if (hex.Road >= RoadType.Road)
            {
                return true;
            }
            if (player.Civilization == Civilization.Hiawatha && 
                (hex.Modifier == HexModifier.Forest || hex.Modifier == HexModifier.Jungle) &&
                hex.IsFriendly == true)
            {
                return true;
            }
            return false;
        }
        public static int MoveCost(Hex Current, Hex Next, bool isRiverBetweenHexes, Player player, Unit unit)
        {
            if (isRiverBetweenHexes)
            {
                if (!(player.HasDiscoveredEngineering && (Current.Road != RoadType.None && Next.Road != RoadType.None)))
                { 
                    return unit.RemainingMicroMoves;
                }
            }

            int cost = CostOfHexType(Next.Type);

            if (Next.Modifier != HexModifier.None)
            {
                cost = CostOfHexModifier(Next.Modifier);
            }

            if (unit.Attributes.Contains(UnitAttribute.IgnoresTerrainCost) ||
                player.Civilization == Civilization.Pachacuti && Next.Type == HexType.Hills)
            {
                cost = 1 * MicroMoveFactor;
            }

            if (Current.Road == RoadType.Railroad && Next.Road == RoadType.Railroad)
            {
                cost = MicroMoveFactor / 10;
            }
            else if (IsRoadlike(Current, player) && IsRoadlike(Next, player))
            {
                cost = MicroMoveFactor / 3;
            }
            else if (Next.Type == HexType.Hills && unit.Attributes.Contains(UnitAttribute.AltitudeTraining))
            {
                cost = cost / 2;
            }
            
            return cost;
        }

        private static int CostOfHexModifier(HexModifier modifier)
        {
            if (modifier == HexModifier.Marsh)
            {
                return 3 * MicroMoveFactor;
            }
            return 2 * MicroMoveFactor;
        }

        public static bool IsPassable(Hex Hex, Player player, Unit unit)
        {
            if (unit.Attributes.Contains(UnitAttribute.HoveringUnit))
            {
                return true;
            }

            if (Hex.Type == HexType.Mountain)
            {
                if (player.Civilization == Civilization.Dido && player.HasEarnedAGreatGeneral)
                {
                    return true;
                }

                
                return false;
            }

            if (Hex.Modifier == HexModifier.Ice)
            {
                if (unit.Attributes.Contains(UnitAttribute.MayEnterIceTiles))
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
