You may play Color Blast on your browser: https://tesla.itch.io/color-blast


Color Blast est un petit jeu simple sur itch.io, créé par Ömer Akyol, qui semble très prolifique dans la création de jeux de browser.

Le gameplay est très facile, on doit seulement cliquer sur les tuiles lorsqu'elles se touchent, jusqu'à ce qu'on aie rempli tous les objectifs donnés.
Il y a 6 niveaux.

J'ai choisi ce jeu parce que j'ai vu immédiatement ou il pourrait être amélioré, tout en ayant une bonne base sur laquelle commencer. Voici un résumé de mes contributions:

Dans le jeu original, on ne peut pas échouer un niveau. On ne fait que cliquer jusqu'à ce que les objectifs soient remplis.
C'est pour ceci que j'ai ajouté un nombre de mouvements limité pour chaque niveau du jeu. Si on ne rempli pas les objectifs avec le nombre de mouvements donnés, on doit recommencer et le niveau est échoué.
J'ai utilisé un **algorithme de contrôle simple** pour se faire. **(point 6, diapo 8)**

J'ai aussi implémenté une progression au jeu en enlevant l'accès aux niveaux supérieurs tant que le niveau précédent n'a pas été réussi.
J'ai utilisé, entre autres, un **array** pour conservées les données des niveaux finis (LevelCompleted). **(point 4, diapo 6)**

J'ai aussi modifié le code existant pour qu'au lieu de prendre des tuiles aléatoires, le jeu prend des tuiles d'un pool, nous permettant aussi de voir dans le log les prochaines tuiles qui suiveront.
Ceci permet de s'assurer d'un mix de couleurs plus varié qu'un choix aléatoire, et donne une bonne base pour des futures implémentations si on veut faire des niveaux plus spéciaux.
C'est ici que la **liste chaînée** est utilisée. **(point 5, diapo 7)**

J'ai fait des **améliorations au code (refactoring)** pour qu'il soit plus lisible et performant. **(point 7, diapo 9)**

Et finalement, j'ai ajouté une **fonctionalité d'accessibilité** en ajoutant un mode daltonien qui peut être activé et désactivé en tout temps. **(point 8, diapo 10)