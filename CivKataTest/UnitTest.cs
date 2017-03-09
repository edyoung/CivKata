using System;
using Xunit;
using CivKata;

namespace CivKataTest
{
    public class Test
    {
        #region Movement costs
        [Fact]
        public void Plains_Costs1MP()
        {
            Hex next = new Hex()
            {
                Type = HexType.Plains
            };

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes:false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(1 * Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void Hills_Costs2MP()
        {
            Hex next = new Hex()
            {
                Type = HexType.Hills
            };

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(2 * Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void ForestOnPlains_Costs2MP()
        {
            Hex next = new Hex()
            {
                Type = HexType.Plains,
                Modifier = HexModifier.Forest
            };

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(2 * Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void ForestOnHills_Costs2MP()
        {
            Hex next = new Hex()
            {
                Type = HexType.Hills,
                Modifier = HexModifier.Forest
            };

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(2 * Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void MarshOnPlains_Costs3MP()
        {
            Hex next = new Hex()
            {
                Type = HexType.Plains,
                Modifier = HexModifier.Marsh
            };

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(3 * Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void ScoutInHills_Costs1MP()
        {
            Hex next = new Hex() { Type = HexType.Hills };

            var scout = Scout();
            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: scout);

            Assert.Equal(1 * Logic.MicroMoveFactor, cost);
        }


        [Fact]
        public void ScoutInPlainsWithAltitudeTraining_Costs1MP()
        {
            Hex next = new Hex() { Type = HexType.Plains };

            var scout = Scout();
            scout.Attributes.Add(UnitAttribute.AltitudeTraining);
            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: scout);

            Assert.Equal(Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void ScoutInHillsWithAltitudeTraining_CostsHalfMP()
        {
            Hex next = new Hex() { Type = HexType.Hills };

            var scout = Scout();
            scout.Attributes.Add(UnitAttribute.AltitudeTraining);
            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: scout);

            Assert.Equal(Logic.MicroMoveFactor / 2, cost);
        }

        [Fact]
        public void BaseInHillsWithAltitudeTraining_Costs1MP()
        {
            Hex next = new Hex() { Type = HexType.Hills };

            var unit = BaseUnit();
            unit.Attributes.Add(UnitAttribute.AltitudeTraining);
            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: unit);

            Assert.Equal(Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void HiawathaInFriendlyForest_CostsOneThirdMP()
        {
            Hex next = new Hex() { Type = HexType.Plains, Modifier = HexModifier.Forest, IsFriendly = true };

            var unit = BaseUnit();
            var player = Hiawatha();

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: Hiawatha(), unit: unit);

            Assert.Equal(Logic.MicroMoveFactor / 3, cost);
        }

        [Fact]
        public void HiawathaInFriendlyJungle_CostsOneThirdMP()
        {
            Hex next = new Hex() { Type = HexType.Plains, Modifier = HexModifier.Jungle, IsFriendly = true };

            var unit = BaseUnit();
            var player = Hiawatha();

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: Hiawatha(), unit: unit);

            Assert.Equal(Logic.MicroMoveFactor / 3, cost);
        }

        [Fact]
        public void HiawathaInUnfriendlyForest_Costs2MP()
        {
            Hex next = new Hex() { Type = HexType.Plains, Modifier = HexModifier.Forest, IsFriendly = false };

            var unit = BaseUnit();
            var player = Hiawatha();

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: Hiawatha(), unit: unit);

            Assert.Equal(Logic.MicroMoveFactor * 2, cost);
        }

        [Fact]
        public void PachacutiInHills_IgnoresTerrainCost()
        {
            Hex next = new Hex() { Type = HexType.Hills, Modifier = HexModifier.Forest };

            var unit = BaseUnit();
            var player = Hiawatha();

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: Pachacuti(), unit: unit);

            Assert.Equal(Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void PachacutiInPlains_StandardCost()
        {
            Hex next = new Hex() { Type = HexType.Plains, Modifier = HexModifier.Forest };

            var unit = BaseUnit();
            var player = Hiawatha();

            var cost = Logic.MoveCost(
                Current: next, Next: next, isRiverBetweenHexes: false, player: Pachacuti(), unit: unit);

            Assert.Equal(Logic.MicroMoveFactor * 2 , cost);
        }

        [Fact]
        public void RoadInOneHex_NoEffect()
        {
            Hex current = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.None
            };

            Hex next = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.Road
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(1 * Logic.MicroMoveFactor, cost);
        }

        [Fact]
        public void RoadInTwoHexes_OneThirdMP()
        {
            Hex current = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.Road
            };

            Hex next = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.Road
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(Logic.MicroMoveFactor/ 3, cost);
        }

        [Fact]
        public void RoadInTwoHexesOverHill_OneThirdMP()
        {
            Hex current = new Hex()
            {
                Type = HexType.Hills,
                Road = RoadType.Road
            };

            Hex next = new Hex()
            {
                Type = HexType.Hills,
                Road = RoadType.Road
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(Logic.MicroMoveFactor / 3, cost);
        }

        [Fact]
        public void RailRoadInTwoHexes_OneTenthMP()
        {
            Hex current = new Hex()
            {
                Type = HexType.Hills,
                Road = RoadType.Railroad
            };

            Hex next = new Hex()
            {
                Type = HexType.Hills,
                Road = RoadType.Railroad
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(Logic.MicroMoveFactor / 10, cost);
        }


        [Fact]
        public void MixedRailAndRoad_CostsSameAsRoad()
        {
            Hex current = new Hex()
            {
                Type = HexType.Hills,
                Road = RoadType.Railroad
            };

            Hex next = new Hex()
            {
                Type = HexType.Hills,
                Road = RoadType.Road
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: false, player: BasePlayer(), unit: BaseUnit());

            Assert.Equal(Logic.MicroMoveFactor / 3, cost);
        }
        [Fact]
        public void River_UsesEverything()
        {
            Unit unit = BaseUnit();
            unit.RemainingMicroMoves = 247;
            Hex current = new Hex()
            {
                Type = HexType.Plains
            };
            Hex next = new Hex()
            {
                Type = HexType.Plains
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: true, player: BasePlayer(), unit: unit);

            Assert.Equal(247, cost);
        }

        [Fact]
        public void RiverAndRoad_UsesEverything()
        {
            Unit unit = BaseUnit();
            unit.RemainingMicroMoves = 247;
            Hex current = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.Road
            };
            Hex next = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.Road
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: true, player: BasePlayer(), unit: unit);

            Assert.Equal(247, cost);
        }


        [Fact]
        public void RiverAndRoadAndEngineering_UsesRoadCost()
        {
            Unit unit = BaseUnit();
            unit.RemainingMicroMoves = 247;
            Hex current = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.Road
            };
            Hex next = new Hex()
            {
                Type = HexType.Plains,
                Road = RoadType.Road
            };

            var cost = Logic.MoveCost(
                Current: current, Next: next, isRiverBetweenHexes: true, player: EngineeringPlayer(), unit: unit);

            Assert.Equal(Logic.MicroMoveFactor / 3, cost);
        }
        #endregion

        #region passability
        [InlineData(HexType.Hills, HexModifier.None)]
        [InlineData(HexType.Coast, HexModifier.None)]
        [Theory]
        public void MostHexTypes_ArePassable(HexType hexType, HexModifier hexModifier)
        {
            Hex h = new Hex() { Type = hexType, Modifier = hexModifier };
            bool passable = Logic.IsPassable(Hex: h, player: BasePlayer(), unit: BaseUnit());
            Assert.True(passable);
        }

        [InlineData(HexType.Mountain, HexModifier.None)]
        [InlineData(HexType.Snow, HexModifier.Ice)]
        [Theory]
        public void SomeHexTypes_AreImassable(HexType hexType, HexModifier hexModifier)
        {
            Hex h = new Hex() { Type = hexType, Modifier = hexModifier };
            bool passable = Logic.IsPassable(Hex: h, player: BasePlayer(), unit: BaseUnit());
            Assert.False(passable);
        }

        [Fact]
        public void MountainsForDidoWithoutGeneral_AreImpassable()
        {
            Player p = Dido();
            Assert.False(p.HasEarnedAGreatGeneral);
            Assert.False(Logic.IsPassable(new Hex { Type = HexType.Mountain }, player: p, unit: BaseUnit()));
        }

        [Fact]
        public void MountainsForDidoWithGeneral_ArePassable()
        {
            Player p = Dido();
            p.HasEarnedAGreatGeneral = true;
            Assert.True(Logic.IsPassable(new Hex { Type = HexType.Mountain }, player: p, unit: BaseUnit()));
        }

        [Fact]
        public void MountainsForHoveringUnit_ArePassable()
        {
            Unit unit = new Unit();
            unit.Attributes.Add(UnitAttribute.HoveringUnit);
            Assert.True(Logic.IsPassable(new Hex { Type = HexType.Mountain }, player: BasePlayer(), unit: unit));
        }

        [Fact]
        public void IceForSuitableUnit_IsPassable()
        {
            Unit unit = new Unit();
            unit.Attributes.Add(UnitAttribute.MayEnterIceTiles);
            Assert.True(Logic.IsPassable(new Hex { Type = HexType.Snow, Modifier = HexModifier.Ice }, player: BasePlayer(), unit: unit));
        }

        [Fact]
        public void IceForHoveringUnit_IsPassable()
        {
            Unit unit = new Unit();
            unit.Attributes.Add(UnitAttribute.HoveringUnit);
            Assert.True(Logic.IsPassable(new Hex { Type = HexType.Snow, Modifier = HexModifier.Ice }, player: BasePlayer(), unit: unit));
        }

        [InlineData(HexModifier.Forest)]
        [InlineData(HexModifier.Jungle)]
        [Theory]
        public void WoodsForUnsuitableUnit_IsImpassable(HexModifier modifier)
        {
            Hex h = new Hex() { Type = HexType.Desert, Modifier = modifier };
            Unit unit = new Unit();
            unit.Attributes.Add(UnitAttribute.CannotEnterForestOrJungle);

            Assert.False(Logic.IsPassable(Hex: h, player: BasePlayer(), unit: unit));
        }
        #endregion  

        #region helpers
        private Unit Scout()
        {
            Unit scout = new Unit();
            scout.Attributes.Add(UnitAttribute.IgnoresTerrainCost);
            return scout;
        }

        private Unit BaseUnit()
        {
            return new Unit();
        }

        private Player BasePlayer()
        {
            return new Player();
        }

        private Player Hiawatha()
        {
            return new Player { Civilization = Civilization.Hiawatha };
        }

        private Player Pachacuti()
        {
            return new Player { Civilization = Civilization.Pachacuti };
        }
        private Player Dido()
        {
            return new Player { Civilization = Civilization.Dido };
        }
        private Player EngineeringPlayer()
        {
            return new Player() { HasDiscoveredEngineering = true };
        }
        #endregion
    }
}
