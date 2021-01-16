from game_agent import CustomPlayer, najib_heuristic_1, najib_heuristic_2

NUM_MATCHES = 10  # number of matches against each opponent
TIME_LIMIT = 15  # number of milliseconds before timeout

    # ID_Improved agent is used for comparison to the performance of the
    # submitted agent for calibration on the performance across different
    # systems; i.e., the performance of the student agent is considered
    # relative to the performance of the ID_Improved agent to account for
    # faster or slower computers.
    
    test_agents = [Agent(CustomPlayer(score_fn=improved_score, **CUSTOM_ARGS), "ID_Improved"),
                   Agent(CustomPlayer(score_fn=aggressive_heuristic, **CUSTOM_ARGS), "Student1"),
                   Agent(CustomPlayer(score_fn=defensive_heuristic, **CUSTOM_ARGS), "Student2"),
                   Agent(CustomPlayer(score_fn=maximizing_win_chances_heuristic, **CUSTOM_ARGS), "Student3"),
                   Agent(CustomPlayer(score_fn=minimizing_losing_chances_heuristic, **CUSTOM_ARGS), "Student4"),
                   Agent(CustomPlayer(score_fn=chances_heuristic, **CUSTOM_ARGS), "Student5"),
                   Agent(CustomPlayer(score_fn=weighted_chances_heuristic, **CUSTOM_ARGS), "Student6"),
                   Agent(CustomPlayer(score_fn=weighted_chances_heuristic_2, **CUSTOM_ARGS), "Student7"),
                   Agent(CustomPlayer(score_fn=najib_heuristic_1, **CUSTOM_ARGS), "najib1"),
                   Agent(CustomPlayer(score_fn=najib_heuristic_2, **CUSTOM_ARGS), "najib2")
                   ]

    ### test only najib agent
    # test_agents = [Agent(CustomPlayer(score_fn=najib_heuristic_1, **CUSTOM_ARGS), "najib1"),
    #               Agent(CustomPlayer(score_fn=najib_heuristic_2, **CUSTOM_ARGS), "najib2")]
    
