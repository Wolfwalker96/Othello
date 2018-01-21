Othello IA - Alphabeta
======================
* Auteur : Paul Jeanbourquin et Dylan Santos de Pinho
* Cours: Intelligence artificielle
* Superviseur : Hatem Ghorbel

Objectif
--------
Implémenter une IA utilisant l'algorithme d'alphabeta pour evaluer ses coups aux jeux Othello.

Evaluation
----------
L'aplhabeta va chercher et évaluer les coups sur 5 niveaux.

Pour l'évaluation, on a décidé d'utiliser un tableau qui donne une valeur à chaque case du plateau:

 		{ 99, -8,   8,  6,  6,  8, -8,   99},
                { -8, -24, -4, -3, -3, -4, -24, -8},
                {  8, -4,   7,  4,  4,  7, -4,   8},
                {  6, -3,   4,  0,  0,  4, -3,   6},
                {  6, -3,   4,  0,  0,  4, -3,   6},
                {  8, -4,   7,  4,  4,  7, -4,   8},
                { -8, -24, -4, -3, -3, -4, -24, -8},
                { 99, -8,   8,  6,  6,  8, -8,   99}

On additionne les valeurs des cases prises par nos pions. Puis, on multiplie cette valeur par 4 pour lui donner un plus grand poids
et on y rajoute le nombre de nos pions. Enfin, on divise cette valeur par le nombre de pions de l'adversaire.
