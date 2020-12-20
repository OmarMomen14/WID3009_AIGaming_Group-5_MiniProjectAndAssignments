def omar_ahmad_17121293(game, player):
 
    if game.is_winner(player):
        return float("inf")
        
    if game.is_loser(player):
        return float("-inf")
        
    own_moves = len(game.get_legal_moves(player))
    opp_moves = len(game.get_legal_moves(game.get_opponent(player)))
    return 100.0 * (own_moves - 1.8 * opp_moves) / (own_moves + opp_moves)


