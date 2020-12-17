import math

def amjad_function(game, player):
    if game.is_winner(player):
        return float('inf')
    if game.is_loser(player):
        return float('-inf')

    my_position = game.get_player_location(player)
    opponent_position = game.get_player_location(game.get_opponent(player))

    dist = math.sqrt(
            (((my_position[0] - opponent_position[0]) ** 2)
           + ((my_position[1] - opponent_position[1]) ** 2))
          )
    
    my_moves = len(game.get_legal_moves(player))
    opponent_moves = len(game.get_legal_moves(game.get_opponent(player)))
    
    return float(((my_moves) - 100*((opponent_moves))) * dist)

