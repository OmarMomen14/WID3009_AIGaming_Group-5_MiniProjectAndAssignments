def najib_heuristic_1(game, player):
    if game.is_loser(player):
        return float("-inf")

    if game.is_winner(player):
        return float("inf")

    my_moves = len(game.get_legal_moves(player))
    opponent_moves = len(game.get_legal_moves(game.get_opponent(player)))

    """
    custom heuristic function that maximises player's move by power of 3
    """
    return math.pow(my_moves, 3) - opponent_moves

def najib_heuristic_2(game, player):
    if game.is_loser(player):
        return float("-inf")

    if game.is_winner(player):
        return float("inf")

    my_moves = len(game.get_legal_moves(player))
    opponent_moves = len(game.get_legal_moves(game.get_opponent(player)))

    """
    custom heuristic function that maximises player's move by multiplying with 10
    """
    return 10*my_moves - opponent_moves
