# Assignment 2: Isolation Game Minimax - Score Function

## By Omar Abdelmome Amin - WID170709 | 17107261/1

## My custom function’s algorithm:

So my function gives a higher score if the player is not beside the edge of the board, this is reasonable as the “L” move will be very limited if the player location is at the edge of the board.

So I use the basic difference method between “available moves for the player” and “available moves for the opponent”, but multiplied with a higher coefficient if the player is not beside the edge of the board.

This coefficient was chosen to be 1.2 after some trials.

Please check the report pdf for results