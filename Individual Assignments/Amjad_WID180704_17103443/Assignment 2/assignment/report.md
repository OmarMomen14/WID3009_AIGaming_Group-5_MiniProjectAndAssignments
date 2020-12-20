## Amjad Alsharafi (WID180704) (17103443/1)

### Algorithm explaination
First, the game will return positive or negative infinity if the game is done:
```python
if game.is_winner(player):
    return float('inf')
if game.is_loser(player):
    return float('-inf')
```

Next it gets the distance between the two players:
```python
my_position = game.get_player_location(player)
opponent_position = game.get_player_location(game.get_opponent(player))

dist = math.sqrt(
        (((my_position[0] - opponent_position[0]) ** 2)
       + ((my_position[1] - opponent_position[1]) ** 2))
      )
```

Lastly, it will get the number of moves for the two player, and compute the score
by getting the difference in the number of available moves (*with the opponent moves
scaled because we want to have the maximum number of moves while minimizing the opponent moves*).
And scale everything by the distance.


```python
my_moves = len(game.get_legal_moves(player))
opponent_moves = len(game.get_legal_moves(game.get_opponent(player)))

return float(((my_moves) - 100*((opponent_moves))) * dist)
```

### Result
```txt
*************************
 Evaluating: ID_Improved 
*************************

Playing Matches:
----------
  Match 1: ID_Improved vs   Random    	Result: 171 to 29
  Match 2: ID_Improved vs   MM_Null   	Result: 143 to 57
  Match 3: ID_Improved vs   MM_Open   	Result: 95 to 105
  Match 4: ID_Improved vs MM_Improved 	Result: 92 to 108
  Match 5: ID_Improved vs   AB_Null   	Result: 123 to 77
  Match 6: ID_Improved vs   AB_Open   	Result: 114 to 86
  Match 7: ID_Improved vs AB_Improved 	Result: 111 to 89


Results:
----------
ID_Improved         60.64%

*************************
    Evaluating: AMJAD    
*************************

Playing Matches:
----------
  Match 1:    AMJAD    vs   Random    	Result: 185 to 15
  Match 2:    AMJAD    vs   MM_Null   	Result: 167 to 33
  Match 3:    AMJAD    vs   MM_Open   	Result: 125 to 75
  Match 4:    AMJAD    vs MM_Improved 	Result: 113 to 87
  Match 5:    AMJAD    vs   AB_Null   	Result: 154 to 46
  Match 6:    AMJAD    vs   AB_Open   	Result: 132 to 68
  Match 7:    AMJAD    vs AB_Improved 	Result: 132 to 68


Results:
----------
AMJAD               72.00%
```
