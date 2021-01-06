def omar_wid170709_heuristic(game, player):

    if game.is_loser(player):
        return float("-inf")

    if game.is_winner(player):
        return float("inf")

    my_moves = len(game.get_legal_moves(player))
    opponent_moves = len(game.get_legal_moves(game.get_opponent(player)))

    weight = 0.0

    x, y = game.get_player_location(player)

    if (x < game.width-1) and (x > 0) and (y < game.height -1) and (y > 0):
        weight = 0.2

    diference_moves = my_moves - opponent_moves

    return diference_moves * (1.0 + weight)