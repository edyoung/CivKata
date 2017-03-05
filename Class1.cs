using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivKata
{
    public class Player
    {
        public bool HasDiscoveredEngineering;
        public bool IsByzantium;
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

    public class Hex
    {
        public HexType Type { get; set; }
        public HexModifier Modifier { get; set; }
        public bool HasRoad { get; set; }
    }

    public enum UnitAttribute
    {
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

        public static int MoveCost(Hex Current, Hex Next, bool isRiverBetweenHexes, Player player, Unit unit)
        {
            int cost = CostOfHexType(Next.Type);

            if (Next.Modifier != HexModifier.None)
            {
                cost = CostOfHexModifier(Next.Modifier);
            }

            if (unit.Attributes.Contains(UnitAttribute.IgnoresTerrainCost))
            {
                cost = 1 * MicroMoveFactor;
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
    }
}
