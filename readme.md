Civilization V (http://civilization5.com/) is a famous strategy game, where you have to build an empire. 
It's also very interesting from a software engineering point of view, because the logic around many operations is complicated, 
and can be modified by many different factors. I wanted to explore different programming styles, and picked a tiny subset of the 
Civilization rules as a sample problem to do so. I find Civ V's rules particularly interesting because they often involve 
the interaction of multiple entities, and don't map trivially to the standard object-oriented model.

NB: This repository does not do anything useful, does not contain a playable game of any kind, and does not contain any code derived from Civilization. 

The problem is: given a unit in one hexagonal tile, it has to pay a certain amount of 'movement points' to move into an adjacent tile. 
How many points are required?

The answer depends on the unit which is moving, the source tile, the destination tile, the civilization that the player is using, 
the technologies the player has discovered, whether there is a river between the tiles. 

A specification is below. This was determined by reading the rules and playing the game. 
I have deliberately left out 'zone of control' and water tiles to simplify things a bit. 
There are probably other inadvertent differences between this and the real algorithm.

Specification:

Movement cost:
The basic movement point cost is based on the target hex, not the current one. This is modified in a number of different ways.

Hex types:
Each requires a base 1 MP, except as noted
Coast, Desert, Grassland, Hills (2 MP), Mountain (Impassable or 1MP), Plains, Snow, Tundra

If the player is Pachacuti, units ignore terrain costs when moving into Hills

Hex modifiers:
If a hex has one of these modifiers it replaces the base MP cost
Fallout: 2 MP
Forest: 2 MP
Ice: Impassable (1MP if passable)
Jungle: 2 MP
Marsh: 3 MP

If the player is Hiawatha, units treat Forest as roads


Rivers:
A river can exist *between* 2 hexes. If there is one, crossing it uses all remaining movement points
	- except if there is a road in both hexes and the player has discovered Engineering

Railroads:
If a railroad exists in both source and target hex, MP cost is 0.1, regardless of terrain

If a road exists in both source and target hex, MP cost is 1/3 MP, regardless of terrain

If a railroad exists in one and a road in the other, that counts as a road

Passability:
If the target hex is a mountain, it is impassable 
	- except if the player is Dido and has previously earned a Great General
	- except if the unit is a hovering unit

If the target hex is Ice, it is Impassable
	- except if the unit has the 'may enter ice tiles' attribute

Unit attributes:
A unit gets a base set of attributes when created (for example, all scouts get 'ignores terrain cost'), 
and can gain additional attributes later (for example by being promoted, or by going next to Mt Kilimanjaro)
	'woodsman' -  half cost for woods or jungle
	'ignores terrain cost' - , every hex is 1 MP (but impassable ones are still impassable)
	'altitude training' - double movement in hills
	'bonuses in snow, tundra and hills' - double movement in snow, tundra and hills
	'cannot enter forest or jungle' -  cannot enter those 
	'hovering unit' - may pass over a mountain
	'rough terrain penalty' - entering 'rough terrain' (MP cost > 1) costs all movement

